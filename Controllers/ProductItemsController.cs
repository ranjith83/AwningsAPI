using AwningsAPI.Database;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProductItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductItemsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all product items (lookup list for quote/invoice line items)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.ProductItems
                .OrderBy(p => p.Description)
                .Select(p => new ProductItemDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    DateCreated = p.DateCreated,
                    CreatedBy = p.CreatedBy
                })
                .ToListAsync();

            return Ok(items);
        }
    }

    public class ProductItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }
}
