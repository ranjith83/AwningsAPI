using AwningsAPI.Dto.Workflow;
using AwningsAPI.Services.WorkflowService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    /// <summary>
    /// Follow-Ups API — driven by InitialEnquiry age, not WorkflowStart flags.
    ///
    /// GET  /api/followup                → active follow-ups (non-dismissed)
    /// GET  /api/followup/all            → all follow-ups including dismissed
    /// GET  /api/followup/customer/{id}  → follow-ups for one customer
    /// POST /api/followup/generate       → scan InitialEnquiry records and create follow-ups
    /// POST /api/followup/{id}/dismiss   → user manually dismisses a follow-up
    ///
    /// TIMER RESET is NOT an API call — it happens automatically inside
    /// WorkflowService.AddInitialEnquiry which calls FollowUpService.DismissActiveForWorkflowAsync.
    /// </summary>
    [ApiController]
    [Route("api/followup")]
    [Authorize]
    public class FollowUpController : ControllerBase
    {
        private readonly FollowUpService _followUpService;
        private readonly ILogger<FollowUpController> _logger;

        public FollowUpController(FollowUpService followUpService, ILogger<FollowUpController> logger)
        {
            _followUpService = followUpService;
            _logger = logger;
        }

        /// <summary>
        /// All active (non-dismissed) follow-ups ordered by most overdue first.
        /// Called by the Follow-Ups screen on load.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowUpDto>>> GetActiveFollowUps()
        {
            try
            {
                return Ok(await _followUpService.GetActiveFollowUpsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active follow-ups");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>All follow-ups including dismissed (for history / audit).</summary>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FollowUpDto>>> GetAllFollowUps()
        {
            try
            {
                return Ok(await _followUpService.GetAllFollowUpsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all follow-ups");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>Active follow-ups for one customer (used on customer detail pages).</summary>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<FollowUpDto>>> GetFollowUpsByCustomer(int customerId)
        {
            try
            {
                return Ok(await _followUpService.GetFollowUpsByCustomerAsync(customerId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving follow-ups for customer {CustomerId}", customerId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Scans all InitialEnquiry records and creates follow-up records for
        /// enquiries older than 3 days on workflows with no quote yet.
        /// Called by the Angular Follow-Ups screen on page load.
        /// In production this could also be a scheduled background job.
        /// </summary>
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateFollowUps()
        {
            try
            {
                int created = await _followUpService.GeneratePendingFollowUpsAsync();
                return Ok(new
                {
                    message = $"Scan complete. {created} new follow-up(s) created.",
                    created
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating follow-ups");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// User-initiated dismiss — marks the follow-up as resolved with a reason.
        /// NOTE: timer-reset dismissals happen automatically inside
        /// WorkflowService.AddInitialEnquiry and are NOT routed through here.
        /// </summary>
        [HttpPost("{followUpId}/dismiss")]
        public async Task<IActionResult> DismissFollowUp(
            int followUpId,
            [FromBody] DismissFollowUpDto dto)
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                var result = await _followUpService.DismissFollowUpAsync(
                    followUpId, dto?.Notes ?? string.Empty, currentUser);

                if (!result)
                    return NotFound(new { error = $"Follow-up {followUpId} not found" });

                return Ok(new { message = "Follow-up dismissed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error dismissing follow-up {FollowUpId}", followUpId);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}