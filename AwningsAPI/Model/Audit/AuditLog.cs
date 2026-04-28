using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Audit
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public int AuditId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public int EntityId { get; set; }

        [MaxLength(255)]
        public string? EntityName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Action { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Changes { get; set; }

        [Required]
        public int PerformedBy { get; set; }

        [Required]
        [MaxLength(255)]
        public string PerformedByName { get; set; } = string.Empty;

        [Required]
        public DateTime PerformedAt { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public static class AuditAction
    {
        public const string CREATE = "CREATE";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string VIEW = "VIEW";
    }

    public static class AuditEntityType
    {
        public const string CUSTOMER = "CUSTOMER";
        public const string CONTACT = "CONTACT";
        public const string WORKFLOW = "WORKFLOW";
        public const string QUOTE = "QUOTE";
        public const string INVOICE = "INVOICE";
        public const string SITE_VISIT = "SITE_VISIT";
    }
}