using AwningsAPI.Dto.Auth;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/workflow")]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        private string CurrentUser => User?.Identity?.Name ?? "System";

        // ════════════════════════════════════════════════════════════════════
        // WORKFLOW CRUD
        // ════════════════════════════════════════════════════════════════════

        [Authorize]
        [HttpGet("GetAllWorfflowsForCustomer")]
        public async Task<ActionResult<IEnumerable<WorkflowDto>>> GetAllWorfflowsForCustomer(int CustomerId)
        {
            var dtos = await _workflowService.GetAllWorfflowsForCustomerAsync(CustomerId);
            return Ok(dtos);
        }

        [Authorize]
        [HttpPost("CreateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> CreateWorkflow([FromBody] WorkflowDto dto)
        {
            var workflow = await _workflowService.CreateWorkflow(dto, CurrentUser);
            return CreatedAtAction(nameof(CreateWorkflow), new { Id = workflow.WorkflowId }, new
            {
                workflow.WorkflowId,
                workflow.CustomerId,
                workflow.WorkflowName,
                TaskId = dto.TaskId
            });
        }

        [Authorize]
        [HttpPut("UpdateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> UpdateWorkflow([FromBody] WorkflowDto dto)
        {
            var workflow = await _workflowService.UpdateWorkflow(dto, CurrentUser);
            return Ok(workflow);
        }

        /// <summary>
        /// DELETE /api/workflow/DeleteWorkflow/{workflowId}
        ///
        /// Returns 200 with <see cref="WorkflowDeleteResult"/> in all cases:
        ///   • Deleted = true  → workflow removed.
        ///   • Deleted = false → blocked; BlockingDependencies describes what is preventing deletion.
        ///
        /// The Angular client inspects Deleted and shows the appropriate modal state.
        /// We return 200 even when blocked (rather than 409 Conflict) so Angular's
        /// error handler is not triggered — the UI handles the "blocked" case inline.
        /// </summary>
        [Authorize]
        [HttpDelete("DeleteWorkflow/{workflowId}")]
        public async Task<ActionResult<WorkflowDeleteResult>> DeleteWorkflow(int workflowId)
        {
            var result = await _workflowService.DeleteWorkflowAsync(workflowId);
            return Ok(result);
        }

        // ════════════════════════════════════════════════════════════════════
        // INITIAL ENQUIRY
        // ════════════════════════════════════════════════════════════════════

        [HttpGet("GeInitialEnquiryForWorkflow")]
        public async Task<ActionResult<IEnumerable<InitialEnquiryDto>>> GeInitialEnquiryForWorkflow(int WorkflowId)
        {
            var enquiries = await _workflowService.GetInitialEnquiryForWorkflowAsync(WorkflowId);
            var dtos = enquiries.Select(c => new InitialEnquiryDto
            {
                EnquiryId = c.EnquiryId,
                WorkflowId = c.WorkflowId,
                Comments = c.Comments,
                Email = c.Email,
                Images = c.Images,
                Signature = c.Signature,
                TaskId = c.TaskId,
                IncomingEmailId = c.IncomingEmailId,
                DateCreated = c.DateCreated,
                CreatedBy = c.CreatedBy
            }).ToList();
            return Ok(dtos);
        }

        [HttpPost("AddInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> AddInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var enquiry = await _workflowService.AddInitialEnquiry(dto, CurrentUser);
            return CreatedAtAction(nameof(AddInitialEnquiry), new { Id = enquiry.EnquiryId }, enquiry);
        }

        [HttpPut("UpdateInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> UpdateInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var enquiry = await _workflowService.UpdateInitialEnquiry(dto, CurrentUser);
            return Ok(enquiry);
        }

        // ════════════════════════════════════════════════════════════════════
        // PRODUCT / PRICING
        // ════════════════════════════════════════════════════════════════════

        [HttpGet("GetStandardWidthsForProduct")]
        public async Task<List<int>> GetStandardWidthsForProduct(int ProductId) =>
            await _workflowService.GetStandardWidthsForProductAsync(ProductId);

        [HttpGet("GetProjectionWidthsForProduct")]
        public async Task<List<int>> GetProjectionWidthsForProduct(int ProductId) =>
            await _workflowService.GetProjectionWidthsForProductAsync(ProductId);

        [HttpGet("GetProjectionPriceForProduct")]
        public async Task<decimal> GetProjectionPriceForProduct(int ProductId, int widthcm, int projectioncm) =>
            await _workflowService.GetProjectionPriceForProductAsync(ProductId, widthcm, projectioncm);

        /// <summary>
        /// GET /api/workflow/GetArmTypeForProjection?ProductId=6&amp;widthcm=300&amp;projectioncm=200
        ///
        /// Returns the ArmTypeId for the matching Projections row.
        /// The Angular client calls this when the user selects a width/projection
        /// so it can re-fetch the brackets dropdown filtered to compatible items.
        /// Returns null (HTTP 200 with null body) when no matching row is found.
        /// </summary>
        [HttpGet("GetArmTypeForProjection")]
        public async Task<ActionResult<int?>> GetArmTypeForProjection(int ProductId, int widthcm, int projectioncm)
        {
            var armTypeId = await _workflowService.GetArmTypeForProjectionAsync(ProductId, widthcm, projectioncm);
            return Ok(armTypeId);
        }

        [HttpGet("GeBracketsForProduct")]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GeBracketsForProduct(int ProductId, int? armTypeId = null)
        {
            var brackets = await _workflowService.GetBracketsForProductAsync(ProductId, armTypeId);
            return Ok(brackets.Select(c => new BracketDto
            {
                BracketId = c.BracketId,
                BracketName = c.BracketName,
                Price = c.Price,
                ArmTypeId = c.ArmTypeId
            }).ToList());
        }

        [HttpGet("GeArmsForProduct")]
        [Obsolete("Arms data is now in the Brackets table. Use GeBracketsForProduct.")]
        public Task<ActionResult<IEnumerable<ArmDto>>> GeArmsForProduct(int ProductId) =>
            Task.FromResult<ActionResult<IEnumerable<ArmDto>>>(Ok(new List<ArmDto>()));

        [HttpGet("GeMotorsForProduct")]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GeMotorsForProduct(int ProductId)
        {
            var motors = await _workflowService.GeMotorsForProductAsync(ProductId);
            return Ok(motors.Select(c => new MotorDto { MotorId = c.MotorId, Description = c.Description, Price = c.Price }).ToList());
        }

        [HttpGet("GeValanceStylePriceForProduct")]
        public async Task<decimal> GeValanceStylePriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeValanceStylePriceForProductAsync(ProductId, widthcm);

        [HttpGet("GeNonStandardRALColourPriceForProduct")]
        public async Task<decimal> GeNonStandardRALColourPriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeNonStandardRALColourPriceForProductAsync(ProductId, widthcm);

        [HttpGet("GeWallSealingProfilerPriceForProduct")]
        public async Task<decimal> GeWallSealingProfilerPriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeWallSealingProfilerPriceForProductAsync(ProductId, widthcm);

        [HttpGet("GeHeatersForProduct")]
        public async Task<ActionResult<IEnumerable<HeaterDto>>> GeHeatersForProduct(int ProductId)
        {
            var heaters = await _workflowService.GeHeatersForProductAsync(ProductId);
            return Ok(heaters.Select(c => new HeaterDto { HeaterId = c.HeaterId, Description = c.Description, Price = c.Price, PriceNonRALColour = c.PriceNonRALColour }).ToList());
        }

        // ════════════════════════════════════════════════════════════════════
        // USER SIGNATURES  (/api/workflow/signatures/...)
        // ════════════════════════════════════════════════════════════════════

        [Authorize]
        [HttpGet("signatures")]
        public async Task<ActionResult<IEnumerable<UserSignatureDto>>> GetSignatures()
        {
            var sigs = await _workflowService.GetSignaturesAsync(CurrentUser);
            return Ok(sigs);
        }

        [Authorize]
        [HttpPost("signatures")]
        public async Task<ActionResult<UserSignatureDto>> CreateSignature([FromBody] UserSignatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Label)) return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText)) return BadRequest("SignatureText is required.");
            var created = await _workflowService.CreateSignatureAsync(dto, CurrentUser);
            return CreatedAtAction(nameof(GetSignatures), new { id = created.SignatureId }, created);
        }

        [Authorize]
        [HttpPut("signatures/{id:int}")]
        public async Task<ActionResult<UserSignatureDto>> UpdateSignature(int id, [FromBody] UserSignatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Label)) return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText)) return BadRequest("SignatureText is required.");
            try { return Ok(await _workflowService.UpdateSignatureAsync(id, dto, CurrentUser)); }
            catch (Exception ex) when (ex.Message == "Signature not found.") { return NotFound(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpPut("signatures/{id:int}/default")]
        public async Task<ActionResult<UserSignatureDto>> SetDefaultSignature(int id)
        {
            try { return Ok(await _workflowService.SetDefaultSignatureAsync(id, CurrentUser)); }
            catch (Exception ex) when (ex.Message == "Signature not found.") { return NotFound(new { message = ex.Message }); }
        }

        [Authorize]
        [HttpDelete("signatures/{id:int}")]
        public async Task<IActionResult> DeleteSignature(int id)
        {
            var deleted = await _workflowService.DeleteSignatureAsync(id, CurrentUser);
            if (!deleted) return NotFound(new { message = $"Signature with ID {id} not found." });
            return Ok(new { message = "Signature deleted." });
        }
    }
}