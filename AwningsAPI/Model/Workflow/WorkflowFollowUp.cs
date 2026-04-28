using System;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Workflow
{
    /// <summary>
    /// Represents a follow-up generated when the most-recent InitialEnquiry
    /// on a workflow is older than 3 days and no quote has been raised yet.
    ///
    /// TIMER RESET RULE:
    ///   - Follow-up is auto-dismissed when a NEW InitialEnquiry is added to
    ///     the same workflow (WorkflowService.AddInitialEnquiry calls DismissForWorkflow).
    ///   - After 3 more days a fresh follow-up is created for the new enquiry.
    /// </summary>
    public class WorkflowFollowUp
    {
        [Key]
        public int FollowUpId { get; set; }

        // ── Workflow / Customer ────────────────────────────────────────────────
        public int WorkflowId { get; set; }
        public int? CustomerId { get; set; }
        public string? CompanyName { get; set; }

        // ── Source enquiry (the one that triggered the follow-up) ─────────────
        /// <summary>FK to the InitialEnquiry record that started the 3-day clock.</summary>
        public int EnquiryId { get; set; }

        /// <summary>
        /// DateCreated of the triggering InitialEnquiry — used as the timer anchor.
        /// Denormalised for quick querying without a join.
        /// </summary>
        public DateTime LastEnquiryDate { get; set; }

        /// <summary>Comments snapshot from the triggering enquiry (for grid display).</summary>
        public string? EnquiryComments { get; set; }

        /// <summary>Email address from the triggering enquiry.</summary>
        public string? EnquiryEmail { get; set; }

        // ── Follow-up metadata ─────────────────────────────────────────────────
        /// <summary>Subject line shown in the Follow-Ups grid.</summary>
        public string Subject { get; set; } = string.Empty;

        public string Category { get; set; } = "Inquiry";

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        // ── Resolution ────────────────────────────────────────────────────────
        public bool IsDismissed { get; set; } = false;
        public DateTime? ResolvedDate { get; set; }
        public string? ResolvedBy { get; set; }
        public string? Notes { get; set; }

        /// <summary>
        /// Reason for dismissal:
        ///   "NewEnquiry"  — a new enquiry was added (timer reset)
        ///   "UserDismiss" — a user manually dismissed it
        ///   "QuoteRaised" — the workflow moved to CreateQuote=true
        /// </summary>
        public string? DismissReason { get; set; }

        // ── Optional link back to the originating email task ──────────────────
        public int? TaskId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }
}