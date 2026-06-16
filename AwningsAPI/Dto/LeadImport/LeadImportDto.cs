namespace AwningsAPI.Dto.LeadImport
{
    public class LeadImportResultDto
    {
        public int TotalEmails { get; set; }
        public int Created { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
        public List<LeadImportItemDto> Results { get; set; } = new();
    }

    public class LeadImportItemDto
    {
        public string Subject { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public DateTime? ReceivedAt { get; set; }
        public string Status { get; set; } = "";   // Created | Skipped | Failed
        public string? CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public string? Note { get; set; }
        public string? EnquiryNotes { get; set; }  // Message / Comments extracted from the form
        public string? Error { get; set; }
    }
}
