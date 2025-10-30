using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class QuoteController : Controller
    {
        private readonly IQuoteService _quoteService;
        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }
        /// <summary>
        /// Get all quotes
        /// </summary>
        /// <returns>List of all quotes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuoteDto>), 200)]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetAllQuotes()
        {
            try
            {
                var quotes = await _quoteService.GetAllQuotesAsync();
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving quotes", error = ex.Message });
            }
        }

        /// <summary>
        /// Get quotes by workflow ID
        /// </summary>
        /// <param name="workflowId">Workflow ID</param>
        /// <returns>List of quotes for the workflow</returns>
        [HttpGet("workflow/{workflowId}")]
        [ProducesResponseType(typeof(IEnumerable<QuoteDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetQuotesByWorkflowId(int workflowId)
        {
            try
            {
                var quotes = await _quoteService.GetQuotesByWorkflowIdAsync(workflowId);
                return Ok(quotes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving quotes", error = ex.Message });
            }
        }

        /// <summary>
        /// Get quote by ID
        /// </summary>
        /// <param name="id">Quote ID</param>
        /// <returns>Quote details</returns>
        [HttpGet("{quoteId}")]
        [ProducesResponseType(typeof(QuoteDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<QuoteDto>> GetQuoteById(int quoteId)
        {
            try
            {
                var quote = await _quoteService.GetQuoteByIdAsync(quoteId);

                if (quote == null)
                    return NotFound(new { message = $"Quote with ID {quoteId} not found" });

                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new quote
        /// </summary>
        /// <param name="createDto">Quote creation data</param>
        /// <returns>Created quote</returns>
        [HttpPost]
        [ProducesResponseType(typeof(QuoteDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<QuoteDto>> CreateQuote([FromBody] CreateQuoteDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var quote = await _quoteService.CreateQuoteAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetQuoteById),
                    new { quoteId = quote.QuoteId},
                    quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing quote
        /// </summary>
        /// <param name="quoteId">Quote ID</param>
        /// <param name="updateDto">Quote update data</param>
        /// <returns>Updated Quote</returns>
        [HttpPut("{quoteId}")]
        [ProducesResponseType(typeof(QuoteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<QuoteDto>> UpdateQuote(int quoteId, [FromBody] UpdateQuoteDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var quote = await _quoteService.UpdateQuoteAsync(quoteId, updateDto, currentUser);

                if (quote == null)
                    return NotFound(new { message = $"Quote with ID {quoteId} not found" });

                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an quote
        /// </summary>
        /// <param name="quoteId">Quote ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{quoteId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteQuote(int quoteId)
        {
            try
            {
                var result = await _quoteService.DeleteQuoteAsync(quoteId);

                if (!result)
                    return NotFound(new { message = $"Quote with ID {quoteId} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting quote", error = ex.Message });
            }
        }
    }
}
