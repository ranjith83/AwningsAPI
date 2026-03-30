using AwningsAPI.Database;
using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.SiteVisit;
using AwningsAPI.Model.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.ConfigurationService
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly AppDbContext _context;

        public ConfigurationService(AppDbContext context)
        {
            _context = context;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SITE VISIT VALUES
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<SiteVisitValueDto>> GetAllSiteVisitValuesAsync()
        {
            var values = await _context.SiteVisitValues
                .OrderBy(v => v.Category).ThenBy(v => v.DisplayOrder)
                .ToListAsync();
            return values.Select(MapSiteVisitValueToDto);
        }

        public async Task<SiteVisitValueDto> GetSiteVisitValueByIdAsync(int id)
        {
            var value = await _context.SiteVisitValues.FindAsync(id);
            return value != null ? MapSiteVisitValueToDto(value) : null;
        }

        public async Task<SiteVisitValueDto> CreateSiteVisitValueAsync(
            CreateSiteVisitValueDto createDto, string currentUser)
        {
            var exists = await _context.SiteVisitValues
                .AnyAsync(v => v.Category == createDto.Category.Trim()
                            && v.Value == createDto.Value.Trim());
            if (exists)
                throw new InvalidOperationException(
                    $"A value '{createDto.Value}' already exists in category '{createDto.Category}'.");

            var entity = new SiteVisitValues
            {
                Category = createDto.Category.Trim(),
                Value = createDto.Value.Trim(),
                DisplayOrder = createDto.DisplayOrder,
                IsActive = createDto.IsActive,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.SiteVisitValues.Add(entity);
            await _context.SaveChangesAsync();
            return MapSiteVisitValueToDto(entity);
        }

        public async Task<SiteVisitValueDto> UpdateSiteVisitValueAsync(
            int id, UpdateSiteVisitValueDto updateDto, string currentUser)
        {
            var entity = await _context.SiteVisitValues.FindAsync(id);
            if (entity == null) return null;

            var duplicate = await _context.SiteVisitValues
                .AnyAsync(v => v.Id != id
                            && v.Category == updateDto.Category.Trim()
                            && v.Value == updateDto.Value.Trim());
            if (duplicate)
                throw new InvalidOperationException(
                    $"A value '{updateDto.Value}' already exists in category '{updateDto.Category}'.");

            entity.Category = updateDto.Category.Trim();
            entity.Value = updateDto.Value.Trim();
            entity.DisplayOrder = updateDto.DisplayOrder;
            entity.IsActive = updateDto.IsActive;
            await _context.SaveChangesAsync();
            return MapSiteVisitValueToDto(entity);
        }

        public async Task<bool> DeleteSiteVisitValueAsync(int id)
        {
            var entity = await _context.SiteVisitValues.FindAsync(id);
            if (entity == null) return false;
            _context.SiteVisitValues.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  BRACKETS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<BracketDto>> GetAllBracketsAsync()
        {
            var brackets = await _context.Brackets
                .OrderBy(b => b.ProductId).ThenBy(b => b.BracketName)
                .ToListAsync();
            return brackets.Select(MapBracketToDto);
        }

        public async Task<IEnumerable<BracketDto>> GetBracketsByProductIdAsync(int productId)
        {
            var brackets = await _context.Brackets
                .Where(b => b.ProductId == productId).OrderBy(b => b.BracketName)
                .ToListAsync();
            return brackets.Select(MapBracketToDto);
        }

        public async Task<BracketDto> GetBracketByIdAsync(int id)
        {
            var bracket = await _context.Brackets.FindAsync(id);
            return bracket != null ? MapBracketToDto(bracket) : null;
        }

        public async Task<BracketDto> CreateBracketAsync(CreateBracketDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new Brackets
            {
                ProductId = createDto.ProductId,
                BracketName = createDto.BracketName.Trim(),
                PartNumber = (createDto.PartNumber ?? string.Empty).Trim(),
                Price = createDto.Price,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Brackets.Add(entity);
            await _context.SaveChangesAsync();
            return MapBracketToDto(entity);
        }

        public async Task<BracketDto> UpdateBracketAsync(int id, UpdateBracketDto updateDto, string currentUser)
        {
            var entity = await _context.Brackets.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.BracketName = updateDto.BracketName.Trim();
            entity.PartNumber = (updateDto.PartNumber ?? string.Empty).Trim();
            entity.Price = updateDto.Price;
            await _context.SaveChangesAsync();
            return MapBracketToDto(entity);
        }

        public async Task<bool> DeleteBracketAsync(int id)
        {
            var entity = await _context.Brackets.FindAsync(id);
            if (entity == null) return false;
            _context.Brackets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SUPPLIERS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.OrderBy(s => s.SupplierName).ToListAsync();
            return suppliers.Select(MapSupplierToDto);
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            return supplier != null ? MapSupplierToDto(supplier) : null;
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto createDto, string currentUser)
        {
            var entity = new Supplier
            {
                SupplierName = createDto.SupplierName.Trim(),
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Suppliers.Add(entity);
            await _context.SaveChangesAsync();
            return MapSupplierToDto(entity);
        }

        public async Task<SupplierDto> UpdateSupplierAsync(int id, UpdateSupplierDto updateDto, string currentUser)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null) return null;
            entity.SupplierName = updateDto.SupplierName.Trim();
            await _context.SaveChangesAsync();
            return MapSupplierToDto(entity);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null) return false;

            if (await _context.ProductTypes.AnyAsync(pt => pt.SupplierId == id))
                throw new InvalidOperationException(
                    "Cannot delete this supplier because it has associated product types. Delete those first.");
            if (await _context.Products.AnyAsync(p => p.SupplierId == id))
                throw new InvalidOperationException(
                    "Cannot delete this supplier because it has associated products. Delete those first.");

            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCT TYPES
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypesAsync()
        {
            var types = await _context.ProductTypes
                .OrderBy(pt => pt.SupplierId).ThenBy(pt => pt.Description)
                .ToListAsync();
            return types.Select(MapProductTypeToDto);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetProductTypesBySupplierIdAsync(int supplierId)
        {
            var types = await _context.ProductTypes
                .Where(pt => pt.SupplierId == supplierId).OrderBy(pt => pt.Description)
                .ToListAsync();
            return types.Select(MapProductTypeToDto);
        }

        public async Task<ProductTypeDto> GetProductTypeByIdAsync(int id)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            return type != null ? MapProductTypeToDto(type) : null;
        }

        public async Task<ProductTypeDto> CreateProductTypeAsync(CreateProductTypeDto createDto, string currentUser)
        {
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == createDto.SupplierId);
            if (!supplierExists)
                throw new InvalidOperationException($"Supplier with ID {createDto.SupplierId} does not exist.");

            var entity = new ProductType
            {
                SupplierId = createDto.SupplierId,
                Description = createDto.Description.Trim(),
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.ProductTypes.Add(entity);
            await _context.SaveChangesAsync();
            return MapProductTypeToDto(entity);
        }

        public async Task<ProductTypeDto> UpdateProductTypeAsync(int id, UpdateProductTypeDto updateDto, string currentUser)
        {
            var entity = await _context.ProductTypes.FindAsync(id);
            if (entity == null) return null;

            var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == updateDto.SupplierId);
            if (!supplierExists)
                throw new InvalidOperationException($"Supplier with ID {updateDto.SupplierId} does not exist.");

            entity.SupplierId = updateDto.SupplierId;
            entity.Description = updateDto.Description.Trim();
            await _context.SaveChangesAsync();
            return MapProductTypeToDto(entity);
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var entity = await _context.ProductTypes.FindAsync(id);
            if (entity == null) return false;

            if (await _context.Products.AnyAsync(p => p.ProductTypeId == id))
                throw new InvalidOperationException(
                    "Cannot delete this product type because it has associated products. Delete those first.");

            _context.ProductTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCTS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.OrderBy(p => p.Description).ToListAsync();
            return products.Select(MapProductToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(int supplierId)
        {
            var products = await _context.Products
                .Where(p => p.SupplierId == supplierId).OrderBy(p => p.Description)
                .ToListAsync();
            return products.Select(MapProductToDto);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product != null ? MapProductToDto(product) : null;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto, string currentUser)
        {
            var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == createDto.SupplierId);
            if (!supplierExists)
                throw new InvalidOperationException($"Supplier with ID {createDto.SupplierId} does not exist.");

            var typeExists = await _context.ProductTypes
                .AnyAsync(pt => pt.ProductTypeId == createDto.ProductTypeId
                             && pt.SupplierId == createDto.SupplierId);
            if (!typeExists)
                throw new InvalidOperationException(
                    $"Product type {createDto.ProductTypeId} does not belong to supplier {createDto.SupplierId}.");

            var entity = new Product
            {
                Description = createDto.Description.Trim(),
                ProductTypeId = createDto.ProductTypeId,
                SupplierId = createDto.SupplierId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return MapProductToDto(entity);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateDto, string currentUser)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return null;

            var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == updateDto.SupplierId);
            if (!supplierExists)
                throw new InvalidOperationException($"Supplier with ID {updateDto.SupplierId} does not exist.");

            var typeExists = await _context.ProductTypes
                .AnyAsync(pt => pt.ProductTypeId == updateDto.ProductTypeId
                             && pt.SupplierId == updateDto.SupplierId);
            if (!typeExists)
                throw new InvalidOperationException(
                    $"Product type {updateDto.ProductTypeId} does not belong to supplier {updateDto.SupplierId}.");

            entity.Description = updateDto.Description.Trim();
            entity.ProductTypeId = updateDto.ProductTypeId;
            entity.SupplierId = updateDto.SupplierId;
            await _context.SaveChangesAsync();
            return MapProductToDto(entity);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null) return false;

            if (await _context.Brackets.AnyAsync(b => b.ProductId == id))
                throw new InvalidOperationException(
                    "Cannot delete this product because it has associated brackets. Delete those first.");
            if (await _context.WorkflowStarts.AnyAsync(w => w.ProductId == id))
                throw new InvalidOperationException(
                    "Cannot delete this product because it is referenced by one or more workflows.");

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ARMS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ArmDto>> GetAllArmsAsync()
        {
            var arms = await _context.Arms
                .OrderBy(a => a.ProductId).ThenBy(a => a.Description)
                .ToListAsync();
            return arms.Select(MapArmToDto);
        }

        public async Task<IEnumerable<ArmDto>> GetArmsByProductIdAsync(int productId)
        {
            var arms = await _context.Arms
                .Where(a => a.ProductId == productId).OrderBy(a => a.Description)
                .ToListAsync();
            return arms.Select(MapArmToDto);
        }

        public async Task<ArmDto> GetArmByIdAsync(int id)
        {
            var arm = await _context.Arms.FindAsync(id);
            return arm != null ? MapArmToDto(arm) : null;
        }

        public async Task<ArmDto> CreateArmAsync(CreateArmDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new Arms
            {
                ProductId = createDto.ProductId,
                Description = createDto.Description.Trim(),
                Price = createDto.Price,
                ArmTypeId = createDto.ArmTypeId,
                BfId = createDto.BfId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Arms.Add(entity);
            await _context.SaveChangesAsync();
            return MapArmToDto(entity);
        }

        public async Task<ArmDto> UpdateArmAsync(int id, UpdateArmDto updateDto, string currentUser)
        {
            var entity = await _context.Arms.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.Description = updateDto.Description.Trim();
            entity.Price = updateDto.Price;
            entity.ArmTypeId = updateDto.ArmTypeId;
            entity.BfId = updateDto.BfId;
            await _context.SaveChangesAsync();
            return MapArmToDto(entity);
        }

        public async Task<bool> DeleteArmAsync(int id)
        {
            var entity = await _context.Arms.FindAsync(id);
            if (entity == null) return false;
            _context.Arms.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MOTORS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<MotorDto>> GetAllMotorsAsync()
        {
            var motors = await _context.Motors
                .OrderBy(m => m.ProductId).ThenBy(m => m.Description)
                .ToListAsync();
            return motors.Select(MapMotorToDto);
        }

        public async Task<IEnumerable<MotorDto>> GetMotorsByProductIdAsync(int productId)
        {
            var motors = await _context.Motors
                .Where(m => m.ProductId == productId).OrderBy(m => m.Description)
                .ToListAsync();
            return motors.Select(MapMotorToDto);
        }

        public async Task<MotorDto> GetMotorByIdAsync(int id)
        {
            var motor = await _context.Motors.FindAsync(id);
            return motor != null ? MapMotorToDto(motor) : null;
        }

        public async Task<MotorDto> CreateMotorAsync(CreateMotorDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new Motors
            {
                ProductId = createDto.ProductId,
                Description = createDto.Description.Trim(),
                Price = createDto.Price,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Motors.Add(entity);
            await _context.SaveChangesAsync();
            return MapMotorToDto(entity);
        }

        public async Task<MotorDto> UpdateMotorAsync(int id, UpdateMotorDto updateDto, string currentUser)
        {
            var entity = await _context.Motors.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.Description = updateDto.Description.Trim();
            entity.Price = updateDto.Price;
            await _context.SaveChangesAsync();
            return MapMotorToDto(entity);
        }

        public async Task<bool> DeleteMotorAsync(int id)
        {
            var entity = await _context.Motors.FindAsync(id);
            if (entity == null) return false;
            _context.Motors.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HEATERS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<HeaterDto>> GetAllHeatersAsync()
        {
            var heaters = await _context.Heaters
                .OrderBy(h => h.ProductId).ThenBy(h => h.Description)
                .ToListAsync();
            return heaters.Select(MapHeaterToDto);
        }

        public async Task<IEnumerable<HeaterDto>> GetHeatersByProductIdAsync(int productId)
        {
            var heaters = await _context.Heaters
                .Where(h => h.ProductId == productId).OrderBy(h => h.Description)
                .ToListAsync();
            return heaters.Select(MapHeaterToDto);
        }

        public async Task<HeaterDto> GetHeaterByIdAsync(int id)
        {
            var heater = await _context.Heaters.FindAsync(id);
            return heater != null ? MapHeaterToDto(heater) : null;
        }

        public async Task<HeaterDto> CreateHeaterAsync(CreateHeaterDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new Heaters
            {
                ProductId = createDto.ProductId,
                Description = createDto.Description.Trim(),
                Price = createDto.Price,
                PriceNonRALColour = createDto.PriceNonRALColour,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Heaters.Add(entity);
            await _context.SaveChangesAsync();
            return MapHeaterToDto(entity);
        }

        public async Task<HeaterDto> UpdateHeaterAsync(int id, UpdateHeaterDto updateDto, string currentUser)
        {
            var entity = await _context.Heaters.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.Description = updateDto.Description.Trim();
            entity.Price = updateDto.Price;
            entity.PriceNonRALColour = updateDto.PriceNonRALColour;
            await _context.SaveChangesAsync();
            return MapHeaterToDto(entity);
        }

        public async Task<bool> DeleteHeaterAsync(int id)
        {
            var entity = await _context.Heaters.FindAsync(id);
            if (entity == null) return false;
            _context.Heaters.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NON-STANDARD RAL COLOURS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<NonStandardRALColourDto>> GetAllNonStandardRALColoursAsync()
        {
            var colours = await _context.nonStandardRALColours
                .OrderBy(c => c.ProductId).ThenBy(c => c.WidthCm)
                .ToListAsync();
            return colours.Select(MapRALColourToDto);
        }

        public async Task<IEnumerable<NonStandardRALColourDto>> GetNonStandardRALColoursByProductIdAsync(int productId)
        {
            var colours = await _context.nonStandardRALColours
                .Where(c => c.ProductId == productId).OrderBy(c => c.WidthCm)
                .ToListAsync();
            return colours.Select(MapRALColourToDto);
        }

        public async Task<NonStandardRALColourDto> GetNonStandardRALColourByIdAsync(intid)
        {
            var colour = await _context.nonStandardRALColours.FindAsync(id);
            return colour != null ? MapRALColourToDto(colour) : null;
        }

        public async Task<NonStandardRALColourDto> CreateNonStandardRALColourAsync(
            CreateNonStandardRALColourDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new NonStandardRALColours
            {
                ProductId = createDto.ProductId,
                WidthCm = createDto.WidthCm,
                Price = createDto.Price,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.nonStandardRALColours.Add(entity);
            await _context.SaveChangesAsync();
            return MapRALColourToDto(entity);
        }

        public async Task<NonStandardRALColourDto> UpdateNonStandardRALColourAsync(
            int id, UpdateNonStandardRALColourDto updateDto, string currentUser)
        {
            var entity = await _context.nonStandardRALColours.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.WidthCm = updateDto.WidthCm;
            entity.Price = updateDto.Price;
            await _context.SaveChangesAsync();
            return MapRALColourToDto(entity);
        }

        public async Task<bool> DeleteNonStandardRALColourAsync(int id)
        {
            var entity = await _context.nonStandardRALColours.FindAsync(id);
            if (entity == null) return false;
            _context.nonStandardRALColours.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PROJECTIONS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ProjectionDto>> GetAllProjectionsAsync()
        {
            var projections = await _context.Projections
                .OrderBy(p => p.ProductId).ThenBy(p => p.Width_cm).ThenBy(p => p.Projection_cm)
                .ToListAsync();
            return projections.Select(MapProjectionToDto);
        }

        public async Task<IEnumerable<ProjectionDto>> GetProjectionsByProductIdAsync(int productId)
        {
            var projections = await _context.Projections
                .Where(p => p.ProductId == productId)
                .OrderBy(p => p.Width_cm).ThenBy(p => p.Projection_cm)
                .ToListAsync();
            return projections.Select(MapProjectionToDto);
        }

        public async Task<ProjectionDto> GetProjectionByIdAsync(int id)
        {
            var projection = await _context.Projections.FindAsync(id);
            return projection != null ? MapProjectionToDto(projection) : null;
        }

        public async Task<ProjectionDto> CreateProjectionAsync(CreateProjectionDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new Projections
            {
                ProductId = createDto.ProductId,
                Width_cm = createDto.WidthCm,
                Projection_cm = createDto.ProjectionCm,
                Price = createDto.Price,
                ArmTypeId = createDto.ArmTypeId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.Projections.Add(entity);
            await _context.SaveChangesAsync();
            return MapProjectionToDto(entity);
        }

        public async Task<ProjectionDto> UpdateProjectionAsync(int id, UpdateProjectionDto updateDto, string currentUser)
        {
            var entity = await _context.Projections.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.Width_cm = updateDto.WidthCm;
            entity.Projection_cm = updateDto.ProjectionCm;
            entity.Price = updateDto.Price;
            entity.ArmTypeId = updateDto.ArmTypeId;
            await _context.SaveChangesAsync();
            return MapProjectionToDto(entity);
        }

        public async Task<bool> DeleteProjectionAsync(int id)
        {
            var entity = await _context.Projections.FindAsync(id);
            if (entity == null) return false;
            _context.Projections.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  RADIO CONTROLLED MOTORS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<RadioControlledMotorDto>> GetAllRadioControlledMotorsAsync()
        {
            var motors = await _context.radioControlledMotors
                .OrderBy(m => m.ProductId).ThenBy(m => m.Width_cm)
                .ToListAsync();
            return motors.Select(MapRadioMotorToDto);
        }

        public async Task<IEnumerable<RadioControlledMotorDto>> GetRadioControlledMotorsByProductIdAsync(int productId)
        {
            var motors = await _context.radioControlledMotors
                .Where(m => m.ProductId == productId).OrderBy(m => m.Width_cm)
                .ToListAsync();
            return motors.Select(MapRadioMotorToDto);
        }

        public async Task<RadioControlledMotorDto> GetRadioControlledMotorByIdAsync(int id)
        {
            var motor = await _context.radioControlledMotors.FindAsync(id);
            return motor != null ? MapRadioMotorToDto(motor) : null;
        }

        public async Task<RadioControlledMotorDto> CreateRadioControlledMotorAsync(
            CreateRadioControlledMotorDto createDto, string currentUser)
        {
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == createDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {createDto.ProductId} does not exist.");

            var entity = new RadioControlledMotors
            {
                ProductId = createDto.ProductId,
                Description = createDto.Description.Trim(),
                Width_cm = createDto.WidthCm,
                Price = createDto.Price,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.radioControlledMotors.Add(entity);
            await _context.SaveChangesAsync();
            return MapRadioMotorToDto(entity);
        }

        public async Task<RadioControlledMotorDto> UpdateRadioControlledMotorAsync(
            int id, UpdateRadioControlledMotorDto updateDto, string currentUser)
        {
            var entity = await _context.radioControlledMotors.FindAsync(id);
            if (entity == null) return null;

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == updateDto.ProductId);
            if (!productExists)
                throw new InvalidOperationException($"Product with ID {updateDto.ProductId} does not exist.");

            entity.ProductId = updateDto.ProductId;
            entity.Description = updateDto.Description.Trim();
            entity.Width_cm = updateDto.WidthCm;
            entity.Price = updateDto.Price;
            await _context.SaveChangesAsync();
            return MapRadioMotorToDto(entity);
        }

        public async Task<bool> DeleteRadioControlledMotorAsync(int id)
        {
            var entity = await _context.radioControlledMotors.FindAsync(id);
            if (entity == null) return false;
            _context.radioControlledMotors.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRIVATE MAPPERS
        // ══════════════════════════════════════════════════════════════════════

        private static SiteVisitValueDto MapSiteVisitValueToDto(SiteVisitValues v) => new()
        {
            Id = v.Id,
            Category = v.Category,
            Value = v.Value,
            DisplayOrder = v.DisplayOrder,
            IsActive = v.IsActive,
            DateCreated = v.DateCreated,
            CreatedBy = v.CreatedBy
        };

        private static AwningsAPI.Dto.Product.BracketDto MapBracketToDto(Brackets b) => new()
        {
            BracketId = b.BracketId,
            ProductId = b.ProductId,
            BracketName = b.BracketName,
            PartNumber = b.PartNumber ?? string.Empty,
            Price = b.Price,
            DateCreated = b.DateCreated,
            CreatedBy = b.CreatedBy
        };

        private static SupplierDto MapSupplierToDto(Supplier s) => new()
        {
            SupplierId = s.SupplierId,
            SupplierName = s.SupplierName,
            DateCreated = s.DateCreated,
            CreatedBy = s.CreatedBy
        };

        private static AwningsAPI.Dto.Product.ProductTypeDto MapProductTypeToDto(ProductType pt) => new()
        {
            ProductTypeId = pt.ProductTypeId,
            SupplierId = pt.SupplierId,
            Description = pt.Description,
            DateCreated = pt.DateCreated,
            CreatedBy = pt.CreatedBy
        };

        private static AwningsAPI.Dto.Product.ProductDto MapProductToDto(Product p) => new()
        {
            ProductId = p.ProductId,
            ProductName = p.Description,
            ProductTypeId = p.ProductTypeId,
            SupplierId = p.SupplierId,
            DateCreated = p.DateCreated,
            CreatedBy = p.CreatedBy
        };

        private static ArmDto MapArmToDto(Arms a) => new()
        {
            ArmId = a.ArmId,
            ProductId = a.ProductId,
            Description = a.Description,
            Price = a.Price,
            ArmTypeId = a.ArmTypeId,
            BfId = a.BfId,
            DateCreated = a.DateCreated,
            CreatedBy = a.CreatedBy
        };

        private static MotorDto MapMotorToDto(Motors m) => new()
        {
            MotorId = m.MotorId,
            ProductId = m.ProductId,
            Description = m.Description,
            Price = m.Price,
            DateCreated = m.DateCreated,
            CreatedBy = m.CreatedBy
        };

        private static HeaterDto MapHeaterToDto(Heaters h) => new()
        {
            HeaterId = h.HeaterId,
            ProductId = h.ProductId,
            Description = h.Description,
            Price = h.Price,
            PriceNonRALColour = h.PriceNonRALColour,
            DateCreated = h.DateCreated,
            CreatedBy = h.CreatedBy ?? string.Empty
        };

        private static NonStandardRALColourDto MapRALColourToDto(NonStandardRALColours c) => new()
        {
            RALColourId = c.RALColourId,
            ProductId = c.ProductId,
            WidthCm = c.WidthCm,
            Price = c.Price,
            DateCreated = c.DateCreated,
            CreatedBy = c.CreatedBy ?? string.Empty
        };

        private static ProjectionDto MapProjectionToDto(Projections p) => new()
        {
            ProjectionId = p.ProjectionId,
            ProductId = p.ProductId,
            WidthCm = p.Width_cm,
            ProjectionCm = p.Projection_cm,
            Price = p.Price,
            ArmTypeId = p.ArmTypeId,
            DateCreated = p.DateCreated,
            CreatedBy = p.CreatedBy
        };

        private static RadioControlledMotorDto MapRadioMotorToDto(RadioControlledMotors m) => new()
        {
            RadioMotorId = m.RadioMotorId,
            ProductId = m.ProductId,
            Description = m.Description,
            WidthCm = m.Width_cm,
            Price = m.Price,
            DateCreated = m.DateCreated,
            CreatedBy = m.CreatedBy
        };
    }
}