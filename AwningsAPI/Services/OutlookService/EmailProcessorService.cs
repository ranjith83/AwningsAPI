using AwningsAPI.Database;
using AwningsAPI.Dto.Audit;
using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Workflow;
using AwningsAPI.Services.Tasks;
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
            HttpClient httpClient)
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
                if (!analysisResult.IsSpam && email.Category != "junk")
                {
                    await ExecuteAutomatedWorkflowAsync(email, analysisResult);
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

                // Mark as read and move to appropriate folder
                await _emailReaderService.MarkEmailAsReadAsync(mailboxEmail, email.EmailId);

                //var folderName = analysisResult.IsSpam ? "Junk" : $"Processed/{email.Category}";
                //await _emailReaderService.MoveEmailToFolderAsync(mailboxEmail, email.EmailId, folderName);

                // _logger.LogInformation($"✅ Email {email.EmailId} processed and moved to {folderName}");
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
                _logger.LogInformation($"🔄 Checking Initial Enquiry conditions for {email.FromEmail}");

                // ── 1. Customer must already exist ────────────────────────────────────
                var existingCustomer = await _context.Customers
                    .Include(c => c.CustomerContacts)
                    .FirstOrDefaultAsync(c =>
                        c.Email == email.FromEmail ||
                        c.CustomerContacts.Any(cc => cc.Email == email.FromEmail));

                if (existingCustomer == null)
                {
                    _logger.LogInformation(
                        $"⏭️ No existing customer found for '{email.FromEmail}' — skipping Initial Enquiry creation");
                    return;
                }

                _logger.LogInformation(
                    $"✅ Existing customer found: ID={existingCustomer.CustomerId}, Name={existingCustomer.Name}");

                // ── 2. Workflow must already exist for that customer ───────────────────
                var existingWorkflow = await _context.WorkflowStarts
                    .Where(w => w.CustomerId == existingCustomer.CustomerId)
                    .OrderByDescending(w => w.DateCreated)
                    .FirstOrDefaultAsync();

                if (existingWorkflow == null)
                {
                    _logger.LogInformation(
                        $"⏭️ Customer {existingCustomer.CustomerId} has no workflow — skipping Initial Enquiry creation");
                    return;
                }

                _logger.LogInformation(
                    $"✅ Existing workflow found: ID={existingWorkflow.WorkflowId}");

                // ── 3. Both exist — create the Initial Enquiry ────────────────────────
                var enquiry = await CreateInitialEnquiry(existingWorkflow.WorkflowId, email, analysisResult, currentUser);

                _logger.LogInformation(
                    $"✅ Initial enquiry created: ID={enquiry.EnquiryId} in workflow {existingWorkflow.WorkflowId}");

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
                throw;
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
            var phone = analysisResult.CustomerInfo.ContainsKey("phone")
                ? analysisResult.CustomerInfo["phone"]
                : "";

            var companyNumber = analysisResult.CustomerInfo.ContainsKey("companyNumber")
                ? analysisResult.CustomerInfo["companyNumber"]
                : "";

            var companyDto = new CompanyWithContactDto
            {
                Name = email.FromName ?? "Unknown Customer",
                Email = email.FromEmail,
                Phone = phone,
                CompanyNumber = companyNumber,
                Residential = true,
                Contacts = new List<CustomerContactDto>
                {
                    new CustomerContactDto
                    {
                        FirstName = GetFirstName(email.FromName),
                        LastName = GetLastName(email.FromName),
                        Email = email.FromEmail,
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
            string currentUser)
        {
            var enquiryDto = new InitialEnquiryDto
            {
                WorkflowId = workflowId,
                Email = email.FromEmail,
                Comments = $"Subject: {email.Subject}\n\n{email.BodyContent}",
                Images = null // Attachments would be processed here
            };

            return await _workflowService.AddInitialEnquiry(enquiryDto, currentUser);
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