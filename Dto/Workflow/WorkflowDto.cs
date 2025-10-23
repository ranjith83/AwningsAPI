namespace AwningsAPI.Dto.Workflow
{
    public class WorkflowDto
    {
        public int WorkflowId { get; set; }
        public string ProductName { get; set; } 
        public string Description { get; set; }   
        public bool InitialEnquiry { get; set; }    
        public bool CreateQuotation { get; set; }   
        public bool InviteShowRoomVisit { get; set; }   
        public bool SetupSiteVisit { get; set; }    
        public bool InvoiceSent { get; set; }   
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }    
        public int CompanyId { get; set; }
        public int SupplierId { get; set; } 
        public int ProductId { get; set; }  
        public int ProductTypeId { get; set; }  
    }
}
