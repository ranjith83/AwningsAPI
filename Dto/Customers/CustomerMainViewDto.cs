namespace AwningsAPI.Dto.Customers
{
    public class CustomerMainViewDto
    {
        public int CompanyId { get; set; }  
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty; 
        public string SiteAddress   { get; set; } = string.Empty;
    }
}
