using System.ComponentModel.DataAnnotations;

namespace AwningsEmailFunction.Models;

public class InitialEnquiry
{
    [Key]
    public int EnquiryId { get; set; }
    public string Comments { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Images { get; set; }
    public string? Signature { get; set; }
    public DateTime DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string? UpdatedBy { get; set; }
    public int WorkflowId { get; set; }
    public int? IncomingEmailId { get; set; }
    public int? TaskId { get; set; }
}
