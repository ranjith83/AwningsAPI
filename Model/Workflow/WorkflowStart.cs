using AwningsAPI.Model.Suppliers;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Workflow
{
    public class WorkflowStart
    {
        [Key]
        public int WorkflowId { get; set; }
        public string Description { get; set; }
        public bool InitialEnquiry { get; set; }   
        public bool CreateQuote { get; set; }   
        public bool InviteShowRoom { get; set; }
        public bool SetupSiteVisit { get; set; }    
        public bool InvoiceSent { get; set; }   
        public DateTime DateCreated { get; set; }   
        public string CreatedBy { get; set; }  
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        //Naviation Properties
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int ProductTypeId { get; set; }  
        public int ProductId { get; set; } 
        public Product Product { get; set; }
        public int CompanyId { get; internal set; }
    }
}
