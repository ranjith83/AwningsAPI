using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Customers
{
    public class Customer
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string? CompanyNumber { get; set; }
        public bool? Residential { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? VATNumber { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? County { get; set; }
        public int? CountryId { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? Eircode { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public ICollection<CustomerContact> CustomerContacts { get; set; }
    }
}
