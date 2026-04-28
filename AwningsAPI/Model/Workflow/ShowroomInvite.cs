using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Workflow;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Showroom
{
    public class ShowroomInvite
    {
        [Key]
        public int ShowroomInviteId { get; set; }

        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(255)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string TimeSlot { get; set; } = string.Empty;

        public bool EmailClient { get; set; }

        [MaxLength(255)]
        public string? OutlookEventId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        public DateTime? CompletedDate { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Audit fields
        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MaxLength(255)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? DateUpdated { get; set; }

        [MaxLength(255)]
        public string? UpdatedBy { get; set; }

        // Navigation properties
        [ForeignKey("WorkflowId")]
        public virtual WorkflowStart? Workflow { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}