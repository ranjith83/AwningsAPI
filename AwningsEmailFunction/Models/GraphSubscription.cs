using System.ComponentModel.DataAnnotations;

namespace AwningsEmailFunction.Models;

public class GraphSubscription
{
    [Key]
    public int Id { get; set; }
    public string SubscriptionId { get; set; } = string.Empty;
    public DateTimeOffset ExpiryDateTime { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
