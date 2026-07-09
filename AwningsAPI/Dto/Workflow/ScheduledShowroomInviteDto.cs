namespace AwningsAPI.Dto.SiteVisit
{
    public class ScheduledShowroomInviteDto
    {
        public int ShowroomInviteId { get; set; }
        public int WorkflowId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerAddress { get; set; }
        public string? CustomerPhone { get; set; }
        public string? SalesPersonName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
