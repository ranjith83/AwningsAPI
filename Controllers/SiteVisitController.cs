using AwningsAPI.Dto.SiteVisit;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteVisitController : ControllerBase
    {
        private readonly ISiteVisitService _siteVisitService;

        public SiteVisitController(ISiteVisitService siteVisitService)
        {
            _siteVisitService = siteVisitService;
        }

        /// <summary>
        /// Get all site visits for a specific workflow
        /// </summary>
        /// <param name="workflowId">The workflow ID</param>
        /// <returns>List of site visits</returns>
        [Authorize]
        [HttpGet("workflow/{workflowId}")]
        public async Task<ActionResult<IEnumerable<SiteVisitDto>>> GetSiteVisitsByWorkflowId(int workflowId)
        {
            try
            {
                var siteVisits = await _siteVisitService.GetSiteVisitsByWorkflowIdAsync(workflowId);

                var siteVisitDtos = siteVisits.Select(sv => new SiteVisitDto
                {
                    SiteVisitId = sv.SiteVisitId,
                    WorkflowId = sv.WorkflowId,
                    ProductModelType = sv.ProductModelType,
                    Model = sv.Model,
                    OtherPleaseSpecify = sv.OtherPleaseSpecify,
                    SiteLayout = sv.SiteLayout,
                    Structure = sv.Structure,
                    PassageHeight = sv.PassageHeight,
                    Width = sv.Width,
                    Projection = sv.Projection,
                    HeightAvailable = sv.HeightAvailable,
                    WallType = sv.WallType,
                    ExternalInsulation = sv.ExternalInsulation,
                    WallFinish = sv.WallFinish,
                    WallThickness = sv.WallThickness,
                    SpecialBrackets = sv.SpecialBrackets,
                    SideInfills = sv.SideInfills,
                    FlashingRequired = sv.FlashingRequired,
                    FlashingDimensions = sv.FlashingDimensions,
                    StandOfBrackets = sv.StandOfBrackets,
                    StandOfBracketDimension = sv.StandOfBracketDimension,
                    Electrician = sv.Electrician,
                    ElectricalConnection = sv.ElectricalConnection,
                    Location = sv.Location,
                    OtherSiteSurveyNotes = sv.OtherSiteSurveyNotes,
                    FixtureType = sv.FixtureType,
                    Operation = sv.Operation,
                    CrankLength = sv.CrankLength,
                    OperationSide = sv.OperationSide,
                    Fabric = sv.Fabric,
                    RAL = sv.RAL,
                    ValanceChoice = sv.ValanceChoice,
                    Valance = sv.Valance,
                    WindSensor = sv.WindSensor,
                    ShadePlusRequired = sv.ShadePlusRequired,
                    ShadeType = sv.ShadeType,
                    ShadeplusFabric = sv.ShadeplusFabric,
                    ShadePlusAnyOtherDetail = sv.ShadePlusAnyOtherDetail,
                    Lights = sv.Lights,
                    LightsType = sv.LightsType,
                    LightsAnyOtherDetails = sv.LightsAnyOtherDetails,
                    Heater = sv.Heater,
                    HeaterManufacturer = sv.HeaterManufacturer,
                    NumberRequired = sv.NumberRequired,
                    HeaterOutput = sv.HeaterOutput,
                    HeaterColour = sv.HeaterColour,
                    RemoteControl = sv.RemoteControl,
                    ControllerBox = sv.ControllerBox,
                    HeaterAnyOtherDetails = sv.HeaterAnyOtherDetails,
                    DateCreated = sv.DateCreated,
                    CreatedBy = sv.CreatedBy,
                    DateUpdated = sv.DateUpdated,
                    UpdatedBy = sv.UpdatedBy
                }).ToList();

                return Ok(siteVisitDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving site visits", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a single site visit by ID
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <returns>Site visit details</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteVisitDto>> GetSiteVisitById(int id)
        {
            try
            {
                var siteVisit = await _siteVisitService.GetSiteVisitByIdAsync(id);

                if (siteVisit == null)
                {
                    return NotFound(new { message = $"Site visit with ID {id} not found" });
                }

                var siteVisitDto = new SiteVisitDto
                {
                    SiteVisitId = siteVisit.SiteVisitId,
                    WorkflowId = siteVisit.WorkflowId,
                    ProductModelType = siteVisit.ProductModelType,
                    Model = siteVisit.Model,
                    OtherPleaseSpecify = siteVisit.OtherPleaseSpecify,
                    SiteLayout = siteVisit.SiteLayout,
                    Structure = siteVisit.Structure,
                    PassageHeight = siteVisit.PassageHeight,
                    Width = siteVisit.Width,
                    Projection = siteVisit.Projection,
                    HeightAvailable = siteVisit.HeightAvailable,
                    WallType = siteVisit.WallType,
                    ExternalInsulation = siteVisit.ExternalInsulation,
                    WallFinish = siteVisit.WallFinish,
                    WallThickness = siteVisit.WallThickness,
                    SpecialBrackets = siteVisit.SpecialBrackets,
                    SideInfills = siteVisit.SideInfills,
                    FlashingRequired = siteVisit.FlashingRequired,
                    FlashingDimensions = siteVisit.FlashingDimensions,
                    StandOfBrackets = siteVisit.StandOfBrackets,
                    StandOfBracketDimension = siteVisit.StandOfBracketDimension,
                    Electrician = siteVisit.Electrician,
                    ElectricalConnection = siteVisit.ElectricalConnection,
                    Location = siteVisit.Location,
                    OtherSiteSurveyNotes = siteVisit.OtherSiteSurveyNotes,
                    FixtureType = siteVisit.FixtureType,
                    Operation = siteVisit.Operation,
                    CrankLength = siteVisit.CrankLength,
                    OperationSide = siteVisit.OperationSide,
                    Fabric = siteVisit.Fabric,
                    RAL = siteVisit.RAL,
                    ValanceChoice = siteVisit.ValanceChoice,
                    Valance = siteVisit.Valance,
                    WindSensor = siteVisit.WindSensor,
                    ShadePlusRequired = siteVisit.ShadePlusRequired,
                    ShadeType = siteVisit.ShadeType,
                    ShadeplusFabric = siteVisit.ShadeplusFabric,
                    ShadePlusAnyOtherDetail = siteVisit.ShadePlusAnyOtherDetail,
                    Lights = siteVisit.Lights,
                    LightsType = siteVisit.LightsType,
                    LightsAnyOtherDetails = siteVisit.LightsAnyOtherDetails,
                    Heater = siteVisit.Heater,
                    HeaterManufacturer = siteVisit.HeaterManufacturer,
                    NumberRequired = siteVisit.NumberRequired,
                    HeaterOutput = siteVisit.HeaterOutput,
                    HeaterColour = siteVisit.HeaterColour,
                    RemoteControl = siteVisit.RemoteControl,
                    ControllerBox = siteVisit.ControllerBox,
                    HeaterAnyOtherDetails = siteVisit.HeaterAnyOtherDetails,
                    DateCreated = siteVisit.DateCreated,
                    CreatedBy = siteVisit.CreatedBy,
                    DateUpdated = siteVisit.DateUpdated,
                    UpdatedBy = siteVisit.UpdatedBy
                };

                return Ok(siteVisitDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving site visit", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new site visit
        /// </summary>
        /// <param name="dto">Site visit creation data</param>
        /// <returns>Created site visit</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SiteVisitDto>> CreateSiteVisit([FromBody] CreateSiteVisitDto dto)
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                var siteVisit = await _siteVisitService.CreateSiteVisitAsync(dto, currentUser);

                var siteVisitDto = new SiteVisitDto
                {
                    SiteVisitId = siteVisit.SiteVisitId,
                    WorkflowId = siteVisit.WorkflowId,
                    ProductModelType = siteVisit.ProductModelType,
                    Model = siteVisit.Model,
                    OtherPleaseSpecify = siteVisit.OtherPleaseSpecify,
                    DateCreated = siteVisit.DateCreated,
                    CreatedBy = siteVisit.CreatedBy
                };

                return CreatedAtAction(nameof(GetSiteVisitById), new { id = siteVisit.SiteVisitId }, siteVisitDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating site visit", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing site visit
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <param name="dto">Updated site visit data</param>
        /// <returns>Updated site visit</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<SiteVisitDto>> UpdateSiteVisit(int id, [FromBody] SiteVisitDto dto)
        {
            try
            {
                var currentUser = User?.Identity?.Name ?? "System";
                var siteVisit = await _siteVisitService.UpdateSiteVisitAsync(id, dto, currentUser);

                var siteVisitDto = new SiteVisitDto
                {
                    SiteVisitId = siteVisit.SiteVisitId,
                    WorkflowId = siteVisit.WorkflowId,
                    ProductModelType = siteVisit.ProductModelType,
                    DateUpdated = siteVisit.DateUpdated,
                    UpdatedBy = siteVisit.UpdatedBy
                };

                return Ok(siteVisitDto);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Error updating site visit", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a site visit
        /// </summary>
        /// <param name="id">Site visit ID</param>
        /// <returns>Success status</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSiteVisit(int id)
        {
            try
            {
                var result = await _siteVisitService.DeleteSiteVisitAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Site visit with ID {id} not found" });
                }

                return Ok(new { message = "Site visit deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting site visit", error = ex.Message });
            }
        }


        /// <summary>
        /// Get all site visit dropdown values grouped by category
        /// </summary>
        /// <returns>Dictionary of categories and their values</returns>
        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetAllValues()
        {
            try
            {
                var values = await _siteVisitService.GetAllValuesDictionaryAsync();
                return Ok(values);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dropdown values", error = ex.Message });
            }
        }

        /// <summary>
        /// Get dropdown values for a specific category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>List of values for the category</returns>
        [Authorize]
        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<string>>> GetValuesByCategory(string category)
        {
            try
            {
                var values = await _siteVisitService.GetValuesByCategoryAsync(category);
                var valueList = values.Select(v => v.Value).ToList();
                return Ok(valueList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving category values", error = ex.Message });
            }
        }
    }
}