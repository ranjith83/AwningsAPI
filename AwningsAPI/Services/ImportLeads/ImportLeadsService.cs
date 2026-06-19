using Anthropic;
using Anthropic.Models.Messages;
using AwningsAPI.Database;
using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.ImportLeads;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Audit;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Text.Json;
using GraphMessage = Microsoft.Graph.Models.Message;
using GraphMailFolder = Microsoft.Graph.Models.MailFolder;
using FileAttachment = Microsoft.Graph.Models.FileAttachment;

namespace AwningsAPI.Services.ImportLeads
{
    public class ImportLeadsService : IImportLeadsService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly ICustomerService _customerService;
        private readonly AppDbContext _context;
        private readonly AnthropicClient _anthropicClient;
        private readonly IConfiguration _configuration;
        private readonly IEmailReaderService _emailReaderService;
        private readonly ITaskService _taskService;
        private readonly IBlobEmailStorageService _blobService;
        private readonly ILogger<ImportLeadsService> _logger;

        private const string ProcessedFolderName = "Processed";
        private const string ExistingCustomersFolderName = "Existing Customers";

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

        public ImportLeadsService(
            GraphServiceClient graphClient,
            ICustomerService customerService,
            AppDbContext context,
            AnthropicClient anthropicClient,
            IConfiguration configuration,
            IEmailReaderService emailReaderService,
            ITaskService taskService,
            IBlobEmailStorageService blobService,
            ILogger<ImportLeadsService> logger)
        {
            _graphClient = graphClient;
            _customerService = customerService;
            _context = context;
            _anthropicClient = anthropicClient;
            _configuration = configuration;
            _emailReaderService = emailReaderService;
            _taskService = taskService;
            _blobService = blobService;
            _logger = logger;
        }

        // ── Entry point ──────────────────────────────────────────────────────────

        public async Task<ImportLeadsResultDto> ProcessLeadsFolderAsync(string folderName, string currentUser)
        {
            var mailbox = GetMailbox();
            var (sourceFolderId, processedFolderId, existingCustomersFolderId) =
                await SetupFoldersAsync(mailbox, folderName);
            var messages = await FetchMessagesAsync(mailbox, sourceFolderId);

            var result = new ImportLeadsResultDto { TotalEmails = messages.Count };
            _logger.LogInformation("Lead import started — {Count} email(s) found in '{Folder}'",
                messages.Count, folderName);

            foreach (var message in messages)
            {
                var item = new ImportLeadsItemDto
                {
                    Subject = message.Subject ?? "(no subject)",
                    FromEmail = message.From?.EmailAddress?.Address ?? "",
                    ReceivedAt = message.ReceivedDateTime?.DateTime
                };

                try
                {
                    var targetFolderId = await ProcessMessageAsync(
                        message, item, result, processedFolderId, existingCustomersFolderId, currentUser);

                    await MoveToFolderAsync(mailbox, message.Id!, targetFolderId);
                }
                catch (Exception ex)
                {
                    await HandleFailureAsync(message, item, result, ex, currentUser);
                }

                result.Results.Add(item);
            }

            _logger.LogInformation(
                "Lead import complete — Created: {C}, Skipped: {S}, Ignored: {I}, Failed: {F} (folder: '{Folder}')",
                result.Created, result.Skipped, result.Ignored, result.Failed, folderName);

            return result;
        }

        // ── Per-message routing ──────────────────────────────────────────────────

