using AwningsAPI.Dto.SiteVisit;
using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteVisitController : ControllerBase
    {
        private readonly ISiteVisitService _siteVisitService;
        private readonly ITaskService _taskService;
        private readonly ILogger<SiteVisitController> _logger;

        public SiteVisitController(ISiteVisitService siteVisitService, ITaskService taskService, ILogger<SiteVisitController> logger)
        {
            _siteVisitService = siteVisitService;
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Get all site visits for a specific workflow
        /// </summary>
        /// <param name="workflowId">The workflow ID</param>
        /// <returns>List of site visits</returns>
        [Authorize]
        [HttpGet("workflow/{workflowId}")]
        public async Task<ActionResult<IEnumerable<SiteVisitDto>>> GetSiteVisitsByWorkflowId(int workflowId)
        {
            return Ok(await _siteVisitService.GetSiteVisitDtosByWorkflowIdAsync(workflowId));
        }

        /// <summary>
        /// Get a single site visit by ID
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <returns>Site visit details</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteVisitDto>> GetSiteVisitById(int id)
        {
            var siteVisitDto = await _siteVisitService.GetSiteVisitDtoByIdAsync(id);
            if (siteVisitDto == null)
                return NotFound(new { message = $"Site visit with ID {id} not found" });
            return Ok(siteVisitDto);
        }

        /// <summary>
        /// Create a new site visit
        /// </summary>
        /// <param name="dto">Site visit creation data</param>
        /// <returns>Created site visit</returns>
      /**  [Authorize]
        [HttpPost]
        public async Task<ActionResult<SiteVisitDto>> CreateSiteVisit_old([FromBody] CreateSiteVisitDto dto)
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                var siteVisit = await _siteVisitService.CreateSiteVisitAsync(dto, currentUser);

                var siteVisitDto = new SiteVisitDto
                {
                    SiteVisitId = siteVisit.SiteVisitId,
                    WorkflowId = siteVisit.WorkflowId,
                    ProductModelType = siteVisit.ProductModelType,
                    Model = siteVisit.Model,
                    OtherPleaseSpecify = siteVisit.OtherPleaseSpecify,
                    DateCreated = siteVisit.DateCreated,
                    CreatedBy = siteVisit.CreatedBy
                };

                return CreatedAtAction(nameof(GetSiteVisitById), new { id = siteVisit.SiteVisitId }, siteVisitDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating site visit", error = ex.Message });
            }
        }**/

        /// <summary>
        /// Update an existing site visit
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <param name="dto">Updated site visit data</param>
        /// <returns>Updated site visit</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<SiteVisitDto>> UpdateSiteVisit(int id, [FromBody] SiteVisitDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var siteVisit = await _siteVisitService.UpdateSiteVisitAsync(id, dto, currentUser);
            _logger.LogInformation("Site visit {SiteVisitId} updated by {User}", id, currentUser);
            return Ok(new SiteVisitDto
            {
                SiteVisitId = siteVisit.SiteVisitId,
                WorkflowId = siteVisit.WorkflowId,
                ProductModelType = siteVisit.ProductModelType,
                DateUpdated = siteVisit.DateUpdated,
                UpdatedBy = siteVisit.UpdatedBy
            });
        }

        /// <summary>
        /// Delete a site visit
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <returns>Success status</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSiteVisit(int id)
        {
            var result = await _siteVisitService.DeleteSiteVisitAsync(id);
            if (!result)
            {
                _logger.LogWarning("Site visit {SiteVisitId} not found for deletion", id);
                return NotFound(new { message = $"Site visit with ID {id} not found" });
            }
            _logger.LogInformation("Site visit {SiteVisitId} deleted by {User}", id, User?.Identity?.Name);
            return Ok(new { message = "Site visit deleted successfully" });
        }


        /// <summary>
        /// Get all site visit dropdown values grouped by category
        /// </summary>
        /// <returns>Dictionary of categories and their values</returns>
        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetAllValues()
        {
            return Ok(await _siteVisitService.GetAllValuesDictionaryAsync());
        }

        [Authorize]
        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<string>>> GetValuesByCategory(string category)
        {
            var values = await _siteVisitService.GetValuesByCategoryAsync(category);
            return Ok(values.Select(v => v.Value).ToList());
        }

        // <summary>
        /// POST api/SiteVisit
        ///
        /// 1. Persists the site visit record.
        /// 2. Creates a linked AppTask (SourceType = "SiteVisit", Status = "New")
        ///    so the task can be assigned to a user from the Task Management screen.
        /// 3. Stamps the SiteVisitId directly on the task row (no JSON parsing needed).
        /// 4. Returns a CreateSiteVisitResponseDto that includes the new TaskId so
        ///    Angular can scroll to / highlight the new task row immediately.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreateSiteVisitResponseDto>> CreateSiteVisit(
            [FromBody] CreateSiteVisitDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";

            // ── Step 1: Persist the site visit ───────────────────────────────
            var siteVisit = await _siteVisitService.CreateSiteVisitAsync(dto, currentUser);

                // ── Step 2: Find the pending task booked via calendar for this
                //            workflow, or create a new one if none exists ───────
                var task = await _taskService.GetPendingSiteVisitTaskByWorkflowIdAsync(dto.WorkflowId);

                if (task == null)
                {
                    var taskDto = new CreateTaskDto
                    {
                        SourceType = TaskSourceType.SiteVisit.ToString(),
                        Title = $"Site Visit – {dto.Model ?? dto.ProductModelType ?? "New"}",
                        Category = "Site Visit",
                       // EmailBody = BuildSiteVisitSummary(dto),
                        Priority = "Normal",
                        WorkflowId = dto.WorkflowId,
                        CustomerName = dto.CustomerName,
                        CustomerEmail = dto.CustomerEmail,
                        CustomerId = dto.CustomerId,
                    };

                    task = await _taskService.CreateTaskAsync(taskDto, currentUser);
                }

                // ── Step 3: Stamp SiteVisitId directly on the task row ───────────
                await _taskService.StoreSiteVisitLinkAsync(
                    task.TaskId, siteVisit.SiteVisitId, currentUser);

                // ── Step 4: The survey has been carried out and saved — complete
                //            the linked task ────────────────────────────────────
                task = await _taskService.CompleteTaskAsync(task.TaskId, "Site survey completed", currentUser);

                // ── Step 5: Return combined response ─────────────────────────────
                var response = new CreateSiteVisitResponseDto
                {
                    SiteVisitId = siteVisit.SiteVisitId,
                    WorkflowId = siteVisit.WorkflowId,
                    ProductModelType = siteVisit.ProductModelType,
                    Model = siteVisit.Model,
                    DateCreated = siteVisit.DateCreated,
                    CreatedBy = siteVisit.CreatedBy,
                    TaskId = task.TaskId   // Angular uses this to navigate to the new task
                };

            _logger.LogInformation("Site visit {SiteVisitId} created for workflow {WorkflowId} by {User}, linked to task {TaskId}", siteVisit.SiteVisitId, siteVisit.WorkflowId, currentUser, task.TaskId);
            return CreatedAtAction(nameof(GetSiteVisitById), new { id = siteVisit.SiteVisitId }, response);
        }

        /// <summary>
        /// GET api/SiteVisit/scheduled
        ///
        /// Upcoming showroom/site-visit calendar bookings (ShowroomInvite rows with
        /// EventDate in the future and Status = "Scheduled"). Powers the Site Survey
        /// "Scheduled" tab — this is the source of truth for what's booked, since it's
        /// written atomically by POST api/outlook/create-showroom-invite.
        /// </summary>
        [Authorize]
        [HttpGet("scheduled")]
        public async Task<ActionResult> GetScheduledSiteVisits([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (items, totalCount) = await _siteVisitService.GetUpcomingShowroomInvitesAsync(page, pageSize);
            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                items
            });
        }

        /// <summary>
        /// GET api/SiteVisit/pending-count
        ///
        /// Count of upcoming showroom/site-visit bookings that haven't been carried out
        /// yet. Powers the Site Survey menu badge.
        /// </summary>
        [Authorize]
        [HttpGet("pending-count")]
        public async Task<ActionResult<SiteVisitPendingCountDto>> GetPendingCount()
        {
            var count = await _siteVisitService.GetUpcomingShowroomInviteCountAsync();
            return Ok(new SiteVisitPendingCountDto { Count = count });
        }

        [Authorize]
        [HttpPost("{siteVisitId}/images")]
        public async Task<IActionResult> UploadImages(int siteVisitId, [FromForm] IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { message = "No files provided" });

            var imageUrls = new List<string>();

            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "site-visits", siteVisitId.ToString());
            Directory.CreateDirectory(uploadDir);

            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExts.Contains(ext)) continue;

                var baseName = Path.GetFileNameWithoutExtension(file.FileName);
                // Strip characters that are invalid in file paths
                var safeName = string.Concat(baseName.Split(Path.GetInvalidFileNameChars()));
                if (string.IsNullOrWhiteSpace(safeName)) safeName = "image";

                var fileName = $"{safeName}{ext}";
                var filePath = Path.Combine(uploadDir, fileName);

                // Avoid overwriting an existing file with the same name
                if (System.IO.File.Exists(filePath))
                {
                    fileName = $"{safeName}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{ext}";
                    filePath = Path.Combine(uploadDir, fileName);
                }

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                imageUrls.Add($"/images/site-visits/{siteVisitId}/{fileName}");
            }

            if (imageUrls.Count == 0)
                return BadRequest(new { message = "No valid image files were uploaded" });

            var currentUser = User?.Identity?.Name ?? "System";
            await _siteVisitService.SaveImageUrlsAsync(siteVisitId, imageUrls, currentUser);

            return Ok(new { imageUrls });
        }

        [Authorize]
        [HttpDelete("{siteVisitId}/images")]
        public async Task<IActionResult> DeleteImages(int siteVisitId, [FromBody] DeleteImagesDto dto)
        {
            if (dto.ImageUrls == null || dto.ImageUrls.Count == 0)
                return BadRequest(new { message = "No image URLs provided" });

            await _siteVisitService.DeleteImagesAsync(siteVisitId, dto.ImageUrls);
            return Ok(new { message = "Images deleted successfully" });
        }
    }

    public class CreateSiteVisitResponseDto
    {
        public int SiteVisitId { get; set; }
        public int WorkflowId { get; set; }
        public string ProductModelType { get; set; }
        public string Model { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public object TaskId { get; set; }
    }

    public class SiteVisitPendingCountDto
    {
        public int Count { get; set; }
    }
}