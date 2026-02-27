namespace AwningsAPI.Dto.Workflow
{
    /// <summary>
    /// Data Transfer Object returned by GET /api/followup endpoints.
    ///
    /// Grid columns:
    ///   FollowUpId | Company | Subject | EnquiryEmail | LastEnquiryDate | DaysOpen | Actions
    ///
    /// Row click → navigate to /workflow/initial-enquiry?workflowId=&customerId=
    /// </summary>
    public class FollowUpDto
    {
        public int FollowUpId { get; set; }

        // ── Workflow / Customer ──────────────────────────────────────────────
        public int WorkflowId { get; set; }
        public int? CustomerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        // ── Enquiry data (source of the follow-up) ───────────────────────────
        public int EnquiryId { get; set; }
        public string? EnquiryEmail { get; set; }
        public string? EnquiryComments { get; set; }

        /// <summary>
        /// DateCreated of the InitialEnquiry that started the 3-day clock.
        /// The grid shows this as "Last Enquiry" and computes DaysOpen from it.
        /// </summary>
        public DateTime LastEnquiryDate { get; set; }

        // ── Display fields ───────────────────────────────────────────────────
        public string Subject { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }       // when the follow-up record was created
        public bool IsDismissed { get; set; }
        public string? DismissReason { get; set; }
        public string? Notes { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? ResolvedBy { get; set; }
        public int? TaskId { get; set; }
    }

    /// <summary>Body for POST /api/followup/{id}/dismiss</summary>
    public class DismissFollowUpDto
    {
        public string? Notes { get; set; }
    }
}