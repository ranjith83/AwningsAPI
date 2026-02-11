using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    public class EmailAnalysisService : IEmailAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailAnalysisService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _apiEndpoint;
        private readonly string _aiProvider;
        private readonly string _model;

        public EmailAnalysisService(
            HttpClient httpClient,
            ILogger<EmailAnalysisService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;

            // Determine which AI provider to use
            _aiProvider = configuration["AI:Provider"] ?? "Claude";

            switch (_aiProvider.ToLower())
            {
                case "azureopenai":
                    _apiKey = configuration["AzureOpenAI:ApiKey"] ?? throw new Exception("Azure OpenAI API key not configured");
                    _apiEndpoint = configuration["AzureOpenAI:Endpoint"] ?? throw new Exception("Azure OpenAI endpoint not configured");
                    var deploymentName = configuration["AzureOpenAI:DeploymentName"] ?? "gpt-4";
                    var apiVersion = configuration["AzureOpenAI:ApiVersion"] ?? "2024-02-15-preview";
                    _apiEndpoint = $"{_apiEndpoint.TrimEnd('/')}/openai/deployments/{deploymentName}/chat/completions?api-version={apiVersion}";
                    _model = deploymentName;
                    _logger.LogInformation("Using Azure OpenAI provider");
                    break;

                case "openai":
                    _apiKey = configuration["OpenAI:ApiKey"] ?? throw new Exception("OpenAI API key not configured");
                    _apiEndpoint = "https://api.openai.com/v1/chat/completions";
                    _model = configuration["OpenAI:Model"] ?? "gpt-4o";
                    _logger.LogInformation("Using OpenAI provider");
                    break;

                case "claude":
                default:
                    _apiKey = configuration["Claude:ApiKey"] ?? throw new Exception("Claude API key not configured");
                    _apiEndpoint = "https://api.anthropic.com/v1/messages";
                    _model = "claude-sonnet-4-20250514";
                    _logger.LogInformation("Using Claude API provider");
                    break;
            }
        }

        public async Task<EmailAnalysisResult> AnalyzeEmailAsync(IncomingEmail email)
        {
            try
            {
                _logger.LogInformation($"Analyzing email: {email.Subject} using {_aiProvider}");

                // Prepare the email content for AI analysis
                var emailContent = PrepareEmailContent(email);

                // Call appropriate AI API
                var analysisResult = _aiProvider.ToLower() switch
                {
                    "claude" => await CallClaudeAPIAsync(emailContent, email),
                    "azureopenai" => await CallOpenAIAPIAsync(emailContent, email, isAzure: true),
                    "openai" => await CallOpenAIAPIAsync(emailContent, email, isAzure: false),
                    _ => throw new Exception($"Unknown AI provider: {_aiProvider}")
                };

                _logger.LogInformation($"Email categorized as: {analysisResult.Category} (confidence: {analysisResult.Confidence:P})");

                return analysisResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing email with AI");

                // Return a fallback result
                return new EmailAnalysisResult
                {
                    Category = EmailCategory.Unknown,
                    Confidence = 0.0,
                    Reasoning = $"Error during analysis: {ex.Message}",
                    Priority = "Normal"
                };
            }
        }

        private string PrepareEmailContent(IncomingEmail email)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Subject: {email.Subject}");
            sb.AppendLine($"From: {email.FromName} <{email.FromEmail}>");
            sb.AppendLine($"Received: {email.ReceivedDateTime:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Importance: {email.Importance}");
            sb.AppendLine();

            // Clean HTML if necessary
            var bodyText = email.IsHtml ? StripHtml(email.BodyContent) : email.BodyContent;
            sb.AppendLine("Body:");
            sb.AppendLine(bodyText);

            if (email.HasAttachments && email.Attachments.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Attachments:");
                foreach (var attachment in email.Attachments)
                {
                    sb.AppendLine($"- {attachment.FileName} ({attachment.ContentType}, {attachment.Size} bytes)");

                    // Include extracted text if available
                    if (!string.IsNullOrEmpty(attachment.ExtractedText))
                    {
                        sb.AppendLine($"  Content preview: {attachment.ExtractedText.Substring(0, Math.Min(500, attachment.ExtractedText.Length))}...");
                    }
                }
            }

            return sb.ToString();
        }

        private async Task<EmailAnalysisResult> CallOpenAIAPIAsync(string emailContent, IncomingEmail email, bool isAzure)
        {
            try
            {
                var systemPrompt = GetSystemPrompt();
                var userPrompt = $@"Analyze the following email and categorize it:

{emailContent}

Please respond with a JSON object containing:
- category: one of [invoice_creation, quote_creation, customer_creation, showroom_booking, product_inquiry, complaint, general_inquiry]
- confidence: a number between 0 and 1
- reasoning: brief explanation of why you chose this category
- extractedData: object with relevant fields extracted from the email (e.g., customer name, amounts, product details)
- requiredActions: array of strings describing what needs to be done
- priority: one of [Low, Normal, High, Urgent]";

                var requestBody = new
                {
                    model = _model,
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userPrompt }
                    },
                    temperature = 0.3,
                    response_format = new { type = "json_object" }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();

                if (isAzure)
                {
                    _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                }

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"OpenAI API error: {response.StatusCode} - {responseContent}");
                    throw new Exception($"OpenAI API returned {response.StatusCode}");
                }

                var apiResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseContent);

                if (apiResponse?.Choices == null || !apiResponse.Choices.Any())
                {
                    throw new Exception("Empty response from OpenAI API");
                }

                var messageContent = apiResponse.Choices.First().Message?.Content;

                if (string.IsNullOrEmpty(messageContent))
                {
                    throw new Exception("No content in OpenAI response");
                }

                // Parse JSON response
                var result = JsonConvert.DeserializeObject<EmailAnalysisResult>(messageContent);

                if (result == null)
                {
                    throw new Exception("Failed to parse OpenAI response");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI API");
                throw;
            }
        }

        private async Task<EmailAnalysisResult> CallClaudeAPIAsync(string emailContent, IncomingEmail email)
        {
            try
            {
                var systemPrompt = GetSystemPrompt();
                var userPrompt = $@"Analyze the following email and categorize it:

{emailContent}

Please respond with a JSON object containing:
- category: one of [invoice_creation, quote_creation, customer_creation, showroom_booking, product_inquiry, complaint, general_inquiry]
- confidence: a number between 0 and 1
- reasoning: brief explanation of why you chose this category
- extractedData: object with relevant fields extracted from the email (e.g., customer name, amounts, product details)
- requiredActions: array of strings describing what needs to be done
- priority: one of [Low, Normal, High, Urgent]";

                var requestBody = new
                {
                    model = _model,
                    max_tokens = 2000,
                    system = systemPrompt,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = userPrompt
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Claude API error: {response.StatusCode} - {responseContent}");
                    throw new Exception($"Claude API returned {response.StatusCode}");
                }

                var apiResponse = JsonConvert.DeserializeObject<ClaudeResponse>(responseContent);

                if (apiResponse?.Content == null || !apiResponse.Content.Any())
                {
                    throw new Exception("Empty response from Claude API");
                }

                var textContent = apiResponse.Content.First().Text;

                // Extract JSON from the response (Claude might wrap it in markdown)
                var jsonMatch = Regex.Match(textContent, @"```json\s*(\{[\s\S]*?\})\s*```");
                var jsonString = jsonMatch.Success ? jsonMatch.Groups[1].Value : textContent;

                // Try to parse as JSON
                var result = JsonConvert.DeserializeObject<EmailAnalysisResult>(jsonString);

                if (result == null)
                {
                    throw new Exception("Failed to parse AI response");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Claude API");
                throw;
            }
        }

        private string GetSystemPrompt()
        {
            return @"You are an intelligent email classification system for an awnings and outdoor products business. 

Your job is to analyze incoming emails and categorize them accurately based on the customer's intent and the content of the message.

Categories you can assign:
1. invoice_creation - Customer is requesting an invoice, asking about billing, or needs an invoice sent
2. quote_creation - Customer is requesting a price quote, asking for pricing, or needs a quotation
3. customer_creation - New customer inquiry, someone asking to become a customer, or registration request
4. showroom_booking - Customer wants to schedule a showroom visit, book an appointment, or view products in person
5. product_inquiry - Questions about specific products, features, specifications, or availability
6. complaint - Customer complaints, issues with service, product problems, or dissatisfaction
7. general_inquiry - General questions that don't fit other categories

For each email, extract relevant structured data such as:
- Customer name
- Contact information
- Product names/types mentioned
- Quantities
- Amounts/prices mentioned
- Dates mentioned
- Urgency indicators

Determine the priority level based on:
- Urgent: Explicit urgent requests, complaints, time-sensitive matters
- High: Quote requests, invoice requests, showroom bookings
- Normal: Product inquiries, general questions
- Low: Thank you emails, acknowledgments

Be accurate and confident in your categorization. If an email is ambiguous, use context clues and make your best judgment.

Always respond with valid JSON only, no additional text or markdown formatting.";
        }

        public async Task<string> ExtractTextFromPdfAsync(byte[] pdfContent)
        {
            try
            {
                _logger.LogInformation($"Extracting text from PDF using {_aiProvider}");

                if (_aiProvider.ToLower() == "claude")
                {
                    return await ExtractTextFromPdfClaudeAsync(pdfContent);
                }
                else
                {
                    // For OpenAI, we would need to use a different approach
                    // OpenAI doesn't support PDF directly, so we'd need to convert to text first
                    _logger.LogWarning("PDF extraction with OpenAI not implemented. Consider using Claude or a dedicated PDF extraction library.");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from PDF");
                return string.Empty;
            }
        }

        private async Task<string> ExtractTextFromPdfClaudeAsync(byte[] pdfContent)
        {
            try
            {
                var base64Pdf = Convert.ToBase64String(pdfContent);

                var requestBody = new
                {
                    model = "claude-sonnet-4-20250514",
                    max_tokens = 4000,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new
                                {
                                    type = "document",
                                    source = new
                                    {
                                        type = "base64",
                                        media_type = "application/pdf",
                                        data = base64Pdf
                                    }
                                },
                                new
                                {
                                    type = "text",
                                    text = "Please extract and return all text content from this PDF document. Focus on key information like amounts, dates, names, and product details."
                                }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

                var response = await _httpClient.PostAsync("https://api.anthropic.com/v1/messages", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Claude API error: {response.StatusCode}");
                    return string.Empty;
                }

                var apiResponse = JsonConvert.DeserializeObject<ClaudeResponse>(responseContent);
                return apiResponse?.Content?.FirstOrDefault()?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from PDF with Claude");
                return string.Empty;
            }
        }

        public async Task<string> ExtractTextFromImageAsync(byte[] imageContent)
        {
            try
            {
                _logger.LogInformation($"Extracting text from image using {_aiProvider}");

                if (_aiProvider.ToLower() == "claude")
                {
                    return await ExtractTextFromImageClaudeAsync(imageContent);
                }
                else if (_aiProvider.ToLower() == "openai" || _aiProvider.ToLower() == "azureopenai")
                {
                    return await ExtractTextFromImageOpenAIAsync(imageContent);
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from image");
                return string.Empty;
            }
        }

        private async Task<string> ExtractTextFromImageClaudeAsync(byte[] imageContent)
        {
            try
            {
                var base64Image = Convert.ToBase64String(imageContent);

                // Detect image type
                var mediaType = "image/jpeg";
                if (imageContent.Length > 2)
                {
                    if (imageContent[0] == 0x89 && imageContent[1] == 0x50) mediaType = "image/png";
                    else if (imageContent[0] == 0xFF && imageContent[1] == 0xD8) mediaType = "image/jpeg";
                }

                var requestBody = new
                {
                    model = "claude-sonnet-4-20250514",
                    max_tokens = 2000,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new
                                {
                                    type = "image",
                                    source = new
                                    {
                                        type = "base64",
                                        media_type = mediaType,
                                        data = base64Image
                                    }
                                },
                                new
                                {
                                    type = "text",
                                    text = "Please extract all visible text from this image. Include any numbers, names, dates, or other important information."
                                }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

                var response = await _httpClient.PostAsync("https://api.anthropic.com/v1/messages", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Claude API error: {response.StatusCode}");
                    return string.Empty;
                }

                var apiResponse = JsonConvert.DeserializeObject<ClaudeResponse>(responseContent);
                return apiResponse?.Content?.FirstOrDefault()?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from image with Claude");
                return string.Empty;
            }
        }

        private async Task<string> ExtractTextFromImageOpenAIAsync(byte[] imageContent)
        {
            try
            {
                var base64Image = Convert.ToBase64String(imageContent);

                // Detect image type
                var mediaType = "image/jpeg";
                if (imageContent.Length > 2)
                {
                    if (imageContent[0] == 0x89 && imageContent[1] == 0x50) mediaType = "image/png";
                    else if (imageContent[0] == 0xFF && imageContent[1] == 0xD8) mediaType = "image/jpeg";
                }

                var requestBody = new
                {
                    model = _model,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new
                                {
                                    type = "text",
                                    text = "Please extract all visible text from this image. Include any numbers, names, dates, or other important information."
                                },
                                new
                                {
                                    type = "image_url",
                                    image_url = new
                                    {
                                        url = $"data:{mediaType};base64,{base64Image}"
                                    }
                                }
                            }
                        }
                    },
                    max_tokens = 2000
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();

                if (_aiProvider.ToLower() == "azureopenai")
                {
                    _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                }

                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"OpenAI API error: {response.StatusCode}");
                    return string.Empty;
                }

                var apiResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseContent);
                return apiResponse?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting text from image with OpenAI");
                return string.Empty;
            }
        }

        private string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            // Remove HTML tags
            var text = Regex.Replace(html, "<.*?>", string.Empty);

            // Decode HTML entities
            text = System.Net.WebUtility.HtmlDecode(text);

            // Remove extra whitespace
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }

        // API response models
        private class ClaudeResponse
        {
            [JsonProperty("content")]
            public List<ContentBlock>? Content { get; set; }
        }

        private class ContentBlock
        {
            [JsonProperty("type")]
            public string? Type { get; set; }

            [JsonProperty("text")]
            public string? Text { get; set; }
        }

        private class OpenAIResponse
        {
            [JsonProperty("choices")]
            public List<OpenAIChoice>? Choices { get; set; }
        }

        private class OpenAIChoice
        {
            [JsonProperty("message")]
            public OpenAIMessage? Message { get; set; }
        }

        private class OpenAIMessage
        {
            [JsonProperty("content")]
            public string? Content { get; set; }
        }
    }
}
