using Microsoft.AspNetCore.Mvc;
using AwningsAPI.Interfaces;
using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;

namespace AwiningsIreland_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SITE VISIT VALUES  —  /api/configuration/site-visit-values
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Get all site visit lookup values</summary>
        [HttpGet("site-visit-values")]
        [ProducesResponseType(typeof(IEnumerable<SiteVisitValueDto>), 200)]
        public async Task<ActionResult<IEnumerable<SiteVisitValueDto>>> GetAllSiteVisitValues()
        {
            try
            {
                var values = await _configurationService.GetAllSiteVisitValuesAsync();
                return Ok(values);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving site visit values", error = ex.Message });
            }
        }

        /// <summary>Get a single site visit value by ID</summary>
        [HttpGet("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SiteVisitValueDto>> GetSiteVisitValueById(int id)
        {
            try
            {
                var value = await _configurationService.GetSiteVisitValueByIdAsync(id);
                if (value == null)
                    return NotFound(new { message = $"Site visit value with ID {id} not found" });

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving site visit value", error = ex.Message });
            }
        }

        /// <summary>Create a new site visit lookup value</summary>
        [HttpPost("site-visit-values")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> CreateSiteVisitValue(
            [FromBody] CreateSiteVisitValueDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var value = await _configurationService.CreateSiteVisitValueAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetSiteVisitValueById),
                    new { id = value.Id },
                    value);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating site visit value", error = ex.Message });
            }
        }

        /// <summary>Update an existing site visit lookup value</summary>
        [HttpPut("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> UpdateSiteVisitValue(
            int id, [FromBody] UpdateSiteVisitValueDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var value = await _configurationService.UpdateSiteVisitValueAsync(id, updateDto, currentUser);

                if (value == null)
                    return NotFound(new { message = $"Site visit value with ID {id} not found" });

                return Ok(value);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating site visit value", error = ex.Message });
            }
        }

        /// <summary>Delete a site visit lookup value</summary>
        [HttpDelete("site-visit-values/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSiteVisitValue(int id)
        {
            try
            {
                var result = await _configurationService.DeleteSiteVisitValueAsync(id);
                if (!result)
                    return NotFound(new { message = $"Site visit value with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting site visit value", error = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  BRACKETS  —  /api/configuration/brackets
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Get all brackets</summary>
        [HttpGet("brackets")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetAllBrackets()
        {
            try
            {
                var brackets = await _configurationService.GetAllBracketsAsync();
                return Ok(brackets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving brackets", error = ex.Message });
            }
        }

        /// <summary>Get brackets for a specific product</summary>
        [HttpGet("brackets/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetBracketsByProductId(int productId)
        {
            try
            {
                var brackets = await _configurationService.GetBracketsByProductIdAsync(productId);
                return Ok(brackets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving brackets", error = ex.Message });
            }
        }

        /// <summary>Get a single bracket by ID</summary>
        [HttpGet("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BracketDto>> GetBracketById(int id)
        {
            try
            {
                var bracket = await _configurationService.GetBracketByIdAsync(id);
                if (bracket == null)
                    return NotFound(new { message = $"Bracket with ID {id} not found" });

                return Ok(bracket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving bracket", error = ex.Message });
            }
        }

        /// <summary>Create a new bracket</summary>
        [HttpPost("brackets")]
        [ProducesResponseType(typeof(BracketDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> CreateBracket([FromBody] CreateBracketDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var bracket = await _configurationService.CreateBracketAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetBracketById),
                    new { id = bracket.BracketId },
                    bracket);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating bracket", error = ex.Message });
            }
        }

        /// <summary>Update an existing bracket</summary>
        [HttpPut("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> UpdateBracket(
            int id, [FromBody] UpdateBracketDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var bracket = await _configurationService.UpdateBracketAsync(id, updateDto, currentUser);

                if (bracket == null)
                    return NotFound(new { message = $"Bracket with ID {id} not found" });

                return Ok(bracket);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating bracket", error = ex.Message });
            }
        }

        /// <summary>Delete a bracket</summary>
        [HttpDelete("brackets/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteBracket(int id)
        {
            try
            {
                var result = await _configurationService.DeleteBracketAsync(id);
                if (!result)
                    return NotFound(new { message = $"Bracket with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting bracket", error = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SUPPLIERS  —  /api/configuration/suppliers
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Get all suppliers</summary>
        [HttpGet("suppliers")]
        [ProducesResponseType(typeof(IEnumerable<SupplierDto>), 200)]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _configurationService.GetAllSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving suppliers", error = ex.Message });
            }
        }

        /// <summary>Get a single supplier by ID</summary>
        [HttpGet("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SupplierDto>> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _configurationService.GetSupplierByIdAsync(id);
                if (supplier == null)
                    return NotFound(new { message = $"Supplier with ID {id} not found" });

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving supplier", error = ex.Message });
            }
        }

        /// <summary>Create a new supplier</summary>
        [HttpPost("suppliers")]
        [ProducesResponseType(typeof(SupplierDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var supplier = await _configurationService.CreateSupplierAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetSupplierById),
                    new { id = supplier.SupplierId },
                    supplier);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating supplier", error = ex.Message });
            }
        }

        /// <summary>Update an existing supplier</summary>
        [HttpPut("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> UpdateSupplier(
            int id, [FromBody] UpdateSupplierDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var supplier = await _configurationService.UpdateSupplierAsync(id, updateDto, currentUser);

                if (supplier == null)
                    return NotFound(new { message = $"Supplier with ID {id} not found" });

                return Ok(supplier);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating supplier", error = ex.Message });
            }
        }

        /// <summary>Delete a supplier</summary>
        [HttpDelete("suppliers/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _configurationService.DeleteSupplierAsync(id);
                if (!result)
                    return NotFound(new { message = $"Supplier with ID {id} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                // Referential integrity violation — return 400 with clear message
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting supplier", error = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCT TYPES  —  /api/configuration/product-types
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Get all product types</summary>
        [HttpGet("product-types")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAllProductTypes()
        {
            try
            {
                var types = await _configurationService.GetAllProductTypesAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message });
            }
        }

        /// <summary>Get product types for a specific supplier</summary>
        [HttpGet("product-types/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypesBySupplierId(int supplierId)
        {
            try
            {
                var types = await _configurationService.GetProductTypesBySupplierIdAsync(supplierId);
                return Ok(types);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message });
            }
        }

        /// <summary>Get a single product type by ID</summary>
        [HttpGet("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductTypeDto>> GetProductTypeById(int id)
        {
            try
            {
                var type = await _configurationService.GetProductTypeByIdAsync(id);
                if (type == null)
                    return NotFound(new { message = $"Product type with ID {id} not found" });

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product type", error = ex.Message });
            }
        }

        /// <summary>Create a new product type</summary>
        [HttpPost("product-types")]
        [ProducesResponseType(typeof(ProductTypeDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> CreateProductType(
            [FromBody] CreateProductTypeDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var type = await _configurationService.CreateProductTypeAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetProductTypeById),
                    new { id = type.ProductTypeId },
                    type);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating product type", error = ex.Message });
            }
        }

        /// <summary>Update an existing product type</summary>
        [HttpPut("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> UpdateProductType(
            int id, [FromBody] UpdateProductTypeDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var type = await _configurationService.UpdateProductTypeAsync(id, updateDto, currentUser);

                if (type == null)
                    return NotFound(new { message = $"Product type with ID {id} not found" });

                return Ok(type);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating product type", error = ex.Message });
            }
        }

        /// <summary>Delete a product type</summary>
        [HttpDelete("product-types/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductType(int id)
        {
            try
            {
                var result = await _configurationService.DeleteProductTypeAsync(id);
                if (!result)
                    return NotFound(new { message = $"Product type with ID {id} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting product type", error = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCTS  —  /api/configuration/products
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Get all products</summary>
        [HttpGet("products")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            try
            {
                var products = await _configurationService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }

        /// <summary>Get products for a specific supplier</summary>
        [HttpGet("products/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplierId(int supplierId)
        {
            try
            {
                var products = await _configurationService.GetProductsBySupplierIdAsync(supplierId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
            }
        }

        /// <summary>Get a single product by ID</summary>
        [HttpGet("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                var product = await _configurationService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product", error = ex.Message });
            }
        }

        /// <summary>Create a new product</summary>
        [HttpPost("products")]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var product = await _configurationService.CreateProductAsync(createDto, currentUser);

                return CreatedAtAction(
                    nameof(GetProductById),
                    new { id = product.ProductId },
                    product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating product", error = ex.Message });
            }
        }

        /// <summary>Update an existing product</summary>
        [HttpPut("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(
            int id, [FromBody] UpdateProductDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var currentUser = User?.Identity?.Name ?? "System";
                var product = await _configurationService.UpdateProductAsync(id, updateDto, currentUser);

                if (product == null)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating product", error = ex.Message });
            }
        }

        /// <summary>Delete a product</summary>
        [HttpDelete("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _configurationService.DeleteProductAsync(id);
                if (!result)
                    return NotFound(new { message = $"Product with ID {id} not found" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting product", error = ex.Message });
            }
        }
    }
}