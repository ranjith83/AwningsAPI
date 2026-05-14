using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;

namespace AwningsAPI.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<IEnumerable<ProductTypeDto>> GetAllProductTypesForSupplierAsync(int SupplierId);
        Task<IEnumerable<ProductDto>> GetAllProductsBySupplierAsync(int SupplierId, int ProductTypeId);
    }
}
