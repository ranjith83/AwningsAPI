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

        _logger.LogInformation("AI ANALYSIS: Category={Category}, Confidence={Confidence:F2}", aiAnalysis.Category, aiAnalysis.Confidence);
        return result;
    }

    private async Task<AIAnalysisResult> AnalyzeEmailWithAIAsync(string subject, string body, string fromEmail)
    {
        try
        {
            var aiProvider = _configuration["AI:Provider"] ?? "Claude";
            var apiKey = _configuration[$"{aiProvider}:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("AI API key not configured, falling back to rules-based categorization");
                return FallbackCategorization(subject, body);
            }

            var prompt = BuildPrompt(subject, body, fromEmail);
            return aiProvider == "Claude"
                ? await CallClaudeAsync(prompt, apiKey)
                : await CallOpenAIAsync(prompt, apiKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AI provider, falling back to rules-based");
            return FallbackCategorization(subject, body);
        }
    }

    private string BuildPrompt(string subject, string body, string fromEmail) => $@"You are an expert email categorization system for an awnings/pergola company.

Analyze this email and categorize it into ONE of these categories:
1. enquiry - New customer enquiries about products, pricing, or general information
2. site_visit - Requests for site visits, measurements, or meetings
3. invoice - Payment reminders or invoice notifications
4. quote - Specific quote requests with specifications
5. showroom - Showroom visit requests
6. complaint - Customer complaints or issues
7. general - Everything else

Email:
Subject: {subject}
From: {fromEmail}
Body: {body}

Respond ONLY with valid JSON:
{{
  ""category"": ""enquiry"",
  ""confidence"": 0.85,
  ""priority"": ""Normal"",
  ""sentiment"": ""Neutral"",
  ""reasoning"": ""Brief explanation"",
  ""extractedData"": {{}}
}}

Rules:
- category must be one of the 7 listed
- confidence: 0.0 to 1.0
- priority: ""Low"", ""Normal"", ""High"", or ""Urgent""
- sentiment: ""Positive"", ""Neutral"", ""Negative"", or ""Urgent""";

    private async Task<AIAnalysisResult> CallClaudeAsync(string prompt, string apiKey)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var requestBody = new
        {
            model = "claude-3-5-sonnet-20241022",
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

    private AIAnalysisResult FallbackCategorization(string subject, string body)
    {
        var combined = $"{subject} {body}".ToLower();

        if (combined.Contains("site visit") || combined.Contains("site survey"))
            return new AIAnalysisResult { Category = "site_visit", Confidence = 0.75, Priority = "High", Sentiment = "Neutral", Reasoning = "Contains site visit keywords (fallback)", ExtractedData = new() };

        if (combined.Contains("invoice") || combined.Contains("payment due"))
            return new AIAnalysisResult { Category = "invoice", Confidence = 0.75, Priority = "High", Sentiment = "Neutral", Reasoning = "Contains invoice/payment keywords (fallback)", ExtractedData = new() };

        var enquiryKeywords = new[] { "enquiry", "looking for", "interested in", "awning", "pergola", "shade" };
        if (enquiryKeywords.Count(k => combined.Contains(k)) >= 2)
            return new AIAnalysisResult { Category = "enquiry", Confidence = 0.70, Priority = "Normal", Sentiment = "Neutral", Reasoning = "Contains enquiry keywords (fallback)", ExtractedData = new() };

        return new AIAnalysisResult { Category = "general", Confidence = 0.60, Priority = "Normal", Sentiment = "Neutral", Reasoning = "Default categorization (fallback)", ExtractedData = new() };
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
