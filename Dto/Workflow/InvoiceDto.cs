using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Workflow
{
    public class InvoiceDto
    {

        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<InvoiceItemDto> InvoiceItems { get; set; } = new List<InvoiceItemDto>();
        public List<InvoicePaymentDto> InvoicePayments { get; set; } = new List<InvoicePaymentDto>();
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
    }

    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalPrice { get; set; }
        public string Unit { get; set; }
        public int SortOrder { get; set; }
    }

    public class InvoicePaymentDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateInvoiceDto
    {
        [Required]
        public int WorkflowId { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public string Notes { get; set; }
        public string Terms { get; set; }

        [Required]
        public List<CreateInvoiceItemDto> InvoiceItems { get; set; } = new List<CreateInvoiceItemDto>();
    }

    public class CreateInvoiceItemDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal TaxRate { get; set; } = 0;
        public decimal DiscountPercentage { get; set; } = 0;
        public string Unit { get; set; } = "pcs";
    }

    public class UpdateInvoiceDto
    {
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        public List<UpdateInvoiceItemDto> InvoiceItems { get; set; }
    }

    public class UpdateInvoiceItemDto
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Unit { get; set; }
    }

    public class CreatePaymentDto
    {
        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string TransactionReference { get; set; }
        public string Notes { get; set; }
    }
}