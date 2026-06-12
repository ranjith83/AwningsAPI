using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AwningsEmailFunction.Services;

public class EmailAnalysisService : IEmailAnalysisService
{
    private readonly ILogger<EmailAnalysisService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public EmailAnalysisService(
        ILogger<EmailAnalysisService> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EmailAnalysisResult> AnalyzeEmailAsync(IncomingEmail email)
    {
        var subject = email.Subject ?? "";
        var body = email.BodyContent ?? "";
        var fromEmail = email.FromEmail?.ToLower() ?? "";

        var result = new EmailAnalysisResult
        {
            CustomerInfo = new Dictionary<string, string>
            {
                { "fromName", email.FromName ?? "" },
                { "fromEmail", email.FromEmail ?? "" },
                { "phone", ExtractPhoneNumber(body) ?? "" },
                { "companyNumber", ExtractCompanyNumber(body) ?? "" }
            },
            Warnings = new List<string>()
        };

        if (IsJunkEmail(fromEmail, subject.ToLower(), body.ToLower()))
        {
            result.Category = "junk";
            result.TaskType = "junk";
            result.Priority = "Low";
            result.Confidence = 0.95;
            result.Reasoning = "Automated system email detected";
            result.IsSpam = true;
            result.RequiredActions = new List<string> { "mark_as_junk" };
            _logger.LogInformation("JUNK EMAIL: {Subject} from {From}", email.Subject, email.FromEmail);
            return result;
        }

        var aiAnalysis = await AnalyzeEmailWithAIAsync(subject, body, fromEmail);

        result.Category = aiAnalysis.Category;
        result.TaskType = MapCategoryToTaskType(aiAnalysis.Category);
        result.Priority = aiAnalysis.Priority;
        result.Confidence = aiAnalysis.Confidence;
        result.Reasoning = aiAnalysis.Reasoning;
        result.Sentiment = aiAnalysis.Sentiment;
        result.ExtractedData = MergeExtractedData(aiAnalysis.ExtractedData, ExtractDataFromEmail(email, aiAnalysis.Category));
        result.CompanyNumber = ExtractCompanyNumber(body) ?? "";
        result.RequiredActions = DetermineRequiredActions(aiAnalysis.Category);
        result.IsSpam = false;
        result.NeedsReply = aiAnalysis.NeedsReply;

        _logger.LogInformation("AI ANALYSIS: Category={Category}, Confidence={Confidence:F2}", aiAnalysis.Category, aiAnalysis.Confidence);
        return result;
    }

    private async Task<AIAnalysisResult> AnalyzeEmailWithAIAsync(string subject, string body, string fromEmail)
    {
        var aiProvider = _configuration["AI:Provider"] ?? "Claude";
        var apiKey = _configuration[$"{aiProvider}:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException($"{aiProvider}:ApiKey is not configured");

        var prompt = BuildPrompt(subject, body, fromEmail);
        return string.Equals(aiProvider, "Claude", StringComparison.OrdinalIgnoreCase)
            ? await CallClaudeAsync(prompt, apiKey)
            : await CallOpenAIAsync(prompt, apiKey);
    }

    private string BuildPrompt(string subject, string body, string fromEmail) => $@"You are an expert email categorization system for an awnings/pergola company.

Analyze this email and categorize it into ONE of these categories:

**Categories:**
1. **enquiry** - New customer enquiries about products, pricing, or general information
   - Looking for awnings, pergolas, canopies, shade solutions
   - Asking about products, models, prices
   - Includes dimensions or specific requirements
   - Example: ""Looking for 3x3m pergola for patio""

2. **site_visit** - Requests for site visits, measurements, or meetings
   - Keywords: site visit, site survey, visual inspection, meeting, appointment
   - Scheduling requests
   - Example: ""Can we schedule a site visit next Tuesday?""

3. **invoice** - Payment reminders or invoice notifications
   - Keywords: invoice due, payment due, overdue, payment reminder
   - Example: ""Invoice #INV-001 due on 15/02/2024""

4. **quote** - Specific quote requests
   - Direct quote requests with specifications
   - Example: ""Please provide quote for 5.2m awning""

5. **showroom** - Showroom visit requests
   - Want to see samples, visit showroom
   - Example: ""Can I visit your showroom to see samples?""

6. **complaint** - Customer complaints, issues, or follow-ups on an existing problem
   - Problems with products or service (faulty, damaged, broken, defective)
   - Warranty or replacement part claims (e.g. replacement frames/parts, damaged parasols/awnings)
   - Requests for purchase receipts, invoices, or order confirmations needed to resolve an issue
   - Replies continuing an existing complaint/warranty thread (e.g. supplier or customer following up on a prior issue)
   - Example: ""The awning is not working properly""
   - Example: ""We need the purchase receipt/invoice to process the warranty claim""

7. **general** - Everything else

**Email to analyze:**
Subject: {subject}
From: {fromEmail}
Body: {body}

**Also determine whether this email needs a reply sent back to the customer:**
- needsReply = true if the customer is asking a question, requesting information, a quote, a site visit, or a showroom appointment, or has raised a complaint/issue that requires acknowledgement
- needsReply = false if the email is an automated notification, an invoice/payment reminder, a thank-you/confirmation, or a message that closes out a conversation and doesn't expect a response (e.g. ""Thanks, that's all I needed"")

**Respond ONLY with valid JSON in this exact format:**
{{
  ""category"": ""enquiry"",
  ""confidence"": 0.85,
  ""priority"": ""Normal"",
  ""sentiment"": ""Neutral"",
  ""needsReply"": true,
  ""reasoning"": ""Brief explanation (1 sentence)"",
  ""extractedData"": {{}}
}}

**Rules:**
- category must be one of: enquiry, site_visit, invoice, quote, showroom, complaint, general
- confidence: 0.0 to 1.0
- priority: ""Low"", ""Normal"", ""High"", or ""Urgent""
- sentiment: ""Positive"", ""Neutral"", ""Negative"", or ""Urgent""
- needsReply: true or false, per the guidance above
- reasoning: Brief explanation (1 sentence)
- extractedData: Any relevant structured data found

Respond ONLY with the JSON object, no other text.";

    private async Task<AIAnalysisResult> CallClaudeAsync(string prompt, string apiKey)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var requestBody = new
        {
            model = _configuration["Claude:Model"] ?? "claude-haiku-4-5",
            max_tokens = 1024,
            messages = new[] { new { role = "user", content = prompt } }
        };

        var response = await client.PostAsync(
            "https://api.anthropic.com/v1/messages",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var claudeResponse = JsonSerializer.Deserialize<ClaudeResponse>(json);
        return ParseAIResponse(claudeResponse!.content[0].text);
    }

    private async Task<AIAnalysisResult> CallOpenAIAsync(string prompt, string apiKey)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new { role = "system", content = "You are an email categorization expert. Respond only with valid JSON." },
                new { role = "user", content = prompt }
            },
            temperature = 0.3
        };

        var response = await client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(json);
        return ParseAIResponse(openAIResponse!.choices[0].message.content);
    }

    private AIAnalysisResult ParseAIResponse(string text)
    {
        var clean = text.Trim()
            .TrimStart('`').TrimStart('j').TrimStart('s').TrimStart('o').TrimStart('n')
            .Trim('`').Trim();

        if (clean.StartsWith("```json")) clean = clean[7..];
        if (clean.StartsWith("```")) clean = clean[3..];
        if (clean.EndsWith("```")) clean = clean[..^3];
        clean = clean.Trim();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = JsonSerializer.Deserialize<AIAnalysisResult>(clean, options)!;

        var validCategories = new[] { "enquiry", "site_visit", "invoice",
            "quote", "showroom", "complaint", "general" };

        if (!validCategories.Contains(result.Category))
            result.Category = "general";

        return result;
    }

    private bool IsJunkEmail(string fromEmail, string subject, string body)
    {
        if (fromEmail.Contains("xero.com") || fromEmail.Contains("noreply") ||
            fromEmail.Contains("no-reply") || fromEmail.Contains("donotreply"))
            return true;

        var crmPatterns = new[] { "a new contact was added", "contact was updated in the crm",
            "generic domain so no account was found", "go and review for accuracy" };

        var combined = $"{subject} {body}";
        return crmPatterns.Any(p => combined.Contains(p));
    }

    private Dictionary<string, object> ExtractDataFromEmail(IncomingEmail email, string category)
    {
        var data = new Dictionary<string, object>
        {
            { "fromName", email.FromName ?? "" },
            { "fromEmail", email.FromEmail ?? "" },
            { "subject", email.Subject ?? "" }
        };

        var phone = ExtractPhoneNumber(email.BodyContent);
        if (!string.IsNullOrEmpty(phone)) data["phone"] = phone;

        var companyNumber = ExtractCompanyNumber(email.BodyContent);
        if (!string.IsNullOrEmpty(companyNumber)) data["companyNumber"] = companyNumber;

        if (category == "enquiry")
        {
            var dimensions = ExtractDimensions(email.BodyContent);
            if (!string.IsNullOrEmpty(dimensions)) data["dimensions"] = dimensions;
        }

        return data;
    }

    private Dictionary<string, object> MergeExtractedData(Dictionary<string, object> aiData, Dictionary<string, object> rulesData)
    {
        var merged = new Dictionary<string, object>(aiData);
        foreach (var kvp in rulesData)
            if (!merged.ContainsKey(kvp.Key))
                merged[kvp.Key] = kvp.Value;
        return merged;
    }

    private string? ExtractPhoneNumber(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;
        var match = Regex.Match(text, @"(\+353|0)[\s-]?(\d{2})[\s-]?(\d{3,4})[\s-]?(\d{4})");
        return match.Success ? match.Value : null;
    }

    private string? ExtractCompanyNumber(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;
        var match = Regex.Match(text, @"company\s*number[:\s]*([A-Z0-9]+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private string? ExtractDimensions(string? text)
    {
        if (string.IsNullOrEmpty(text)) return null;
        var match = Regex.Match(text, @"\d+\s*(?:m|cm)\s*(?:x|by)\s*\d+\s*(?:m|cm)", RegexOptions.IgnoreCase);
        return match.Success ? match.Value : null;
    }

    private string MapCategoryToTaskType(string category) => category switch
    {
        "enquiry" => "enquiry",
        "site_visit" => "site_visit",
        "invoice" => "invoice",
        "quote" => "quote",
        "showroom" => "showroom",
        "complaint" => "complaint",
        _ => "general"
    };

    private List<string> DetermineRequiredActions(string category) => category switch
    {
        "enquiry" => new List<string> { "check_customer", "create_workflow", "create_task" },
        "site_visit" => new List<string> { "create_task" },
        "invoice" => new List<string> { "create_task" },
        "junk" => new List<string> { "mark_as_junk" },
        _ => new List<string> { "create_task" }
    };

    private class AIAnalysisResult
    {
        public string Category { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Priority { get; set; } = "Normal";
        public string Sentiment { get; set; } = "Neutral";
        public bool NeedsReply { get; set; }
        public string Reasoning { get; set; } = string.Empty;
        public Dictionary<string, object> ExtractedData { get; set; } = new();
    }

    private class ClaudeResponse
    {
        public Content[] content { get; set; } = Array.Empty<Content>();
    }

    private class Content
    {
        public string text { get; set; } = string.Empty;
    }

    private class OpenAIResponse
    {
        public Choice[] choices { get; set; } = Array.Empty<Choice>();
    }

    private class Choice
    {
        public Message message { get; set; } = new();
    }

    private class Message
    {
        public string content { get; set; } = string.Empty;
    }
}
