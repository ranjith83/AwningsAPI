using System.ComponentModel.DataAnnotations;

namespace AwningsEmailFunction.Models;

public class WorkflowStart
{
    [Key]
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CustomerId { get; set; }
    public DateTime DateCreated { get; set; }
    public string? CreatedBy { get; set; }
}
