using Microsoft.AspNetCore.Mvc;
using AwningsAPI.Interfaces;
using AwningsAPI.Dto.Configuration;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;

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

        [HttpGet("site-visit-values")]
        [ProducesResponseType(typeof(IEnumerable<SiteVisitValueDto>), 200)]
        public async Task<ActionResult<IEnumerable<SiteVisitValueDto>>> GetAllSiteVisitValues()
        {
            try { return Ok(await _configurationService.GetAllSiteVisitValuesAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving site visit values", error = ex.Message }); }
        }

        [HttpGet("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SiteVisitValueDto>> GetSiteVisitValueById(int id)
        {
            try
            {
                var value = await _configurationService.GetSiteVisitValueByIdAsync(id);
                return value == null ? NotFound(new { message = $"Site visit value with ID {id} not found" }) : Ok(value);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving site visit value", error = ex.Message }); }
        }

        [HttpPost("site-visit-values")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> CreateSiteVisitValue([FromBody] CreateSiteVisitValueDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var value = await _configurationService.CreateSiteVisitValueAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetSiteVisitValueById), new { id = value.Id }, value);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating site visit value", error = ex.Message }); }
        }

        [HttpPut("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> UpdateSiteVisitValue(int id, [FromBody] UpdateSiteVisitValueDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var value = await _configurationService.UpdateSiteVisitValueAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return value == null ? NotFound(new { message = $"Site visit value with ID {id} not found" }) : Ok(value);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating site visit value", error = ex.Message }); }
        }

        [HttpDelete("site-visit-values/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSiteVisitValue(int id)
        {
            try
            {
                var result = await _configurationService.DeleteSiteVisitValueAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Site visit value with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting site visit value", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  BRACKETS  —  /api/configuration/brackets
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("brackets")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetAllBrackets()
        {
            try { return Ok(await _configurationService.GetAllBracketsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving brackets", error = ex.Message }); }
        }

        [HttpGet("brackets/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetBracketsByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetBracketsByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving brackets", error = ex.Message }); }
        }

        [HttpGet("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BracketDto>> GetBracketById(int id)
        {
            try
            {
                var bracket = await _configurationService.GetBracketByIdAsync(id);
                return bracket == null ? NotFound(new { message = $"Bracket with ID {id} not found" }) : Ok(bracket);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving bracket", error = ex.Message }); }
        }

        [HttpPost("brackets")]
        [ProducesResponseType(typeof(BracketDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> CreateBracket([FromBody] CreateBracketDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var bracket = await _configurationService.CreateBracketAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetBracketById), new { id = bracket.BracketId }, bracket);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating bracket", error = ex.Message }); }
        }

        [HttpPut("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> UpdateBracket(int id, [FromBody] UpdateBracketDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var bracket = await _configurationService.UpdateBracketAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return bracket == null ? NotFound(new { message = $"Bracket with ID {id} not found" }) : Ok(bracket);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating bracket", error = ex.Message }); }
        }

        [HttpDelete("brackets/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteBracket(int id)
        {
            try
            {
                var result = await _configurationService.DeleteBracketAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Bracket with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting bracket", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SUPPLIERS  —  /api/configuration/suppliers
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("suppliers")]
        [ProducesResponseType(typeof(IEnumerable<SupplierDto>), 200)]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
        {
            try { return Ok(await _configurationService.GetAllSuppliersAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving suppliers", error = ex.Message }); }
        }

        [HttpGet("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SupplierDto>> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _configurationService.GetSupplierByIdAsync(id);
                return supplier == null ? NotFound(new { message = $"Supplier with ID {id} not found" }) : Ok(supplier);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving supplier", error = ex.Message }); }
        }

        [HttpPost("suppliers")]
        [ProducesResponseType(typeof(SupplierDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var supplier = await _configurationService.CreateSupplierAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.SupplierId }, supplier);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating supplier", error = ex.Message }); }
        }

        [HttpPut("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> UpdateSupplier(int id, [FromBody] UpdateSupplierDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var supplier = await _configurationService.UpdateSupplierAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return supplier == null ? NotFound(new { message = $"Supplier with ID {id} not found" }) : Ok(supplier);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating supplier", error = ex.Message }); }
        }

        [HttpDelete("suppliers/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _configurationService.DeleteSupplierAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Supplier with ID {id} not found" });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting supplier", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCT TYPES  —  /api/configuration/product-types
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("product-types")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAllProductTypes()
        {
            try { return Ok(await _configurationService.GetAllProductTypesAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message }); }
        }

        [HttpGet("product-types/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypesBySupplierId(int supplierId)
        {
            try { return Ok(await _configurationService.GetProductTypesBySupplierIdAsync(supplierId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving product types", error = ex.Message }); }
        }

        [HttpGet("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductTypeDto>> GetProductTypeById(int id)
        {
            try
            {
                var type = await _configurationService.GetProductTypeByIdAsync(id);
                return type == null ? NotFound(new { message = $"Product type with ID {id} not found" }) : Ok(type);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving product type", error = ex.Message }); }
        }

        [HttpPost("product-types")]
        [ProducesResponseType(typeof(ProductTypeDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> CreateProductType([FromBody] CreateProductTypeDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _configurationService.CreateProductTypeAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetProductTypeById), new { id = type.ProductTypeId }, type);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating product type", error = ex.Message }); }
        }

        [HttpPut("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> UpdateProductType(int id, [FromBody] UpdateProductTypeDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var type = await _configurationService.UpdateProductTypeAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return type == null ? NotFound(new { message = $"Product type with ID {id} not found" }) : Ok(type);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating product type", error = ex.Message }); }
        }

        [HttpDelete("product-types/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductType(int id)
        {
            try
            {
                var result = await _configurationService.DeleteProductTypeAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Product type with ID {id} not found" });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting product type", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCTS  —  /api/configuration/products
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("products")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            try { return Ok(await _configurationService.GetAllProductsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving products", error = ex.Message }); }
        }

        [HttpGet("products/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplierId(int supplierId)
        {
            try { return Ok(await _configurationService.GetProductsBySupplierIdAsync(supplierId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving products", error = ex.Message }); }
        }

        [HttpGet("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                var product = await _configurationService.GetProductByIdAsync(id);
                return product == null ? NotFound(new { message = $"Product with ID {id} not found" }) : Ok(product);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving product", error = ex.Message }); }
        }

        [HttpPost("products")]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var product = await _configurationService.CreateProductAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating product", error = ex.Message }); }
        }

        [HttpPut("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var product = await _configurationService.UpdateProductAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return product == null ? NotFound(new { message = $"Product with ID {id} not found" }) : Ok(product);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating product", error = ex.Message }); }
        }

        [HttpDelete("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _configurationService.DeleteProductAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Product with ID {id} not found" });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting product", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ARMS  —  /api/configuration/arms
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("arms")]
        [ProducesResponseType(typeof(IEnumerable<ArmDto>), 200)]
        public async Task<ActionResult<IEnumerable<ArmDto>>> GetAllArms()
        {
            try { return Ok(await _configurationService.GetAllArmsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving arms", error = ex.Message }); }
        }

        [HttpGet("arms/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<ArmDto>), 200)]
        public async Task<ActionResult<IEnumerable<ArmDto>>> GetArmsByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetArmsByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving arms", error = ex.Message }); }
        }

        [HttpGet("arms/{id}")]
        [ProducesResponseType(typeof(ArmDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ArmDto>> GetArmById(int id)
        {
            try
            {
                var arm = await _configurationService.GetArmByIdAsync(id);
                return arm == null ? NotFound(new { message = $"Arm with ID {id} not found" }) : Ok(arm);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving arm", error = ex.Message }); }
        }

        [HttpPost("arms")]
        [ProducesResponseType(typeof(ArmDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ArmDto>> CreateArm([FromBody] CreateArmDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var arm = await _configurationService.CreateArmAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetArmById), new { id = arm.ArmId }, arm);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating arm", error = ex.Message }); }
        }

        [HttpPut("arms/{id}")]
        [ProducesResponseType(typeof(ArmDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ArmDto>> UpdateArm(int id, [FromBody] UpdateArmDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var arm = await _configurationService.UpdateArmAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return arm == null ? NotFound(new { message = $"Arm with ID {id} not found" }) : Ok(arm);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating arm", error = ex.Message }); }
        }

        [HttpDelete("arms/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteArm(int id)
        {
            try
            {
                var result = await _configurationService.DeleteArmAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Arm with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting arm", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MOTORS  —  /api/configuration/motors
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("motors")]
        [ProducesResponseType(typeof(IEnumerable<MotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GetAllMotors()
        {
            try { return Ok(await _configurationService.GetAllMotorsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving motors", error = ex.Message }); }
        }

        [HttpGet("motors/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<MotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GetMotorsByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetMotorsByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving motors", error = ex.Message }); }
        }

        [HttpGet("motors/{id}")]
        [ProducesResponseType(typeof(MotorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MotorDto>> GetMotorById(int id)
        {
            try
            {
                var motor = await _configurationService.GetMotorByIdAsync(id);
                return motor == null ? NotFound(new { message = $"Motor with ID {id} not found" }) : Ok(motor);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving motor", error = ex.Message }); }
        }

        [HttpPost("motors")]
        [ProducesResponseType(typeof(MotorDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MotorDto>> CreateMotor([FromBody] CreateMotorDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var motor = await _configurationService.CreateMotorAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetMotorById), new { id = motor.MotorId }, motor);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating motor", error = ex.Message }); }
        }

        [HttpPut("motors/{id}")]
        [ProducesResponseType(typeof(MotorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MotorDto>> UpdateMotor(int id, [FromBody] UpdateMotorDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var motor = await _configurationService.UpdateMotorAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return motor == null ? NotFound(new { message = $"Motor with ID {id} not found" }) : Ok(motor);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating motor", error = ex.Message }); }
        }

        [HttpDelete("motors/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteMotor(int id)
        {
            try
            {
                var result = await _configurationService.DeleteMotorAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Motor with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting motor", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HEATERS  —  /api/configuration/heaters
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("heaters")]
        [ProducesResponseType(typeof(IEnumerable<HeaterDto>), 200)]
        public async Task<ActionResult<IEnumerable<HeaterDto>>> GetAllHeaters()
        {
            try { return Ok(await _configurationService.GetAllHeatersAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving heaters", error = ex.Message }); }
        }

        [HttpGet("heaters/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<HeaterDto>), 200)]
        public async Task<ActionResult<IEnumerable<HeaterDto>>> GetHeatersByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetHeatersByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving heaters", error = ex.Message }); }
        }

        [HttpGet("heaters/{id}")]
        [ProducesResponseType(typeof(HeaterDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HeaterDto>> GetHeaterById(int id)
        {
            try
            {
                var heater = await _configurationService.GetHeaterByIdAsync(id);
                return heater == null ? NotFound(new { message = $"Heater with ID {id} not found" }) : Ok(heater);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving heater", error = ex.Message }); }
        }

        [HttpPost("heaters")]
        [ProducesResponseType(typeof(HeaterDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HeaterDto>> CreateHeater([FromBody] CreateHeaterDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var heater = await _configurationService.CreateHeaterAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetHeaterById), new { id = heater.HeaterId }, heater);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating heater", error = ex.Message }); }
        }

        [HttpPut("heaters/{id}")]
        [ProducesResponseType(typeof(HeaterDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HeaterDto>> UpdateHeater(int id, [FromBody] UpdateHeaterDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var heater = await _configurationService.UpdateHeaterAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return heater == null ? NotFound(new { message = $"Heater with ID {id} not found" }) : Ok(heater);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating heater", error = ex.Message }); }
        }

        [HttpDelete("heaters/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteHeater(int id)
        {
            try
            {
                var result = await _configurationService.DeleteHeaterAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Heater with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting heater", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NON-STANDARD RAL COLOURS  —  /api/configuration/ral-colours
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("ral-colours")]
        [ProducesResponseType(typeof(IEnumerable<NonStandardRALColourDto>), 200)]
        public async Task<ActionResult<IEnumerable<NonStandardRALColourDto>>> GetAllNonStandardRALColours()
        {
            try { return Ok(await _configurationService.GetAllNonStandardRALColoursAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving RAL colours", error = ex.Message }); }
        }

        [HttpGet("ral-colours/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<NonStandardRALColourDto>), 200)]
        public async Task<ActionResult<IEnumerable<NonStandardRALColourDto>>> GetNonStandardRALColoursByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetNonStandardRALColoursByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving RAL colours", error = ex.Message }); }
        }

        [HttpGet("ral-colours/{id}")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NonStandardRALColourDto>> GetNonStandardRALColourById(int id)
        {
            try
            {
                var colour = await _configurationService.GetNonStandardRALColourByIdAsync(id);
                return colour == null ? NotFound(new { message = $"RAL colour entry with ID {id} not found" }) : Ok(colour);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving RAL colour", error = ex.Message }); }
        }

        [HttpPost("ral-colours")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NonStandardRALColourDto>> CreateNonStandardRALColour([FromBody] CreateNonStandardRALColourDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var colour = await _configurationService.CreateNonStandardRALColourAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetNonStandardRALColourById), new { id = colour.RALColourId }, colour);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating RAL colour entry", error = ex.Message }); }
        }

        [HttpPut("ral-colours/{id}")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NonStandardRALColourDto>> UpdateNonStandardRALColour(int id, [FromBody] UpdateNonStandardRALColourDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var colour = await _configurationService.UpdateNonStandardRALColourAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return colour == null ? NotFound(new { message = $"RAL colour entry with ID {id} not found" }) : Ok(colour);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating RAL colour entry", error = ex.Message }); }
        }

        [HttpDelete("ral-colours/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteNonStandardRALColour(int id)
        {
            try
            {
                var result = await _configurationService.DeleteNonStandardRALColourAsync(id);
                return result ? NoContent() : NotFound(new { message = $"RAL colour entry with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting RAL colour entry", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PROJECTIONS  —  /api/configuration/projections
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("projections")]
        [ProducesResponseType(typeof(IEnumerable<ProjectionDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjectionDto>>> GetAllProjections()
        {
            try { return Ok(await _configurationService.GetAllProjectionsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving projections", error = ex.Message }); }
        }

        [HttpGet("projections/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectionDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjectionDto>>> GetProjectionsByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetProjectionsByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving projections", error = ex.Message }); }
        }

        [HttpGet("projections/{id}")]
        [ProducesResponseType(typeof(ProjectionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProjectionDto>> GetProjectionById(int id)
        {
            try
            {
                var projection = await _configurationService.GetProjectionByIdAsync(id);
                return projection == null ? NotFound(new { message = $"Projection with ID {id} not found" }) : Ok(projection);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving projection", error = ex.Message }); }
        }

        [HttpPost("projections")]
        [ProducesResponseType(typeof(ProjectionDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProjectionDto>> CreateProjection([FromBody] CreateProjectionDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var projection = await _configurationService.CreateProjectionAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetProjectionById), new { id = projection.ProjectionId }, projection);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating projection", error = ex.Message }); }
        }

        [HttpPut("projections/{id}")]
        [ProducesResponseType(typeof(ProjectionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProjectionDto>> UpdateProjection(int id, [FromBody] UpdateProjectionDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var projection = await _configurationService.UpdateProjectionAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return projection == null ? NotFound(new { message = $"Projection with ID {id} not found" }) : Ok(projection);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating projection", error = ex.Message }); }
        }

        [HttpDelete("projections/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProjection(int id)
        {
            try
            {
                var result = await _configurationService.DeleteProjectionAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Projection with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting projection", error = ex.Message }); }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  RADIO CONTROLLED MOTORS  —  /api/configuration/radio-motors
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("radio-motors")]
        [ProducesResponseType(typeof(IEnumerable<RadioControlledMotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<RadioControlledMotorDto>>> GetAllRadioControlledMotors()
        {
            try { return Ok(await _configurationService.GetAllRadioControlledMotorsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving radio motors", error = ex.Message }); }
        }

        [HttpGet("radio-motors/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<RadioControlledMotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<RadioControlledMotorDto>>> GetRadioControlledMotorsByProductId(int productId)
        {
            try { return Ok(await _configurationService.GetRadioControlledMotorsByProductIdAsync(productId)); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving radio motors", error = ex.Message }); }
        }

        [HttpGet("radio-motors/{id}")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RadioControlledMotorDto>> GetRadioControlledMotorById(int id)
        {
            try
            {
                var motor = await _configurationService.GetRadioControlledMotorByIdAsync(id);
                return motor == null ? NotFound(new { message = $"Radio motor with ID {id} not found" }) : Ok(motor);
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error retrieving radio motor", error = ex.Message }); }
        }

        [HttpPost("radio-motors")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RadioControlledMotorDto>> CreateRadioControlledMotor([FromBody] CreateRadioControlledMotorDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var motor = await _configurationService.CreateRadioControlledMotorAsync(createDto, User?.Identity?.Name ?? "System");
                return CreatedAtAction(nameof(GetRadioControlledMotorById), new { id = motor.RadioMotorId }, motor);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error creating radio motor", error = ex.Message }); }
        }

        [HttpPut("radio-motors/{id}")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RadioControlledMotorDto>> UpdateRadioControlledMotor(int id, [FromBody] UpdateRadioControlledMotorDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var motor = await _configurationService.UpdateRadioControlledMotorAsync(id, updateDto, User?.Identity?.Name ?? "System");
                return motor == null ? NotFound(new { message = $"Radio motor with ID {id} not found" }) : Ok(motor);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { message = "Error updating radio motor", error = ex.Message }); }
        }

        [HttpDelete("radio-motors/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteRadioControlledMotor(int id)
        {
            try
            {
                var result = await _configurationService.DeleteRadioControlledMotorAsync(id);
                return result ? NoContent() : NotFound(new { message = $"Radio motor with ID {id} not found" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = "Error deleting radio motor", error = ex.Message }); }
        }
    }
}