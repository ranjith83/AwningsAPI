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

        /// <summary>Get all quotes</summary>
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

        /// <summary>Get quotes by workflow ID</summary>
        [HttpGet("workflow/{workflowId}")]
        [ProducesResponseType(typeof(IEnumerable<QuoteDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotesByWorkflowId(int workflowId)
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

        /// <summary>Get quote by ID</summary>
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

        /// <summary>Create a new draft quote</summary>
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

                return CreatedAtAction(nameof(GetQuoteById), new { quoteId = quote.QuoteId }, quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating quote", error = ex.Message });
            }
        }

        /// <summary>Create a final quote from a draft quote</summary>
        [HttpPost("finalize")]
        [ProducesResponseType(typeof(QuoteDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<QuoteDto>> CreateFinalQuote([FromBody] CreateFinalQuoteDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var quote = await _quoteService.CreateFinalQuoteAsync(createDto, currentUser);

                return CreatedAtAction(nameof(GetQuoteById), new { quoteId = quote.QuoteId }, quote);
            }   
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating final quote", error = ex.Message });
            }
        }

        /// <summary>Update an existing quote. Draft quotes with a final quote cannot be edited.</summary>
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating quote", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a quote. Draft quotes with a final quote cannot be deleted.
        /// Deleting a final quote resets the draft so it can be finalized again.
        /// </summary>
        [HttpDelete("{quoteId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteQuote(int quoteId)
        {
            try
            {
                var result = await _quoteService.DeleteQuoteAsync(quoteId);

                if (!result)
                    return NotFound(new { message = $"Quote with ID {quoteId} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting quote", error = ex.Message });
            }
        }
    }
}
