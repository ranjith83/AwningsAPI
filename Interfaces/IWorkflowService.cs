using AwningsAPI.Dto.Auth;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Workflow;

namespace AwningsAPI.Interfaces
{
    public interface IWorkflowService
    {
        // ── Workflow ──────────────────────────────────────────────────────────

        /// <summary>
        /// Returns all workflows for a customer.
        /// Each WorkflowDto includes stage-enabled flags, server-computed
        /// stage-completed flags, and a HasDependencies guard flag.
        /// Return type is WorkflowDto (NOT WorkflowStart) — the service builds
        /// the full DTO including completion data in a single pass.
        /// </summary>
        Task<IEnumerable<WorkflowDto>> GetAllWorfflowsForCustomerAsync(int customerId);

        Task<WorkflowStart> CreateWorkflow(WorkflowDto dto, string currentUser);
        Task<WorkflowStart> UpdateWorkflow(WorkflowDto dto, string currentUser);

        /// <summary>
        /// Checks all dependency tables before deleting.
        /// Returns a WorkflowDeleteResult — never throws.
        /// result.Deleted = true  → deleted successfully.
        /// result.Deleted = false → blocked; BlockingDependencies lists what prevents deletion.
        /// </summary>
        Task<WorkflowDeleteResult> DeleteWorkflowAsync(int workflowId);

        // ── Initial Enquiry ───────────────────────────────────────────────────
        Task<IEnumerable<InitialEnquiry>> GetInitialEnquiryForWorkflowAsync(int workflowId);
        Task<InitialEnquiry> AddInitialEnquiry(InitialEnquiryDto dto, string currentUser);
        Task<InitialEnquiry> UpdateInitialEnquiry(InitialEnquiryDto dto, string currentUser);

        // ── Product / Pricing ─────────────────────────────────────────────────
        Task<List<int>> GetStandardWidthsForProductAsync(int productId);
        Task<List<int>> GetProjectionWidthsForProductAsync(int productId);
        Task<decimal> GetProjectionPriceForProductAsync(int productId, int widthcm, int projectioncm);
       // Task<List<Brackets>> GeBracketsForProductAsync(int productId);
      //  Task<List<Arms>> GeArmsForProductAsync(int productId);
        Task<List<Motors>> GeMotorsForProductAsync(int productId);
        Task<decimal> GeValanceStylePriceForProductAsync(int productId, int widthcm);
        Task<decimal> GeNonStandardRALColourPriceForProductAsync(int productId, int widthcm);
        Task<decimal> GeWallSealingProfilerPriceForProductAsync(int productId, int widthcm);
        Task<List<Heaters>> GeHeatersForProductAsync(int productId);

        Task<ShadePlusOptionsDto> GetShadePlusOptionsAsync(int productId, int widthcm);


        // ── User Signatures ───────────────────────────────────────────────────
        Task<IEnumerable<UserSignatureDto>> GetSignaturesAsync(string username);
        Task<UserSignatureDto> CreateSignatureAsync(UserSignatureDto dto, string username);
        Task<UserSignatureDto> UpdateSignatureAsync(int signatureId, UserSignatureDto dto, string username);
        Task<UserSignatureDto> SetDefaultSignatureAsync(int signatureId, string username);
        Task<bool> DeleteSignatureAsync(int signatureId, string username);

        Task<int?> GetArmTypeForProjectionAsync(int productId, int widthcm, int projectioncm);
        Task<List<Brackets>> GeBracketsForProductAsync(int productId, int? armTypeId = null);

        Task<bool> HasNonStandardRALColoursAsync(int productId);
        Task<bool> HasShadePlusAsync(int productId);
        Task<bool> HasValanceStylesAsync(int productId);
        Task<bool> HasWallSealingProfilesAsync(int productId);

    }
}