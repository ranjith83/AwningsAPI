using AwningsAPI.Dto.Workflow;

namespace AwningsAPI.Interfaces
{
    public interface IPaymentScheduleService
    {
        Task<IEnumerable<PaymentScheduleDto>> GetAllPaymentSchedulesAsync();
        Task<IEnumerable<PaymentScheduleDto>> GetPaymentScheduleByInvoiceIdAsync(int invoiceId);
        Task<PaymentScheduleSummaryDto> GetPaymentScheduleSummaryAsync(int invoiceId);
        Task<PaymentScheduleDto> GetPaymentScheduleItemByIdAsync(int id);
        Task<IEnumerable<PaymentScheduleDto>> CreatePaymentScheduleAsync(CreatePaymentScheduleDto createDto, string currentUser);
        Task<PaymentScheduleDto> UpdatePaymentScheduleItemAsync(int id, UpdatePaymentScheduleItemDto updateDto, string currentUser);
        Task<PaymentScheduleDto> RecordPaymentAsync(int id, RecordSchedulePaymentDto paymentDto, string currentUser);
        Task<bool> DeletePaymentScheduleItemAsync(int id);
        Task<bool> DeletePaymentScheduleByInvoiceIdAsync(int invoiceId);
        Task<bool> ExportToXeroAsync(int invoiceId);
    }
}