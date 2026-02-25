using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Tasks
{
    public class EmailTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public int IncomingEmailId { get; set; }


        [Required]
        [StringLength(255)]
        public string? FromName { get; set; }

        [Required]
        [StringLength(255)]
        public string? FromEmail { get; set; }

        // Subject from email
        [Required]
        [StringLength(500)]
        public string? Subject { get; set; }

        // Category display (e.g., "New Invoice", "New Quote", "Site Visit")
        [Required]
        [StringLength(100)]
        public string? Category { get; set; }

        // Status: "New", "More Info", "In Progress", "Closed", "Reopened", "Completed", "Junk"
        [Required]
        [StringLength(50)]
        public string? Status { get; set; } = "New";

        // Task Type for backend (invoice_creation, quote_creation, etc.)
        [StringLength(50)]
        public string? TaskType { get; set; }

        [StringLength(50)]
        public string? Priority { get; set; } = "Normal";

        // DateAdded (shown in table)
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        // Assignment (Assign To dropdown - e.g., "Deirdre", "Michael", "James")
        public int? AssignedToUserId { get; set; }

        [StringLength(255)]
        public string? AssignedToUserName { get; set; }

        public int? AssignedByUserId { get; set; }

        [StringLength(255)]
        public string? AssignedByUserName { get; set; }

        // Tracks the previous assignee when task is re-assigned (triggers "More Info" status)
        public int? PreviousAssignedToUserId { get; set; }

        [StringLength(255)]
        public string? PreviousAssignedToUserName { get; set; }

        // Company Number (shown in email viewer top-right)
        [StringLength(50)]
        public string? CompanyNumber { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(255)]
        public string? CustomerName { get; set; }

        [StringLength(255)]
        public string? CustomerEmail { get; set; }

        public int? WorkflowId { get; set; }

        // Email Body (Contents of Email box)
        [Column(TypeName = "nvarchar(MAX)")]
        public string? EmailBody { get; set; }

        // Indicates if email has attachments
        public bool HasAttachments { get; set; }

        // Action selected (Add Company, Generate Quote, etc.)
        [StringLength(100)]
        public string? SelectedAction { get; set; }

        // Processing dates
        public DateTime? DateProcessed { get; set; }

        [StringLength(100)]
        public string? ProcessedBy { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(100)]
        public string? CompletedBy { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? CompletionNotes { get; set; }

        // AI Analysis Results
        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExtractedData { get; set; } // JSON from AI

        public double? AIConfidence { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? AIReasoning { get; set; }

        // Audit fields
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateUpdated { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; } = "System";

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
        public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }

    public class TaskComment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string? CommentText { get; set; }

        public int UserId { get; set; }

        [StringLength(255)]
        public string? UserName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateUpdated { get; set; }

        public bool IsEdited { get; set; } = false;

        // Navigation Property
        [ForeignKey("TaskId")]
        public virtual EmailTask EmailTask { get; set; }
    }

    public class TaskAttachment
    {
        [Key]
        public int AttachmentId { get; set; }

        [Required]
        public int TaskId { get; set; }

        // Link to email attachment if from email
        public int? EmailAttachmentId { get; set; }

        [Required]
        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(100)]
        public string? FileType { get; set; }

        public long FileSize { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }

        [StringLength(500)]
        public string? BlobUrl { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExtractedText { get; set; }

        public DateTime DateUploaded { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? UploadedBy { get; set; }

        // Navigation Property
        [ForeignKey("TaskId")]
        public virtual EmailTask EmailTask { get; set; }
    }

    public class TaskHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } // Created, StatusChanged, Assigned, Updated, Completed, Commented

        [StringLength(50)]
        public string? OldValue { get; set; }

        [StringLength(50)]
        public string? NewValue { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Details { get; set; }

        // ── Audit-grid columns — denormalised from EmailTask at write time ────
        // These three columns power the front-end "Task Audit" tab.
        // Migration: dotnet ef migrations add AddTaskHistoryAuditColumns
        //            dotnet ef database update
        [StringLength(255)]
        public string? CustomerName { get; set; }

        [StringLength(500)]
        public string? Subject { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }
        // ─────────────────────────────────────────────────────────────────────

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        // Navigation Property
        [ForeignKey("TaskId")]
        public virtual EmailTask EmailTask { get; set; }

        //public string PerformedBy { get; set; } = string.Empty;
        //public DateTime PerformedDate { get; set; }
    }

    // Task Statistics Model
    public class TaskStatistics
    {
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int HighPriorityTasks { get; set; }
        public int UrgentTasks { get; set; }
        public Dictionary<string, int> TasksByType { get; set; } = new();
        public Dictionary<string, int> TasksByAssignee { get; set; } = new();
    }
}