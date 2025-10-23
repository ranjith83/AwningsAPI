using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwiningsIreland_WebAPI.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkflowId { get; set; }

        [Required]
        [StringLength(100)]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; }

        [StringLength(500)]
        public string CustomerAddress { get; set; }

        [StringLength(100)]
        public string CustomerEmail { get; set; }

        [StringLength(20)]
        public string CustomerPhone { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue, Cancelled

        [StringLength(1000)]
        public string Notes { get; set; }

        [StringLength(2000)]
        public string Terms { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        [StringLength(100)]
        public string UpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

        public virtual ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();
    }

    public class InvoiceItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

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

        [StringLength(50)]
        public string Unit { get; set; } = "pcs";

        public int SortOrder { get; set; }

        // Navigation Property
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
    }

    public class InvoicePayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // Cash, Card, Bank Transfer, Cheque

        [StringLength(100)]
        public string TransactionReference { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string CreatedBy { get; set; }

        // Navigation Property
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
    }
}

