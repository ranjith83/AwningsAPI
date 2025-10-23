using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Services.Suppliers;

namespace AwningsAPI.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<IEnumerable<ProductType>> GetAllProductTypesForSupplierAsync(int SupplierId);
        Task<IEnumerable<Product>> GetAllProductsBySupplierAsync(int SupplierId, int ProductTypeId);
    }
}
