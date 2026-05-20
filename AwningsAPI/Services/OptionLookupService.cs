using AwningsAPI.Database;
using AwningsAPI.Dto.Common;
using AwningsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services
{
    public class OptionLookupService : IOptionLookupService
    {
        private readonly AppDbContext _context;

        public OptionLookupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionLookupDto>> GetByCategoryAsync(string category)
        {
            return await _context.OptionLookups
                .AsNoTracking()
                .Where(o => o.Category == category && o.IsActive)
                .OrderBy(o => o.DisplayOrder)
                .Select(o => new OptionLookupDto
                {
                    Id           = o.Id,
                    Category     = o.Category,
                    Label        = o.Label,
                    Value        = o.Value,
                    Price        = o.Price,
                    DisplayOrder = o.DisplayOrder
                })
                .ToListAsync();
        }
    }
}
