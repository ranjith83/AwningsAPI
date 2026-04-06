namespace AwningsAPI.Dto.Workflow
{
    public class WorkflowDto
    {
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // ── Stage enabled flags (toggled by the user) ─────────────────────────
        public bool InitialEnquiry { get; set; }
        public bool CreateQuotation { get; set; }
        public bool InviteShowRoomVisit { get; set; }
        public bool SetupSiteVisit { get; set; }
        public bool InvoiceSent { get; set; }

        // ── Stage completed flags (computed server-side from real activity) ────
        /// <summary>True when at least one InitialEnquiry record exists for this workflow.</summary>
        public bool InitialEnquiryCompleted { get; set; }

        /// <summary>True when at least one Quote record exists for this workflow.</summary>
        public bool CreateQuotationCompleted { get; set; }

        /// <summary>True when at least one ShowroomInvite record exists for this workflow.</summary>
        public bool InviteShowRoomCompleted { get; set; }

        /// <summary>True when at least one SiteVisit record exists for this workflow.</summary>
        public bool SetupSiteVisitCompleted { get; set; }

        /// <summary>True when at least one Invoice record exists for this workflow.</summary>
        public bool InvoiceSentCompleted { get; set; }

        // ── Audit ─────────────────────────────────────────────────────────────
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int CompanyId { get; set; }

        /// <summary>
        /// The email task that triggered this workflow's creation.
        /// Populated when a user creates a workflow from the Tasks screen.
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// True when any dependency record exists (enquiry, quote, showroom, site visit or invoice).
        /// The delete button is locked in the UI when this is true.
        /// </summary>
        public bool HasDependencies { get; set; }
    }
}