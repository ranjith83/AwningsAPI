using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Net.Http;
using System.Text;

namespace AwningsAPI.Services.Email
{
    /// <summary>
    /// Email Analysis Service with AI Provider
    /// Uses AI (Claude/OpenAI) for intelligent categorization
    /// Rules-based detection for junk emails
    /// </summary>
    public class EmailAnalysisService : IEmailAnalysisService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmailAnalysisService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailAnalysisService(
            AppDbContext context,
            ILogger<EmailAnalysisService> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        #region IEmailAnalysisService Implementation

        public async Task<EmailAnalysisResult> AnalyzeEmailAsync(IncomingEmail email)
        {
            try
            {
                var subject = email.Subject ?? "";
                var body = email.BodyContent ?? "";
                var fromEmail = email.FromEmail?.ToLower() ?? "";

                // Initialize result
                var result = new EmailAnalysisResult
                {
                    ExtractedData = new Dictionary<string, object>(),
                    RequiredActions = new List<string>(),
                    CustomerInfo = new Dictionary<string, string>(),
                    Warnings = new List<string>(),
                    CompanyNumber = "",
                    Sentiment = "Neutral"
                };

                // 1. CHECK IF JUNK FIRST (Rules-based, fast)
                if (IsJunkEmail(fromEmail, subject.ToLower(), body.ToLower()))
                {
                    result.Category = "junk";
                    result.TaskType = "junk";
                    result.Priority = "Low";
                    result.Confidence = 0.95;
                    result.Reasoning = "Automated system email: CRM contact update or Xero notification";
                    result.IsSpam = true;
                    result.RequiredActions = new List<string> { "mark_as_junk", "move_to_folder" };

                    _logger.LogInformation($"JUNK EMAIL (Rules): {email.Subject} from {email.FromEmail}");
                    return result;
                }

                // 2. USE AI PROVIDER FOR INTELLIGENT CATEGORIZATION ✨
                var aiAnalysis = await AnalyzeEmailWithAIAsync(subject, body, fromEmail);

                // 3. Extract additional data based on AI category
                var extractedData = ExtractDataFromEmail(email, aiAnalysis.Category);

                // 4. Build customer info
                result.CustomerInfo = new Dictionary<string, string>
                {
                    { "fromName", email.FromName ?? "" },
                    { "fromEmail", email.FromEmail ?? "" },
                    { "phone", ExtractPhoneNumber(body) ?? "" },
                    { "companyNumber", ExtractCompanyNumber(body) ?? "" }
                };

                // 5. Build final result using AI analysis
                result.Category = aiAnalysis.Category;
                result.TaskType = MapCategoryToTaskType(aiAnalysis.Category);
                result.Priority = aiAnalysis.Priority;
                result.Confidence = aiAnalysis.Confidence;
                result.Reasoning = aiAnalysis.Reasoning;
                result.ExtractedData = MergeExtractedData(aiAnalysis.ExtractedData, extractedData);
                result.CompanyNumber = ExtractCompanyNumber(body) ?? "";
                result.RequiredActions = DetermineRequiredActions(aiAnalysis.Category);
                result.IsSpam = false;
                result.Sentiment = aiAnalysis.Sentiment;

                _logger.LogInformation($"AI ANALYSIS: Category={aiAnalysis.Category}, Confidence={aiAnalysis.Confidence:F2}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error analyzing email {email.EmailId}");
                throw;
            }
        }

        public Task<string> ExtractTextFromImageAsync(byte[] imageContent)
        {
            _logger.LogInformation("OCR extraction requested (not yet implemented)");
            return Task.FromResult("");
        }

        public Task<string> ExtractTextFromPdfAsync(byte[] pdfContent)
        {
            _logger.LogInformation("PDF extraction requested (not yet implemented)");
            return Task.FromResult("");
        }

        #endregion

        #region AI Provider Integration

        /// <summary>
        /// Use AI (Claude/OpenAI) to intelligently categorize email
        /// </summary>
        private async Task<AIAnalysisResult> AnalyzeEmailWithAIAsync(string subject, string body, string fromEmail)
        {
            try
            {
                // Get AI provider settings
                var aiProvider = _configuration["AI:Provider"] ?? "Claude"; // Claude or OpenAI
                var apiKey = _configuration[$"{aiProvider}:ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogWarning("AI API key not configured, falling back to rules-based categorization");
                    return FallbackToDeterministicCategorization(subject, body);
                }

                // Build AI prompt
                var prompt = BuildCategorizationPrompt(subject, body, fromEmail);

                AIAnalysisResult aiResult;

                if (aiProvider == "Claude")
                {
                    aiResult = await CallClaudeAPIAsync(prompt, apiKey);
                }
                else // OpenAI
                {
                    aiResult = await CallOpenAIAPIAsync(prompt, apiKey);
                }

                return aiResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling AI provider, falling back to rules-based");
                return FallbackToDeterministicCategorization(subject, body);
            }
        }

        private string BuildCategorizationPrompt(string subject, string body, string fromEmail)
        {
            return $@"You are an expert email categorization system for an awnings/pergola company.

Analyze this email and categorize it into ONE of these categories:

**Categories:**
1. **initial_enquiry** - New customer enquiries about products, pricing, or general information
   - Looking for awnings, pergolas, canopies, shade solutions
   - Asking about products, models, prices
   - Includes dimensions or specific requirements
   - Example: ""Looking for 3x3m pergola for patio""

2. **site_visit_meeting** - Requests for site visits, measurements, or meetings
   - Keywords: site visit, site survey, visual inspection, meeting, appointment
   - Scheduling requests
   - Example: ""Can we schedule a site visit next Tuesday?""

3. **invoice_due** - Payment reminders or invoice notifications
   - Keywords: invoice due, payment due, overdue, payment reminder
   - Example: ""Invoice #INV-001 due on 15/02/2024""

4. **quote_creation** - Specific quote requests
   - Direct quote requests with specifications
   - Example: ""Please provide quote for 5.2m awning""

5. **showroom_booking** - Showroom visit requests
   - Want to see samples, visit showroom
   - Example: ""Can I visit your showroom to see samples?""

6. **complaint** - Customer complaints or issues
   - Problems with products or service
   - Example: ""The awning is not working properly""

7. **general_inquiry** - Everything else

**Email to analyze:**
Subject: {subject}
From: {fromEmail}
Body: {body}

**Respond ONLY with valid JSON in this exact format:**
{{
  ""category"": ""initial_enquiry"",
  ""confidence"": 0.85,
  ""priority"": ""Normal"",
  ""sentiment"": ""Neutral"",
  ""reasoning"": ""Email contains enquiry keywords and product dimensions"",
  ""extractedData"": {{
    ""productType"": ""pergola"",
    ""dimensions"": ""3x3m"",
    ""urgency"": ""normal""
  }}
}}

**Rules:**
- category must be one of: initial_enquiry, site_visit_meeting, invoice_due, quote_creation, showroom_booking, complaint, general_inquiry
- confidence: 0.0 to 1.0
- priority: ""Low"", ""Normal"", ""High"", or ""Urgent""
- sentiment: ""Positive"", ""Neutral"", ""Negative"", or ""Urgent""
- reasoning: Brief explanation (1 sentence)
- extractedData: Any relevant structured data found

Respond ONLY with the JSON object, no other text.";
        }

        private async Task<AIAnalysisResult> CallClaudeAPIAsync(string prompt, string apiKey)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

            var requestBody = new
            {
                model = "claude-3-5-sonnet-20241022",
                max_tokens = 1024,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.anthropic.com/v1/messages", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var claudeResponse = JsonSerializer.Deserialize<ClaudeResponse>(responseJson);

            // Extract JSON from Claude's response
            var aiResponseText = claudeResponse.content[0].text;

            // Parse the JSON response
            return ParseAIResponse(aiResponseText);
        }

        private async Task<AIAnalysisResult> CallOpenAIAPIAsync(string prompt, string apiKey)
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

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

            var aiResponseText = openAIResponse.choices[0].message.content;

            return ParseAIResponse(aiResponseText);
        }

