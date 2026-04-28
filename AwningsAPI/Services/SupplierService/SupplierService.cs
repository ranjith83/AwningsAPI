using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.Suppliers
{
    public class SupplierService: ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }
        public async Task<IEnumerable<ProductType>> GetAllProductTypesForSupplierAsync(int SupplierId)
        {
            return await _context.ProductTypes.Where(s=>s.SupplierId==SupplierId).ToListAsync();
        }
    
        public async Task<IEnumerable<Product>> GetAllProductsBySupplierAsync(int SupplierId, int ProductTypeId)
        {
            return await _context.Products.Where(p=>p.SupplierId == SupplierId && p.ProductTypeId==ProductTypeId).ToListAsync();
        }
    }
}
