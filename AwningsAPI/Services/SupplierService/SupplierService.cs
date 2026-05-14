using AwningsAPI.Database;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AwningsAPI.Services.Suppliers
{
    public class SupplierService: ISupplierService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        private static readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            Size = 1
        };

        public SupplierService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            const string key = "sup:all";
            if (_cache.TryGetValue(key, out IEnumerable<SupplierDto> cached)) return cached!;
            var result = (await _context.Suppliers.ToListAsync())
                .Select(c => new SupplierDto { SupplierId = c.SupplierId, SupplierName = c.SupplierName })
                .ToList();
            _cache.Set(key, (IEnumerable<SupplierDto>)result, _cacheOptions);
            return result;
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypesForSupplierAsync(int SupplierId)
        {
            var key = $"sup:types:{SupplierId}";
            if (_cache.TryGetValue(key, out IEnumerable<ProductTypeDto> cached)) return cached!;
            var result = (await _context.ProductTypes.Where(s => s.SupplierId == SupplierId).ToListAsync())
                .Select(c => new ProductTypeDto { ProductTypeId = c.ProductTypeId, Description = c.Description })
                .ToList();
            _cache.Set(key, (IEnumerable<ProductTypeDto>)result, _cacheOptions);
            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsBySupplierAsync(int SupplierId, int ProductTypeId)
        {
            var key = $"sup:products:{SupplierId}:{ProductTypeId}";
            if (_cache.TryGetValue(key, out IEnumerable<ProductDto> cached)) return cached!;
            var result = (await _context.Products.Where(p => p.SupplierId == SupplierId && p.ProductTypeId == ProductTypeId).ToListAsync())
                .Select(c => new ProductDto { ProductId = c.ProductId, ProductName = c.Description })
                .ToList();
            _cache.Set(key, (IEnumerable<ProductDto>)result, _cacheOptions);
            return result;
        }
    }
}
