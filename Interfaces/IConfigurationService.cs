using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;

namespace AwningsAPI.Interfaces
{
    public interface IConfigurationService
    {
        // ── SiteVisitValues ──────────────────────────────────────────────────
        Task<IEnumerable<SiteVisitValueDto>> GetAllSiteVisitValuesAsync();
        Task<SiteVisitValueDto> GetSiteVisitValueByIdAsync(int id);
        Task<SiteVisitValueDto> CreateSiteVisitValueAsync(CreateSiteVisitValueDto createDto, string currentUser);
        Task<SiteVisitValueDto> UpdateSiteVisitValueAsync(int id, UpdateSiteVisitValueDto updateDto, string currentUser);
        Task<bool> DeleteSiteVisitValueAsync(int id);

        // ── Brackets  (BracketDto lives in AwningsAPI.Dto.Product) ──────────
        Task<IEnumerable<BracketDto>> GetAllBracketsAsync();
        Task<IEnumerable<BracketDto>> GetBracketsByProductIdAsync(int productId);
        Task<BracketDto> GetBracketByIdAsync(int id);
        Task<BracketDto> CreateBracketAsync(CreateBracketDto createDto, string currentUser);
        Task<BracketDto> UpdateBracketAsync(int id, UpdateBracketDto updateDto, string currentUser);
        Task<bool> DeleteBracketAsync(int id);

        // ── Suppliers ─────────────────────────────────────────────────────────
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto> GetSupplierByIdAsync(int id);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createDto, string currentUser);
        Task<SupplierDto> UpdateSupplierAsync(int id, UpdateSupplierDto updateDto, string currentUser);
        Task<bool> DeleteSupplierAsync(int id);

        // ── ProductTypes  (ProductTypeDto lives in AwningsAPI.Dto.Product) ───
        Task<IEnumerable<ProductTypeDto>> GetAllProductTypesAsync();
        Task<IEnumerable<ProductTypeDto>> GetProductTypesBySupplierIdAsync(int supplierId);
        Task<ProductTypeDto> GetProductTypeByIdAsync(int id);
        Task<ProductTypeDto> CreateProductTypeAsync(CreateProductTypeDto createDto, string currentUser);
        Task<ProductTypeDto> UpdateProductTypeAsync(int id, UpdateProductTypeDto updateDto, string currentUser);
        Task<bool> DeleteProductTypeAsync(int id);

        // ── Products  (ProductDto lives in AwningsAPI.Dto.Product) ───────────
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(int supplierId);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto, string currentUser);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateDto, string currentUser);
        Task<bool> DeleteProductAsync(int id);
    }
}