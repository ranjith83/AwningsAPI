using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AwningsEmailFunction.Services;

public class EmailProcessorService : IEmailProcessorService
{
    private readonly EmailFunctionDbContext _context;
    private readonly IEmailReaderService _emailReaderService;
    private readonly IEmailAnalysisService _analysisService;
    private readonly IBlobEmailStorageService _blobService;
    private readonly ILogger<EmailProcessorService> _logger;

    public EmailProcessorService(
        EmailFunctionDbContext context,
        IEmailReaderService emailReaderService,
        IEmailAnalysisService analysisService,
        IBlobEmailStorageService blobService,
        ILogger<EmailProcessorService> logger)
    {
        _context = context;
        _emailReaderService = emailReaderService;
        _analysisService = analysisService;
        _blobService = blobService;
        _logger = logger;
    }

    public async Task ProcessIncomingEmailAsync(string messageId, string mailboxEmail)
    {
        _logger.LogInformation("Processing incoming email {MessageId} from mailbox {Mailbox}", messageId, mailboxEmail);

        IncomingEmail? email = null;
        try
        {
            email = await FetchAndSaveEmailAsync(messageId, mailboxEmail);
            if (email == null) return;

            await AnalyzeEmailAsync(email);

            var task = await CreateTaskAsync(email);
            await AutoLinkCustomerAndWorkflowAsync(task, email.FromEmail);
            if (IsInitialEnquiry(email))
                await CreateInitialEnquiryAsync(task, email);

            await MarkCompletedAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process email {MessageId}", messageId);

            if (email != null)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.IncomingEmails.Attach(email);
                    email.ProcessingStatus = "Failed";
                    email.ErrorMessage = ex.Message[..Math.Min(ex.Message.Length, 500)];
                    email.DateProcessed = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                catch (Exception saveEx)
                {
                    _logger.LogError(saveEx, "Failed to update error status for email {MessageId}", messageId);
                }
            }

            throw; // let the queue retry the message
        }
    }

    #region Step 1 — Fetch and Save

    private async Task<IncomingEmail?> FetchAndSaveEmailAsync(string messageId, string mailboxEmail)
    {
        var existing = await _context.IncomingEmails
            .Include(e => e.Attachments)
            .FirstOrDefaultAsync(e => e.EmailId == messageId);

        if (existing != null)
        {
            if (existing.ProcessingStatus == "Completed")
            {
                _logger.LogInformation("Email {MessageId} already completed — skipping", messageId);
                return null;
            }
            _logger.LogInformation("Email {MessageId} found with status {Status} — retrying", messageId, existing.ProcessingStatus);
            return existing;
        }

        var fetched = await _emailReaderService.GetCompleteEmailAsync(mailboxEmail, messageId);

        var bodyBlobUrl = await UploadBodyToBlobAsync(fetched.EmailId, fetched.BodyContent, fetched.IsHtml);

        var email = new IncomingEmail
        {
            EmailId = fetched.EmailId,
            Subject = fetched.Subject,
            FromEmail = fetched.FromEmail,
            FromName = fetched.FromName,
            BodyPreview = fetched.BodyPreview,
            BodyContent = fetched.BodyContent,
            BodyBlobUrl = bodyBlobUrl,
            IsHtml = fetched.IsHtml,
            ReceivedDateTime = fetched.ReceivedDateTime,
            HasAttachments = fetched.HasAttachments,
            Importance = fetched.Importance,
            ProcessingStatus = "Pending",
            DateCreated = DateTime.UtcNow
        };

        foreach (var att in fetched.Attachments)
        {
            var attBlobUrl = await UploadAttachmentToBlobAsync(fetched.EmailId, att);

            email.Attachments.Add(new EmailAttachment
            {
                AttachmentId = att.AttachmentId,
                FileName = att.FileName,
                ContentType = att.ContentType,
                Size = att.Size,
                IsInline = att.IsInline,
                Base64Content = att.Base64Content,
                BlobStorageUrl = attBlobUrl ?? att.BlobStorageUrl,
                ExtractedText = att.ExtractedText,
                DateDownloaded = DateTime.UtcNow
            });
        }

        try
        {
            _context.IncomingEmails.Add(email);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_IncomingEmails_EmailId_Unique") == true)
        {
            // Two concurrent executions both passed the AnyAsync check before either saved.
            // The other instance won — this one can safely skip.
            _logger.LogInformation("Email {MessageId} already saved by a concurrent execution — skipping", messageId);
            return null;
        }

        _logger.LogInformation("Saved email {MessageId} — Subject: {Subject}, Id: {Id}", messageId, email.Subject, email.Id);
        return email;
    }

    #endregion

    #region Step 2 — AI Analysis

    private async Task AnalyzeEmailAsync(IncomingEmail email)
    {
        email.ProcessingStatus = "Processing";
        await _context.SaveChangesAsync();

        var analysis = await _analysisService.AnalyzeEmailAsync(email);

        email.Category = analysis.Category;
        email.CategoryConfidence = analysis.Confidence;
        email.ExtractedData = JsonConvert.SerializeObject(analysis.ExtractedData);
        await _context.SaveChangesAsync();

        _logger.LogInformation("AI analysis complete — Category: {Category}, Confidence: {Confidence:P0}",
            analysis.Category, analysis.Confidence);
    }

    #endregion

    #region Step 3 — Task Creation

    private async Task<AppTask> CreateTaskAsync(IncomingEmail email)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.IncomingEmailId == email.Id);
        if (existingTask != null)
        {
            _logger.LogInformation("Task already exists for email {Id} — reusing task {TaskId}", email.Id, existingTask.TaskId);
            return existingTask;
        }

        var priority = DeterminePriority(email.Importance, email.Category);
        var displayCategory = MapCategoryToDisplay(email.Category);

        var task = new AppTask
        {
            SourceType = "Email",
            Title = email.Subject,
            IncomingEmailId = email.Id,
            FromName = email.FromName,
            FromEmail = email.FromEmail,
            Subject = email.Subject,
            Category = displayCategory,
            TaskType = email.Category,
            EmailBody = email.BodyContent,
            Priority = priority,
            Status = "New",
            HasAttachments = email.HasAttachments,
            ExtractedData = email.ExtractedData,
            AIConfidence = email.CategoryConfidence,
            DueDate = CalculateDueDate(priority),
            DateAdded = DateTime.UtcNow,
            DateCreated = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        await AddHistoryAsync(task.TaskId, "Created", null, null,
            "Task created from email", "System",
            subject: email.Subject, category: displayCategory);

        if (email.HasAttachments && email.Attachments.Any())
            await CopyAttachmentsToTaskAsync(task.TaskId, email.Attachments);

        _logger.LogInformation("Task {TaskId} created — Category: {Category}, Priority: {Priority}",
            task.TaskId, displayCategory, priority);

        return task;
    }

    private async Task CopyAttachmentsToTaskAsync(int taskId, IEnumerable<EmailAttachment> attachments)
    {
        foreach (var att in attachments)
        {
            _context.TaskAttachments.Add(new TaskAttachment
            {
                TaskId = taskId,
                EmailAttachmentId = att.Id,
                FileName = att.FileName,
                FileType = att.ContentType,
                FileSize = att.Size,
                BlobUrl = att.BlobStorageUrl,
                ExtractedText = att.ExtractedText,
                DateUploaded = DateTime.UtcNow,
                UploadedBy = "System"
            });
        }
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Step 4 — Auto-link Customer and Workflow

    private async Task AutoLinkCustomerAndWorkflowAsync(AppTask task, string? fromEmail)
    {
        if (string.IsNullOrWhiteSpace(fromEmail)) return;

        try
        {
            var customer = await _context.Customers
                .Include(c => c.CustomerContacts)
                .FirstOrDefaultAsync(c =>
                    (c.Email != null && c.Email.ToLower() == fromEmail.ToLower()) ||
                    c.CustomerContacts.Any(cc => cc.Email != null && cc.Email.ToLower() == fromEmail.ToLower()));

            if (customer == null)
            {
                _logger.LogInformation("No existing customer found for {FromEmail}", fromEmail);
                return;
            }

            task.CustomerId = customer.CustomerId;
            task.CustomerName = customer.Name;
            task.CompanyNumber ??= customer.CompanyNumber;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = "System";

            var workflows = await _context.WorkflowStarts
                .Where(w => w.CustomerId == customer.CustomerId)
                .ToListAsync();

            var hasSingleWorkflow = workflows.Count == 1;
            if (hasSingleWorkflow)
            {
                task.WorkflowId = workflows[0].WorkflowId;
                task.Status = "Completed";
                task.CompletedDate = DateTime.UtcNow;
                task.CompletedBy = "System";
                task.CompletionNotes =
                    $"Auto-completed: customer '{customer.Name}' and workflow {task.WorkflowId} matched from sender email.";
            }

            await _context.SaveChangesAsync();

            await AddHistoryAsync(task.TaskId, "CustomerLinked", null, customer.CustomerId.ToString(),
                hasSingleWorkflow
                    ? $"Auto-linked customer '{customer.Name}' and workflow {task.WorkflowId}"
                    : $"Auto-linked customer '{customer.Name}'",
                "System", customer.Name, task.Subject, task.Category);

            if (hasSingleWorkflow)
            {
                await AddHistoryAsync(task.TaskId, "WorkflowLinked", null, task.WorkflowId.ToString(),
                    $"Workflow {task.WorkflowId} auto-linked", "System", customer.Name, task.Subject, task.Category);

                await AddHistoryAsync(task.TaskId, "Completed", "New", "Completed",
                    task.CompletionNotes, "System", customer.Name, task.Subject, task.Category);
            }

            _logger.LogInformation("Auto-linked customer {CustomerId} ({Name}) to task {TaskId}",
                customer.CustomerId, customer.Name, task.TaskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AutoLinkCustomerAndWorkflow failed for task {TaskId}", task.TaskId);
        }
    }

    #endregion

    #region Step 5 — Initial Enquiry

    private async Task CreateInitialEnquiryAsync(AppTask task, IncomingEmail email)
    {
        try
        {
            if (task.WorkflowId == null) return;

            var exists = await _context.InitialEnquiries.AnyAsync(e => e.TaskId == task.TaskId);
            if (exists) return;

            var comments = $"Email subject: {email.Subject}";
            if (!string.IsNullOrWhiteSpace(email.BodyPreview))
                comments += email.BodyPreview[..Math.Min(email.BodyPreview.Length, 500)];

            _context.InitialEnquiries.Add(new InitialEnquiry
            {
                WorkflowId = task.WorkflowId.Value,
                Comments = comments,
                Email = email.FromEmail ?? string.Empty,
                TaskId = task.TaskId,
                IncomingEmailId = email.Id,
                DateCreated = email.ReceivedDateTime != default ? email.ReceivedDateTime : DateTime.UtcNow,
                CreatedBy = "System (email processor)"
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("InitialEnquiry created for task {TaskId} on workflow {WorkflowId}",
                task.TaskId, task.WorkflowId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateInitialEnquiry failed for task {TaskId}", task.TaskId);
        }
    }

    #endregion

    #region Step 6 — Mark Completed

    private async Task MarkCompletedAsync(IncomingEmail email)
    {
        email.ProcessingStatus = "Completed";
        email.DateProcessed = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Email {Id} processing complete", email.Id);
    }

    #endregion

    #region Helpers

    private async Task AddHistoryAsync(int taskId, string action, string? oldValue, string? newValue,
        string? details, string createdBy,
        string? customerName = null, string? subject = null, string? category = null)
    {
        _context.TaskHistories.Add(new TaskHistory
        {
            TaskId = taskId,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            Details = details,
            DateCreated = DateTime.UtcNow,
            CreatedBy = createdBy,
            CustomerName = customerName,
            Subject = subject,
            Category = category
        });
        await _context.SaveChangesAsync();
    }

    private static bool IsJunk(IncomingEmail email) =>
        string.Equals(email.Category, "junk", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(email.Category, "general", StringComparison.OrdinalIgnoreCase);

    private static bool IsInitialEnquiry(IncomingEmail email) =>
        string.Equals(email.Category, "enquiry", StringComparison.OrdinalIgnoreCase);

    private static string MapCategoryToDisplay(string? category) => category switch
    {
        "enquiry" => "Enquiry",
        "site_visit" => "Site Visit",
        "invoice" => "Invoice",
        "quote" => "Quote",
        "showroom" => "Showroom",
        "complaint" => "Complaint",
        _ => category ?? "General"
    };

    private static string DeterminePriority(string? importance, string? category)
    {
        if (category == "complaint") return "Urgent";
        if (importance == "High") return "High";
        if (category is "invoice" or "quote") return "High";
        return "Normal";
    }

    private static DateTime CalculateDueDate(string priority) => priority switch
    {
        "Urgent" => DateTime.UtcNow.AddDays(1),
        "High" => DateTime.UtcNow.AddDays(3),
        "Normal" => DateTime.UtcNow.AddDays(7),
        _ => DateTime.UtcNow.AddDays(14)
    };

    private async Task<string?> UploadBodyToBlobAsync(string emailId, string? bodyContent, bool isHtml)
    {
        if (string.IsNullOrEmpty(bodyContent)) return null;
        try
        {
            return await _blobService.UploadEmailBodyAsync(emailId, bodyContent, isHtml);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload email body to blob for {EmailId} — body stored in DB only", emailId);
            return null;
        }
    }

    private async Task<string?> UploadAttachmentToBlobAsync(string emailId, EmailAttachment att)
    {
        if (string.IsNullOrEmpty(att.Base64Content)) return null;
        try
        {
            var bytes = Convert.FromBase64String(att.Base64Content);
            return await _blobService.UploadAttachmentAsync(emailId, att.AttachmentId, att.FileName, bytes, att.ContentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload attachment {FileName} to blob for {EmailId} — base64 stored in DB only", att.FileName, emailId);
            return null;
        }
    }

    #endregion
}
