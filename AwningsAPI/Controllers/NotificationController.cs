using AwningsAPI.Database;
using AwningsAPI.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(AppDbContext context, ILogger<NotificationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>Returns all unread notifications, newest first.</summary>
        [HttpGet]
        public async Task<IActionResult> GetUnread()
        {
            var notifications = await _context.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Type,
                    n.Title,
                    n.Message,
                    n.EntityType,
                    n.EntityId,
                    n.WorkflowId,
                    n.IsRead,
                    n.CreatedAt
                })
                .ToListAsync();

            return Ok(notifications);
        }

        /// <summary>Returns the count of unread notifications.</summary>
        [HttpGet("count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var count = await _context.Notifications.CountAsync(n => !n.IsRead);
            return Ok(new { count });
        }

        /// <summary>Returns all notifications (read and unread), paginated.</summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = _context.Notifications.OrderByDescending(n => n.CreatedAt);
            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }

        /// <summary>Marks a single notification as read.</summary>
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Marks all unread notifications as read.</summary>
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            var unread = await _context.Notifications.Where(n => !n.IsRead).ToListAsync();
            unread.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();
            return Ok(new { marked = unread.Count });
        }
    }
}
