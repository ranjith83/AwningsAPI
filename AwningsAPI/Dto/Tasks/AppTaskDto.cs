using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Tasks
{
    // ─────────────────────────────────────────────────────────────────────────
    // Main task DTO — returned by GET endpoints and used in the task list / detail
    // ─────────────────────────────────────────────────────────────────────────
    public class AppTaskDto
    {
        public int TaskId { get; set; }

        // ── Discriminator ─────────────────────────────────────────────────────
        /// <summary>"Email" | "SiteVisit" | "Manual"</summary>
        public string SourceType { get; set; } = "Email";

        // ── Display title ─────────────────────────────────────────────────────
        /// <summary>
        /// The label shown in the task list header.
        /// Computed server-side: Title ?? Subject ?? "(No title)"
        /// </summary>
        public string DisplayTitle { get; set; } = string.Empty;

        // ── Email-origin fields (null for non-email tasks) ────────────────────
        public int? IncomingEmailId { get; set; }
        public string? FromName { get; set; }
        public string? FromEmail { get; set; }
        public string? Subject { get; set; }

        // ── Common fields ─────────────────────────────────────────────────────
        public string? Category { get; set; }
        public DateTime DateAdded { get; set; }
        public string Status { get; set; } = "New";
        public string? TaskType { get; set; }
        public string Priority { get; set; } = "Normal";

        // ── Assignment ────────────────────────────────────────────────────────
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public int? AssignedByUserId { get; set; }
        public string? AssignedByUserName { get; set; }
        public int? PreviousAssignedToUserId { get; set; }
        public string? PreviousAssignedToUserName { get; set; }

        // ── Email viewer ──────────────────────────────────────────────────────
        public string? CompanyNumber { get; set; }
        public string? EmailBody { get; set; }
        public bool HasAttachments { get; set; }
        public string? SelectedAction { get; set; }

        // ── Customer / Workflow ───────────────────────────────────────────────
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public int? WorkflowId { get; set; }

        // ── Site Visit deep-link ──────────────────────────────────────────────
        /// <summary>
        /// Populated when SourceType == "SiteVisit".
        /// The Angular task card uses this to build the route /site-visit/:siteVisitId.
        /// </summary>
        public int? SiteVisitId { get; set; }

        // ── Dates ─────────────────────────────────────────────────────────────
        public DateTime? DueDate { get; set; }
        public DateTime? DateProcessed { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? CompletedBy { get; set; }
        public string? CompletionNotes { get; set; }

        // ── AI Analysis ───────────────────────────────────────────────────────
        public string? ExtractedData { get; set; }
        public double? AIConfidence { get; set; }
        public string? AIReasoning { get; set; }

        // ── Audit ─────────────────────────────────────────────────────────────
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // ── Related collections ───────────────────────────────────────────────
        public List<TaskCommentDto> Comments { get; set; } = new();
        public List<TaskAttachmentDto> Attachments { get; set; } = new();
        public List<TaskHistoryDto> History { get; set; } = new();
    }

    // Keep the old name as a type alias so existing service/controller code
    // compiles without a rename sweep.  Remove once you've updated all usages.
    [Obsolete("Use AppTaskDto instead. This alias will be removed in a future release.")]
    public class EmailTaskDto : AppTaskDto { }

    // ─────────────────────────────────────────────────────────────────────────
    // Comment DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class TaskCommentDto
    {
        public int CommentId { get; set; }
        public int TaskId { get; set; }
        public string? CommentText { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsEdited { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Attachment DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class TaskAttachmentDto
    {
        public int AttachmentId { get; set; }
        public int TaskId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public long FileSize { get; set; }
        public string? FilePath { get; set; }
        public string? BlobUrl { get; set; }
        public DateTime DateUploaded { get; set; }
        public string? UploadedBy { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // History DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class TaskHistoryDto
    {
        public int HistoryId { get; set; }
        public int TaskId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Details { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }

        // Audit-grid columns (Created / Assigned / Unassigned only)
        public string? CustomerName { get; set; }
        public string? Subject { get; set; }
        public string? Category { get; set; }
        public string? AssignedTo { get; set; }
        public string? AssignedBy { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Paged history wrapper — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class TaskHistoryPagedDto
    {
        public List<TaskHistoryDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Create DTO
    // Key changes:
    //   • IncomingEmailId is now nullable — not required for SiteVisit / Manual
    //   • SourceType added — caller must always supply this
    //   • Title added — used when SourceType != Email
    // ─────────────────────────────────────────────────────────────────────────
    public class CreateTaskDto
    {
        // ── Required for ALL task types ───────────────────────────────────────
        [Required]
        public string SourceType { get; set; } = "Email";  // "Email" | "SiteVisit" | "Manual"

        // ── Required for Email tasks; null for everything else ────────────────
        public int? IncomingEmailId { get; set; }

        // ── Required for SiteVisit / Manual tasks ────────────────────────────
        /// <summary>
        /// Shown as the task header in the list.
        /// For Email tasks this defaults to Subject when left null.
        /// </summary>
        public string? Title { get; set; }

        // ── Site Visit link ───────────────────────────────────────────────────
        /// <summary>
        /// Set by SiteVisitController so the Angular card can deep-link.
        /// Null for Email / Manual tasks.
        /// </summary>
        public int? SiteVisitId { get; set; }

        // ── Common optional fields ────────────────────────────────────────────
        public string? FromName { get; set; }
        public string? FromEmail { get; set; }
        public string? Subject { get; set; }
        public string? Category { get; set; }
        public string? EmailBody { get; set; }
        public string? CompanyName { get; set; }
        public string Priority { get; set; } = "Normal";
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? CustomerId { get; set; }
        public int? WorkflowId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CompanyNumber { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Update DTO — unchanged apart from removing email-specific assumptions
    // ─────────────────────────────────────────────────────────────────────────
    public class UpdateTaskDto
    {
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? SelectedAction { get; set; }
        public int? CustomerId { get; set; }
        public int? WorkflowId { get; set; }
        public string? CompanyNumber { get; set; }
        public string? Title { get; set; }  // allow title edits on manual tasks
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Status update DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class UpdateTaskStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
        public string? CompletionNotes { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Assignment DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class AssignTaskDto
    {
        [Required]
        public int AssignedToUserId { get; set; }
        public string? Notes { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Comment DTO — unchanged
    // ─────────────────────────────────────────────────────────────────────────
    public class AddCommentDto
    {
        [Required]
        public string CommentText { get; set; } = string.Empty;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Filter DTO
    // Key changes:
    //   • SourceType / SourceTypes added for tab-level filtering
    //   • Statuses list was already present — kept
    // ─────────────────────────────────────────────────────────────────────────
    public class TaskFilterDto
    {
        // ── Source filter (drives tab selection in Angular) ───────────────────
        /// <summary>Single source filter. Overridden by SourceTypes when set.</summary>
        public string? SourceType { get; set; }

        /// <summary>
        /// Multi-source filter, e.g. ["Email"] for the Email Tasks tab,
        /// ["SiteVisit"] for the Site Visits tab, null for All Tasks.
        /// </summary>
        public List<string>? SourceTypes { get; set; }

        // ── Status filters ────────────────────────────────────────────────────
        public string? Status { get; set; }
        public List<string>? Statuses { get; set; }

        // ── Other filters — unchanged ─────────────────────────────────────────
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
        public int? CurrentUserId { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}