        // Returns the target subfolder ID to move to. All outcomes move the email out of the
        // source folder so it is not picked up again on the next import run.
        private async Task<string> ProcessMessageAsync(
            GraphMessage message, ImportLeadsItemDto item, ImportLeadsResultDto result,
            string processedFolderId, string existingCustomersFolderId, string currentUser)
        {
            var body = message.Body?.Content ?? message.BodyPreview ?? "";
            var extracted = await ExtractCustomerDetailsAsync(
                message.Subject, body,
                message.From?.EmailAddress?.Address,
                message.From?.EmailAddress?.Name);

            var leadEmail = !string.IsNullOrWhiteSpace(extracted.Email)
                ? extracted.Email.Trim()
                : message.From?.EmailAddress?.Address ?? "";

            item.EnquiryNotes = extracted.Notes;

            if (!HasUsefulContent(extracted, message.From?.EmailAddress?.Name))
            {
                await HandleIgnoredAsync(item, result, leadEmail, currentUser);
                return processedFolderId; // move ignored emails out so they don't repeat
            }

            var existing = await FindExistingCustomerAsync(leadEmail);
            if (existing != null)
                return await HandleSkippedAsync(item, result, existing, leadEmail, existingCustomersFolderId, currentUser);

            var modelName = ExtractModelFromNotes(extracted.Notes);
            var product = await ResolveProductAsync(modelName);

            await HandleNewCustomerAsync(message, item, result, extracted, product, modelName, body, currentUser);
            return processedFolderId;
        }

        // ── Case handlers ────────────────────────────────────────────────────────

        private async Task HandleIgnoredAsync(
            ImportLeadsItemDto item, ImportLeadsResultDto result, string leadEmail, string currentUser)
        {
            item.Status = "Ignored";
            item.Note = "Email contains no useful information";
            result.Ignored++;
            AddAuditLog(0, leadEmail, "IGNORED", currentUser,
                "Email contains no useful information beyond email address");
            await _context.SaveChangesAsync();
            _logger.LogInformation("Skipping content-empty email from {Email}", leadEmail);
        }

        private async Task<string> HandleSkippedAsync(
            ImportLeadsItemDto item, ImportLeadsResultDto result,
            Customer existing, string leadEmail, string existingCustomersFolderId, string currentUser)
        {
            item.Status = "Skipped";
            item.CustomerName = existing.Name;
            item.CustomerId = existing.CustomerId;
            item.Note = "Customer already exists";
            result.Skipped++;
            AddAuditLog(existing.CustomerId, existing.Name, "SKIPPED", currentUser,
                $"Lead import: customer already exists ({leadEmail})");
            await _context.SaveChangesAsync();
            _logger.LogInformation("Existing customer: '{Name}' ({Email})", existing.Name, leadEmail);
            return existingCustomersFolderId;
        }

