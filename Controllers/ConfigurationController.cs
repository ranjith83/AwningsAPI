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
        private readonly ILogger<ConfigurationController> _logger;

        public ConfigurationController(IConfigurationService configurationService, ILogger<ConfigurationController> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SITE VISIT VALUES  —  /api/configuration/site-visit-values
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("site-visit-values")]
        [ProducesResponseType(typeof(IEnumerable<SiteVisitValueDto>), 200)]
        public async Task<ActionResult<IEnumerable<SiteVisitValueDto>>> GetAllSiteVisitValues()
        {
            return Ok(await _configurationService.GetAllSiteVisitValuesAsync());
        }

        [HttpGet("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SiteVisitValueDto>> GetSiteVisitValueById(int id)
        {
            var value = await _configurationService.GetSiteVisitValueByIdAsync(id);
            return value == null ? NotFound(new { message = $"Site visit value with ID {id} not found" }) : Ok(value);
        }

        [HttpPost("site-visit-values")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> CreateSiteVisitValue([FromBody] CreateSiteVisitValueDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var value = await _configurationService.CreateSiteVisitValueAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetSiteVisitValueById), new { id = value.Id }, value);
        }

        [HttpPut("site-visit-values/{id}")]
        [ProducesResponseType(typeof(SiteVisitValueDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SiteVisitValueDto>> UpdateSiteVisitValue(int id, [FromBody] UpdateSiteVisitValueDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var value = await _configurationService.UpdateSiteVisitValueAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return value == null ? NotFound(new { message = $"Site visit value with ID {id} not found" }) : Ok(value);
        }

        [HttpDelete("site-visit-values/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSiteVisitValue(int id)
        {
            var result = await _configurationService.DeleteSiteVisitValueAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Site visit value with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  BRACKETS  —  /api/configuration/brackets
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("brackets")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetAllBrackets()
        {
            return Ok(await _configurationService.GetAllBracketsAsync());
        }

        [HttpGet("brackets/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<BracketDto>), 200)]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GetBracketsByProductId(int productId)
        {
            return Ok(await _configurationService.GetBracketsByProductIdAsync(productId));
        }

        [HttpGet("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BracketDto>> GetBracketById(int id)
        {
            var bracket = await _configurationService.GetBracketByIdAsync(id);
            return bracket == null ? NotFound(new { message = $"Bracket with ID {id} not found" }) : Ok(bracket);
        }

        [HttpPost("brackets")]
        [ProducesResponseType(typeof(BracketDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> CreateBracket([FromBody] CreateBracketDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var bracket = await _configurationService.CreateBracketAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetBracketById), new { id = bracket.BracketId }, bracket);
        }

        [HttpPut("brackets/{id}")]
        [ProducesResponseType(typeof(BracketDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BracketDto>> UpdateBracket(int id, [FromBody] UpdateBracketDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var bracket = await _configurationService.UpdateBracketAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return bracket == null ? NotFound(new { message = $"Bracket with ID {id} not found" }) : Ok(bracket);
        }

        [HttpDelete("brackets/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteBracket(int id)
        {
            var result = await _configurationService.DeleteBracketAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Bracket with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SUPPLIERS  —  /api/configuration/suppliers
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("suppliers")]
        [ProducesResponseType(typeof(IEnumerable<SupplierDto>), 200)]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
        {
            return Ok(await _configurationService.GetAllSuppliersAsync());
        }

        [HttpGet("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SupplierDto>> GetSupplierById(int id)
        {
            var supplier = await _configurationService.GetSupplierByIdAsync(id);
            return supplier == null ? NotFound(new { message = $"Supplier with ID {id} not found" }) : Ok(supplier);
        }

        [HttpPost("suppliers")]
        [ProducesResponseType(typeof(SupplierDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var supplier = await _configurationService.CreateSupplierAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.SupplierId }, supplier);
        }

        [HttpPut("suppliers/{id}")]
        [ProducesResponseType(typeof(SupplierDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SupplierDto>> UpdateSupplier(int id, [FromBody] UpdateSupplierDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var supplier = await _configurationService.UpdateSupplierAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return supplier == null ? NotFound(new { message = $"Supplier with ID {id} not found" }) : Ok(supplier);
        }

        [HttpDelete("suppliers/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var result = await _configurationService.DeleteSupplierAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Supplier with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCT TYPES  —  /api/configuration/product-types
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("product-types")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetAllProductTypes()
        {
            return Ok(await _configurationService.GetAllProductTypesAsync());
        }

        [HttpGet("product-types/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypesBySupplierId(int supplierId)
        {
            return Ok(await _configurationService.GetProductTypesBySupplierIdAsync(supplierId));
        }

        [HttpGet("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductTypeDto>> GetProductTypeById(int id)
        {
            var type = await _configurationService.GetProductTypeByIdAsync(id);
            return type == null ? NotFound(new { message = $"Product type with ID {id} not found" }) : Ok(type);
        }

        [HttpPost("product-types")]
        [ProducesResponseType(typeof(ProductTypeDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> CreateProductType([FromBody] CreateProductTypeDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var type = await _configurationService.CreateProductTypeAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetProductTypeById), new { id = type.ProductTypeId }, type);
        }

        [HttpPut("product-types/{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductTypeDto>> UpdateProductType(int id, [FromBody] UpdateProductTypeDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var type = await _configurationService.UpdateProductTypeAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return type == null ? NotFound(new { message = $"Product type with ID {id} not found" }) : Ok(type);
        }

        [HttpDelete("product-types/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProductType(int id)
        {
            var result = await _configurationService.DeleteProductTypeAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Product type with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PRODUCTS  —  /api/configuration/products
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("products")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            return Ok(await _configurationService.GetAllProductsAsync());
        }

        [HttpGet("products/supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplierId(int supplierId)
        {
            return Ok(await _configurationService.GetProductsBySupplierIdAsync(supplierId));
        }

        [HttpGet("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _configurationService.GetProductByIdAsync(id);
            return product == null ? NotFound(new { message = $"Product with ID {id} not found" }) : Ok(product);
        }

        [HttpPost("products")]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _configurationService.CreateProductAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpPut("products/{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _configurationService.UpdateProductAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return product == null ? NotFound(new { message = $"Product with ID {id} not found" }) : Ok(product);
        }

        [HttpDelete("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _configurationService.DeleteProductAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Product with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ARMS  —  /api/configuration/arms
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("arms")]
        [ProducesResponseType(typeof(IEnumerable<ArmDto>), 200)]
        public async Task<ActionResult<IEnumerable<ArmDto>>> GetAllArms()
        {
            return Ok(await _configurationService.GetAllArmsAsync());
        }

        [HttpGet("arms/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<ArmDto>), 200)]
        public async Task<ActionResult<IEnumerable<ArmDto>>> GetArmsByProductId(int productId)
        {
            return Ok(await _configurationService.GetArmsByProductIdAsync(productId));
        }

        [HttpGet("arms/{id}")]
        [ProducesResponseType(typeof(ArmDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ArmDto>> GetArmById(int id)
        {
            var arm = await _configurationService.GetArmByIdAsync(id);
            return arm == null ? NotFound(new { message = $"Arm with ID {id} not found" }) : Ok(arm);
        }

        [HttpPost("arms")]
        [ProducesResponseType(typeof(ArmDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ArmDto>> CreateArm([FromBody] CreateArmDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var arm = await _configurationService.CreateArmAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetArmById), new { id = arm.ArmId }, arm);
        }

        [HttpPut("arms/{id}")]
        [ProducesResponseType(typeof(ArmDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ArmDto>> UpdateArm(int id, [FromBody] UpdateArmDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var arm = await _configurationService.UpdateArmAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return arm == null ? NotFound(new { message = $"Arm with ID {id} not found" }) : Ok(arm);
        }

        [HttpDelete("arms/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteArm(int id)
        {
            var result = await _configurationService.DeleteArmAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Arm with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  MOTORS  —  /api/configuration/motors
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("motors")]
        [ProducesResponseType(typeof(IEnumerable<MotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GetAllMotors()
        {
            return Ok(await _configurationService.GetAllMotorsAsync());
        }

        [HttpGet("motors/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<MotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<MotorDto>>> GetMotorsByProductId(int productId)
        {
            return Ok(await _configurationService.GetMotorsByProductIdAsync(productId));
        }

        [HttpGet("motors/{id}")]
        [ProducesResponseType(typeof(MotorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MotorDto>> GetMotorById(int id)
        {
            var motor = await _configurationService.GetMotorByIdAsync(id);
            return motor == null ? NotFound(new { message = $"Motor with ID {id} not found" }) : Ok(motor);
        }

        [HttpPost("motors")]
        [ProducesResponseType(typeof(MotorDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MotorDto>> CreateMotor([FromBody] CreateMotorDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var motor = await _configurationService.CreateMotorAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetMotorById), new { id = motor.MotorId }, motor);
        }

        [HttpPut("motors/{id}")]
        [ProducesResponseType(typeof(MotorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MotorDto>> UpdateMotor(int id, [FromBody] UpdateMotorDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var motor = await _configurationService.UpdateMotorAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return motor == null ? NotFound(new { message = $"Motor with ID {id} not found" }) : Ok(motor);
        }

        [HttpDelete("motors/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteMotor(int id)
        {
            var result = await _configurationService.DeleteMotorAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Motor with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HEATERS  —  /api/configuration/heaters
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("heaters")]
        [ProducesResponseType(typeof(IEnumerable<HeaterDto>), 200)]
        public async Task<ActionResult<IEnumerable<HeaterDto>>> GetAllHeaters()
        {
            return Ok(await _configurationService.GetAllHeatersAsync());
        }

        [HttpGet("heaters/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<HeaterDto>), 200)]
        public async Task<ActionResult<IEnumerable<HeaterDto>>> GetHeatersByProductId(int productId)
        {
            return Ok(await _configurationService.GetHeatersByProductIdAsync(productId));
        }

        [HttpGet("heaters/{id}")]
        [ProducesResponseType(typeof(HeaterDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<HeaterDto>> GetHeaterById(int id)
        {
            var heater = await _configurationService.GetHeaterByIdAsync(id);
            return heater == null ? NotFound(new { message = $"Heater with ID {id} not found" }) : Ok(heater);
        }

        [HttpPost("heaters")]
        [ProducesResponseType(typeof(HeaterDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HeaterDto>> CreateHeater([FromBody] CreateHeaterDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var heater = await _configurationService.CreateHeaterAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetHeaterById), new { id = heater.HeaterId }, heater);
        }

        [HttpPut("heaters/{id}")]
        [ProducesResponseType(typeof(HeaterDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<HeaterDto>> UpdateHeater(int id, [FromBody] UpdateHeaterDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var heater = await _configurationService.UpdateHeaterAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return heater == null ? NotFound(new { message = $"Heater with ID {id} not found" }) : Ok(heater);
        }

        [HttpDelete("heaters/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteHeater(int id)
        {
            var result = await _configurationService.DeleteHeaterAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Heater with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NON-STANDARD RAL COLOURS  —  /api/configuration/ral-colours
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("ral-colours")]
        [ProducesResponseType(typeof(IEnumerable<NonStandardRALColourDto>), 200)]
        public async Task<ActionResult<IEnumerable<NonStandardRALColourDto>>> GetAllNonStandardRALColours()
        {
            return Ok(await _configurationService.GetAllNonStandardRALColoursAsync());
        }

        [HttpGet("ral-colours/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<NonStandardRALColourDto>), 200)]
        public async Task<ActionResult<IEnumerable<NonStandardRALColourDto>>> GetNonStandardRALColoursByProductId(int productId)
        {
            return Ok(await _configurationService.GetNonStandardRALColoursByProductIdAsync(productId));
        }

        [HttpGet("ral-colours/{id}")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NonStandardRALColourDto>> GetNonStandardRALColourById(int id)
        {
            var colour = await _configurationService.GetNonStandardRALColourByIdAsync(id);
            return colour == null ? NotFound(new { message = $"RAL colour entry with ID {id} not found" }) : Ok(colour);
        }

        [HttpPost("ral-colours")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NonStandardRALColourDto>> CreateNonStandardRALColour([FromBody] CreateNonStandardRALColourDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var colour = await _configurationService.CreateNonStandardRALColourAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetNonStandardRALColourById), new { id = colour.RALColourId }, colour);
        }

        [HttpPut("ral-colours/{id}")]
        [ProducesResponseType(typeof(NonStandardRALColourDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NonStandardRALColourDto>> UpdateNonStandardRALColour(int id, [FromBody] UpdateNonStandardRALColourDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var colour = await _configurationService.UpdateNonStandardRALColourAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return colour == null ? NotFound(new { message = $"RAL colour entry with ID {id} not found" }) : Ok(colour);
        }

        [HttpDelete("ral-colours/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteNonStandardRALColour(int id)
        {
            var result = await _configurationService.DeleteNonStandardRALColourAsync(id);
            return result ? NoContent() : NotFound(new { message = $"RAL colour entry with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PROJECTIONS  —  /api/configuration/projections
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("projections")]
        [ProducesResponseType(typeof(IEnumerable<ProjectionDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjectionDto>>> GetAllProjections()
        {
            return Ok(await _configurationService.GetAllProjectionsAsync());
        }

        [HttpGet("projections/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectionDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjectionDto>>> GetProjectionsByProductId(int productId)
        {
            return Ok(await _configurationService.GetProjectionsByProductIdAsync(productId));
        }

        [HttpGet("projections/{id}")]
        [ProducesResponseType(typeof(ProjectionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProjectionDto>> GetProjectionById(int id)
        {
            var projection = await _configurationService.GetProjectionByIdAsync(id);
            return projection == null ? NotFound(new { message = $"Projection with ID {id} not found" }) : Ok(projection);
        }

        [HttpPost("projections")]
        [ProducesResponseType(typeof(ProjectionDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProjectionDto>> CreateProjection([FromBody] CreateProjectionDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var projection = await _configurationService.CreateProjectionAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetProjectionById), new { id = projection.ProjectionId }, projection);
        }

        [HttpPut("projections/{id}")]
        [ProducesResponseType(typeof(ProjectionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProjectionDto>> UpdateProjection(int id, [FromBody] UpdateProjectionDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var projection = await _configurationService.UpdateProjectionAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return projection == null ? NotFound(new { message = $"Projection with ID {id} not found" }) : Ok(projection);
        }

        [HttpDelete("projections/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProjection(int id)
        {
            var result = await _configurationService.DeleteProjectionAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Projection with ID {id} not found" });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  RADIO CONTROLLED MOTORS  —  /api/configuration/radio-motors
        // ══════════════════════════════════════════════════════════════════════

        [HttpGet("radio-motors")]
        [ProducesResponseType(typeof(IEnumerable<RadioControlledMotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<RadioControlledMotorDto>>> GetAllRadioControlledMotors()
        {
            return Ok(await _configurationService.GetAllRadioControlledMotorsAsync());
        }

        [HttpGet("radio-motors/product/{productId}")]
        [ProducesResponseType(typeof(IEnumerable<RadioControlledMotorDto>), 200)]
        public async Task<ActionResult<IEnumerable<RadioControlledMotorDto>>> GetRadioControlledMotorsByProductId(int productId)
        {
            return Ok(await _configurationService.GetRadioControlledMotorsByProductIdAsync(productId));
        }

        [HttpGet("radio-motors/{id}")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<RadioControlledMotorDto>> GetRadioControlledMotorById(int id)
        {
            var motor = await _configurationService.GetRadioControlledMotorByIdAsync(id);
            return motor == null ? NotFound(new { message = $"Radio motor with ID {id} not found" }) : Ok(motor);
        }

        [HttpPost("radio-motors")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RadioControlledMotorDto>> CreateRadioControlledMotor([FromBody] CreateRadioControlledMotorDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var motor = await _configurationService.CreateRadioControlledMotorAsync(createDto, User?.Identity?.Name ?? "System");
            return CreatedAtAction(nameof(GetRadioControlledMotorById), new { id = motor.RadioMotorId }, motor);
        }

        [HttpPut("radio-motors/{id}")]
        [ProducesResponseType(typeof(RadioControlledMotorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RadioControlledMotorDto>> UpdateRadioControlledMotor(int id, [FromBody] UpdateRadioControlledMotorDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var motor = await _configurationService.UpdateRadioControlledMotorAsync(id, updateDto, User?.Identity?.Name ?? "System");
            return motor == null ? NotFound(new { message = $"Radio motor with ID {id} not found" }) : Ok(motor);
        }

        [HttpDelete("radio-motors/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteRadioControlledMotor(int id)
        {
            var result = await _configurationService.DeleteRadioControlledMotorAsync(id);
            return result ? NoContent() : NotFound(new { message = $"Radio motor with ID {id} not found" });
        }
    }
}
