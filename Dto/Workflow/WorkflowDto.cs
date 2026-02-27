namespace AwningsAPI.Dto.Workflow
{
    public class WorkflowDto
    {
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool InitialEnquiry { get; set; }
        public bool CreateQuotation { get; set; }
        public bool InviteShowRoomVisit { get; set; }
        public bool SetupSiteVisit { get; set; }
        public bool InvoiceSent { get; set; }
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
        /// Used to auto-link the task back to this workflow.
        /// </summary>
        public int? TaskId { get; set; }
    }
}