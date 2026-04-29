using System.ComponentModel.DataAnnotations;

namespace AwningsEmailFunction.Models;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CompanyNumber { get; set; }
    public bool? Residential { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? County { get; set; }
    public string? Eircode { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }

    public ICollection<CustomerContact> CustomerContacts { get; set; } = new List<CustomerContact>();
}

public class CustomerContact
{
    [Key]
    public int ContactId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
