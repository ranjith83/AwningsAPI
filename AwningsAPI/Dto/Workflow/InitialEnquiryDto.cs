namespace AwningsAPI.Dto.Workflow
{
    /// <summary>
    /// DTO for InitialEnquiry records.
    ///
    /// Used for:
    ///  - GET  /api/workflow/GeInitialEnquiryForWorkflow → list for the workflow screen
    ///  - POST /api/workflow/AddInitialEnquiry           → save a manual or email-linked enquiry
    ///  - PUT  /api/workflow/UpdateInitialEnquiry        → edit an existing enquiry
    /// </summary>
    public class InitialEnquiryDto
    {
        public int? EnquiryId { get; set; }
        public int WorkflowId { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Images { get; set; }

        /// <summary>
        /// Plain-text email signature appended to outgoing emails.
        /// e.g. "Kindest regards,\nMichael Maguire\nAwnings of Ireland\n..."
        /// NULL for enquiries where no signature was supplied.
        /// </summary>
        public string? Signature { get; set; }

        // ── Email linkage (populated when created by the email processor) ─────
        /// <summary>
        /// The EmailTask.TaskId that originated this enquiry.
        /// Set by TaskService.CreateTaskFromEmailAsync when category = initial_enquiry.
        /// NULL for manually entered enquiries.
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// The IncomingEmail.Id (Graph email) that originated this enquiry.
        /// NULL for manually entered enquiries.
        /// </summary>
        public int? IncomingEmailId { get; set; }

        // ── Read-only audit fields (returned by GET, ignored by POST/PUT) ─────
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}