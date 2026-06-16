using Anthropic;
using Anthropic.Models.Messages;
using AwningsAPI.Database;
using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.LeadImport;
using AwningsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Text.Json;
using GraphMessage = Microsoft.Graph.Models.Message;
using GraphMailFolder = Microsoft.Graph.Models.MailFolder;

namespace AwningsAPI.Services.LeadImport
{
    public class LeadImportService : ILeadImportService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly ICustomerService _customerService;
        private readonly AppDbContext _context;
        private readonly AnthropicClient _anthropicClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LeadImportService> _logger;

        private const string ProcessedFolderName = "Processed";

        private const string ExtractionSystemPrompt =
            "You are a data extraction assistant for Awnings of Ireland. " +
            "You receive emails that are web-form submissions — either from HubSpot or from the company website contact form. " +
            "Extract the customer contact details and return ONLY a valid JSON object (no markdown, no explanation).\n\n" +

            "Field recognition rules:\n" +
            "- 'First name' / 'Last name' → firstName, lastName; fullName = first + last\n" +
            "- 'Name:' (single field) → split into firstName / lastName; fullName = full value\n" +
            "- 'Email' / 'Email:' → email\n" +
            "- 'Phone number' / 'Phone:' / 'Mobile:' → if the number starts with 08 it is Irish mobile → mobile; otherwise → phone\n" +
            "- 'Postcode:' / 'Eircode:' → eircode  (Irish postcode, e.g. 'V94 N61X' or 'D09 XY12')\n" +
            "- 'County:' → county\n" +
            "- 'Address:' / 'Address 1:' / 'Street:' → address1\n" +
            "- 'Address 2:' / 'Town:' / 'City:' → address2\n" +
            "- 'Type: Residential' or 'Business/Residential: Residential' → isResidential: true\n" +
            "- 'Type: Business' or 'Business/Residential: Business' → isResidential: false\n" +
            "- 'Your Message' / 'Comments:' / 'Message:' → notes (capture the full enquiry text)\n" +
            "- 'Which products are you interested in?' / 'Model interested in:' → append to notes\n" +
            "- 'What budget do you have in mind?' → append to notes\n\n" +

            "Return JSON in exactly this format (use null for missing fields):\n" +
            "{\n" +
            "  \"firstName\": \"\",\n" +
            "  \"lastName\": \"\",\n" +
            "  \"fullName\": \"\",\n" +
            "  \"email\": \"\",\n" +
            "  \"phone\": \"\",\n" +
            "  \"mobile\": \"\",\n" +
            "  \"address1\": \"\",\n" +
            "  \"address2\": \"\",\n" +
            "  \"county\": \"\",\n" +
            "  \"eircode\": \"\",\n" +
            "  \"isResidential\": true,\n" +
            "  \"notes\": \"\"\n" +
            "}";

