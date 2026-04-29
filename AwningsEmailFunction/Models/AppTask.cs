using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsEmailFunction.Models;

[Table("Tasks")]
public class AppTask
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    [StringLength(50)]
    public string SourceType { get; set; } = "Email";

    [StringLength(500)]
    public string? Title { get; set; }

    public int? IncomingEmailId { get; set; }

    [StringLength(255)]
    public string? FromName { get; set; }

    [StringLength(255)]
    public string? FromEmail { get; set; }

    [StringLength(500)]
    public string? Subject { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "New";

    [StringLength(50)]
    public string? TaskType { get; set; }

    [StringLength(50)]
    public string Priority { get; set; } = "Normal";

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public int? AssignedToUserId { get; set; }

    [StringLength(255)]
    public string? AssignedToUserName { get; set; }

    public int? AssignedByUserId { get; set; }

    [StringLength(255)]
    public string? AssignedByUserName { get; set; }

    public int? PreviousAssignedToUserId { get; set; }

    [StringLength(255)]
    public string? PreviousAssignedToUserName { get; set; }

    [StringLength(50)]
    public string? CompanyNumber { get; set; }

    public int? CustomerId { get; set; }

    [StringLength(255)]
    public string? CustomerName { get; set; }

    [StringLength(255)]
    public string? CustomerEmail { get; set; }

    public int? WorkflowId { get; set; }

    public int? SiteVisitId { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? EmailBody { get; set; }

    public bool HasAttachments { get; set; }

    [StringLength(100)]
    public string? SelectedAction { get; set; }

    public DateTime? DateProcessed { get; set; }

    [StringLength(100)]
    public string? ProcessedBy { get; set; }

    public DateTime? CompletedDate { get; set; }

    [StringLength(100)]
    public string? CompletedBy { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? CompletionNotes { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? ExtractedData { get; set; }

    public double? AIConfidence { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? AIReasoning { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime? DateUpdated { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; } = "System";

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    public virtual ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
    public virtual ICollection<TaskHistory> TaskHistories { get; set; } = new List<TaskHistory>();
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
    public virtual AppTask Task { get; set; } = null!;
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

    [StringLength(50)]
    public string? OldValue { get; set; }

    [StringLength(50)]
    public string? NewValue { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? Details { get; set; }

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
    public virtual AppTask Task { get; set; } = null!;
}