        private async Task HandleNewCustomerAsync(
            GraphMessage message, ImportLeadsItemDto item, ImportLeadsResultDto result,
            ExtractedLeadData extracted, Product? product, string? modelName, string body, string currentUser)
        {
            var dto = BuildCustomerDto(extracted, message);
            var hasAttachment = message.HasAttachments ?? false;
            var hasMeasurements = HasMeasurements(extracted.Notes, body);

            // Upload body and attachments to blob BEFORE the transaction.
            // Blob operations are not transactional; doing them outside ensures the DB transaction
            // only contains DB work and blob uploads are idempotent on retry (same Graph message.Id key).
            var mailbox = GetMailbox();
            var (bodyBlobUrl, attachmentResults) =
                await UploadEmailToBlobAsync(message, mailbox);

            // SqlServerRetryingExecutionStrategy requires all operations inside CreateExecutionStrategy
            // so that the whole unit can be retried atomically on transient failures.
            Customer? customer = null;
            string emailBody = string.Empty;
            int? incomingEmailId = null;
            int? workflowId = null;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                _context.ChangeTracker.Clear(); // start clean on each attempt
                using var tx = await _context.Database.BeginTransactionAsync();

                customer = await _customerService.SaveCompanyWithContact(dto, currentUser);
                emailBody = BuildLeadEmailBody(customer, product, hasAttachment && hasMeasurements);
                incomingEmailId = await SaveIncomingEmailAsync(
                    message, customer, currentUser, bodyBlobUrl, attachmentResults);
                workflowId = await CreateWorkflowAndEnquiryAsync(
                    customer, extracted, product, emailBody, incomingEmailId, currentUser);

                if (incomingEmailId.HasValue)
                    await _taskService.CreateTaskFromEmailAsync(incomingEmailId.Value, currentUser, workflowId, customer!.CustomerId);

                await tx.CommitAsync();
            });

            item.Status = "Created";
            item.CustomerName = customer!.Name;
            item.CustomerId = customer.CustomerId;
            item.Note = DetermineItemNote(product, modelName, hasAttachment, hasMeasurements);
            result.Created++;

            _logger.LogInformation("Customer created: '{Name}' ID={Id} — product: {Product}",
                customer.Name, customer.CustomerId, product?.Description ?? "none");

            await SendEnquiryEmailAsync(customer, product, emailBody);
        }

        private async Task HandleFailureAsync(
            GraphMessage message, ImportLeadsItemDto item, ImportLeadsResultDto result,
            Exception ex, string currentUser)
        {
            item.Status = "Failed";
            item.Error = ex.Message;
            result.Failed++;
            _context.ChangeTracker.Clear();
            try
            {
                AddAuditLog(0, message.Subject, AuditAction.CREATE, currentUser,
                    $"Lead import failed: [{ex.GetType().Name}] {ex.Message}");
                await _context.SaveChangesAsync();
            }
            catch { }
            _logger.LogError(ex, "Failed to process lead from '{Subject}': [{ExType}] {ExMessage}",
                message.Subject, ex.GetType().Name, ex.Message);
        }

        // ── Setup helpers ────────────────────────────────────────────────────────

        private string GetMailbox() =>
            _configuration["AzureAd:MonitoredMailbox"]
            ?? _configuration["AzureAd:OrganizerEmail"]
            ?? throw new InvalidOperationException("Monitored mailbox not configured");

        private async Task<(string sourceFolderId, string processedFolderId, string existingCustomersFolderId)>
            SetupFoldersAsync(string mailbox, string folderName)
        {
            var sourceFolderId = await FindChildFolderIdAsync(mailbox, "inbox", folderName)
                ?? throw new InvalidOperationException($"Folder '{folderName}' not found under Inbox.");

            var processedFolderId =
                await GetOrCreateChildFolderAsync(mailbox, sourceFolderId, ProcessedFolderName);
            var existingCustomersFolderId =
                await GetOrCreateChildFolderAsync(mailbox, sourceFolderId, ExistingCustomersFolderName);

            return (sourceFolderId, processedFolderId, existingCustomersFolderId);
        }

        private async Task<List<GraphMessage>> FetchMessagesAsync(string mailbox, string sourceFolderId)
        {
            var response = await _graphClient.Users[mailbox]
                .MailFolders[sourceFolderId]
                .Messages
                .GetAsync(req =>
                {
                    req.QueryParameters.Select = new[]
                    {
                        "id", "subject", "from", "body", "bodyPreview", "receivedDateTime", "hasAttachments"
                    };
                    req.QueryParameters.Top = 200;
                });
            return response?.Value ?? new List<GraphMessage>();
        }

        private async Task MoveToFolderAsync(string mailbox, string messageId, string targetFolderId)
        {
            await _graphClient.Users[mailbox]
                .Messages[messageId]
                .Move
                .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Move.MovePostRequestBody
                {
                    DestinationId = targetFolderId
                });
        }

        // ── DB lookups ───────────────────────────────────────────────────────────

        private async Task<Customer?> FindExistingCustomerAsync(string leadEmail)
        {
            if (string.IsNullOrWhiteSpace(leadEmail)) return null;
            return await _context.Customers
                .Include(c => c.CustomerContacts)
                .FirstOrDefaultAsync(c =>
                    (c.Email != null && c.Email.ToLower() == leadEmail.ToLower()) ||
                    c.CustomerContacts.Any(cc => cc.Email != null && cc.Email.ToLower() == leadEmail.ToLower()));
        }

        private async Task<Product?> ResolveProductAsync(string? modelName)
        {
            if (modelName == null) return null;
            return await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Description.Contains(modelName));
        }

        // ── Blob storage ─────────────────────────────────────────────────────────

        private async Task<(string? bodyBlobUrl, List<AttachmentBlobResult> attachments)>
            UploadEmailToBlobAsync(GraphMessage message, string mailbox)
        {
            string? bodyBlobUrl = null;
            var attachments = new List<AttachmentBlobResult>();

            var bodyContent = message.Body?.Content;
            if (!string.IsNullOrWhiteSpace(bodyContent) && !string.IsNullOrEmpty(message.Id))
            {
                var isHtml = message.Body?.ContentType == BodyType.Html;
                bodyBlobUrl = await _blobService.UploadEmailBodyAsync(message.Id, bodyContent, isHtml);
            }

            if ((message.HasAttachments ?? false) && !string.IsNullOrEmpty(message.Id))
            {
                try
                {
                    var attResponse = await _graphClient.Users[mailbox]
                        .Messages[message.Id]
                        .Attachments
                        .GetAsync();

                    foreach (var att in attResponse?.Value ?? [])
                    {
                        if (att is not FileAttachment fileAtt) continue;

                        var bytes = fileAtt.ContentBytes;
                        var attId = fileAtt.Id ?? Guid.NewGuid().ToString();
                        var fileName = fileAtt.Name ?? "attachment";
                        var contentType = fileAtt.ContentType ?? "application/octet-stream";

                        string? blobUrl = null;
                        string? base64Fallback = null;

                        if (bytes != null)
                        {
                            blobUrl = await _blobService.UploadAttachmentAsync(
                                message.Id, attId, fileName, bytes, contentType);
                            if (blobUrl == null)
                                base64Fallback = Convert.ToBase64String(bytes);
                        }

                        attachments.Add(new AttachmentBlobResult(
                            AttachmentId: attId,
                            FileName: fileName,
                            ContentType: contentType,
                            Size: fileAtt.Size ?? 0,
                            IsInline: fileAtt.IsInline ?? false,
                            ContentId: fileAtt.ContentId,
                            BlobUrl: blobUrl,
                            Base64Content: base64Fallback));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch/upload attachments for message {Id}", message.Id);
                }
            }

            return (bodyBlobUrl, attachments);
        }

        // ── Email helpers ────────────────────────────────────────────────────────

        private static string BuildLeadEmailBody(Customer customer, Product? product, bool hasQuoteMaterial)
        {
            var name = customer.Name ?? customer.Email ?? "";
            if (product != null)
                return BuildEnquiryEmailHtml(name, product.Description);
            return hasQuoteMaterial
                ? BuildQuoteOrShowroomEmailHtml(name)
                : BuildNoProductEmailHtml(name);
        }

        private static string? DetermineItemNote(
            Product? product, string? modelName, bool hasAttachment, bool hasMeasurements)
        {
            if (product != null) return null;
            if (hasAttachment && hasMeasurements) return "Measurements and photo provided — quote pending";
            return modelName == null
                ? "No product interest found in email"
                : $"Product '{modelName}' not found in catalogue";
        }

        private async Task SendEnquiryEmailAsync(Customer customer, Product? product, string emailBody)
        {
            if (string.IsNullOrWhiteSpace(customer.Email)) return;
            try
            {
                await _emailReaderService.SendDirectEmailAsync(
                    toEmail: customer.Email,
                    toName: customer.Name,
                    subject: "Thank you for your enquiry - Awnings of Ireland",
                    body: emailBody,
                    attachBrochure: product != null,
                    productIds: product != null ? new List<int> { product.ProductId } : null);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Enquiry email failed for customer {Id}", customer.CustomerId);
            }
        }

        // ── Workflow & enquiry ───────────────────────────────────────────────────

        private async Task<int?> CreateWorkflowAndEnquiryAsync(
            Customer customer, ExtractedLeadData extracted, Product? product,
            string emailBody, int? incomingEmailId, string currentUser)
        {
            int productId, supplierId, productTypeId;

            if (product != null)
            {
                productId = product.ProductId;
                supplierId = product.SupplierId;
                productTypeId = product.ProductTypeId;
            }
            else
            {
                // No specific product matched — use first available to satisfy the non-nullable FK
                var defaultProduct = await _context.Products.AsNoTracking()
                    .OrderBy(p => p.ProductId)
                    .Select(p => new { p.ProductId, p.SupplierId, p.ProductTypeId })
                    .FirstOrDefaultAsync();

                if (defaultProduct == null)
                {
                    _logger.LogWarning(
                        "No products found in catalogue — skipping workflow creation for customer {Id}",
                        customer.CustomerId);
                    return null;
                }

                productId = defaultProduct.ProductId;
                supplierId = defaultProduct.SupplierId;
                productTypeId = defaultProduct.ProductTypeId;
            }

            var workflow = new WorkflowStart
            {
                WorkflowName = "Lead Enquiry",
                Description = extracted.Notes ?? "Lead imported from email",
                CustomerId = customer.CustomerId,
                CompanyId = customer.CustomerId,
                SupplierId = supplierId,
                ProductTypeId = productTypeId,
                ProductId = productId,
                InitialEnquiry = true,
                CreateQuote = false,
                InviteShowRoom = false,
                SetupSiteVisit = false,
                InvoiceSent = false,
                FinalQuote = false,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.WorkflowStarts.Add(workflow);
            await _context.SaveChangesAsync();

            _context.InitialEnquiries.Add(new InitialEnquiry
            {
                WorkflowId = workflow.WorkflowId,
                Comments = emailBody,
                Email = customer.Email ?? "",
                AutoReplyContent = emailBody,
                IncomingEmailId = incomingEmailId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            });
            await _context.SaveChangesAsync();
            return workflow.WorkflowId;
        }

        private async Task<int?> SaveIncomingEmailAsync(
            GraphMessage message, Customer customer, string currentUser,
            string? bodyBlobUrl, List<AttachmentBlobResult> attachmentResults)
        {
            // If the webhook already saved this email, update its ExtractedData to mark it as an
            // ImportLeads email so HandleInitialEnquiryWorkflow skips creating a duplicate
            // InitialEnquiry with the original customer email body.
            // Also return the existing ID so the InitialEnquiry and task are correctly linked.
            if (!string.IsNullOrEmpty(message.Id))
            {
                var existing = await _context.IncomingEmails
                    .FirstOrDefaultAsync(e => e.EmailId == message.Id);
                if (existing != null)
                {
                    existing.ExtractedData = JsonSerializer.Serialize(new Dictionary<string, object>
                    {
                        ["customerId"] = customer.CustomerId,
                        ["customerName"] = customer.Name ?? "",
                        ["source"] = "ImportLeads"
                    });
                    await _context.SaveChangesAsync();
                    return existing.Id;
                }
            }

            var bodyContent = message.Body?.Content;
            var incomingEmail = new IncomingEmail
            {
                EmailId = message.Id ?? "",
                Subject = message.Subject ?? "(no subject)",
                // Use the actual customer email/name extracted from the form body.
                // For website/HubSpot contact forms the message sender is the company's own
                // forwarding address (e.g. hello@awningsofireland.com), not the customer.
                FromEmail = customer.Email ?? message.From?.EmailAddress?.Address ?? "",
                FromName = customer.Name ?? message.From?.EmailAddress?.Name ?? "",
                BodyPreview = message.BodyPreview ?? "",
                // Blob is the source of truth; only fall back to DB when blob upload failed
                BodyContent = bodyBlobUrl == null ? bodyContent : null,
                BodyBlobUrl = bodyBlobUrl,
                IsHtml = message.Body?.ContentType == BodyType.Html,
                ReceivedDateTime = message.ReceivedDateTime?.DateTime ?? DateTime.UtcNow,
                HasAttachments = message.HasAttachments ?? false,
                Category = "initial_enquiry",
                CategoryConfidence = 1.0,
                ProcessingStatus = "Completed",
                DateProcessed = DateTime.UtcNow,
                ExtractedData = JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    ["customerId"] = customer.CustomerId,
                    ["customerName"] = customer.Name ?? "",
                    ["source"] = "ImportLeads"
                })
            };

            foreach (var att in attachmentResults)
            {
                incomingEmail.Attachments.Add(new EmailAttachment
                {
                    AttachmentId = att.AttachmentId,
                    FileName = att.FileName,
                    ContentType = att.ContentType,
                    Size = att.Size,
                    IsInline = att.IsInline,
                    ContentId = att.ContentId,
                    BlobStorageUrl = att.BlobUrl,
                    Base64Content = att.BlobUrl == null ? att.Base64Content : null,
                    DateDownloaded = DateTime.UtcNow
                });
            }

            _context.IncomingEmails.Add(incomingEmail);
            await _context.SaveChangesAsync();
            return incomingEmail.Id;
        }

        // ── Audit ────────────────────────────────────────────────────────────────

        private void AddAuditLog(int entityId, string? entityName, string action,
            string performedByName, string? notes = null)
        {
            _context.AuditLogs.Add(new AuditLog
            {
                EntityType = AuditEntityType.CUSTOMER,
                EntityId = entityId,
                EntityName = entityName,
                Action = action,
                PerformedBy = 0,
                PerformedByName = performedByName,
                PerformedAt = DateTime.UtcNow,
                Notes = notes
            });
        }

        // ── Email HTML templates ─────────────────────────────────────────────────

        private static string BuildEnquiryEmailHtml(string customerName, string model) => $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 650px; margin: 0 auto; padding: 20px;'>
  <p>Dear {customerName},</p>
  <p>Thank you for reaching out to us at Awnings of Ireland.</p>
  <p>Please find attached a quote for the {model}. Additionally, I've included some information about the {model} and attached a brochure for your reference.</p>
  <p>These awnings are manufactured in Germany. The aluminium powder-coated cassette is designed to withstand rain without any risk of rust.</p>
  <p>The awning can endure winds of up to 40 km/h when fully extended. With our self-cleaning fabric, your new awning will require minimal maintenance.</p>
  <p>You can choose from a range of RAL colours for the frame, as well as over 250 fabric options. The awning also features an integrated drainage system to keep you dry and shaded underneath.</p>
  <p>Please note that all quotations are subject to a site visit. If you're ready to schedule a site survey, feel free to contact me.</p>
  <p>Alternatively, we have a showroom located in Sandyford, and I would be happy to assist you with our full range of products if you would like to visit.</p>
  <p>Best regards,<br><strong>Awnings of Ireland</strong></p>
