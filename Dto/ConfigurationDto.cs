using System.ComponentModel.DataAnnotations;

// The read DTOs for Brackets, Products and ProductTypes live in AwningsAPI.Dto.Product
// so they can be shared with the existing workflow / addon API without duplication.
// Only the Create/Update command DTOs and the config-only entities (SiteVisitValues,
// Suppliers) are defined here.
using AwningsAPI.Dto.Product;

namespace AwningsAPI.Dto.Configuration
{
    // ══════════════════════════════════════════════════════════════════════════
    //  RE-EXPORTS — make the shared read DTOs visible inside this namespace
    //  so ConfigurationService and ConfigurationController need only one using.
    // ══════════════════════════════════════════════════════════════════════════

    // BracketDto      → AwningsAPI.Dto.Product.BracketDto
    // ProductDto      → AwningsAPI.Dto.Product.ProductDto
    // ProductTypeDto  → AwningsAPI.Dto.Product.ProductTypeDto

    // ══════════════════════════════════════════════════════════════════════════
    //  SITE VISIT VALUES
    // ══════════════════════════════════════════════════════════════════════════

    public class SiteVisitValueDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateSiteVisitValueDto
    {
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        [Range(1, int.MaxValue)]
        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateSiteVisitValueDto
    {
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        [Range(1, int.MaxValue)]
        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  BRACKETS  (read DTO = AwningsAPI.Dto.Product.BracketDto)
    // ══════════════════════════════════════════════════════════════════════════

    public class CreateBracketDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(300)]
        public string BracketName { get; set; }

        [MaxLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }

    public class UpdateBracketDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(300)]
        public string BracketName { get; set; }

        [MaxLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  SUPPLIERS  (no counterpart in .Product — defined here only)
    // ══════════════════════════════════════════════════════════════════════════

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateSupplierDto
    {
        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; }
    }

    public class UpdateSupplierDto
    {
        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  PRODUCT TYPES  (read DTO = AwningsAPI.Dto.Product.ProductTypeDto)
    // ══════════════════════════════════════════════════════════════════════════

    public class CreateProductTypeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
    }

    public class UpdateProductTypeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
    }

    // ══════════════════════════════════════════════════════════════════════════
    //  PRODUCTS  (read DTO = AwningsAPI.Dto.Product.ProductDto)
    // ══════════════════════════════════════════════════════════════════════════

    public class CreateProductDto
    {
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductTypeId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductTypeId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }
    }
}