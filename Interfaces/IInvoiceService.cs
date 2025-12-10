using AwningsAPI.Dto.Workflow;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<IEnumerable<InvoiceDto>> GetInvoicesByWorkflowIdAsync(int workflowId);
        Task<IEnumerable<InvoiceDto>> GetInvoicesByCustomerIdAsync(int customerId);
        Task<InvoiceDto> GetInvoiceByIdAsync(int id);
        Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createDto, string currentUser);
        Task<InvoiceDto> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateDto, string currentUser);
        Task<bool> DeleteInvoiceAsync(int id);
        Task<InvoiceDto> UpdateInvoiceStatusAsync(int id, string status, string currentUser);
        Task<InvoicePaymentDto> AddPaymentAsync(int invoiceId, CreatePaymentDto paymentDto, string currentUser);
        Task<byte[]> GenerateInvoicePdfAsync(int id);
        Task<bool> SendInvoiceEmailAsync(int id);
    }
}
