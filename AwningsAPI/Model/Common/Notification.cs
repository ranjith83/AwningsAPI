using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Common
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Type of notification, e.g. "new_enquiry".</summary>
        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>Related entity type, e.g. "InitialEnquiry".</summary>
        public string? EntityType { get; set; }

        public int? EntityId { get; set; }

        public int? WorkflowId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
