namespace AwningsAPI.Dto.Workflow
{
    public class InitialEnquiryDto
    {
        public int EnquiryId { get; set; }
        public int WorkflowId { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public string Images { get; set; }   
    }
}
