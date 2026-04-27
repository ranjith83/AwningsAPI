using Microsoft.AspNetCore.Mvc;
using AwningsAPI.Interfaces;
using AwningsAPI.Dto.Workflow;

namespace AwiningsIreland_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PaymentScheduleController : ControllerBase
    {
        private readonly IPaymentScheduleService _paymentScheduleService;
        private readonly ILogger<PaymentScheduleController> _logger;

        public PaymentScheduleController(IPaymentScheduleService paymentScheduleService, ILogger<PaymentScheduleController> logger)
        {
            _paymentScheduleService = paymentScheduleService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 200)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> GetAllPaymentSchedules()
        {
            return Ok(await _paymentScheduleService.GetAllPaymentSchedulesAsync());
        }

        [HttpGet("invoice/{invoiceId}")]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 200)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> GetPaymentScheduleByInvoiceId(int invoiceId)
        {
            return Ok(await _paymentScheduleService.GetPaymentScheduleByInvoiceIdAsync(invoiceId));
        }

        [HttpGet("invoice/{invoiceId}/summary")]
        [ProducesResponseType(typeof(PaymentScheduleSummaryDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentScheduleSummaryDto>> GetPaymentScheduleSummary(int invoiceId)
        {
            var summary = await _paymentScheduleService.GetPaymentScheduleSummaryAsync(invoiceId);
            if (summary == null) return NotFound(new { message = $"Invoice with ID {invoiceId} not found" });
            return Ok(summary);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentScheduleDto>> GetPaymentScheduleItemById(int id)
        {
            var schedule = await _paymentScheduleService.GetPaymentScheduleItemByIdAsync(id);
            if (schedule == null) return NotFound(new { message = $"Payment schedule item with ID {id} not found" });
            return Ok(schedule);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> CreatePaymentSchedule(
            [FromBody] CreatePaymentScheduleDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var schedules = await _paymentScheduleService.CreatePaymentScheduleAsync(createDto, currentUser);
            _logger.LogInformation("Payment schedule created for invoice {InvoiceId} by {User}", createDto.InvoiceId, currentUser);
            return CreatedAtAction(nameof(GetPaymentScheduleByInvoiceId), new { invoiceId = createDto.InvoiceId }, schedules);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaymentScheduleDto>> UpdatePaymentScheduleItem(
            int id, [FromBody] UpdatePaymentScheduleItemDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var schedule = await _paymentScheduleService.UpdatePaymentScheduleItemAsync(id, updateDto, currentUser);
            if (schedule == null) return NotFound(new { message = $"Payment schedule item with ID {id} not found" });
            return Ok(schedule);
        }

        [HttpPost("{id}/record-payment")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaymentScheduleDto>> RecordPayment(
            int id, [FromBody] RecordSchedulePaymentDto paymentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var schedule = await _paymentScheduleService.RecordPaymentAsync(id, paymentDto, currentUser);
            if (schedule == null) return NotFound(new { message = $"Payment schedule item with ID {id} not found" });
            _logger.LogInformation("Payment recorded for schedule item {Id} by {User}", id, currentUser);
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePaymentScheduleItem(int id)
        {
            var result = await _paymentScheduleService.DeletePaymentScheduleItemAsync(id);
            if (!result) return NotFound(new { message = $"Payment schedule item with ID {id} not found" });
            return NoContent();
        }

        [HttpDelete("invoice/{invoiceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePaymentScheduleByInvoiceId(int invoiceId)
        {
            var result = await _paymentScheduleService.DeletePaymentScheduleByInvoiceIdAsync(invoiceId);
            if (!result) return NotFound(new { message = $"No payment schedule found for invoice ID {invoiceId}" });
            return NoContent();
        }

        [HttpPost("invoice/{invoiceId}/export-xero")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ExportToXero(int invoiceId)
        {
            var result = await _paymentScheduleService.ExportToXeroAsync(invoiceId);
            if (result) return Ok(new { message = "Payment schedule exported to Xero successfully" });
            return StatusCode(500, new { message = "Failed to export to Xero" });
        }
    }
}
