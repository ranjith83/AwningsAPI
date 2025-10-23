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

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Get all invoices
        /// </summary>
        /// <returns>List of all invoices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), 200)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            try
            {
                var invoices = await _invoiceService.GetAllInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving invoices", error = ex.Message });
            }
        }

        /// <summary>
        /// Get invoices by workflow ID
        /// </summary>
        /// <param name="workflowId">Workflow ID</param>
        /// <returns>List of invoices for the workflow</returns>
        [HttpGet("workflow/{workflowId}")]
        [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByWorkflowId(int workflowId)
        {
            try
            {
                var invoices = await _invoiceService.GetInvoicesByWorkflowIdAsync(workflowId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving invoices", error = ex.Message });
            }
        }

        /// <summary>
        /// Get invoice by ID
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <returns>Invoice details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceById(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

                if (invoice == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving invoice", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new invoice
        /// </summary>
        /// <param name="createDto">Invoice creation data</param>
        /// <returns>Created invoice</returns>
        [HttpPost]
        [ProducesResponseType(typeof(InvoiceDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var invoice = await _invoiceService.CreateInvoiceAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetInvoiceById),
                    new { id = invoice.Id },
                    invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating invoice", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing invoice
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <param name="updateDto">Invoice update data</param>
        /// <returns>Updated invoice</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoice(int id, [FromBody] UpdateInvoiceDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var invoice = await _invoiceService.UpdateInvoiceAsync(id, updateDto, currentUser);

                if (invoice == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating invoice", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an invoice
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.DeleteInvoiceAsync(id);

                if (!result)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting invoice", error = ex.Message });
            }
        }

        /// <summary>
        /// Update invoice status
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <param name="status">New status</param>
        /// <returns>Updated invoice</returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(InvoiceDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoiceDto>> UpdateInvoiceStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            try
            {
                if (string.IsNullOrEmpty(statusDto.Status))
                    return BadRequest(new { message = "Status is required" });

                var validStatuses = new[] { "Draft", "Sent", "Paid", "Overdue", "Cancelled", "Partially Paid" };
                if (!validStatuses.Contains(statusDto.Status))
                    return BadRequest(new { message = "Invalid status value" });

                var currentUser = User?.Identity?.Name ?? "System";
                var invoice = await _invoiceService.UpdateInvoiceStatusAsync(id, statusDto.Status, currentUser);

                if (invoice == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating invoice status", error = ex.Message });
            }
        }

        /// <summary>
        /// Add payment to invoice
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <param name="paymentDto">Payment data</param>
        /// <returns>Created payment</returns>
        [HttpPost("{id}/payments")]
        [ProducesResponseType(typeof(InvoicePaymentDto), 201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<InvoicePaymentDto>> AddPayment(int id, [FromBody] CreatePaymentDto paymentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var payment = await _invoiceService.AddPaymentAsync(id, paymentDto, currentUser);

                if (payment == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                return CreatedAtAction(
                    nameof(GetInvoiceById),
                    new { id = id },
                    payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding payment", error = ex.Message });
            }
        }

        /// <summary>
        /// Generate PDF for invoice
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <returns>PDF file</returns>
        [HttpGet("{id}/pdf")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GenerateInvoicePdf(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                var pdfBytes = await _invoiceService.GenerateInvoicePdfAsync(id);
                return File(pdfBytes, "application/pdf", $"Invoice-{invoice.InvoiceNumber}.pdf");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "PDF generation not yet implemented" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
            }
        }

        /// <summary>
        /// Send invoice via email
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <returns>Success status</returns>
        [HttpPost("{id}/send")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> SendInvoiceEmail(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                    return NotFound(new { message = $"Invoice with ID {id} not found" });

                var result = await _invoiceService.SendInvoiceEmailAsync(id);

                if (result)
                    return Ok(new { message = "Invoice sent successfully" });

                return StatusCode(500, new { message = "Failed to send invoice" });
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Email sending not yet implemented" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending invoice", error = ex.Message });
            }
        }
    }

    // Additional DTO for status update
    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }
}