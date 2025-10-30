using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Workflow;

namespace AwningsAPI.Interfaces
{
    public interface IWorkflowService
    {
        Task<IEnumerable<WorkflowStart>> GetAllWorfflowsForCustomerAsync(int CustomerId);
        Task<WorkflowStart> CreateWorkflow(WorkflowDto dto);
        Task<WorkflowStart> UpdateWorkflow(WorkflowDto dto);
        Task<IEnumerable<InitialEnquiry>> GetInitialEnquiryForWorkflowAsync(int WorkflowId);
        Task<InitialEnquiry> UpdateInitialEnquiry(InitialEnquiryDto dto);
        Task<InitialEnquiry> AddInitialEnquiry(InitialEnquiryDto dto);
        Task<List<int>> GetStandardWidthsForProductAsync(int productId);
        Task<List<int>> GetProjectionWidthsForProductAsync(int productId);
        Task<decimal> GetProjectionPriceForProductAsync(int productId, int widthcm, int projectioncm);
        Task<List<Brackets>> GeBracketsForProductAsync(int productId);
        Task<List<Arms>> GeArmsForProductAsync(int productId);
        Task<List<Motors>> GeMotorsForProductAsync(int productId);
        Task<decimal> GeValanceStylePriceForProductAsync(int productId, int widthcm);
        Task<decimal> GeNonStandardRALColourPriceForProductAsync(int productId, int widthcm);
        Task<decimal> GeWallSealingProfilerPriceForProductAsync(int productId, int widthcm);
    }
}
