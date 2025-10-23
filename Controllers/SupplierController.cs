using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Customers;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Services.CustomerService;
using AwningsAPI.Services.Suppliers;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();

            var suppliersDto = suppliers.Select(c => new SupplierDto
            {
                 SupplierId = c.SupplierId,
                 SupplierName = c.SupplierName,
            }).ToList();

            return Ok(suppliersDto);
        }

        [HttpGet("GetAllProductTypesForSupplier")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetAllProductTypesForSupplier(int SupplierId)
        {
            var productTypes = await _supplierService.GetAllProductTypesForSupplierAsync(SupplierId);

            var productTypeDto = productTypes.Select(c => new ProductTypeDto
            {
                 ProductTypeId = c.ProductTypeId,
                 Description = c.Description,
            }).ToList();

            return Ok(productTypeDto);
        }
       
        [HttpGet("GetAllProductsBySupplier")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsBySupplier(int SupplierId, int ProductTypeId)
        {
            var products = await _supplierService.GetAllProductsBySupplierAsync(SupplierId, ProductTypeId);

            var productDto = products.Select(c => new ProductDto
            {
                ProductId = c.ProductId,
                ProductName = c.Description,
            }).ToList();

            return Ok(productDto);
        }
    }
}
