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

        public PaymentScheduleController(IPaymentScheduleService paymentScheduleService)
        {
            _paymentScheduleService = paymentScheduleService;
        }

        /// <summary>
        /// Get all payment schedules
        /// </summary>
        /// <returns>List of all payment schedules</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 200)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> GetAllPaymentSchedules()
        {
            try
            {
                var schedules = await _paymentScheduleService.GetAllPaymentSchedulesAsync();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment schedules", error = ex.Message });
            }
        }

        /// <summary>
        /// Get payment schedule by invoice ID
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns>List of payment schedule items for the invoice</returns>
        [HttpGet("invoice/{invoiceId}")]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> GetPaymentScheduleByInvoiceId(int invoiceId)
        {
            try
            {
                var schedules = await _paymentScheduleService.GetPaymentScheduleByInvoiceIdAsync(invoiceId);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment schedule", error = ex.Message });
            }
        }

        /// <summary>
        /// Get payment schedule summary by invoice ID
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns>Payment schedule summary with totals</returns>
        [HttpGet("invoice/{invoiceId}/summary")]
        [ProducesResponseType(typeof(PaymentScheduleSummaryDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentScheduleSummaryDto>> GetPaymentScheduleSummary(int invoiceId)
        {
            try
            {
                var summary = await _paymentScheduleService.GetPaymentScheduleSummaryAsync(invoiceId);

                if (summary == null)
                    return NotFound(new { message = $"Invoice with ID {invoiceId} not found" });

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment schedule summary", error = ex.Message });
            }
        }

        /// <summary>
        /// Get payment schedule item by ID
        /// </summary>
        /// <param name="id">Payment schedule item ID</param>
        /// <returns>Payment schedule item details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaymentScheduleDto>> GetPaymentScheduleItemById(int id)
        {
            try
            {
                var schedule = await _paymentScheduleService.GetPaymentScheduleItemByIdAsync(id);

                if (schedule == null)
                    return NotFound(new { message = $"Payment schedule item with ID {id} not found" });

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving payment schedule item", error = ex.Message });
            }
        }

        /// <summary>
        /// Create payment schedule
        /// </summary>
        /// <param name="createDto">Payment schedule creation data</param>
        /// <returns>Created payment schedule items</returns>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<PaymentScheduleDto>), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<PaymentScheduleDto>>> CreatePaymentSchedule(
            [FromBody] CreatePaymentScheduleDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var schedules = await _paymentScheduleService.CreatePaymentScheduleAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetPaymentScheduleByInvoiceId),
                    new { invoiceId = createDto.InvoiceId },
                    schedules);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating payment schedule", error = ex.Message });
            }
        }

        /// <summary>
        /// Update payment schedule item
        /// </summary>
        /// <param name="id">Payment schedule item ID</param>
        /// <param name="updateDto">Payment schedule update data</param>
        /// <returns>Updated payment schedule item</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaymentScheduleDto>> UpdatePaymentScheduleItem(
            int id, [FromBody] UpdatePaymentScheduleItemDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var schedule = await _paymentScheduleService.UpdatePaymentScheduleItemAsync(id, updateDto, currentUser);

                if (schedule == null)
                    return NotFound(new { message = $"Payment schedule item with ID {id} not found" });

                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating payment schedule item", error = ex.Message });
            }
        }

        /// <summary>
        /// Record payment for schedule item
        /// </summary>
        /// <param name="id">Payment schedule item ID</param>
        /// <param name="paymentDto">Payment data</param>
        /// <returns>Updated payment schedule item</returns>
        [HttpPost("{id}/record-payment")]
        [ProducesResponseType(typeof(PaymentScheduleDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaymentScheduleDto>> RecordPayment(
            int id, [FromBody] RecordSchedulePaymentDto paymentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var schedule = await _paymentScheduleService.RecordPaymentAsync(id, paymentDto, currentUser);

                if (schedule == null)
                    return NotFound(new { message = $"Payment schedule item with ID {id} not found" });

                return Ok(schedule);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error recording payment", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete payment schedule item
        /// </summary>
        /// <param name="id">Payment schedule item ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePaymentScheduleItem(int id)
        {
            try
            {
                var result = await _paymentScheduleService.DeletePaymentScheduleItemAsync(id);

                if (!result)
                    return NotFound(new { message = $"Payment schedule item with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting payment schedule item", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete entire payment schedule for an invoice
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("invoice/{invoiceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePaymentScheduleByInvoiceId(int invoiceId)
        {
            try
            {
                var result = await _paymentScheduleService.DeletePaymentScheduleByInvoiceIdAsync(invoiceId);

                if (!result)
                    return NotFound(new { message = $"No payment schedule found for invoice ID {invoiceId}" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting payment schedule", error = ex.Message });
            }
        }

        /// <summary>
        /// Export payment schedule to Xero
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns>Success status</returns>
        [HttpPost("invoice/{invoiceId}/export-xero")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ExportToXero(int invoiceId)
        {
            try
            {
                var result = await _paymentScheduleService.ExportToXeroAsync(invoiceId);

                if (result)
                    return Ok(new { message = "Payment schedule exported to Xero successfully" });

                return StatusCode(500, new { message = "Failed to export to Xero" });
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Xero export not yet implemented" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error exporting to Xero", error = ex.Message });
            }
        }
    }
}