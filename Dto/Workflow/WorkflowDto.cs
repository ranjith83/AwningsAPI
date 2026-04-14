namespace AwningsAPI.Dto.Workflow
{
    /// <summary>
    /// Represents a single ShadePlus option (one description/price row for a given width).
    /// </summary>
    public class ShadePlusDto
    {
        public int ShadePlusId { get; set; }

        /// <summary>
        /// The description of this ShadePlus option (e.g. "Surcharge for height 210 cm with gearbox").
        /// Null for products that have a single unnamed entry — the frontend will use a plain text field.
        /// </summary>
        public string? Description { get; set; }

        public int WidthCm { get; set; }
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Returned by GetShadePlusOptionsForProduct.
    /// When HasMultiple is true the frontend shows a dropdown so the user can pick
    /// which surcharge applies; the selected Description is stored rather than "ShadePlus".
    /// When HasMultiple is false the frontend falls back to a single price + editable text field.
    /// </summary>
    public class ShadePlusOptionsDto
    {
        /// <summary>True when more than one distinct Description exists for this product + width.</summary>
        public bool HasMultiple { get; set; }

        /// <summary>All matching ShadePlus rows for the requested product + width.</summary>
        public List<ShadePlusDto> Options { get; set; } = new();
    }

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