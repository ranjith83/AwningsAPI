using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Workflow
{
    public class InitialEnquiry
    {
        [Key]
        public int EnquiryId { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }   
        public string Images { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }  
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }  

        //Navigation property to link to Workflow
        public int WorkflowId { get; set; } 
    }
}
