using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Workflow
{
    public class Quote
    {
        [Key]
        public int QuoteId { get; set; }

        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public string QuoteNumber { get; set; }

        [Required]
        public DateTime QuoteDate { get; set; }

        [Required]
        public DateTime FollowUpDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [StringLength(2000)]
        public string Terms { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<QuoteItem> QuoteItems { get; set; } = new List<QuoteItem>();
        [Required]
        public int CustomerId { get; set; }
    }

    public class QuoteItem
    {
        [Key]
        public int QuoteItemId { get; set; }

        [Required]
        public int QuoteId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxRate { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public int SortOrder { get; set; }

        // Navigation Property
        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }
    }
}
