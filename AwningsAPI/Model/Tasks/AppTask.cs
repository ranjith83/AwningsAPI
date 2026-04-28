using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Tasks
{
    // ─────────────────────────────────────────────────────────────────────────
    // RENAME GUIDE
    //
    //  Old class name : EmailTask          → AppTask
    //  Old DB table   : EmailTasks         → Tasks  (via [Table] attribute)
    //  Old nav refs   : .EmailTask         → .Task  (in child classes below)
    //  Old DbSet name : context.EmailTasks → context.Tasks  (update AppDbContext)
    //
    // After updating AppDbContext run:
    //   dotnet ef migrations add RenameEmailTasksToTasks
    //   dotnet ef database update
    //
    // The migration script at the bottom of this file shows the exact SQL.
    // ─────────────────────────────────────────────────────────────────────────

    [Table("Tasks")]
    public class AppTask
    {
        [Key]
        public int TaskId { get; set; }

        // ── Discriminator ─────────────────────────────────────────────────────
        /// <summary>
        /// Identifies where / how this task was created.
        /// "Email"     – created automatically from an incoming Outlook email
        /// "SiteVisit" – created automatically when a site visit is saved
        /// "Manual"    – created directly by a user in the Task Management screen
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SourceType { get; set; } = TaskSourceType.Email.ToString();

        // ── Display title ─────────────────────────────────────────────────────
        /// <summary>
        /// Human-readable title shown in task lists.
        /// For Email tasks this is auto-populated from Subject.
        /// For SiteVisit / Manual tasks this is set explicitly.
        /// </summary>
        [StringLength(500)]
        public string? Title { get; set; }

        // ── Email-origin fields (nullable for non-email tasks) ────────────────
        /// <summary>Null for SiteVisit / Manual tasks.</summary>
        public int? IncomingEmailId { get; set; }

        [StringLength(255)]
        public string? FromName { get; set; }

        [StringLength(255)]
        public string? FromEmail { get; set; }

        /// <summary>
        /// Raw email subject. For non-email tasks, use Title instead.
        /// </summary>
        [StringLength(500)]
        public string? Subject { get; set; }

        // ── Category (used for tab/filter grouping in the UI) ─────────────────
        // e.g. "New Invoice", "New Quote", "Site Visit", "General"
        [StringLength(100)]
        public string? Category { get; set; }

        // ── Status ────────────────────────────────────────────────────────────
        // "New" | "In Progress" | "More Info" | "Closed" | "Reopened" | "Completed" | "Junk"
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "New";

        // ── TaskType (AI / workflow discriminator) ────────────────────────────
        // e.g. "invoice_creation", "quote_creation", "site_visit", "manual"
        [StringLength(50)]
        public string? TaskType { get; set; }

        [StringLength(50)]
        public string Priority { get; set; } = "Normal";

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        // ── Assignment ────────────────────────────────────────────────────────
        public int? AssignedToUserId { get; set; }

        [StringLength(255)]
        public string? AssignedToUserName { get; set; }

        public int? AssignedByUserId { get; set; }

        [StringLength(255)]
        public string? AssignedByUserName { get; set; }

        public int? PreviousAssignedToUserId { get; set; }

        [StringLength(255)]
        public string? PreviousAssignedToUserName { get; set; }

        // ── Customer / Workflow context ───────────────────────────────────────
        [StringLength(50)]
        public string? CompanyNumber { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(255)]
        public string? CustomerName { get; set; }

        [StringLength(255)]
        public string? CustomerEmail { get; set; }

        public int? WorkflowId { get; set; }

        // ── Site Visit link ───────────────────────────────────────────────────
        /// <summary>
        /// Foreign key to SiteVisit.SiteVisitId.
        /// Populated only when SourceType == "SiteVisit".
        /// Allows the Angular task card to deep-link straight to the survey.
        /// </summary>
        public int? SiteVisitId { get; set; }

        // ── Email body / attachments ──────────────────────────────────────────
        [Column(TypeName = "nvarchar(MAX)")]
        public string? EmailBody { get; set; }

        public bool HasAttachments { get; set; }

        [StringLength(100)]
        public string? SelectedAction { get; set; }

        // ── Processing dates ──────────────────────────────────────────────────
        public DateTime? DateProcessed { get; set; }

        [StringLength(100)]
        public string? ProcessedBy { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(100)]
        public string? CompletedBy { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? CompletionNotes { get; set; }

        // ── AI Analysis ───────────────────────────────────────────────────────
        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExtractedData { get; set; }

        public double? AIConfidence { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? AIReasoning { get; set; }

        // ── Audit ─────────────────────────────────────────────────────────────
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateUpdated { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; } = "System";

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        // ── Navigation ────────────────────────────────────────────────────────
        public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
        public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Constants — use these everywhere instead of raw strings
    // ─────────────────────────────────────────────────────────────────────────
    public enum TaskSourceType
    {
        Email,
        SiteVisit,
        Manual
    }

    public static class TaskStatusValue
    {
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string MoreInfo = "More Info";
        public const string Closed = "Closed";
        public const string Reopened = "Reopened";
        public const string Completed = "Completed";
        public const string Junk = "Junk";
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Child entities — only the navigation property name changes (Task not EmailTask)
    // ─────────────────────────────────────────────────────────────────────────

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

        [ForeignKey("TaskId")]
        public virtual AppTask Task { get; set; }
    }

    public class TaskAttachment
    {
        [Key]
        public int AttachmentId { get; set; }

        [Required]
        public int TaskId { get; set; }

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

        [ForeignKey("TaskId")]
        public virtual AppTask Task { get; set; }
    }

    public class TaskHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;
        // Created | StatusChanged | Assigned | Unassigned | Updated
        // Completed | Commented | CustomerLinked | WorkflowLinked | SiteVisitLinked

        [StringLength(50)]
        public string? OldValue { get; set; }

        [StringLength(50)]
        public string? NewValue { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Details { get; set; }

        // Denormalised audit-grid columns (written at insert time)
        [StringLength(255)]
        public string? CustomerName { get; set; }

        [StringLength(500)]
        public string? Subject { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [ForeignKey("TaskId")]
        public virtual AppTask Task { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Statistics — unchanged
    // ─────────────────────────────────────────────────────────────────────────

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
        public Dictionary<string, int> TasksBySource { get; set; } = new(); // NEW
    }
}