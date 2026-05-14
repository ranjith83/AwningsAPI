using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Interfaces;
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
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
        {
            try
            {
                return Ok(await _supplierService.GetAllSuppliersAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suppliers");
                return StatusCode(500, new { message = "Error retrieving suppliers", error = ex.Message });
            }
        }

        [HttpGet("GetAllProductTypesForSupplier")]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAllProductTypesForSupplier(int SupplierId)
        {
            try
            {
                return Ok(await _supplierService.GetAllProductTypesForSupplierAsync(SupplierId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product types for supplier {SupplierId}", SupplierId);
                return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message });
            }
        }

        [HttpGet("GetAllProductsBySupplier")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsBySupplier(int SupplierId, int ProductTypeId)
        {
            try
            {
                return Ok(await _supplierService.GetAllProductsBySupplierAsync(SupplierId, ProductTypeId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products for supplier {SupplierId}", SupplierId);
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }
    }
}
