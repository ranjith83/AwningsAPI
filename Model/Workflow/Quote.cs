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

        /// <summary>
        /// 'Percentage', 'Fixed', or "" (empty string = no discount).
        /// DB column is NOT NULL — QuoteService uses "" as the no-discount sentinel.
        /// To allow null: run migration, change to string?, update QuoteService accordingly.
        /// </summary>
        [StringLength(20)]
        public string DiscountType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; } = 0;

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

        public bool IsFinal { get; set; } = false;

        public DateTime? FinalizedAt { get; set; }

        // Links a final quote back to its originating draft quote
        public int? DraftQuoteId { get; set; }

        [ForeignKey("DraftQuoteId")]
        public virtual Quote DraftQuote { get; set; }
    }

    public class QuoteItem
    {
        [Key]
        public int QuoteItemId { get; set; }

        [Required]
        public int QuoteId { get; set; }

        [Required]
        [StringLength(500)]  // Increased from 200 — supports free-text Extras descriptions
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

        public int? ProductItemId { get; set; }

        // Navigation Properties
        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }

        [ForeignKey("ProductItemId")]
        public virtual ProductItem ProductItem { get; set; }
    }
}