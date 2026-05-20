using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionLookupController : ControllerBase
    {
        private readonly IOptionLookupService _service;

        public OptionLookupController(IOptionLookupService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByCategory([FromQuery] string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest("category query parameter is required.");

            var options = await _service.GetByCategoryAsync(category);
            return Ok(options);
        }
    }
}