        public LeadImportService(
            GraphServiceClient graphClient,
            ICustomerService customerService,
            AppDbContext context,
            AnthropicClient anthropicClient,
            IConfiguration configuration,
            ILogger<LeadImportService> logger)
        {
            _graphClient = graphClient;
            _customerService = customerService;
            _context = context;
            _anthropicClient = anthropicClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LeadImportResultDto> ProcessLeadsFolderAsync(string folderName, string currentUser)
        {
            var mailbox = _configuration["AzureAd:MonitoredMailbox"]
                ?? _configuration["AzureAd:OrganizerEmail"]
                ?? throw new InvalidOperationException("Monitored mailbox not configured");

            // Find the named folder under Inbox
            var sourceFolderId = await FindChildFolderIdAsync(mailbox, "inbox", folderName)
                ?? throw new InvalidOperationException($"Folder '{folderName}' not found under Inbox.");

            // Ensure a "Processed" subfolder exists inside the source folder
            var processedFolderId = await GetOrCreateChildFolderAsync(mailbox, sourceFolderId, ProcessedFolderName);

            // Fetch all messages from the source folder (max 200)
            var messagesResponse = await _graphClient.Users[mailbox]
                .MailFolders[sourceFolderId]
                .Messages
                .GetAsync(req =>
                {
                    req.QueryParameters.Select = new[]
                    {
                        "id", "subject", "from", "body", "bodyPreview", "receivedDateTime"
                    };
                    req.QueryParameters.Top = 200;
                });

            var messages = messagesResponse?.Value ?? new List<GraphMessage>();
            var result = new LeadImportResultDto { TotalEmails = messages.Count };

            _logger.LogInformation("Lead import started — {Count} email(s) found in '{Folder}'",
                messages.Count, folderName);

            foreach (var message in messages)
            {
                var item = new LeadImportItemDto
                {
                    Subject = message.Subject ?? "(no subject)",
                    FromEmail = message.From?.EmailAddress?.Address ?? "",
                    ReceivedAt = message.ReceivedDateTime?.DateTime
                };

                try
                {
                    // AI-extract customer fields from email body
                    var body = message.Body?.Content ?? message.BodyPreview ?? "";
                    var extracted = await ExtractCustomerDetailsAsync(
                        message.Subject,
                        body,
                        message.From?.EmailAddress?.Address,
                        message.From?.EmailAddress?.Name);

                    // Email to use for duplicate detection (extracted > sender)
                    var leadEmail = !string.IsNullOrWhiteSpace(extracted.Email)
                        ? extracted.Email.Trim()
                        : message.From?.EmailAddress?.Address ?? "";

                    // Duplicate check
                    var existing = string.IsNullOrWhiteSpace(leadEmail)
                        ? null
                        : await _context.Customers
                            .Include(c => c.CustomerContacts)
                            .FirstOrDefaultAsync(c =>
                                (c.Email != null && c.Email.ToLower() == leadEmail.ToLower()) ||
                                c.CustomerContacts.Any(cc => cc.Email != null && cc.Email.ToLower() == leadEmail.ToLower()));

                    // Carry the enquiry notes through to the result regardless of outcome
                    item.EnquiryNotes = extracted.Notes;

                    if (existing != null)
                    {
                        item.Status = "Skipped";
                        item.CustomerName = existing.Name;
                        item.CustomerId = existing.CustomerId;
                        item.Note = "Customer already exists";
                        result.Skipped++;
                        _logger.LogInformation("Skipped duplicate: '{Name}' ({Email})", existing.Name, leadEmail);
                    }
                    else
                    {
                        var dto = BuildCustomerDto(extracted, message);
                        var customer = await _customerService.SaveCompanyWithContact(dto, currentUser);
                        item.Status = "Created";
                        item.CustomerName = customer.Name;
                        item.CustomerId = customer.CustomerId;
                        result.Created++;
                        _logger.LogInformation("Customer created: '{Name}' ID={Id} from '{Subject}'",
                            customer.Name, customer.CustomerId, message.Subject);
                    }

                    // Copy to Processed subfolder (original stays in source folder)
                    await _graphClient.Users[mailbox]
                        .Messages[message.Id]
                        .Copy
                        .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Copy.CopyPostRequestBody
                        {
                            DestinationId = processedFolderId
                        });
                }
                catch (Exception ex)
                {
                    item.Status = "Failed";
                    item.Error = ex.Message;
                    result.Failed++;
                    _logger.LogError(ex, "Failed to process lead from '{Subject}': [{ExType}] {ExMessage}",
                        message.Subject, ex.GetType().Name, ex.Message);
                }

                result.Results.Add(item);
            }

            _logger.LogInformation(
                "Lead import complete — Created: {C}, Skipped: {S}, Failed: {F} (folder: '{Folder}')",
                result.Created, result.Skipped, result.Failed, folderName);

            return result;
        }

        #region AI Extraction

        private async Task<ExtractedLeadData> ExtractCustomerDetailsAsync(
            string? subject, string body, string? fromEmail, string? fromName)
        {
            var sanitised = SanitiseBody(body);

            var userMessage =
                $"From: {fromName} <{fromEmail}>\n" +
                $"Subject: {subject}\n\n" +
                $"Email body:\n{sanitised}";

            var parameters = new MessageCreateParams
            {
                Model = _configuration["Claude:Model"] ?? "claude-haiku-4-5",
                MaxTokens = 512,
                System = new List<TextBlockParam>
                {
                    new()
                    {
                        Text = ExtractionSystemPrompt,
                        CacheControl = new CacheControlEphemeral()
                    }
                },
                Messages = [new() { Role = Role.User, Content = userMessage }]
            };

            var response = await _anthropicClient.Messages.Create(parameters);
            var text = response.Content
                .Select(b => b.Value)
                .OfType<TextBlock>()
                .FirstOrDefault()?.Text ?? "{}";

            return ParseExtractedData(text);
        }

