using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return Ok(suppliers.Select(c => new SupplierDto { SupplierId = c.SupplierId, SupplierName = c.SupplierName }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suppliers");
                return StatusCode(500, new { message = "Error retrieving suppliers", error = ex.Message });
            }
        }

        [HttpGet("GetAllProductTypesForSupplier")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAllProductTypesForSupplier(int SupplierId)
        {
            try
            {
                var productTypes = await _supplierService.GetAllProductTypesForSupplierAsync(SupplierId);
                return Ok(productTypes.Select(c => new ProductTypeDto { ProductTypeId = c.ProductTypeId, Description = c.Description }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product types for supplier {SupplierId}", SupplierId);
                return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message });
            }
        }

        [HttpGet("GetAllProductsBySupplier")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsBySupplier(int SupplierId, int ProductTypeId)
        {
            try
            {
                var products = await _supplierService.GetAllProductsBySupplierAsync(SupplierId, ProductTypeId);
                return Ok(products.Select(c => new ProductDto { ProductId = c.ProductId, ProductName = c.Description }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products for supplier {SupplierId}", SupplierId);
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }
    }
}