</body>
</html>";

        private static string BuildQuoteOrShowroomEmailHtml(string name) => $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 650px; margin: 0 auto; padding: 20px;'>
  <p>Dear {name},</p>
  <p>Thank you for reaching out to us at Awnings of Ireland.</p>
  <p>We have received your enquiry along with the measurements and photos you provided. Our team will review your requirements and get back to you with a quote as soon as possible.</p>
  <p>Alternatively, we would love to invite you to visit our showroom in Sandyford where our team can discuss your requirements in person and guide you through our full range of awning solutions.</p>
  <p>If you have any further questions in the meantime, please do not hesitate to get in touch.</p>
  <p>Best regards,<br><strong>Awnings of Ireland</strong></p>
</body>
</html>";

        private static string BuildNoProductEmailHtml(string name) => $@"
<html>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 650px; margin: 0 auto; padding: 20px;'>
  <p>Dear {name},</p>
  <p>Thank you for reaching out to us at Awnings of Ireland.</p>
  <p>We have received your enquiry. As there is no specific product information included, one of our sales team will be in touch with you shortly to assist you further.</p>
  <p>If you have any specific product or model in mind, please feel free to reply to this email and let us know.</p>
  <p>Alternatively, you are welcome to visit our showroom in Sandyford where our team will be happy to guide you through our full range of products.</p>
  <p>Best regards,<br><strong>Awnings of Ireland</strong></p>
