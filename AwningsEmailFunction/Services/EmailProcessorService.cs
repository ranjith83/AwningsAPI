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
    private readonly IEmailAnalysisService _analysisService;
    private readonly ILogger<EmailProcessorService> _logger;

    public EmailProcessorService(
        EmailFunctionDbContext context,
        IEmailAnalysisService analysisService,
        ILogger<EmailProcessorService> logger)
    {
        _context = context;
        _analysisService = analysisService;
        _logger = logger;
    }

    public async Task ProcessEmailAsync(int incomingEmailId)
    {
        var email = await _context.IncomingEmails
            .Include(e => e.Attachments)
            .FirstOrDefaultAsync(e => e.Id == incomingEmailId);

        if (email == null)
        {
            _logger.LogWarning("ProcessEmailAsync: email {Id} not found", incomingEmailId);
            return;
        }

        try
        {
            _logger.LogInformation("Processing email {Id}: {Subject} from {From}", email.Id, email.Subject, email.FromEmail);

            email.ProcessingStatus = "Processing";
            await _context.SaveChangesAsync();

            // AI analysis
            var analysis = await _analysisService.AnalyzeEmailAsync(email);

            email.Category = analysis.Category;
            email.CategoryConfidence = analysis.Confidence;
            email.ExtractedData = JsonConvert.SerializeObject(analysis.ExtractedData);
            await _context.SaveChangesAsync();

            _logger.LogInformation("AI result: {Category} ({Confidence:P0})", analysis.Category, analysis.Confidence);

            if (!analysis.IsSpam && analysis.Category != "junk")
            {
                var task = await CreateTaskAsync(email, analysis);
                await TryAutoLinkCustomerAndWorkflowAsync(task.TaskId, email.FromEmail);

                if (string.Equals(email.Category, "initial_enquiry", StringComparison.OrdinalIgnoreCase))
                    await TryCreateInitialEnquiryAsync(task.TaskId, email);
            }
            else
            {
                _logger.LogInformation("Junk email — no task created");
            }

            email.ProcessingStatus = "Completed";
            email.DateProcessed = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Email {Id} processed successfully", email.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing email {Id}", incomingEmailId);
            email.ProcessingStatus = "Failed";
            email.ErrorMessage = ex.Message;
            email.DateProcessed = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<AppTask> CreateTaskAsync(IncomingEmail email, EmailAnalysisResult analysis)
    {
        var priority = DeterminePriority(email.Importance, analysis.Category);
        var category = MapCategoryToDisplay(analysis.Category);

        var task = new AppTask
        {
            SourceType = "Email",
            Title = email.Subject,
            IncomingEmailId = email.Id,
            FromName = email.FromName,
            FromEmail = email.FromEmail,
            Subject = email.Subject,
            Category = category,
            TaskType = analysis.TaskType,
            EmailBody = email.BodyContent,
            Priority = priority,
            Status = "New",
            HasAttachments = email.HasAttachments,
            ExtractedData = email.ExtractedData,
            AIConfidence = analysis.Confidence,
            DueDate = CalculateDueDate(priority),
            DateAdded = DateTime.UtcNow,
            DateCreated = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        await AddHistoryAsync(task.TaskId, "Created", null, null,
            "Task created from email", "System", null, email.Subject, category);

        // Copy attachments from email to task
        if (email.HasAttachments && email.Attachments.Any())
        {
            foreach (var att in email.Attachments)
            {
                _context.TaskAttachments.Add(new TaskAttachment
                {
                    TaskId = task.TaskId,
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

        _logger.LogInformation("Task {TaskId} created — Category={Category}, Priority={Priority}",
            task.TaskId, category, priority);

        return task;
    }

    private async Task TryAutoLinkCustomerAndWorkflowAsync(int taskId, string? fromEmail)
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
                _logger.LogInformation("No customer found for {FromEmail}", fromEmail);
                return;
            }

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return;

            task.CustomerId = customer.CustomerId;
            task.CustomerName = customer.Name;
            task.CompanyNumber ??= customer.CompanyNumber;
            task.DateUpdated = DateTime.UtcNow;
            task.UpdatedBy = "System";

            var workflows = await _context.WorkflowStarts
                .Where(w => w.CustomerId == customer.CustomerId)
                .ToListAsync();

            var singleWorkflow = workflows.Count == 1;
            if (singleWorkflow)
                task.WorkflowId = workflows[0].WorkflowId;

            if (singleWorkflow)
            {
                task.Status = "Completed";
                task.CompletedDate = DateTime.UtcNow;
                task.CompletedBy = "System";
                task.CompletionNotes =
                    $"Auto-completed: customer '{customer.Name}' and workflow {task.WorkflowId} matched from sender email.";
            }

            await _context.SaveChangesAsync();

            await AddHistoryAsync(taskId, "CustomerLinked", null, customer.CustomerId.ToString(),
                singleWorkflow
                    ? $"Auto-linked customer '{customer.Name}' and workflow {task.WorkflowId}"
                    : $"Auto-linked customer '{customer.Name}'",
                "System", customer.Name, task.Subject, task.Category);

            if (singleWorkflow)
            {
                await AddHistoryAsync(taskId, "WorkflowLinked", null, task.WorkflowId.ToString(),
                    $"Workflow {task.WorkflowId} auto-linked", "System", customer.Name, task.Subject, task.Category);

                await AddHistoryAsync(taskId, "Completed", "New", "Completed",
                    task.CompletionNotes, "System", customer.Name, task.Subject, task.Category);
            }

            _logger.LogInformation("Auto-linked customer {CustomerId} to task {TaskId}", customer.CustomerId, taskId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TryAutoLinkCustomerAndWorkflow failed for task {TaskId}", taskId);
        }
    }

    private async Task TryCreateInitialEnquiryAsync(int taskId, IncomingEmail email)
    {
        try
        {
            var task = await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == taskId);
            if (task?.WorkflowId == null) return;

            var exists = await _context.InitialEnquiries.AnyAsync(e => e.TaskId == taskId);
            if (exists) return;

            var preview = email.BodyPreview ?? string.Empty;
            var comments = $"Email subject: {email.Subject}";
            if (!string.IsNullOrWhiteSpace(preview))
                comments += preview[..Math.Min(preview.Length, 500)];

            _context.InitialEnquiries.Add(new InitialEnquiry
            {
                WorkflowId = task.WorkflowId.Value,
                Comments = comments,
                Email = email.FromEmail ?? string.Empty,
                TaskId = taskId,
                IncomingEmailId = email.Id,
                DateCreated = email.ReceivedDateTime != default ? email.ReceivedDateTime : DateTime.UtcNow,
                CreatedBy = "System (email processor)"
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("InitialEnquiry created for task {TaskId} on workflow {WorkflowId}", taskId, task.WorkflowId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TryCreateInitialEnquiry failed for task {TaskId}", taskId);
        }
    }

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

    private string MapCategoryToDisplay(string category) => category switch
    {
        "invoice_due" => "Invoice Due",
        "quote_creation" => "New Quote",
        "showroom_booking" => "Showroom Booking",
        "complaint" => "Complaint",
        "site_visit_meeting" => "Site Visit",
        "initial_enquiry" => "initial_enquiry",
        _ => category ?? "Inquiry"
    };

    private string DeterminePriority(string? importance, string? category)
    {
        if (category == "complaint") return "Urgent";
        if (importance == "High") return "High";
        if (category is "invoice_due" or "quote_creation") return "High";
        return "Normal";
    }

    private DateTime CalculateDueDate(string priority) => priority switch
    {
        "Urgent" => DateTime.UtcNow.AddDays(1),
        "High" => DateTime.UtcNow.AddDays(3),
        "Normal" => DateTime.UtcNow.AddDays(7),
        _ => DateTime.UtcNow.AddDays(14)
    };
}
