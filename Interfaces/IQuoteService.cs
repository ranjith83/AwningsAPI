using AwningsAPI.Dto.Workflow;

namespace AwningsAPI.Interfaces
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteDto>> GetAllQuotesAsync();
        Task<IEnumerable<QuoteDto>> GetQuotesByWorkflowIdAsync(int workflowId);
        Task<QuoteDto> GetQuoteByIdAsync(int quoteId);
        Task<QuoteDto> CreateQuoteAsync(CreateQuoteDto createDto, string currentUser);
        Task<QuoteDto> UpdateQuoteAsync(int quoteId, UpdateQuoteDto updateDto, string currentUser);
        Task<bool> DeleteQuoteAsync(int quoteId);
    }
}