</body>
</html>";

        // ── AI extraction ────────────────────────────────────────────────────────

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

        // ── Customer DTO ─────────────────────────────────────────────────────────

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
                Name = Trunc(fullName, 255) ?? email,
                Email = Trunc(email, 255),
                Phone = Trunc(d.Phone, 20),
                Mobile = Trunc(d.Mobile, 20),
                Address1 = d.Address1,
                Address2 = d.Address2,
                County = Trunc(d.County, 100),
                Eircode = Trunc(d.Eircode, 10),
                Residential = d.IsResidential ?? true,
                Contacts = new List<CustomerContactDto>
                {
                    new()
                    {
                        FirstName = Trunc(firstName, 100) ?? "",
                        LastName = Trunc(lastName, 100) ?? "",
                        Email = Trunc(email, 255) ?? "",
                        Phone = Trunc(d.Phone ?? d.Mobile, 20) ?? ""
                    }
                }
            };
        }

        private static string? Trunc(string? value, int maxLength) =>
            value is null ? null : (value.Length <= maxLength ? value : value[..maxLength]);

        // ── Content helpers ──────────────────────────────────────────────────────

        private static bool HasUsefulContent(ExtractedLeadData d, string? senderName) =>
            !string.IsNullOrWhiteSpace(d.FirstName) ||
            !string.IsNullOrWhiteSpace(d.LastName) ||
            !string.IsNullOrWhiteSpace(d.FullName) ||
            !string.IsNullOrWhiteSpace(senderName) ||
            !string.IsNullOrWhiteSpace(d.Phone) ||
            !string.IsNullOrWhiteSpace(d.Mobile) ||
            !string.IsNullOrWhiteSpace(d.Address1) ||
            !string.IsNullOrWhiteSpace(d.Notes);

        private static bool HasMeasurements(string? notes, string? body)
        {
            var text = $"{notes} {body}".ToLowerInvariant();
            return text.Contains("width") || text.Contains("height") || text.Contains("projection") ||
                   text.Contains("metre") || text.Contains("meter") || text.Contains(" cm") ||
                   text.Contains("feet") || text.Contains("foot") || text.Contains("measurement") ||
                   text.Contains("dimension") || text.Contains("size");
        }

        private static string? ExtractModelFromNotes(string? notes)
        {
            if (string.IsNullOrWhiteSpace(notes)) return null;

            foreach (var line in notes.Split('\n', '\r', StringSplitOptions.RemoveEmptyEntries))
            {
                var lower = line.ToLowerInvariant();
                if (lower.Contains("model") || lower.Contains("markilux") || lower.Contains("product interest"))
                {
                    var colonIdx = line.IndexOf(':');
                    if (colonIdx >= 0 && colonIdx < line.Length - 1)
                    {
                        var val = line[(colonIdx + 1)..].Trim();
                        if (!string.IsNullOrWhiteSpace(val) && val.Length < 100)
                            return val;
                    }
                }
            }
            return null;
        }

        private static string SanitiseBody(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "(no body)";
            var text = System.Text.RegularExpressions.Regex.Replace(raw, "<[^>]+>", " ");
            text = System.Net.WebUtility.HtmlDecode(text);
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
            return text.Length <= 3000 ? text : text[..3000];
        }

        // ── Graph folder helpers ─────────────────────────────────────────────────

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

        // ── Inner types ──────────────────────────────────────────────────────────

        private record AttachmentBlobResult(
            string AttachmentId,
            string FileName,
            string ContentType,
            long Size,
            bool IsInline,
            string? ContentId,
            string? BlobUrl,
            string? Base64Content);

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
    }
}
