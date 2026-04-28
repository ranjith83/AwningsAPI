using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Workflow
{
    public class PaymentScheduleDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
        public string Reference { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreatePaymentScheduleDto
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public string ProductCategory { get; set; }

        [Required]
        public List<CreatePaymentScheduleItemDto> ScheduleItems { get; set; } = new List<CreatePaymentScheduleItemDto>();
    }

    public class CreatePaymentScheduleItemDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 100)]
        public decimal Percentage { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public string Reference { get; set; }
    }

    public class UpdatePaymentScheduleItemDto
    {
        public string Description { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
    }

    public class RecordSchedulePaymentDto
    {
        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Reference { get; set; }

        public string Notes { get; set; }
    }

    public class PaymentScheduleSummaryDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDue { get; set; }
        public List<PaymentScheduleDto> ScheduleItems { get; set; } = new List<PaymentScheduleDto>();
    }
}