using AwningsAPI.Dto.Customers;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Supplier;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Workflow;
using AwningsAPI.Services.CustomerService;
using AwningsAPI.Services.Suppliers;
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
                AddedBy = "Michael" // To be implemented
            }).ToList();

            return Ok(workflowDtos);
        }

        [HttpPost("CreateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> CreateWorkflow([FromBody] WorkflowDto dto)
        {
            var workflow = await _workflowService.CreateWorkflow(dto);

            return CreatedAtAction(nameof(CreateWorkflow), new { Id = workflow.CompanyId }, workflow);
        }

        [HttpPut("UpdateWorkflow")]
        public async Task<ActionResult<WorkflowStart>> UpdateWorkflow([FromBody] WorkflowDto dto)
        {
            var workflow = await _workflowService.UpdateWorkflow(dto);

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
            var initialEnquiry = await _workflowService.UpdateInitialEnquiry(dto);

            return Ok(initialEnquiry);
        }

        [HttpPost("AddInitialEnquiry")]
        public async Task<ActionResult<InitialEnquiry>> AddInitialEnquiry([FromBody] InitialEnquiryDto dto)
        {
            var initialEnquiry = await _workflowService.AddInitialEnquiry(dto);
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
    }
}
