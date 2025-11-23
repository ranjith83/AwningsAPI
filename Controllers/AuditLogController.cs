// Controllers/AuditLogController.cs
using AwningsAPI.Dto.Audit;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogController> _logger;

        public AuditLogController(
            IAuditLogService auditLogService,
            ILogger<AuditLogController> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new audit log entry
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AuditLogDto>> CreateAuditLog([FromBody] CreateAuditLogDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest(new { message = "Invalid audit log data" });
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                var auditLog = await _auditLogService.CreateAuditLogAsync(dto, ipAddress, userAgent);

                var result = await _auditLogService.GetAuditLogByIdAsync(auditLog.AuditId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating audit log");
                return StatusCode(500, new { message = "Error creating audit log", error = ex.Message });
            }
        }

        /// <summary>
        /// Get audit logs with filters and pagination
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AuditLogPagedResultDto>> GetAuditLogs([FromQuery] AuditLogFilterDto filter)
        {
            try
            {
                var result = await _auditLogService.GetAuditLogsAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit logs with filter {@Filter}", filter);
                return StatusCode(500, new { message = "Error retrieving audit logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Get audit logs for a specific entity
        /// </summary>
        [HttpGet("entity/{entityType}/{entityId}")]
        [Authorize]
        public async Task<ActionResult<List<AuditLogDto>>> GetEntityAuditLogs(string entityType, int entityId)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(entityType))
                {
                    return BadRequest(new { message = "Entity type is required" });
                }

                if (entityId <= 0)
                {
                    return BadRequest(new { message = "Valid entity ID is required" });
                }

                // Validate entity type is a valid enum value
                if (!Enum.TryParse<AuditEntityType>(entityType, true, out var parsedEntityType))
                {
                    return BadRequest(new { message = $"Invalid entity type: {entityType}" });
                }

                _logger.LogInformation("Fetching audit logs for {EntityType} with ID {EntityId}", entityType, entityId);

                var auditLogs = await _auditLogService.GetEntityAuditLogsAsync(entityType, entityId);

                return Ok(auditLogs);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Invalid argument for entity audit logs: {EntityType}, {EntityId}", entityType, entityId);
                return BadRequest(new { message = argEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity audit logs for {EntityType} {EntityId}", entityType, entityId);
                return StatusCode(500, new { message = "Error retrieving entity audit logs", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific audit log by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AuditLogDto>> GetAuditLogById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Valid audit log ID is required" });
                }

                var auditLog = await _auditLogService.GetAuditLogByIdAsync(id);

                if (auditLog == null)
                {
                    return NotFound(new { message = $"Audit log with ID {id} not found" });
                }

                return Ok(auditLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit log with ID {AuditLogId}", id);
                return StatusCode(500, new { message = "Error retrieving audit log", error = ex.Message });
            }
        }

        /// <summary>
        /// Get audit summary statistics
        /// </summary>
        [HttpGet("summary")]
        [Authorize]
        public async Task<ActionResult<AuditSummaryDto>> GetAuditSummary(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                // Validate date range
                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    return BadRequest(new { message = "Start date cannot be after end date" });
                }

                var summary = await _auditLogService.GetAuditSummaryAsync(startDate, endDate);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit summary");
                return StatusCode(500, new { message = "Error retrieving audit summary", error = ex.Message });
            }
        }

        /// <summary>
        /// Health check endpoint for audit log system
        /// </summary>
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "healthy", service = "AuditLog", timestamp = DateTime.UtcNow });
        }
    }

    /// <summary>
    /// Enum matching the frontend AuditEntityType
    /// </summary>
    public enum AuditEntityType
    {
        CUSTOMER,
        CONTACT,
        WORKFLOW,
        QUOTE,
        INVOICE,
        SITE_VISIT
    }
}