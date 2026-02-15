using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Tasks
{
    public class EmailTaskDto
    {
        public int TaskId { get; set; }
        public int IncomingEmailId { get; set; }

        // Table display fields (From | Subject | Category | DateAdded)
        public string FromName { get; set; }  // "Joe Bloggs", "Deirdre"
        public string FromEmail { get; set; } // "JoeBloggs@awnings.com"
        public string Subject { get; set; }
        public string Category { get; set; }  // "New Invoice", "New Quote", "Site Visit"
        public DateTime DateAdded { get; set; }

        // Status for tabs
        public string Status { get; set; }  // "Pending", "Processed", "Junk"
        public string TaskType { get; set; } // Backend type: "invoice_creation", etc.
        public string Priority { get; set; }

        // Assignment fields (Assign To dropdown)
        public int? AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
        public int? AssignedByUserId { get; set; }
        public string AssignedByUserName { get; set; }

        // Email viewer fields
        public string CompanyNumber { get; set; }  // Shown top-right in viewer
        public string EmailBody { get; set; }      // Contents of Email
        public bool HasAttachments { get; set; }

        // Action dropdown
        public string SelectedAction { get; set; }

        // Additional fields
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int? WorkflowId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string CompletedBy { get; set; }
        public string CompletionNotes { get; set; }

        // AI Analysis
        public string ExtractedData { get; set; }
        public double? AIConfidence { get; set; }
        public string AIReasoning { get; set; }

        // Audit
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        // Related data
        public List<TaskCommentDto> Comments { get; set; } = new();
        public List<TaskAttachmentDto> Attachments { get; set; } = new();
        public List<TaskHistoryDto> History { get; set; } = new();
    }

    public class TaskCommentDto
    {
        public int CommentId { get; set; }
        public int TaskId { get; set; }
        public string CommentText { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsEdited { get; set; }
    }

    public class TaskAttachmentDto
    {
        public int AttachmentId { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public string BlobUrl { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UploadedBy { get; set; }
    }

    public class TaskHistoryDto
    {
        public int HistoryId { get; set; }
        public int TaskId { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Details { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateTaskDto
    {
        [Required]
        public int IncomingEmailId { get; set; }

        // Will be auto-filled from email
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string EmailBody { get; set; }
        public string CompanyNumber { get; set; }

        public string Priority { get; set; } = "Normal";
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? CustomerId { get; set; }
        public int? WorkflowId { get; set; }
    }

    public class UpdateTaskDto
    {
        public string Status { get; set; }  // Pending, Processed, Junk
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public string SelectedAction { get; set; }
        public int? CustomerId { get; set; }
        public int? WorkflowId { get; set; }
        public string CompanyNumber { get; set; }
    }

    public class UpdateTaskStatusDto
    {
        [Required]
        public string Status { get; set; }

        public string? CompletionNotes { get; set; }
    }

    public class AssignTaskDto
    {
        [Required]
        public int AssignedToUserId { get; set; }

        public string? Notes { get; set; }
    }

    public class AddCommentDto
    {
        [Required]
        public string CommentText { get; set; }
    }

    public class TaskFilterDto
    {
        public string? Status { get; set; }
        public string? TaskType { get; set; } 
        public string? Priority { get; set; } 
        public int? AssignedToUserId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "DateAdded";
        public string SortDirection { get; set; } = "DESC";
    }
}