using AwningsAPI.Dto.Auth;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    /// <summary>
    /// Unified workflow controller.
    /// All signature endpoints previously in UserSignatureController have been
    /// consolidated here and delegate entirely to IWorkflowService.
    ///
    /// Workflow endpoints    → /api/workflow/...
    /// Signature endpoints   → /api/workflow/signatures/...
    /// </summary>
    [ApiController]
    [Route("api/workflow")]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        // ── Convenience: resolved identity of the caller ──────────────────────
        private string CurrentUser => User?.Identity?.Name ?? "System";

        // ════════════════════════════════════════════════════════════════════
        // WORKFLOW CRUD
        // ════════════════════════════════════════════════════════════════════

        [Authorize]
        [HttpGet("GetAllWorfflowsForCustomer")]
        public async Task<ActionResult<IEnumerable<WorkflowStart>>> GetAllWorfflowsForCustomer(int CustomerId)
        {
            var workflows = await _workflowService.GetAllWorfflowsForCustomerAsync(CustomerId);

            var dtos = workflows.Select(c => new WorkflowDto
            {
                WorkflowId = c.WorkflowId,
                WorkflowName = c.WorkflowName,
                ProductName = c.Product.Description,
                Description = c.Description,
                InitialEnquiry = c.InitialEnquiry,
                CreateQuotation = c.CreateQuote,
                InviteShowRoomVisit = c.InviteShowRoom,
                SetupSiteVisit = c.SetupSiteVisit,
                InvoiceSent = c.InvoiceSent,
                DateAdded = c.DateCreated,
                AddedBy = c.CreatedBy,
                CompanyId = c.CompanyId,
                SupplierId = c.SupplierId,
                ProductId = c.ProductId,
                ProductTypeId = c.ProductTypeId,
                CustomerId = c.CustomerId
            }).ToList();

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

        [Authorize]
        [HttpDelete("DeleteWorkflow/{workflowId}")]
        public async Task<IActionResult> DeleteWorkflow(int workflowId)
        {
            var result = await _workflowService.DeleteWorkflowAsync(workflowId);

            if (!result)
                return NotFound(new { message = $"Workflow with ID {workflowId} not found" });

            return Ok(new { message = "Workflow deleted successfully" });
        }

        // ════════════════════════════════════════════════════════════════════
        // INITIAL ENQUIRY
        // ════════════════════════════════════════════════════════════════════

        [HttpGet("GeInitialEnquiryForWorkflow")]
        public async Task<ActionResult<IEnumerable<WorkflowStart>>> GeInitialEnquiryForWorkflow(int WorkflowId)
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
        public async Task<ActionResult<IEnumerable<InitialEnquiry>>> UpdateInitialEnquiry([FromBody] InitialEnquiryDto dto)
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

        [HttpGet("GeBracketsForProduct")]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GeBracketsForProduct(int ProductId)
        {
            var brackets = await _workflowService.GeBracketsForProductAsync(ProductId);

            var dtos = brackets.Select(c => new BracketDto
            {
                BracketId = c.BracketId,
                BracketName = c.BracketName,
                Price = c.Price
            }).ToList();

            return Ok(dtos);
        }

        /// <summary>
        /// DEPRECATED — Arms are now in the Brackets table.
        /// Returns an empty list for backwards compatibility.
        /// </summary>
        [HttpGet("GeArmsForProduct")]
        [Obsolete("Arms data is now stored in the Brackets table. Use GeBracketsForProduct.")]
        public Task<ActionResult<IEnumerable<ArmDto>>> GeArmsForProduct(int ProductId) =>
            Task.FromResult<ActionResult<IEnumerable<ArmDto>>>(Ok(new List<ArmDto>()));

        [HttpGet("GeMotorsForProduct")]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GeMotorsForProduct(int ProductId)
        {
            var motors = await _workflowService.GeMotorsForProductAsync(ProductId);

            var dtos = motors.Select(c => new MotorDto
            {
                MotorId = c.MotorId,
                Description = c.Description,
                Price = c.Price
            }).ToList();

            return Ok(dtos);
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

            var dtos = heaters.Select(c => new HeaterDto
            {
                HeaterId = c.HeaterId,
                Description = c.Description,
                Price = c.Price,
                PriceNonRALColour = c.PriceNonRALColour
            }).ToList();

            return Ok(dtos);
        }

        // ════════════════════════════════════════════════════════════════════
        // USER SIGNATURES
        // Previously in UserSignatureController (/api/signatures/...).
        // All five endpoints are now under /api/workflow/signatures/...
        // and delegate entirely to IWorkflowService — no direct DB access here.
        // ════════════════════════════════════════════════════════════════════

        /// <summary>GET /api/workflow/signatures
        /// Returns all signatures saved by the authenticated user.</summary>
        [Authorize]
        [HttpGet("signatures")]
        public async Task<ActionResult<IEnumerable<UserSignatureDto>>> GetSignatures()
        {
            var sigs = await _workflowService.GetSignaturesAsync(CurrentUser);
            return Ok(sigs);
        }

        /// <summary>POST /api/workflow/signatures
        /// Creates a new signature for the authenticated user.</summary>
        [Authorize]
        [HttpPost("signatures")]
        public async Task<ActionResult<UserSignatureDto>> CreateSignature([FromBody] UserSignatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Label))
                return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText))
                return BadRequest("SignatureText is required.");

            var created = await _workflowService.CreateSignatureAsync(dto, CurrentUser);
            return CreatedAtAction(nameof(GetSignatures), new { id = created.SignatureId }, created);
        }

        /// <summary>PUT /api/workflow/signatures/{id}
        /// Updates an existing signature owned by the authenticated user.</summary>
        [Authorize]
        [HttpPut("signatures/{id:int}")]
        public async Task<ActionResult<UserSignatureDto>> UpdateSignature(
            int id, [FromBody] UserSignatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Label))
                return BadRequest("Label is required.");
            if (string.IsNullOrWhiteSpace(dto.SignatureText))
                return BadRequest("SignatureText is required.");

            try
            {
                var updated = await _workflowService.UpdateSignatureAsync(id, dto, CurrentUser);
                return Ok(updated);
            }
            catch (Exception ex) when (ex.Message == "Signature not found.")
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>PUT /api/workflow/signatures/{id}/default
        /// Marks the specified signature as the default, clearing all others.</summary>
        [Authorize]
        [HttpPut("signatures/{id:int}/default")]
        public async Task<ActionResult<UserSignatureDto>> SetDefaultSignature(int id)
        {
            try
            {
                var result = await _workflowService.SetDefaultSignatureAsync(id, CurrentUser);
                return Ok(result);
            }
            catch (Exception ex) when (ex.Message == "Signature not found.")
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>DELETE /api/workflow/signatures/{id}
        /// Deletes a signature owned by the authenticated user.</summary>
        [Authorize]
        [HttpDelete("signatures/{id:int}")]
        public async Task<IActionResult> DeleteSignature(int id)
        {
            var deleted = await _workflowService.DeleteSignatureAsync(id, CurrentUser);

            if (!deleted)
                return NotFound(new { message = $"Signature with ID {id} not found." });

            return Ok(new { message = "Signature deleted." });
        }
    }
}