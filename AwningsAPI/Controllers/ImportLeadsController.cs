using AwningsAPI.Dto.ImportLeads;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/import-leads")]
    [Authorize]
    public class ImportLeadsController : ControllerBase
    {
        private readonly IImportLeadsService _service;
        private readonly ILogger<ImportLeadsController> _logger;

        public ImportLeadsController(IImportLeadsService service, ILogger<ImportLeadsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// On-demand: reads every email in the specified Inbox sub-folder, creates any new
        /// customers found, sends a welcome email, and copies each processed email into a
        /// "Processed" sub-folder.
        /// </summary>
        /// <param name="folder">Inbox sub-folder to read (default: "New Leads June")</param>
        [HttpPost("process")]
        public async Task<ActionResult<ImportLeadsResultDto>> ProcessLeads(
            [FromQuery] string folder = "New Leads June")
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                _logger.LogInformation("Lead import triggered by {User} for folder '{Folder}'",
                    currentUser, folder);

                var result = await _service.ProcessLeadsFolderAsync(folder, currentUser);
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
