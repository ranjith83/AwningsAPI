using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Dto.Workflow
{
    public class QuoteDto
    {
        public int QuoteId { get; set; }
        public int WorkflowId { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime FollowUpDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? DiscountType { get; set; }    // ← nullable: null when no discount applied
        public decimal DiscountValue { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsFinal { get; set; }
        public DateTime? FinalizedAt { get; set; }
        public int? DraftQuoteId { get; set; }
        public virtual ICollection<QuoteItemDto> QuoteItems { get; set; } = new List<QuoteItemDto>();
        public int CustomerId { get; set; }
    }

    public class QuoteItemDto
    {
        public int QuoteItemId { get; set; }
        public int QuoteId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalPrice { get; set; }
        public int SortOrder { get; set; }
        public int? ProductItemId { get; set; }

        [ForeignKey("QuoteId")]
        public virtual QuoteDto Quote { get; set; }
    }

    public class CreateQuoteDto
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public DateTime QuoteDate { get; set; }

        [Required]
        public DateTime FollowUpDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public string Notes { get; set; }
        public string Terms { get; set; }

        /// <summary>
        /// Optional. Set to 'Percentage' or 'Fixed'. Leave null/empty for no discount.
        /// </summary>
        public string? DiscountType { get; set; }

        /// <summary>
        /// Optional. Only used when DiscountType is set. Defaults to 0.
        /// </summary>
        public decimal DiscountValue { get; set; } = 0;

        [Required]
        public List<CreateQuoteItemDto> QuoteItems { get; set; } = new List<CreateQuoteItemDto>();
    }

    public class CreateQuoteItemDto
    {
        [Required]
        [MaxLength(500)]  // Increased from 200 — supports free-text Extras descriptions
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]  // Allow 0 price — Extras can be informational
        public decimal UnitPrice { get; set; }

        public decimal TaxRate { get; set; } = 0;
        public decimal DiscountPercentage { get; set; } = 0;
        public int? ProductItemId { get; set; }
    }

    public class UpdateQuoteDto
    {
        public DateTime? QuoteDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        /// <summary>Set to null or empty string to clear the discount.</summary>
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public List<UpdateQuoteItemDto> QuoteItems { get; set; }
    }

    public class CreateFinalQuoteDto
    {
        [Required]
        public int DraftQuoteId { get; set; }

        [Required]
        public DateTime QuoteDate { get; set; }

        [Required]
        public DateTime FollowUpDate { get; set; }

        public string Notes { get; set; }
        public string Terms { get; set; }
        public string? DiscountType { get; set; }
        public decimal DiscountValue { get; set; } = 0;

        [Required]
        public List<CreateQuoteItemDto> QuoteItems { get; set; } = new List<CreateQuoteItemDto>();
    }

    public class UpdateQuoteItemDto
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? ProductItemId { get; set; }
    }
}