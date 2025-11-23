using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/workflow")]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }
        [Authorize]
        [HttpGet("GetAllWorfflowsForCustomer")]
        public async Task<ActionResult<IEnumerable<WorkflowStart>>> GetAllWorfflowsForCustomer(int CustomerId)
        {
            var workflows = await _workflowService.GetAllWorfflowsForCustomerAsync(CustomerId);

            var workflowDtos = workflows.Select(c => new WorkflowDto
            {
                WorkflowId = c.WorkflowId,
                ProductName = c.Product.Description,
                Description = c.Description,
                InitialEnquiry = c.InitialEnquiry,
                CreateQuotation = c.CreateQuote,
                InviteShowRoomVisit = c.InviteShowRoom,
                SetupSiteVisit = c.SetupSiteVisit,
                InvoiceSent = c.InvoiceSent,
                DateAdded = c.DateCreated,
                AddedBy =  c.CreatedBy,
                CompanyId = c.CompanyId,
                SupplierId = c.SupplierId,
                ProductId = c.ProductId,
                ProductTypeId = c.ProductTypeId,
                CustomerId = c.CustomerId
            }).ToList();

            return Ok(workflowDtos);
        }

        [Authorize]
        [HttpPost("CreateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> CreateWorkflow([FromBody] WorkflowDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var workflow = await _workflowService.CreateWorkflow(dto, currentUser);

            return CreatedAtAction(nameof(CreateWorkflow), new { Id = workflow.CustomerId }, workflow);
        }

        [Authorize]
        [HttpPut("UpdateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> UpdateWorkflow([FromBody] WorkflowDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var workflow = await _workflowService.UpdateWorkflow(dto, currentUser);

            return Ok(workflow);
        }

        [HttpGet("GeInitialEnquiryForWorkflow")]
        public async Task<ActionResult<IEnumerable<WorkflowStart>>> GeInitialEnquiryForWorkflow(int WorkflowId)
        {
            var initialEnquiry = await _workflowService.GetInitialEnquiryForWorkflowAsync(WorkflowId);

            var initialEnquiryDto = initialEnquiry.Select(c => new InitialEnquiryDto
            {
                EnquiryId = c.EnquiryId,
                WorkflowId = c.WorkflowId,
                Comments = c.Comments,
                Email = c.Email,
                Images = c.Images
            }).ToList();

            return Ok(initialEnquiryDto);
        }

        [HttpPut("UpdateInitialEnquiry")]
        public async Task<ActionResult<IEnumerable<InitialEnquiry>>> UpdateInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var initialEnquiry = await _workflowService.UpdateInitialEnquiry(dto, currentUser);

            return Ok(initialEnquiry);
        }

        [HttpPost("AddInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> AddInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var currentUser = User?.Identity?.Name ?? "System";
            var initialEnquiry = await _workflowService.AddInitialEnquiry(dto, currentUser);
            return CreatedAtAction(nameof(AddInitialEnquiry), new { Id = initialEnquiry.EnquiryId }, initialEnquiry);
        }

        [HttpGet("GetStandardWidthsForProduct")]
        public async Task<List<int>> GetStandardWidthsForProduct(int ProductId)
        {
            return await _workflowService.GetStandardWidthsForProductAsync(ProductId);
        }

        [HttpGet("GetProjectionWidthsForProduct")]
        public async Task<List<int>> GetProjectionWidthsForProduct(int ProductId)
        {
            return await _workflowService.GetProjectionWidthsForProductAsync(ProductId);
        }

        [HttpGet("GetProjectionPriceForProduct")]
        public async Task<decimal> GetProjectionPriceForProduct(int ProductId, int widthcm, int projectioncm)
        {
            return await _workflowService.GetProjectionPriceForProductAsync(ProductId, widthcm, projectioncm);
        }

        [HttpGet("GeBracketsForProduct")]
        public async Task<ActionResult<IEnumerable<BracketDto>>> GeBracketsForProduct(int ProductId)
        {
           var brackets = await _workflowService.GeBracketsForProductAsync(ProductId);

            var bracketsDto = brackets.Select(c => new BracketDto
            {
                BracketId = c.BracketId,
                BracketName = c.BracketName,
               Price = c.Price
            }).ToList();

            return Ok(bracketsDto);
        }

        [HttpGet("GeArmsForProduct")]
        public async Task<ActionResult<IEnumerable<ArmsDto>>> GeArmsForProduct(int ProductId)
        {
            var arms = await _workflowService.GeArmsForProductAsync(ProductId);

            var armsDto = arms.Select(c => new ArmsDto
            {
                ArmId = c.ArmId,
                Description = c.Description,
                Price = c.Price,
                BfId = c.BfId
            }).ToList();

            return Ok(armsDto);
        }

        [HttpGet("GeMotorsForProduct")]
        public async Task<ActionResult<IEnumerable<MotorsDto>>> GeMotorsForProduct(int ProductId)
        {
            var motors = await _workflowService.GeMotorsForProductAsync(ProductId);

            var motorsDto = motors.Select(c => new MotorsDto
            {
                MotorId = c.MotorId,
                Description = c.Description,
                Price = c.Price
            }).ToList();

            return Ok(motorsDto);
        }

        [HttpGet("GeValanceStylePriceForProduct")]
        public async Task<decimal> GeValanceStylePriceForProduct(int ProductId, int widthcm)
        {
            return await _workflowService.GeValanceStylePriceForProductAsync(ProductId, widthcm);
        }

        [HttpGet("GeNonStandardRALColourPriceForProduct")]
        public async Task<decimal> GeNonStandardRALColourPriceForProduct(int ProductId, int widthcm)
        {
            return await _workflowService.GeNonStandardRALColourPriceForProductAsync(ProductId, widthcm);
        }

        [HttpGet("GeWallSealingProfilerPriceForProduct")]
        public async Task<decimal> GeWallSealingProfilerPriceForProduct(int ProductId, int widthcm)
        {
            return await _workflowService.GeWallSealingProfilerPriceForProductAsync(ProductId, widthcm);
        }

        [HttpGet("GeHeatersForProduct")]
        public async Task<ActionResult<IEnumerable<HeatersDto>>> GeHeatersForProduct(int ProductId)
        {
            var heaters = await _workflowService.GeHeatersForProductAsync(ProductId);

            var heatersDto = heaters.Select(c => new HeatersDto
            {
                HeaterId = c.HeaterId,
                Description = c.Description,
                Price = c.Price,                
                PriceNonRALColour = c.PriceNonRALColour
            }).ToList();

            return Ok(heatersDto);
        }
    }
}
