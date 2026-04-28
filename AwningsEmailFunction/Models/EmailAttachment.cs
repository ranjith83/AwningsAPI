using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsEmailFunction.Models;

public class EmailAttachment
{
    [Key]
    public int Id { get; set; }

    public int IncomingEmailId { get; set; }

    [Required]
    public string AttachmentId { get; set; } = string.Empty;

    [Required]
    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string? BlobStorageUrl { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? Base64Content { get; set; }

    public bool IsInline { get; set; }

    [Column(TypeName = "nvarchar(MAX)")]
    public string? ExtractedText { get; set; }

    public DateTime DateDownloaded { get; set; } = DateTime.UtcNow;

    [ForeignKey("IncomingEmailId")]
    public virtual IncomingEmail IncomingEmail { get; set; } = null!;
}
