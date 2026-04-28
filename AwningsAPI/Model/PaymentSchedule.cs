using AwiningsIreland_WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Workflow
{
    [Table("PaymentSchedules")]
    public class PaymentSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Percentage { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountDue { get; set; }

        [Required]
        [MaxLength(100)]
        public string Reference { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(255)]
        public string UpdatedBy { get; set; }
    }
}