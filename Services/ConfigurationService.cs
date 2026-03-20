using AwningsAPI.Database;
using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.SiteVisit;
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
                .OrderBy(v => v.Category)
                .ThenBy(v => v.DisplayOrder)
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
            // Guard: category + value combination must be unique (DB has unique index)
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
            if (entity == null)
                return null;

            // Guard: unique constraint — exclude the current row from the check
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
            if (entity == null)
                return false;

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
                .OrderBy(b => b.ProductId)
                .ThenBy(b => b.BracketName)
                .ToListAsync();

            return brackets.Select(MapBracketToDto);
        }

        public async Task<IEnumerable<BracketDto>> GetBracketsByProductIdAsync(int productId)
        {
            var brackets = await _context.Brackets
                .Where(b => b.ProductId == productId)
                .OrderBy(b => b.BracketName)
                .ToListAsync();

            return brackets.Select(MapBracketToDto);
        }

        public async Task<BracketDto> GetBracketByIdAsync(int id)
        {
            var bracket = await _context.Brackets.FindAsync(id);
            return bracket != null ? MapBracketToDto(bracket) : null;
        }

        public async Task<BracketDto> CreateBracketAsync(
            CreateBracketDto createDto, string currentUser)
        {
            // Validate product exists
            var productExists = await _context.Products
                .AnyAsync(p => p.ProductId == createDto.ProductId);

            if (!productExists)
                throw new InvalidOperationException(
                    $"Product with ID {createDto.ProductId} does not exist.");

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

        public async Task<BracketDto> UpdateBracketAsync(
            int id, UpdateBracketDto updateDto, string currentUser)
        {
            var entity = await _context.Brackets.FindAsync(id);
            if (entity == null)
                return null;

            // Validate product exists
            var productExists = await _context.Products
                .AnyAsync(p => p.ProductId == updateDto.ProductId);

            if (!productExists)
                throw new InvalidOperationException(
                    $"Product with ID {updateDto.ProductId} does not exist.");

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
            if (entity == null)
                return false;

            _context.Brackets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SUPPLIERS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers
                .OrderBy(s => s.SupplierName)
                .ToListAsync();

            return suppliers.Select(MapSupplierToDto);
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            return supplier != null ? MapSupplierToDto(supplier) : null;
        }

        public async Task<SupplierDto> CreateSupplierAsync(
            CreateSupplierDto createDto, string currentUser)
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

        public async Task<SupplierDto> UpdateSupplierAsync(
            int id, UpdateSupplierDto updateDto, string currentUser)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null)
                return null;

            entity.SupplierName = updateDto.SupplierName.Trim();

            await _context.SaveChangesAsync();

            return MapSupplierToDto(entity);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null)
                return false;

            // Guard: prevent deleting a supplier that still has product types or products
            var hasProductTypes = await _context.ProductTypes.AnyAsync(pt => pt.SupplierId == id);
            if (hasProductTypes)
                throw new InvalidOperationException(
                    "Cannot delete this supplier because it has associated product types. " +
                    "Delete those first.");

            var hasProducts = await _context.Products.AnyAsync(p => p.SupplierId == id);
            if (hasProducts)
                throw new InvalidOperationException(
                    "Cannot delete this supplier because it has associated products. " +
                    "Delete those first.");

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
                .OrderBy(pt => pt.SupplierId)
                .ThenBy(pt => pt.Description)
                .ToListAsync();

            return types.Select(MapProductTypeToDto);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetProductTypesBySupplierIdAsync(int supplierId)
        {
            var types = await _context.ProductTypes
                .Where(pt => pt.SupplierId == supplierId)
                .OrderBy(pt => pt.Description)
                .ToListAsync();

            return types.Select(MapProductTypeToDto);
        }

        public async Task<ProductTypeDto> GetProductTypeByIdAsync(int id)
        {
            var type = await _context.ProductTypes.FindAsync(id);
            return type != null ? MapProductTypeToDto(type) : null;
        }

        public async Task<ProductTypeDto> CreateProductTypeAsync(
            CreateProductTypeDto createDto, string currentUser)
        {
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.SupplierId == createDto.SupplierId);

            if (!supplierExists)
                throw new InvalidOperationException(
                    $"Supplier with ID {createDto.SupplierId} does not exist.");

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

        public async Task<ProductTypeDto> UpdateProductTypeAsync(
            int id, UpdateProductTypeDto updateDto, string currentUser)
        {
            var entity = await _context.ProductTypes.FindAsync(id);
            if (entity == null)
                return null;

            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.SupplierId == updateDto.SupplierId);

            if (!supplierExists)
                throw new InvalidOperationException(
                    $"Supplier with ID {updateDto.SupplierId} does not exist.");

            entity.SupplierId = updateDto.SupplierId;
            entity.Description = updateDto.Description.Trim();

            await _context.SaveChangesAsync();

            return MapProductTypeToDto(entity);
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var entity = await _context.ProductTypes.FindAsync(id);
            if (entity == null)
                return false;

            // Guard: prevent deleting a type that still has products
            var hasProducts = await _context.Products
                .AnyAsync(p => p.ProductTypeId == id);

            if (hasProducts)
                throw new InvalidOperationException(
                    "Cannot delete this product type because it has associated products. " +
                    "Delete those first.");

            _context.ProductTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCTS
        // ══════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .OrderBy(p => p.Description)
                .ToListAsync();

            return products.Select(MapProductToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierIdAsync(int supplierId)
        {
            var products = await _context.Products
                .Where(p => p.SupplierId == supplierId)
                .OrderBy(p => p.Description)
                .ToListAsync();

            return products.Select(MapProductToDto);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product != null ? MapProductToDto(product) : null;
        }

        public async Task<ProductDto> CreateProductAsync(
            CreateProductDto createDto, string currentUser)
        {
            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.SupplierId == createDto.SupplierId);

            if (!supplierExists)
                throw new InvalidOperationException(
                    $"Supplier with ID {createDto.SupplierId} does not exist.");

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

        public async Task<ProductDto> UpdateProductAsync(
            int id, UpdateProductDto updateDto, string currentUser)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null)
                return null;

            var supplierExists = await _context.Suppliers
                .AnyAsync(s => s.SupplierId == updateDto.SupplierId);

            if (!supplierExists)
                throw new InvalidOperationException(
                    $"Supplier with ID {updateDto.SupplierId} does not exist.");

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
            if (entity == null)
                return false;

            // Guard: prevent deleting a product that has brackets, projections, motors etc.
            var hasBrackets = await _context.Brackets.AnyAsync(b => b.ProductId == id);
            if (hasBrackets)
                throw new InvalidOperationException(
                    "Cannot delete this product because it has associated brackets. " +
                    "Delete those first.");

            var hasWorkflows = await _context.WorkflowStarts.AnyAsync(w => w.ProductId == id);
            if (hasWorkflows)
                throw new InvalidOperationException(
                    "Cannot delete this product because it is referenced by one or more workflows.");

            _context.Products.Remove(entity);
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
            ProductName = p.Description,   // Description column → ProductName property
            ProductTypeId = p.ProductTypeId,
            SupplierId = p.SupplierId,
            DateCreated = p.DateCreated,
            CreatedBy = p.CreatedBy
        };
    }
}