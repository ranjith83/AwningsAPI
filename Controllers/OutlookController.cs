using AwningsAPI.Dto.Outlook;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/outlook")]
    [Authorize]
    public class OutlookController : ControllerBase
    {
        private readonly IOutlookService _outlookService;
        private readonly ILogger<OutlookController> _logger;

        public OutlookController(IOutlookService outlookService, ILogger<OutlookController> logger)
        {
            _outlookService = outlookService;
            _logger = logger;
        }

        [HttpPost("create-showroom-invite")]
        public async Task<ActionResult> CreateShowroomInvite([FromBody] ShowroomInviteDto dto)
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                var result = await _outlookService.CreateShowroomInviteAsync(dto, currentUser);
                return Ok(new { message = "Showroom invite created successfully", eventId = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating showroom invite");
                return StatusCode(500, new { message = "Error creating showroom invite", error = ex.Message });
            }
        }

        [HttpGet("events")]
        public async Task<ActionResult> GetCalendarEvents([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var events = await _outlookService.GetCalendarEventsAsync(startDate, endDate);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calendar events");
                return StatusCode(500, new { message = "Error retrieving calendar events", error = ex.Message });
            }
        }

        [HttpPut("events/{eventId}")]
        public async Task<ActionResult> UpdateCalendarEvent(string eventId, [FromBody] OutlookEventDto eventDto)
        {
            try
            {
                await _outlookService.UpdateCalendarEventAsync(eventId, eventDto);
                return Ok(new { message = "Event updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating calendar event");
                return StatusCode(500, new { message = "Error updating calendar event", error = ex.Message });
            }
        }

        [HttpDelete("events/{eventId}")]
        public async Task<ActionResult> DeleteCalendarEvent(string eventId)
        {
            try
            {
                await _outlookService.DeleteCalendarEventAsync(eventId);
                return Ok(new { message = "Event deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting calendar event");
                return StatusCode(500, new { message = "Error deleting calendar event", error = ex.Message });
            }
        }

        [HttpPost("send-invite-email")]
        public async Task<ActionResult> SendShowroomInviteEmail([FromBody] ShowroomInviteDto dto)
        {
            try
            {
                await _outlookService.SendShowroomInviteEmailAsync(dto);
                return Ok(new { message = "Invite email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending invite email");
                return StatusCode(500, new { message = "Error sending invite email", error = ex.Message });
            }
        }
    }
}