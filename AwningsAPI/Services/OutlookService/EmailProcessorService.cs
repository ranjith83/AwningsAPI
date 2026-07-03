using AwningsAPI.Database;
using AwningsAPI.Dto.Audit;
using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Hubs;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Common;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Workflow;
using AwningsAPI.Services.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    /// <summary>
    /// Email Processor Service - Monitors mailbox, analyzes with AI, and executes automated workflows
    /// Flow: Email Reader → AI Analysis → Automated Actions (Customer/Workflow/Audit Log) → Task Creation
    /// </summary>
    public class EmailProcessorService : IEmailProcessorService
    {
        private readonly IEmailReaderService _emailReaderService;
        private readonly IEmailAnalysisService _emailAnalysisService;
        private readonly ICustomerService _customerService;
        private readonly IWorkflowService _workflowService;
        private readonly IAuditLogService _auditLogService;
        private readonly ITaskService _taskService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailProcessorService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<NotificationHub> _hub;

        public EmailProcessorService(
            IEmailReaderService emailReaderService,
            IEmailAnalysisService emailAnalysisService,
            ICustomerService customerService,
            IWorkflowService workflowService,
            IAuditLogService auditLogService,
            ITaskService taskService,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<EmailProcessorService> logger,
            HttpClient httpClient,
            IHubContext<NotificationHub> hub)
        {
            _emailReaderService = emailReaderService;
            _emailAnalysisService = emailAnalysisService;
            _customerService = customerService;
            _workflowService = workflowService;
            _auditLogService = auditLogService;
            _taskService = taskService;
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
            _hub = hub;
        }

        #region Email Monitoring & Processing

        public async Task ProcessIncomingEmailsAsync()
        {
            try
            {
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new Exception("Monitored mailbox not configured");

                _logger.LogInformation($"🔍 Monitoring mailbox: {mailboxEmail}");

                // Get unread emails from mailbox
                var emails = await _emailReaderService.GetUnreadEmailsAsync(mailboxEmail);

                if (!emails.Any())
                {
                    _logger.LogInformation("✅ No unread emails to process");
                    return;
                }

                _logger.LogInformation($"📧 Found {emails.Count} unread emails to process");

                foreach (var email in emails)
                {
                    try
                    {
                        await ProcessEmailAsync(email, mailboxEmail);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Error processing email: {email.Subject}");

                        // Save error to database
                        email.ProcessingStatus = "Failed";
                        email.ErrorMessage = ex.Message;
                        email.DateProcessed = DateTime.UtcNow;

                        _context.IncomingEmails.Add(email);
                        await _context.SaveChangesAsync();
                    }
                }

                _logger.LogInformation("✅ Email processing cycle completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in email processing cycle");
                throw;
            }
        }

        public async Task ProcessSingleEmailAsync(string emailId)
        {
            try
            {
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new Exception("Monitored mailbox not configured");

                // ── Duplicate guard ───────────────────────────────────────────
                // Multiple Graph subscriptions (from previous restarts) can fire
                // for the same message simultaneously. Check the DB first so we
                // never insert the same Graph message ID twice.
                var alreadyProcessed = await _context.IncomingEmails
                    .AnyAsync(e => e.EmailId == emailId);

                if (alreadyProcessed)
                {
                    _logger.LogWarning(
                        "⚠️ Duplicate notification ignored — emailId {EmailId} already exists in DB.",
                        emailId);
                    return;
                }

                _logger.LogInformation($"📧 Processing single email: {emailId}");

                var email = await _emailReaderService.GetCompleteEmailAsync(mailboxEmail, emailId);
                await ProcessEmailAsync(email, mailboxEmail);

                _logger.LogInformation($"✅ Email {emailId} processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error processing email {emailId}");
                throw;
            }
        }

        #endregion

        #region Main Email Processing Logic

        private async Task ProcessEmailAsync(IncomingEmail email, string mailboxEmail)
        {
            try
            {
                _logger.LogInformation($"📧 Processing: {email.Subject} from {email.FromEmail}");

                // ── Second duplicate guard (covers the batch poll path) ────────
                if (!string.IsNullOrEmpty(email.EmailId))
                {
                    var exists = await _context.IncomingEmails
                        .AnyAsync(e => e.EmailId == email.EmailId);

                    if (exists)
                    {
                        _logger.LogWarning(
                            "⚠️ Skipping duplicate email — EmailId {EmailId} already in DB.",
                            email.EmailId);
                        return;
                    }
                }

                // Update status
                email.ProcessingStatus = "Processing";

                // Download attachments if present
                if (email.HasAttachments && !email.Attachments.Any())
                {
                    var attachments = await _emailReaderService.DownloadAttachmentsAsync(mailboxEmail, email.EmailId);
                    email.Attachments = attachments;

                    // Extract text from attachments
                    await ExtractTextFromAttachmentsAsync(email);
                }

                // ✨ ANALYZE EMAIL WITH AI ✨
                var analysisResult = await _emailAnalysisService.AnalyzeEmailAsync(email);

                // Update email with AI analysis results
                email.Category = analysisResult.Category;
                email.CategoryConfidence = analysisResult.Confidence;
                email.ExtractedData = JsonConvert.SerializeObject(analysisResult.ExtractedData);

                // Save email to database first
                _context.IncomingEmails.Add(email);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"🤖 AI Analysis: {email.Category} (confidence: {email.CategoryConfidence:P})");

                // 🔄 EXECUTE AUTOMATED WORKFLOWS BASED ON CATEGORY
                // Wrapped in its own try-catch so a workflow failure never blocks task creation.
                if (!analysisResult.IsSpam && email.Category != "junk")
                {
                    try
                    {
                        await ExecuteAutomatedWorkflowAsync(email, analysisResult);
                    }
                    catch (Exception wfEx)
                    {
                        _logger.LogError(wfEx,
                            "⚠️ Automated workflow failed for email {EmailId} — task will still be created",
                            email.EmailId);
                    }
                }
                else
                {
                    _logger.LogInformation($"🗑️ Junk email detected, no workflow executed");
                }

                // Create task from email (uses TaskService)
                await CreateTaskFromEmail(email, analysisResult);

                // Mark email as processed
                email.ProcessingStatus = "Completed";
                email.DateProcessed = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Mark as read
                await _emailReaderService.MarkEmailAsReadAsync(mailboxEmail, email.EmailId);

                // Move to "No Customer Email" folder only for Kendlebell answering-service emails
                // where the body contains no customer email address.
                // Regular customer emails are excluded — their address is in the From header, not the body.
                var isKendlebell = email.FromEmail?.EndsWith("kbell.ie", StringComparison.OrdinalIgnoreCase) == true;
                if (!analysisResult.IsSpam && isKendlebell)
                {
                    // A customer email is present if EITHER:
                    //   (a) the regex found an email address in the raw body, OR
                    //   (b) the Kendlebell "Email:" field was parsed and overrode CustomerInfo["fromEmail"]
                    //       (catches cases where HTML encoding might fool the regex)
                    var hasBodyEmail = analysisResult.CustomerInfo.TryGetValue("bodyEmail", out var bodyEmail)
                        && !string.IsNullOrWhiteSpace(bodyEmail);
                    var hasParsedEmail = analysisResult.CustomerInfo.TryGetValue("fromEmail", out var parsedEmail)
                        && !string.IsNullOrWhiteSpace(parsedEmail)
                        && !parsedEmail.EndsWith("kbell.ie", StringComparison.OrdinalIgnoreCase);

                    if (!hasBodyEmail && !hasParsedEmail)
                    {
                        try
                        {
                            await _emailReaderService.MoveEmailToFolderAsync(mailboxEmail, email.EmailId, "No Customer Email");
                            _logger.LogInformation("📁 Email {EmailId} moved to 'No Customer Email' folder — Kendlebell message with no customer email in body", email.EmailId);
                        }
                        catch (Exception moveEx)
                        {
                            _logger.LogWarning(moveEx, "⚠️ Could not move email {EmailId} to 'No Customer Email' folder", email.EmailId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error processing email {email.EmailId}");
                email.ProcessingStatus = "Failed";
                email.ErrorMessage = ex.Message;
                email.DateProcessed = DateTime.UtcNow;

                if (email.Id == 0) // Not yet in database
                {
                    _context.IncomingEmails.Add(email);
                }

                await _context.SaveChangesAsync();
                throw;
            }
        }

        #endregion

        #region Automated Workflow Execution

        /// <summary>
        /// Execute automated workflows based on AI-determined category
        /// </summary>
        private async Task ExecuteAutomatedWorkflowAsync(IncomingEmail email, EmailAnalysisResult analysisResult)
        {
            var currentUser = "EmailProcessor";

            switch (analysisResult.Category)
            {
                case "initial_enquiry":
                    await HandleInitialEnquiryWorkflow(email, analysisResult, currentUser);
                    break;

                /** case "site_visit_meeting":
                     await HandleSiteVisitWorkflow(email, analysisResult, currentUser);
                     break;

                 case "invoice_due":
                     await HandleInvoiceDueWorkflow(email, analysisResult, currentUser);
                     break;

                 case "quote_creation":
                     await HandleQuoteRequestWorkflow(email, analysisResult, currentUser);
                     break; */

                default:
                    _logger.LogInformation($"ℹ️ No automated workflow for category: {analysisResult.Category}");
                    break;
            }
        }

        /// <summary>
        /// Handle Initial Enquiry: only inserts an InitialEnquiry record when BOTH the customer
        /// AND a workflow already exist in the database.  If either is missing, no action is taken
        /// and the email will surface as a normal manual task for staff to handle.
        /// </summary>
        private async Task HandleInitialEnquiryWorkflow(IncomingEmail email, EmailAnalysisResult analysisResult, string currentUser)
        {
            try
            {
                var isKendlebell = email.FromEmail?.EndsWith("kbell.ie", StringComparison.OrdinalIgnoreCase) == true;

                // ── Guard: Kendlebell emails without a customer email in the body are unactionable.
                // Do NOT create a customer, workflow, or initial enquiry — and do not look anything up.
                // ProcessEmailAsync will move this email to the "No Customer Email" folder.
                if (isKendlebell)
                {
                    var hasBodyEmail = analysisResult.CustomerInfo.TryGetValue("bodyEmail", out var be)
                        && !string.IsNullOrWhiteSpace(be);

                    if (!hasBodyEmail)
                    {
                        _logger.LogInformation(
                            "⏭️ Kendlebell email {EmailId} has no customer email in body — skipping customer and enquiry creation",
                            email.EmailId);
                        return;
                    }
                }

                // For Kendlebell emails the real customer email is parsed from the body into CustomerInfo["fromEmail"].
                // For all other senders the From header IS the customer email.
                var effectiveEmail = analysisResult.CustomerInfo.TryGetValue("fromEmail", out var ce) && !string.IsNullOrWhiteSpace(ce)
                    ? ce
                    : email.FromEmail;

                _logger.LogInformation($"🔄 Checking Initial Enquiry conditions for {effectiveEmail}");

                // ── 1. Customer lookup ────────────────────────────────────────────────
                var existingCustomer = await _context.Customers
                    .Include(c => c.CustomerContacts)
                    .FirstOrDefaultAsync(c =>
                        c.Email == effectiveEmail ||
                        c.CustomerContacts.Any(cc => cc.Email == effectiveEmail));

                if (existingCustomer == null)
                {
                    // Only create a new customer from a Kendlebell email when a real customer email
                    // was actually found in the body (bodyEmail is populated by regex that already
                    // excludes the Kendlebell sender address, so it can never be kbell.ie).
                    var hasBodyEmail = analysisResult.CustomerInfo.TryGetValue("bodyEmail", out var bodyEmailValue)
                        && !string.IsNullOrWhiteSpace(bodyEmailValue);

                    if (isKendlebell && hasBodyEmail)
                    {
                        _logger.LogInformation($"📝 New customer from Kendlebell form: {effectiveEmail}");
                        existingCustomer = await CreateCustomerFromEmail(email, analysisResult, currentUser);
                    }
                    else
                    {
                        _logger.LogInformation(
                            $"⏭️ No existing customer found for '{effectiveEmail}' — skipping Initial Enquiry creation");
                        return;
                    }
                }

                _logger.LogInformation(
                    $"✅ Customer: ID={existingCustomer.CustomerId}, Name={existingCustomer.Name}");

                // ── 2. Workflow lookup / creation ─────────────────────────────────────
                var existingWorkflow = await _context.WorkflowStarts
                    .Where(w => w.CustomerId == existingCustomer.CustomerId)
                    .OrderByDescending(w => w.DateCreated)
                    .FirstOrDefaultAsync();

                if (existingWorkflow == null)
                {
                    if (isKendlebell)
                    {
                        // New lead via Kendlebell — create the workflow
                        _logger.LogInformation($"📝 Creating workflow for new Kendlebell customer {existingCustomer.CustomerId}");
                        existingWorkflow = await GetOrCreateWorkflow(existingCustomer.CustomerId, currentUser);
                    }
                    else
                    {
                        _logger.LogInformation(
                            $"⏭️ Customer {existingCustomer.CustomerId} has no workflow — skipping Initial Enquiry creation");
                        return;
                    }
                }

                _logger.LogInformation($"✅ Workflow: ID={existingWorkflow.WorkflowId}");

                // ── 3. Guard — skip if an InitialEnquiry already exists for this email ──
                var enquiryAlreadyExists = await _context.InitialEnquiries
                    .AnyAsync(e => e.IncomingEmailId == email.Id);
                if (enquiryAlreadyExists)
                {
                    _logger.LogInformation(
                        "InitialEnquiry already exists for IncomingEmail {Id} — skipping duplicate creation",
                        email.Id);
                    return;
                }

                // ── 4. Create the Initial Enquiry ─────────────────────────────────────
                var enquiry = await CreateInitialEnquiry(existingWorkflow.WorkflowId, email, analysisResult, currentUser, effectiveEmail);

                _logger.LogInformation(
                    $"✅ Initial enquiry created: ID={enquiry.EnquiryId} in workflow {existingWorkflow.WorkflowId}");

                // ── 5. Notification ───────────────────────────────────────────────────
                await CreateEnquiryNotificationAsync(enquiry.EnquiryId, existingWorkflow.WorkflowId, existingCustomer.Name, effectiveEmail, email.Subject);

                // Store IDs in ExtractedData so the task created afterwards is pre-linked
                email.ExtractedData = JsonConvert.SerializeObject(new Dictionary<string, object>
                {
                    ["customerId"] = existingCustomer.CustomerId,
                    ["customerName"] = existingCustomer.Name,
                    ["workflowId"] = existingWorkflow.WorkflowId,
                    ["enquiryId"] = enquiry.EnquiryId,
                    ["aiExtractedData"] = analysisResult.ExtractedData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in initial enquiry workflow");
            }
        }

        private async Task CreateEnquiryNotificationAsync(int enquiryId, int workflowId, string customerName, string fromEmail, string subject)
        {
            try
            {
                var notification = new Notification
                {
                    Type = "new_enquiry",
                    Title = "New Initial Enquiry",
                    Message = $"New enquiry from {customerName} ({fromEmail}): {subject}",
                    EntityType = "InitialEnquiry",
                    EntityId = enquiryId,
                    WorkflowId = workflowId,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                var unreadCount = await _context.Notifications.CountAsync(n => !n.IsRead);
                await _hub.Clients.All.SendAsync("ReceiveNotification", new
                {
                    count = unreadCount,
                    notification = new
                    {
                        notification.Id,
                        notification.Type,
                        notification.Title,
                        notification.Message,
                        notification.EntityType,
                        notification.EntityId,
                        notification.WorkflowId,
                        notification.IsRead,
                        notification.CreatedAt
                    }
                });

                _logger.LogInformation("Notification created for enquiry {EnquiryId}", enquiryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create notification for enquiry {EnquiryId}", enquiryId);
            }
        }

        /// <summary>
        /// Handle Site Visit: Create audit log
        /// </summary>
        private async Task HandleSiteVisitWorkflow(IncomingEmail email, EmailAnalysisResult analysisResult, string currentUser)
        {
            try
            {
                _logger.LogInformation($"🔄 Executing Site Visit Workflow");

                // CREATE AUDIT LOG
                var changes = new List<FieldChange>
                {
                    new FieldChange
                    {
                        FieldName = "SiteVisit",
                        FieldLabel = "Site Visit Type",
                        OldValue = null,
                        NewValue = "Site Visit Requested",
                        DataType = "string"
                    }
                };

                if (analysisResult.ExtractedData.ContainsKey("preferredDate"))
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = "PreferredDate",
                        FieldLabel = "Preferred Date",
                        OldValue = null,
                        NewValue = analysisResult.ExtractedData["preferredDate"],
                        DataType = "string"
                    });
                }

                var auditDto = new CreateAuditLogDto
                {
                    EntityType = "Email",
                    EntityId = email.Id,
                    EntityName = email.Subject,
                    Action = "SITE_VISIT_REQUESTED",
                    Changes = changes,
                    PerformedBy = 0,
                    PerformedByName = currentUser,
                    Notes = $"Site visit requested from {email.FromEmail}"
                };

                await _auditLogService.CreateAuditLogAsync(auditDto, "System", "EmailProcessor");

                _logger.LogInformation($"✅ Site visit audit log created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in site visit workflow");
                throw;
            }
        }

        /// <summary>
        /// Handle Invoice Due: Create audit log
        /// </summary>
        private async Task HandleInvoiceDueWorkflow(IncomingEmail email, EmailAnalysisResult analysisResult, string currentUser)
        {
            try
            {
                _logger.LogInformation($"🔄 Executing Invoice Due Workflow");

                // CREATE AUDIT LOG
                var changes = new List<FieldChange>
                {
                    new FieldChange
                    {
                        FieldName = "InvoiceStatus",
                        FieldLabel = "Invoice Status",
                        OldValue = null,
                        NewValue = "Payment Due",
                        DataType = "string"
                    }
                };

                if (analysisResult.ExtractedData.ContainsKey("invoiceNumber"))
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = "InvoiceNumber",
                        FieldLabel = "Invoice Number",
                        OldValue = null,
                        NewValue = analysisResult.ExtractedData["invoiceNumber"],
                        DataType = "string"
                    });
                }

                if (analysisResult.ExtractedData.ContainsKey("amount"))
                {
                    changes.Add(new FieldChange
                    {
                        FieldName = "Amount",
                        FieldLabel = "Amount Due",
                        OldValue = null,
                        NewValue = analysisResult.ExtractedData["amount"],
                        DataType = "number"
                    });
                }

                var auditDto = new CreateAuditLogDto
                {
                    EntityType = "Email",
                    EntityId = email.Id,
                    EntityName = email.Subject,
                    Action = "INVOICE_DUE",
                    Changes = changes,
                    PerformedBy = 0,
                    PerformedByName = currentUser,
                    Notes = $"Invoice due notification from {email.FromEmail}"
                };

                await _auditLogService.CreateAuditLogAsync(auditDto, "System", "EmailProcessor");

                _logger.LogInformation($"✅ Invoice due audit log created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in invoice due workflow");
                throw;
            }
        }

        /// <summary>
        /// Handle Quote Request
        /// </summary>
        private async Task HandleQuoteRequestWorkflow(IncomingEmail email, EmailAnalysisResult analysisResult, string currentUser)
        {
            _logger.LogInformation($"ℹ️ Quote request workflow - task will be created for manual processing");
            // Task will be created by CreateTaskFromEmail
        }

        #endregion

        #region Helper Methods - Customer & Workflow Creation

        private async Task<Customer> CreateCustomerFromEmail(
            IncomingEmail email,
            EmailAnalysisResult analysisResult,
            string currentUser)
        {
            // Use body-parsed values from CustomerInfo (set for Kendlebell emails) with header values as fallback
            var effectiveEmail = analysisResult.CustomerInfo.TryGetValue("fromEmail", out var ce) && !string.IsNullOrWhiteSpace(ce)
                ? ce : email.FromEmail;

            // Hard safety guard — never persist a Kendlebell address as a customer email
            if (effectiveEmail.EndsWith("kbell.ie", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError(
                    "🚫 Blocked attempt to create customer with Kendlebell email {Email} — this should have been caught earlier",
                    effectiveEmail);
                throw new InvalidOperationException(
                    $"Customer creation blocked: Kendlebell address '{effectiveEmail}' cannot be used as a customer email.");
            }
            var effectiveName = analysisResult.CustomerInfo.TryGetValue("fromName", out var cn) && !string.IsNullOrWhiteSpace(cn)
                ? cn : (email.FromName ?? "Unknown Customer");
            var phone = analysisResult.CustomerInfo.TryGetValue("phone", out var ph) ? ph : "";
            var companyNumber = analysisResult.CustomerInfo.TryGetValue("companyNumber", out var co) ? co : "";
            var isResidential = !analysisResult.CustomerInfo.TryGetValue("residential", out var res)
                || string.Equals(res, "true", StringComparison.OrdinalIgnoreCase);

            var companyDto = new CompanyWithContactDto
            {
                Name = effectiveName,
                Email = effectiveEmail,
                Phone = phone,
                CompanyNumber = companyNumber,
                Residential = isResidential,
                Contacts = new List<CustomerContactDto>
                {
                    new CustomerContactDto
                    {
                        FirstName = GetFirstName(effectiveName),
                        LastName = GetLastName(effectiveName),
                        Email = effectiveEmail,
                        Phone = phone
                    }
                }
            };

            return await _customerService.SaveCompanyWithContact(companyDto, currentUser);
        }

        private async Task<WorkflowStart> GetOrCreateWorkflow(int customerId, string currentUser)
        {
            var existing = await _context.WorkflowStarts
                .Where(w => w.CustomerId == customerId)
                .OrderByDescending(w => w.DateCreated)
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing;

            var workflowDto = new WorkflowDto
            {
                CustomerId = customerId,
                Description = "Workflow from email enquiry",
                InitialEnquiry = true,
                CreateQuotation = false,
                InviteShowRoomVisit = false,
                SetupSiteVisit = false,
                InvoiceSent = false,
                ProductId = 1,
                ProductTypeId = 1
            };

            return await _workflowService.CreateWorkflow(workflowDto, currentUser);
        }

        private async Task<InitialEnquiry> CreateInitialEnquiry(
            int workflowId,
            IncomingEmail email,
            EmailAnalysisResult analysisResult,
            string currentUser,
            string effectiveEmail = null)
        {
            var enquiryDto = new InitialEnquiryDto
            {
                WorkflowId = workflowId,
                Email = effectiveEmail ?? email.FromEmail,
                Comments = $"Subject: {email.Subject}\n\n{email.BodyPreview ?? ""}",
                Images = null
            };

            return await _workflowService.AddInitialEnquiry(enquiryDto, currentUser);
        }

        private static bool IsImportLeadsEmail(string? extractedData)
        {
            if (string.IsNullOrWhiteSpace(extractedData)) return false;
            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(extractedData);
                return data != null &&
                       data.TryGetValue("source", out var source) &&
                       string.Equals(source?.ToString(), "ImportLeads", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private string GetFirstName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "Unknown";
            var parts = fullName.Split(' ');
            return parts[0];
        }

        private string GetLastName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "";
            var parts = fullName.Split(' ');
            return parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";
        }

        #endregion

        #region Task Creation

        private async Task CreateTaskFromEmail(IncomingEmail email, EmailAnalysisResult analysisResult)
        {
            try
            {
                _logger.LogInformation($"📋 Creating task from email: {email.Subject}");

                // TaskService will use the email's ExtractedData, Category, and CategoryConfidence
                // which were already set from AI analysis
                var task = await _taskService.CreateTaskFromEmailAsync(email.Id, "System");

                if (task != null)
                {
                    _logger.LogInformation($"✅ Task created: ID={task.TaskId}, Category={task.Category}, Priority={task.Priority}");
                }
                else
                {
                    _logger.LogWarning($"⚠️ Task creation returned null for email {email.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error creating task from email {email.Id}");
                // Don't throw - email is still marked as processed
            }
        }

        #endregion

        #region Attachment Processing

        private async Task ExtractTextFromAttachmentsAsync(IncomingEmail email)
        {
            foreach (var attachment in email.Attachments)
            {
                try
                {
                    if (string.IsNullOrEmpty(attachment.Base64Content))
                        continue;

                    var content = Convert.FromBase64String(attachment.Base64Content);

                    if (attachment.ContentType.Contains("pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"📄 Extracting text from PDF: {attachment.FileName}");
                        attachment.ExtractedText = await _emailAnalysisService.ExtractTextFromPdfAsync(content);
                    }
                    else if (attachment.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"🖼️ Extracting text from image: {attachment.FileName}");
                        attachment.ExtractedText = await _emailAnalysisService.ExtractTextFromImageAsync(content);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ Error extracting text from attachment: {attachment.FileName}");
                }
            }
        }

        #endregion

        #region Optional Email Routing

        private async Task RouteEmailAsync(IncomingEmail email, EmailAnalysisResult analysisResult)
        {
            try
            {
                var routingConfig = _configuration.GetSection($"EmailRouting:{email.Category}");
                var apiEndpoint = routingConfig["ApiEndpoint"];

                if (string.IsNullOrEmpty(apiEndpoint))
                {
                    _logger.LogInformation($"ℹ️ No API endpoint configured for category: {email.Category}");
                    return;
                }

                _logger.LogInformation($"🔀 Routing email to: {apiEndpoint}");

                // Prepare payload
                var payload = new
                {
                    emailId = email.Id,
                    category = email.Category,
                    confidence = email.CategoryConfidence,
                    subject = email.Subject,
                    from = new
                    {
                        name = email.FromName,
                        email = email.FromEmail
                    },
                    bodyPreview = email.BodyPreview,
                    bodyContent = email.BodyContent,
                    receivedDateTime = email.ReceivedDateTime,
                    importance = email.Importance,
                    hasAttachments = email.HasAttachments,
                    attachments = email.Attachments.Select(a => new
                    {
                        fileName = a.FileName,
                        contentType = a.ContentType,
                        size = a.Size,
                        extractedText = a.ExtractedText
                    }).ToList(),
                    extractedData = analysisResult.ExtractedData,
                    requiredActions = analysisResult.RequiredActions,
                    priority = analysisResult.Priority,
                    reasoning = analysisResult.Reasoning
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add custom headers if configured
                var headers = routingConfig.GetSection("Headers");
                foreach (var header in headers.GetChildren())
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                var response = await _httpClient.PostAsync(apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"✅ Successfully routed email to {apiEndpoint}");
                }
                else
                {
                    _logger.LogError($"❌ Failed to route email to {apiEndpoint}. Status: {response.StatusCode}, Error: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error routing email to API for category: {email.Category}");
            }
        }

        #endregion
    }
}