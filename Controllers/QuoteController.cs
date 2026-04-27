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
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(IQuoteService quoteService, ILogger<QuoteController> logger)
        {
            _quoteService = quoteService;
            _logger = logger;
        }

        /// <summary>Get all quotes</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuoteDto>), 200)]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetAllQuotes()
        {
            var quotes = await _quoteService.GetAllQuotesAsync();
            return Ok(quotes);
        }

        /// <summary>Get quotes by workflow ID</summary>
        [HttpGet("workflow/{workflowId}")]
        [ProducesResponseType(typeof(IEnumerable<QuoteDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotesByWorkflowId(int workflowId)
        {
            var quotes = await _quoteService.GetQuotesByWorkflowIdAsync(workflowId);
            return Ok(quotes);
        }

        /// <summary>Get quote by ID</summary>
        [HttpGet("{quoteId}")]
        [ProducesResponseType(typeof(QuoteDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<QuoteDto>> GetQuoteById(int quoteId)
        {
            var quote = await _quoteService.GetQuoteByIdAsync(quoteId);
            if (quote == null)
            {
                _logger.LogWarning("Quote {QuoteId} not found", quoteId);
                return NotFound(new { message = $"Quote with ID {quoteId} not found" });
            }
            return Ok(quote);
        }

        /// <summary>Create a new draft quote</summary>
        [HttpPost]
        [ProducesResponseType(typeof(QuoteDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<QuoteDto>> CreateQuote([FromBody] CreateQuoteDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var quote = await _quoteService.CreateQuoteAsync(createDto, currentUser);
            _logger.LogInformation("Draft quote {QuoteId} created by {User} for workflow {WorkflowId}", quote.QuoteId, currentUser, createDto.WorkflowId);
            return CreatedAtAction(nameof(GetQuoteById), new { quoteId = quote.QuoteId }, quote);
        }

        /// <summary>Create a final quote from a draft quote</summary>
        [HttpPost("finalize")]
        [ProducesResponseType(typeof(QuoteDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<QuoteDto>> CreateFinalQuote([FromBody] CreateFinalQuoteDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var quote = await _quoteService.CreateFinalQuoteAsync(createDto, currentUser);
            _logger.LogInformation("Final quote {QuoteId} created by {User}", quote.QuoteId, currentUser);
            return CreatedAtAction(nameof(GetQuoteById), new { quoteId = quote.QuoteId }, quote);
        }

        /// <summary>Update an existing quote. Draft quotes with a final quote cannot be edited.</summary>
        [HttpPut("{quoteId}")]
        [ProducesResponseType(typeof(QuoteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<QuoteDto>> UpdateQuote(int quoteId, [FromBody] UpdateQuoteDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUser = User?.Identity?.Name ?? "System";
            var quote = await _quoteService.UpdateQuoteAsync(quoteId, updateDto, currentUser);
            if (quote == null)
            {
                _logger.LogWarning("Quote {QuoteId} not found for update", quoteId);
                return NotFound(new { message = $"Quote with ID {quoteId} not found" });
            }
            _logger.LogInformation("Quote {QuoteId} updated by {User}", quoteId, currentUser);
            return Ok(quote);
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
            var result = await _quoteService.DeleteQuoteAsync(quoteId);
            if (!result)
            {
                _logger.LogWarning("Quote {QuoteId} not found for deletion", quoteId);
                return NotFound(new { message = $"Quote with ID {quoteId} not found" });
            }
            _logger.LogInformation("Quote {QuoteId} deleted by {User}", quoteId, User?.Identity?.Name);
            return NoContent();
        }
    }
}
