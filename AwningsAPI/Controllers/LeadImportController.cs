using AwningsAPI.Dto.LeadImport;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/lead-import")]
    [Authorize]
    public class LeadImportController : ControllerBase
    {
        private readonly ILeadImportService _leadImportService;
        private readonly ILogger<LeadImportController> _logger;

        public LeadImportController(ILeadImportService leadImportService, ILogger<LeadImportController> logger)
        {
            _leadImportService = leadImportService;
            _logger = logger;
        }

        /// <summary>
        /// On-demand: reads every email in the specified Inbox sub-folder, creates any new
        /// customers found, and moves each processed email into a "Processed" sub-folder.
        /// </summary>
        /// <param name="folder">Inbox sub-folder to read (default: "New Leads June")</param>
        [HttpPost("process")]
        public async Task<ActionResult<LeadImportResultDto>> ProcessLeads(
            [FromQuery] string folder = "New Leads June")
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                _logger.LogInformation("Lead import triggered by {User} for folder '{Folder}'",
                    currentUser, folder);

                var result = await _leadImportService.ProcessLeadsFolderAsync(folder, currentUser);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Lead import config error: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lead import failed");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