        private AIAnalysisResult ParseAIResponse(string aiResponseText)
        {
            try
            {
                // Clean up response - remove markdown code blocks if present
                var cleanJson = aiResponseText.Trim();
                if (cleanJson.StartsWith("```json"))
                {
                    cleanJson = cleanJson.Substring(7);
                }
                if (cleanJson.StartsWith("```"))
                {
                    cleanJson = cleanJson.Substring(3);
                }
                if (cleanJson.EndsWith("```"))
                {
                    cleanJson = cleanJson.Substring(0, cleanJson.Length - 3);
                }
                cleanJson = cleanJson.Trim();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<AIAnalysisResult>(cleanJson, options);

                // Validate category
                var validCategories = new[] { "initial_enquiry", "site_visit_meeting", "invoice_due",
                    "quote_creation", "showroom_booking", "complaint", "general_inquiry" };

                if (!validCategories.Contains(result.Category))
                {
                    _logger.LogWarning($"AI returned invalid category: {result.Category}, defaulting to general_inquiry");
                    result.Category = "general_inquiry";
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error parsing AI response: {aiResponseText}");
                throw;
            }
        }

        private AIAnalysisResult FallbackToDeterministicCategorization(string subject, string body)
        {
            var combined = $"{subject} {body}".ToLower();

            // Fallback to rules-based categorization
            if (combined.Contains("site visit") || combined.Contains("site survey") || combined.Contains("visual"))
                return new AIAnalysisResult
                {
                    Category = "site_visit_meeting",
                    Confidence = 0.75,
                    Priority = "High",
                    Sentiment = "Neutral",
                    Reasoning = "Contains site visit keywords (fallback detection)",
                    ExtractedData = new Dictionary<string, object>()
                };

            if (combined.Contains("invoice") || combined.Contains("payment due"))
                return new AIAnalysisResult
                {
                    Category = "invoice_due",
                    Confidence = 0.75,
                    Priority = "High",
                    Sentiment = "Neutral",
                    Reasoning = "Contains invoice/payment keywords (fallback detection)",
                    ExtractedData = new Dictionary<string, object>()
                };

            var enquiryKeywords = new[] { "enquiry", "looking for", "interested in", "awning", "pergola", "shade" };
            if (enquiryKeywords.Count(k => combined.Contains(k)) >= 2)
                return new AIAnalysisResult
                {
                    Category = "initial_enquiry",
                    Confidence = 0.70,
                    Priority = "Normal",
                    Sentiment = "Neutral",
                    Reasoning = "Contains enquiry keywords (fallback detection)",
                    ExtractedData = new Dictionary<string, object>()
                };

            return new AIAnalysisResult
            {
                Category = "general_inquiry",
                Confidence = 0.60,
                Priority = "Normal",
                Sentiment = "Neutral",
                Reasoning = "Default categorization (fallback)",
                ExtractedData = new Dictionary<string, object>()
            };
        }

        #endregion

        #region Junk Email Detection (Rules-Based)

        /// <summary>
        /// Rules-based junk detection - Fast and deterministic
        /// Detects: CRM notifications, Xero emails, automated systems
        /// </summary>
        private bool IsJunkEmail(string fromEmail, string subject, string body)
        {
            // XERO EMAILS
            if (fromEmail.Contains("xero.com") ||
                fromEmail.Contains("@xero") ||
                fromEmail.Contains("messaging-service@post.xero") || fromEmail.Contains("noreply"))
            {
                return true;
            }

            // CRM CONTACT UPDATE NOTIFICATIONS
            var crmPatterns = new[]
            {
                "a new contact was added",
                "contact was added or updated",
                "contact was updated in the crm",
                "new contact was added in",
                "contact name:",
                "go and review for accuracy",
                "generic domain so no account was found",
                "their email was from a generic domain"
            };

            var combined = $"{subject} {body}";
            foreach (var pattern in crmPatterns)
            {
                if (combined.Contains(pattern))
                    return true;
            }

            // QUOTE ACCEPTED FROM XERO
            if (combined.Contains("has accepted quote") && fromEmail.Contains("xero"))
                return true;

            // AUTOMATED SYSTEM EMAILS
            var systemPatterns = new[] { "noreply@", "no-reply@", "donotreply@", "do-not-reply@" };
            foreach (var pattern in systemPatterns)
            {
                if (fromEmail.Contains(pattern))
                    return true;
            }

            return false;
        }

        #endregion

        #region Data Extraction (Rules-Based)

        private Dictionary<string, object> ExtractDataFromEmail(IncomingEmail email, string category)
        {
            var data = new Dictionary<string, object>
            {
                { "fromName", email.FromName ?? "" },
                { "fromEmail", email.FromEmail ?? "" },
                { "subject", email.Subject ?? "" }
            };

            var phone = ExtractPhoneNumber(email.BodyContent);
            if (!string.IsNullOrEmpty(phone))
                data["phone"] = phone;

            var companyNumber = ExtractCompanyNumber(email.BodyContent);
            if (!string.IsNullOrEmpty(companyNumber))
                data["companyNumber"] = companyNumber;

            // Category-specific extraction
            switch (category)
            {
                case "initial_enquiry":
                    var dimensions = ExtractDimensions(email.BodyContent);
                    if (!string.IsNullOrEmpty(dimensions))
                        data["dimensions"] = dimensions;
                    break;

                case "site_visit_meeting":
                    var date = ExtractPreferredDate(email.BodyContent);
                    if (!string.IsNullOrEmpty(date))
                        data["preferredDate"] = date;
                    break;

                case "invoice_due":
                    var invoiceNum = ExtractInvoiceNumber(email.BodyContent);
                    if (!string.IsNullOrEmpty(invoiceNum))
                        data["invoiceNumber"] = invoiceNum;

                    var amount = ExtractAmount(email.BodyContent);
                    if (amount.HasValue)
                        data["amount"] = amount.Value;
                    break;
            }

            return data;
        }

        private Dictionary<string, object> MergeExtractedData(
            Dictionary<string, object> aiData,
            Dictionary<string, object> rulesData)
        {
            var merged = new Dictionary<string, object>(aiData);

            foreach (var kvp in rulesData)
            {
                if (!merged.ContainsKey(kvp.Key))
                    merged[kvp.Key] = kvp.Value;
            }

            return merged;
        }

        private string ExtractCompanyNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"company\s*number[:\s]*([A-Z0-9]+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        private string ExtractPhoneNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"(\+353|0)[\s-]?(\d{2})[\s-]?(\d{3,4})[\s-]?(\d{4})");
            return match.Success ? match.Value : null;
        }

