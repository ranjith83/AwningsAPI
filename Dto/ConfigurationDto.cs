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


   
}