using AwningsAPI.Database;
using AwningsAPI.Dto.Auth;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/workflow")]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly IEmailAutoReplyService _autoReplyService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WorkflowController> _logger;

        public WorkflowController(
            IWorkflowService workflowService,
            IEmailAutoReplyService autoReplyService,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<WorkflowController> logger)
        {
            _workflowService = workflowService;
            _autoReplyService = autoReplyService;
            _context = context;
            _configuration = configuration;
            _logger = logger;
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
            _logger.LogInformation("Workflow {WorkflowId} created for customer {CustomerId} by {User}", workflow.WorkflowId, workflow.CustomerId, CurrentUser);
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
            _logger.LogInformation("Workflow {WorkflowId} updated by {User}", dto.WorkflowId, CurrentUser);
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
            if (result.Deleted)
                _logger.LogInformation("Workflow {WorkflowId} deleted by {User}", workflowId, CurrentUser);
            else
                _logger.LogWarning("Workflow {WorkflowId} deletion blocked: {Dependencies}", workflowId, result.BlockingDependencies);
            return Ok(result);
        }

        // ════════════════════════════════════════════════════════════════════
        // INITIAL ENQUIRY
        // ════════════════════════════════════════════════════════════════════

        [Authorize]
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
                AutoReplyDraftId = c.AutoReplyDraftId,
                AutoReplyContent = c.AutoReplyContent,
                DateCreated = c.DateCreated,
                CreatedBy = c.CreatedBy
            }).ToList();

            // Import-leads path: InitialEnquiry is not written until the reply is sent.
            // Surface the task's Claude draft so the frontend can show it for review.
            if (!dtos.Any())
            {
                var pendingTask = await _context.Tasks
                    .Where(t => t.WorkflowId == WorkflowId && t.NeedsReply && t.DraftReply != null)
                    .OrderByDescending(t => t.DateUpdated ?? t.DateCreated)
                    .Select(t => new { t.TaskId, t.DraftReply, t.CustomerEmail, t.FromEmail })
                    .FirstOrDefaultAsync();

                if (pendingTask != null)
                {
                    dtos.Add(new InitialEnquiryDto
                    {
                        EnquiryId       = null,
                        WorkflowId      = WorkflowId,
                        Email           = pendingTask.CustomerEmail ?? pendingTask.FromEmail ?? "",
                        Comments        = pendingTask.DraftReply,
                        AutoReplyContent = pendingTask.DraftReply,
                        PendingTaskId   = pendingTask.TaskId
                    });
                }
            }

            return Ok(dtos);
        }

        [Authorize]
        [HttpPost("AddInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> AddInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var enquiry = await _workflowService.AddInitialEnquiry(dto, CurrentUser);
            return CreatedAtAction(nameof(AddInitialEnquiry), new { Id = enquiry.EnquiryId }, enquiry);
        }

        [Authorize]
        [HttpPut("UpdateInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> UpdateInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var enquiry = await _workflowService.UpdateInitialEnquiry(dto, CurrentUser);
            return Ok(enquiry);
        }

        [Authorize]
        [HttpDelete("DeleteInitialEnquiry/{enquiryId:int}")]
        public async Task<IActionResult> DeleteInitialEnquiry(int enquiryId)
        {
            var deleted = await _workflowService.DeleteInitialEnquiryAsync(enquiryId, CurrentUser);
            if (!deleted)
                return NotFound(new { message = $"Enquiry {enquiryId} not found." });
            _logger.LogInformation("InitialEnquiry {EnquiryId} soft-deleted by {User}", enquiryId, CurrentUser);
            return Ok(new { message = "Enquiry deleted." });
        }

        [Authorize]
        [HttpPost("SendAutoReplyDraft/{enquiryId:int}")]
        public async Task<IActionResult> SendAutoReplyDraft(int enquiryId)
        {
            var enquiry = await _context.InitialEnquiries.FindAsync(enquiryId);
            if (enquiry == null)
                return NotFound(new { message = "Enquiry not found." });

            if (string.IsNullOrEmpty(enquiry.AutoReplyDraftId))
                return BadRequest(new { message = "No auto-reply draft exists for this enquiry." });

            var mailbox = _configuration["AzureAd:OrganizerEmail"] ?? "";
            await _autoReplyService.SendDraftAsync(enquiry.AutoReplyDraftId, mailbox);

            // Clear the draft ID so the UI no longer shows the unsent banner
            enquiry.AutoReplyDraftId = null;

            // Sending the reply is the trigger that clears the notification
            var relatedNotifs = await _context.Notifications
                .Where(n => !n.IsRead && n.EntityType == "InitialEnquiry" && n.EntityId == enquiryId)
                .ToListAsync();
            relatedNotifs.ForEach(n => n.IsRead = true);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Auto-reply draft sent for enquiry {EnquiryId} by {User}", enquiryId, CurrentUser);
            return Ok(new { message = "Auto-reply sent successfully." });
        }

        [Authorize]
        [HttpPost("GenerateAutoReply/{enquiryId:int}")]
        public async Task<IActionResult> GenerateAutoReply(int enquiryId)
        {
            try
            {
                var (draftId, content) = await _autoReplyService.GenerateAutoReplyForEnquiryAsync(enquiryId);
                _logger.LogInformation("Auto-reply draft generated for enquiry {EnquiryId} by {User}", enquiryId, CurrentUser);
                return Ok(new { draftId, content });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
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
            var brackets = await _workflowService.GeBracketsForProductAsync(ProductId, armTypeId);
            return Ok(brackets.Select(c => new BracketDto
            {
                BracketId = c.BracketId,
                BracketName = c.BracketName,
                Price = c.Price,
                ArmTypeId = c.ArmTypeId,
                IsDefault = c.IsDefault,
                IsPriceIgnored = c.IsPriceIgnored
            }).ToList());
        }

        [HttpGet("GeArmsForProduct")]
        [Obsolete("Arms data is now in the Brackets table. Use GeBracketsForProduct.")]
        public Task<ActionResult<IEnumerable<ArmDto>>> GeArmsForProduct(int ProductId) =>
            Task.FromResult<ActionResult<IEnumerable<ArmDto>>>(Ok(new List<ArmDto>()));

        [HttpGet("GeMotorsForProduct")]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GeMotorsForProduct(int ProductId, int? armTypeId = null)
        {
            var motors = await _workflowService.GeMotorsForProductAsync(ProductId, armTypeId);
            return Ok(motors.Select(c => new MotorDto { MotorId = c.MotorId, Description = c.Description, Price = c.Price }).ToList());
        }

        [HttpGet("GeControlsForProduct")]
        public async Task<ActionResult<IEnumerable<ControlDto>>> GetControlsForProduct(int ProductId)
        {
            var controls = await _workflowService.GetControlsForProductAsync(ProductId);
            return Ok(controls.Select(c => new ControlDto { ControlId = c.ControlId, Description = c.Description, Price = c.Price }).ToList());
        }

        [HttpGet("HasControls")]
        public async Task<bool> HasControls(int ProductId) =>
            await _workflowService.HasControlsAsync(ProductId);

        [HttpGet("GeLightingCassettesForProduct")]
        public async Task<ActionResult<IEnumerable<LightingCassetteDto>>> GetLightingForProduct(int ProductId)
        {
            var lighting = await _workflowService.GetLightingForProductAsync(ProductId);
            return Ok(lighting.Select(l => new LightingCassetteDto { LightingId = l.LightingId, Description = l.Description, Price = l.Price }).ToList());
        }

        [HttpGet("HasLighting")]
        public async Task<bool> HasLighting(int ProductId) =>
            await _workflowService.HasLightingAsync(ProductId);

        [HttpGet("GeValanceStylePriceForProduct")]
        public async Task<decimal> GeValanceStylePriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeValanceStylePriceForProductAsync(ProductId, widthcm);

        [HttpGet("GeNonStandardRALColourPriceForProduct")]
        public async Task<decimal> GeNonStandardRALColourPriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeNonStandardRALColourPriceForProductAsync(ProductId, widthcm);

        /// <summary>
        /// GET /api/workflow/GetShadePlusOptionsForProduct?ProductId=8&amp;widthcm=500
        ///
        /// Returns all ShadePlus options for the given product and width.
        /// When HasMultiple is true the frontend renders a dropdown so the user can
        /// choose which surcharge applies (e.g. gearbox vs hard-wired motor vs radio).
        /// The chosen option's Description is stored on the quote line — NOT "ShadePlus".
        /// The text field must remain editable after selection so the user can refine it.
        /// When HasMultiple is false a single price is shown with an editable text field.
        /// </summary>
        [HttpGet("GetShadePlusOptionsForProduct")]
        public async Task<ActionResult<ShadePlusOptionsDto>> GetShadePlusOptionsForProduct(int ProductId, int widthcm)
        {
            var options = await _workflowService.GetShadePlusOptionsAsync(ProductId, widthcm);
            return Ok(options);
        }

        [HttpGet("GeWallSealingProfilerPriceForProduct")]
        public async Task<decimal> GeWallSealingProfilerPriceForProduct(int ProductId, int widthcm) =>
            await _workflowService.GeWallSealingProfilerPriceForProductAsync(ProductId, widthcm);

        // ── Addon availability checks ─────────────────────────────────────────

        /// <summary>Returns true if the product has any non-standard RAL colour records.</summary>
        [HttpGet("HasNonStandardRALColours")]
        public async Task<bool> HasNonStandardRALColours(int ProductId) =>
            await _workflowService.HasNonStandardRALColoursAsync(ProductId);

        /// <summary>Returns true if the product has any ShadePlus records.</summary>
        [HttpGet("HasShadePlus")]
        public async Task<bool> HasShadePlus(int ProductId) =>
            await _workflowService.HasShadePlusAsync(ProductId);

        /// <summary>Returns true if the product has any ValanceStyle records.</summary>
        [HttpGet("HasValanceStyles")]
        public async Task<bool> HasValanceStyles(int ProductId) =>
            await _workflowService.HasValanceStylesAsync(ProductId);

        /// <summary>Returns true if the product has any WallSealingProfile records.</summary>
        [HttpGet("HasWallSealingProfiles")]
        public async Task<bool> HasWallSealingProfiles(int ProductId) =>
            await _workflowService.HasWallSealingProfilesAsync(ProductId);

        /// <summary>Returns true if the product supports frame colour selection (Folding-arm Cassette Awnings).</summary>
        [HttpGet("HasFrameColour")]
        public async Task<bool> HasFrameColour(int ProductId) =>
            await _workflowService.HasFrameColourAsync(ProductId);

        /// <summary>Returns frame colour options for a specific product ordered by DisplayOrder.</summary>
        [HttpGet("GetFrameColourOptions")]
        public async Task<ActionResult<IEnumerable<FrameColourOptionDto>>> GetFrameColourOptions([FromQuery] int productId) =>
            Ok(await _workflowService.GetFrameColourOptionsAsync(productId));

        /// <summary>
        /// GET /api/workflow/GetFrameColourPrice?productId=1&amp;frameColourOptionId=5&amp;widthCm=300
        /// Returns the price surcharge for the selected frame colour. Returns 0 when included (isNonStandardRAL=false).
        /// </summary>
        [HttpGet("GetFrameColourPrice")]
        public async Task<decimal> GetFrameColourPrice([FromQuery] int productId, [FromQuery] int frameColourOptionId, [FromQuery] int widthCm) =>
            await _workflowService.GetFrameColourPriceAsync(productId, frameColourOptionId, widthCm);

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