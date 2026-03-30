using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;

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


        // ── Arms ──────────────────────────────────────────────────────────────
        Task<IEnumerable<ArmDto>> GetAllArmsAsync();
        Task<IEnumerable<ArmDto>> GetArmsByProductIdAsync(int productId);
        Task<ArmDto> GetArmByIdAsync(int id);
        Task<ArmDto> CreateArmAsync(CreateArmDto createDto, string currentUser);
        Task<ArmDto> UpdateArmAsync(int id, UpdateArmDto updateDto, string currentUser);
        Task<bool> DeleteArmAsync(int id);

        // ── Motors ────────────────────────────────────────────────────────────
        Task<IEnumerable<MotorDto>> GetAllMotorsAsync();
        Task<IEnumerable<MotorDto>> GetMotorsByProductIdAsync(int productId);
        Task<MotorDto> GetMotorByIdAsync(int id);
        Task<MotorDto> CreateMotorAsync(CreateMotorDto createDto, string currentUser);
        Task<MotorDto> UpdateMotorAsync(int id, UpdateMotorDto updateDto, string currentUser);
        Task<bool> DeleteMotorAsync(int id);

        // ── Heaters ───────────────────────────────────────────────────────────
        Task<IEnumerable<HeaterDto>> GetAllHeatersAsync();
        Task<IEnumerable<HeaterDto>> GetHeatersByProductIdAsync(int productId);
        Task<HeaterDto> GetHeaterByIdAsync(int id);
        Task<HeaterDto> CreateHeaterAsync(CreateHeaterDto createDto, string currentUser);
        Task<HeaterDto> UpdateHeaterAsync(int id, UpdateHeaterDto updateDto, string currentUser);
        Task<bool> DeleteHeaterAsync(int id);

        // ── Non-Standard RAL Colours ──────────────────────────────────────────
        Task<IEnumerable<NonStandardRALColourDto>> GetAllNonStandardRALColoursAsync();
        Task<IEnumerable<NonStandardRALColourDto>> GetNonStandardRALColoursByProductIdAsync(int productId);
        Task<NonStandardRALColourDto> GetNonStandardRALColourByIdAsync(int id);
        Task<NonStandardRALColourDto> CreateNonStandardRALColourAsync(CreateNonStandardRALColourDto createDto, string currentUser);
        Task<NonStandardRALColourDto> UpdateNonStandardRALColourAsync(int id, UpdateNonStandardRALColourDto updateDto, string currentUser);
        Task<bool> DeleteNonStandardRALColourAsync(int id);

        // ── Projections ───────────────────────────────────────────────────────
        Task<IEnumerable<ProjectionDto>> GetAllProjectionsAsync();
        Task<IEnumerable<ProjectionDto>> GetProjectionsByProductIdAsync(int productId);
        Task<ProjectionDto> GetProjectionByIdAsync(int id);
        Task<ProjectionDto> CreateProjectionAsync(CreateProjectionDto createDto, string currentUser);
        Task<ProjectionDto> UpdateProjectionAsync(int id, UpdateProjectionDto updateDto, string currentUser);
        Task<bool> DeleteProjectionAsync(int id);

        // ── Radio Controlled Motors ───────────────────────────────────────────
        Task<IEnumerable<RadioControlledMotorDto>> GetAllRadioControlledMotorsAsync();
        Task<IEnumerable<RadioControlledMotorDto>> GetRadioControlledMotorsByProductIdAsync(int productId);
        Task<RadioControlledMotorDto> GetRadioControlledMotorByIdAsync(int id);
        Task<RadioControlledMotorDto> CreateRadioControlledMotorAsync(CreateRadioControlledMotorDto createDto, string currentUser);
        Task<RadioControlledMotorDto> UpdateRadioControlledMotorAsync(int id, UpdateRadioControlledMotorDto updateDto, string currentUser);
        Task<bool> DeleteRadioControlledMotorAsync(int id);
    }
}