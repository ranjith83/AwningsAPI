using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsEmailFunction.Models;

public class IncomingEmail
{
    [Key]
    public int Id { get; set; }

    public string EmailId { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string BodyPreview { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(MAX)")]
    public string? BodyContent { get; set; }

    public string? BodyBlobUrl { get; set; }

    public bool IsHtml { get; set; }
    public DateTime ReceivedDateTime { get; set; }
    public bool HasAttachments { get; set; }
    public string Importance { get; set; } = "Normal";
    public string ProcessingStatus { get; set; } = "Pending";
    public string? Category { get; set; }
    public double? CategoryConfidence { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? ExtractedData { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateProcessed { get; set; }
    public string? ErrorMessage { get; set; }

    public virtual ICollection<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
}
