using Microsoft.AspNetCore.Mvc;
using AwningsAPI.Interfaces;
using AwningsAPI.Dto.Workflow;

namespace AwiningsIreland_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), 200)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            return Ok(await _invoiceService.GetAllInvoicesAsync());
        }

        [HttpGet("workflow/{workflowId}")]
        [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), 200)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByWorkflowId(int workflowId)
        {
            return Ok(await _invoiceService.GetInvoicesByWorkflowIdAsync(workflowId));
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), 200)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByCustomerId(int customerId)
        {
            return Ok(await _invoiceService.GetInvoicesByCustomerIdAsync(customerId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceById(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound(new { message = $"Invoice with ID {id} not found" });
            return Ok(invoice);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InvoiceDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var invoice = await _invoiceService.CreateInvoiceAsync(createDto, currentUser);
            _logger.LogInformation("Invoice {InvoiceId} created for workflow {WorkflowId} by {User}", invoice.Id, createDto.WorkflowId, currentUser);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoice(int id, [FromBody] UpdateInvoiceDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var invoice = await _invoiceService.UpdateInvoiceAsync(id, updateDto, currentUser);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found for update", id);
                return NotFound(new { message = $"Invoice with ID {id} not found" });
            }
            _logger.LogInformation("Invoice {InvoiceId} updated by {User}", id, currentUser);
            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(id);
            if (!result)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found for deletion", id);
                return NotFound(new { message = $"Invoice with ID {id} not found" });
            }
            _logger.LogInformation("Invoice {InvoiceId} deleted by {User}", id, User?.Identity?.Name);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoiceStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            if (string.IsNullOrEmpty(statusDto.Status))
                return BadRequest(new { message = "Status is required" });

            var validStatuses = new[] { "Draft", "Sent", "Paid", "Overdue", "Cancelled", "Partially Paid" };
            if (!validStatuses.Contains(statusDto.Status))
                return BadRequest(new { message = "Invalid status value" });

            var currentUser = User?.Identity?.Name ?? "System";
            var invoice = await _invoiceService.UpdateInvoiceStatusAsync(id, statusDto.Status, currentUser);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found for status update", id);
                return NotFound(new { message = $"Invoice with ID {id} not found" });
            }
            _logger.LogInformation("Invoice {InvoiceId} status changed to {Status} by {User}", id, statusDto.Status, currentUser);
            return Ok(invoice);
        }

        [HttpPost("{id}/payments")]
        [ProducesResponseType(typeof(InvoicePaymentDto), 201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoicePaymentDto>> AddPayment(int id, [FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var payment = await _invoiceService.AddPaymentAsync(id, paymentDto, currentUser);
            if (payment == null) return NotFound(new { message = $"Invoice with ID {id} not found" });
            _logger.LogInformation("Payment added to invoice {InvoiceId} by {User}", id, currentUser);
            return CreatedAtAction(nameof(GetInvoiceById), new { id }, payment);
        }

        [HttpGet("{id}/pdf")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GenerateInvoicePdf(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound(new { message = $"Invoice with ID {id} not found" });
            var pdfBytes = await _invoiceService.GenerateInvoicePdfAsync(id);
            return File(pdfBytes, "application/pdf", $"Invoice-{invoice.InvoiceNumber}.pdf");
        }

        [HttpPost("{id}/send")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> SendInvoiceEmail(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound(new { message = $"Invoice with ID {id} not found" });
            var result = await _invoiceService.SendInvoiceEmailAsync(id);
            if (result) return Ok(new { message = "Invoice sent successfully" });
            return StatusCode(500, new { message = "Failed to send invoice" });
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }
}