        private static ExtractedLeadData ParseExtractedData(string json)
        {
            try
            {
                var clean = json.Trim();
                if (clean.StartsWith("```json")) clean = clean[7..];
                if (clean.StartsWith("```")) clean = clean[3..];
                if (clean.EndsWith("```")) clean = clean[..^3];
                clean = clean.Trim();

                return JsonSerializer.Deserialize<ExtractedLeadData>(clean,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new ExtractedLeadData();
            }
            catch
            {
                return new ExtractedLeadData();
            }
        }

        #endregion

        #region Customer DTO

        private static CompanyWithContactDto BuildCustomerDto(ExtractedLeadData d, GraphMessage message)
        {
            var email = !string.IsNullOrWhiteSpace(d.Email)
                ? d.Email.Trim()
                : message.From?.EmailAddress?.Address ?? "";

            var senderName = message.From?.EmailAddress?.Name ?? "";

            var firstName = !string.IsNullOrWhiteSpace(d.FirstName)
                ? d.FirstName
                : (senderName.Contains(' ') ? senderName[..senderName.IndexOf(' ')] : senderName);

            var lastName = !string.IsNullOrWhiteSpace(d.LastName)
                ? d.LastName
                : (senderName.Contains(' ') ? senderName[(senderName.IndexOf(' ') + 1)..] : "");

            var fullName = !string.IsNullOrWhiteSpace(d.FullName)
                ? d.FullName
                : (!string.IsNullOrWhiteSpace(senderName) ? senderName : email);

            return new CompanyWithContactDto
            {
                Name = fullName,
                Email = email,
                Phone = d.Phone,
                Mobile = d.Mobile,
                Address1 = d.Address1,
                Address2 = d.Address2,
                County = d.County,
                Eircode = d.Eircode,
                Residential = d.IsResidential ?? true,
                Contacts = new List<CustomerContactDto>
                {
                    new()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Phone = d.Phone ?? d.Mobile ?? ""
                    }
                }
            };
        }

        #endregion

        #region Graph Folder Helpers

        private async Task<string?> FindChildFolderIdAsync(
            string mailbox, string parentFolderIdOrWellKnown, string childName)
        {
            var response = await _graphClient.Users[mailbox]
                .MailFolders[parentFolderIdOrWellKnown]
                .ChildFolders
                .GetAsync(req =>
                    req.QueryParameters.Filter = $"displayName eq '{childName}'");

            return response?.Value?.FirstOrDefault()?.Id;
        }

        private async Task<string> GetOrCreateChildFolderAsync(
            string mailbox, string parentFolderId, string folderName)
        {
            var existing = await FindChildFolderIdAsync(mailbox, parentFolderId, folderName);
            if (existing != null) return existing;

            var created = await _graphClient.Users[mailbox]
                .MailFolders[parentFolderId]
                .ChildFolders
                .PostAsync(new GraphMailFolder { DisplayName = folderName });

            return created?.Id
                ?? throw new InvalidOperationException($"Failed to create folder '{folderName}'");
        }

        #endregion

        #region Helpers

        private static string SanitiseBody(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "(no body)";
            var text = System.Text.RegularExpressions.Regex.Replace(raw, "<[^>]+>", " ");
            text = System.Net.WebUtility.HtmlDecode(text);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
            return text.Length <= 3000 ? text : text[..3000];
        }

        private class ExtractedLeadData
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Mobile { get; set; }
            public string? Address1 { get; set; }
            public string? Address2 { get; set; }
            public string? County { get; set; }
            public string? Eircode { get; set; }
            public bool? IsResidential { get; set; }
            public string? Notes { get; set; }
        }

        #endregion
    }
}