        private string ExtractDimensions(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"\d+\s*(?:m|cm)\s*(?:x|by)\s*\d+\s*(?:m|cm)", RegexOptions.IgnoreCase);
            return match.Success ? match.Value : null;
        }

        private string ExtractPreferredDate(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"between\s+(\d{1,2}:\d{2})\s+and\s+(\d{1,2}:\d{2})", RegexOptions.IgnoreCase);
            return match.Success ? match.Value : null;
        }

        private string ExtractInvoiceNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"invoice\s*#?\s*(?:no\.?)?(\d+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        private decimal? ExtractAmount(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            var match = Regex.Match(text, @"[€£$]\s*(\d+(?:,\d{3})*(?:\.\d{2})?)");
            if (match.Success && decimal.TryParse(match.Groups[1].Value.Replace(",", ""), out var value))
                return value;
            return null;
        }

        #endregion

        #region Helper Methods

        private string MapCategoryToTaskType(string category)
        {
            return category switch
            {
                "initial_enquiry" => "initial_enquiry",
                "site_visit_meeting" => "site_visit",
                "invoice_due" => "payment_followup",
                "quote_creation" => "quote_creation",
                "showroom_booking" => "showroom_booking",
                "complaint" => "complaint",
                _ => "general_inquiry"
            };
        }

        private List<string> DetermineRequiredActions(string category)
        {
            return category switch
            {
                "initial_enquiry" => new List<string> { "check_customer", "create_workflow", "create_enquiry", "create_task" },
                "site_visit_meeting" => new List<string> { "create_audit_log", "create_task" },
                "invoice_due" => new List<string> { "create_audit_log", "create_task" },
                "junk" => new List<string> { "mark_as_junk" },
                _ => new List<string> { "create_task" }
            };
        }

        #endregion

        #region AI Response Models

        private class AIAnalysisResult
        {
            public string Category { get; set; }
            public double Confidence { get; set; }
            public string Priority { get; set; }
            public string Sentiment { get; set; }
            public string Reasoning { get; set; }
            public Dictionary<string, object> ExtractedData { get; set; } = new();
        }

        private class ClaudeResponse
        {
            public Content[] content { get; set; }
        }

        private class Content
        {
            public string text { get; set; }
        }

        private class OpenAIResponse
        {
            public Choice[] choices { get; set; }
        }

        private class Choice
        {
            public Message message { get; set; }
        }

        private class Message
        {
            public string content { get; set; }
        }

        #endregion
    }
}