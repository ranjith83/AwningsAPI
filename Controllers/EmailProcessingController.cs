using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using AwningsAPI.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Add your authorization if needed
    public class EmailProcessingController : ControllerBase
    {
        private readonly IEmailProcessorService _emailProcessorService;
        private readonly AppDbContext _context;
        private readonly ILogger<EmailProcessingController> _logger;

        public EmailProcessingController(
            IEmailProcessorService emailProcessorService,
            AppDbContext context,
            ILogger<EmailProcessingController> logger)
        {
            _emailProcessorService = emailProcessorService;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Manually trigger email processing
        /// </summary>
        [HttpPost("process")]
        public async Task<IActionResult> ProcessEmails()
        {
            try
            {
                _logger.LogInformation("Manual email processing triggered");
                await _emailProcessorService.ProcessIncomingEmailsAsync();
                return Ok(new { message = "Email processing completed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during manual email processing");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Process a specific email by ID
        /// </summary>
        [AllowAnonymous]
        [HttpPost("process/{emailId}")]
        public async Task<IActionResult> ProcessSingleEmail(string emailId)
        {
            try
            {
                _logger.LogInformation($"Manual processing triggered for email: {emailId}");
                await _emailProcessorService.ProcessSingleEmailAsync(emailId);
                return Ok(new { message = $"Email {emailId} processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing email {emailId}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get all processed emails
        /// </summary>
        [AllowAnonymous]
        [HttpGet("emails")]
        public async Task<IActionResult> GetProcessedEmails(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] string? status = null)
        {
            try
            {
                var query = _context.IncomingEmails
                    .Include(e => e.Attachments)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(e => e.Category == category);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(e => e.ProcessingStatus == status);
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var emails = await query
                    .OrderByDescending(e => e.ReceivedDateTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new
                    {
                        e.Id,
                        e.EmailId,
                        e.Subject,
                        e.FromEmail,
                        e.FromName,
                        e.BodyPreview,
                        e.ReceivedDateTime,
                        e.Category,
                        e.CategoryConfidence,
                        e.ProcessingStatus,
                        e.DateProcessed,
                        e.HasAttachments,
                        AttachmentCount = e.Attachments.Count,
                        e.Importance,
                        e.ErrorMessage
                    })
                    .ToListAsync();

                return Ok(new
                {
                    page,
                    pageSize,
                    totalPages,
                    totalCount,
                    emails
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving processed emails");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific processed email by ID
        /// </summary>
        [HttpGet("emails/{id}")]
        public async Task<IActionResult> GetEmailById(int id)
        {
            try
            {
                var email = await _context.IncomingEmails
                    .Include(e => e.Attachments)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (email == null)
                {
                    return NotFound(new { error = "Email not found" });
                }

                return Ok(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving email {id}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get email processing statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var query = _context.IncomingEmails.AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(e => e.ReceivedDateTime >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(e => e.ReceivedDateTime <= endDate.Value);
                }

                var stats = await query.GroupBy(e => 1).Select(g => new
                {
                    totalEmails = g.Count(),
                    completed = g.Count(e => e.ProcessingStatus == "Completed"),
                    failed = g.Count(e => e.ProcessingStatus == "Failed"),
                    pending = g.Count(e => e.ProcessingStatus == "Pending"),
                    processing = g.Count(e => e.ProcessingStatus == "Processing")
                }).FirstOrDefaultAsync();

                var categoryStats = await query
                    .Where(e => e.Category != null)
                    .GroupBy(e => e.Category)
                    .Select(g => new
                    {
                        category = g.Key,
                        count = g.Count(),
                        averageConfidence = g.Average(e => e.CategoryConfidence ?? 0)
                    })
                    .OrderByDescending(x => x.count)
                    .ToListAsync();

                return Ok(new
                {
                    overall = stats ?? new { totalEmails = 0, completed = 0, failed = 0, pending = 0, processing = 0 },
                    byCategory = categoryStats
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving statistics");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get emails by category
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetEmailsByCategory(string category, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var totalCount = await _context.IncomingEmails
                    .Where(e => e.Category == category)
                    .CountAsync();

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var emails = await _context.IncomingEmails
                    .Include(e => e.Attachments)
                    .Where(e => e.Category == category)
                    .OrderByDescending(e => e.ReceivedDateTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    category,
                    page,
                    pageSize,
                    totalPages,
                    totalCount,
                    emails
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving emails for category {category}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Reprocess a failed email
        /// </summary>
        [HttpPost("reprocess/{id}")]
        public async Task<IActionResult> ReprocessEmail(int id)
        {
            try
            {
                var email = await _context.IncomingEmails
                    .Include(e => e.Attachments)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (email == null)
                {
                    return NotFound(new { error = "Email not found" });
                }

                if (email.ProcessingStatus != "Failed")
                {
                    return BadRequest(new { error = "Only failed emails can be reprocessed" });
                }

                // Reset status and reprocess
                email.ProcessingStatus = "Pending";
                email.ErrorMessage = null;
                await _context.SaveChangesAsync();

                await _emailProcessorService.ProcessSingleEmailAsync(email.EmailId);

                return Ok(new { message = "Email reprocessing initiated" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reprocessing email {id}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}