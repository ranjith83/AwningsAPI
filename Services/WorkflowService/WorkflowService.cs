using AwningsAPI.Database;
using AwningsAPI.Dto.Product;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

namespace AwningsAPI.Services.WorkflowService
{
    public class WorkflowService : IWorkflowService
    {
        private readonly AppDbContext _context;

        public WorkflowService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<WorkflowStart>> GetAllWorfflowsForCustomerAsync(int CustomerId)
        {
            return await _context.WorkflowStarts.Include(p => p.Product).Where(w=>w.CompanyId==CustomerId).ToListAsync();
        }

        public async Task<WorkflowStart> CreateWorkflow(WorkflowDto dto)
        {
            var workflow = new WorkflowStart
            {
                Description = dto.Description,
                InitialEnquiry = dto.InitialEnquiry,
                CreateQuote = dto.CreateQuotation,  
                InviteShowRoom = dto.InviteShowRoomVisit,
                SetupSiteVisit = dto.SetupSiteVisit,
                InvoiceSent = dto.InvoiceSent,  
                SupplierId = dto.SupplierId,
                CompanyId = dto.CompanyId,
                ProductId = dto.ProductId,
                ProductTypeId = dto.ProductTypeId,  
                DateCreated = DateTime.UtcNow,  
                CreatedBy = 1 // To be replaced with actual user I
            };

            _context.WorkflowStarts.Add(workflow);
            await _context.SaveChangesAsync();

            return workflow;
        }

        public async Task<WorkflowStart> UpdateWorkflow(WorkflowDto dto)
        {
            var existingWorkflow = await _context.WorkflowStarts.FindAsync(dto.WorkflowId);

            if (existingWorkflow == null)
            {
                throw new Exception("Workflow not found");
            }

            existingWorkflow.Description = dto.Description;
            existingWorkflow.InitialEnquiry = dto.InitialEnquiry;
            existingWorkflow.CreateQuote = dto.CreateQuotation;
            existingWorkflow.InviteShowRoom = dto.InviteShowRoomVisit;
            existingWorkflow.SetupSiteVisit = dto.SetupSiteVisit;
            existingWorkflow.InvoiceSent = dto.InvoiceSent;
            existingWorkflow.SupplierId = dto.SupplierId;
            existingWorkflow.CompanyId = dto.CompanyId;
            existingWorkflow.ProductId = dto.ProductId;
            existingWorkflow.ProductTypeId = dto.ProductTypeId;
            existingWorkflow.DateUpdated = DateTime.UtcNow;
            existingWorkflow.UpdatedBy = 1; // To be replaced with actual user I

            _context.WorkflowStarts.Update(existingWorkflow);
            await _context.SaveChangesAsync();

            return existingWorkflow;
        }

        public async Task<IEnumerable<InitialEnquiry>> GetInitialEnquiryForWorkflowAsync(int WorkflowId)
        {
            return await _context.InitialEnquiries.Where(w => w.WorkflowId == WorkflowId).ToListAsync();
        }

        public async Task<InitialEnquiry> AddInitialEnquiry(InitialEnquiryDto dto)
        {
            var initialEnquiry = new InitialEnquiry
            {
                WorkflowId = dto.WorkflowId,
                Comments = dto.Comments,
                Email = dto.Email,
                Images = dto.Images,
                DateCreated = DateTime.UtcNow,
                CreatedBy = 1 // To be replaced with actual user I
            };
            _context.InitialEnquiries.Add(initialEnquiry);
            await _context.SaveChangesAsync();
            return initialEnquiry;
        }

        public async Task<InitialEnquiry> UpdateInitialEnquiry(InitialEnquiryDto dto)
        {
            var existingInquiry = await _context.InitialEnquiries.FindAsync(dto.EnquiryId);

            if (existingInquiry == null)
            {
                throw new Exception("Enquiry not found");
            }

            existingInquiry.Comments = dto.Comments;
            existingInquiry.Email = dto.Email;
            existingInquiry.Images = dto.Images;

            existingInquiry.DateUpdated = DateTime.UtcNow;
            existingInquiry.UpdatedBy = 1; // To be replaced with actual user I

            _context.InitialEnquiries.Update(existingInquiry);
            await _context.SaveChangesAsync();

            return existingInquiry;
        }

        public async Task<List<int>> GetStandardWidthsForProductAsync(int productId)
        {
            return await _context.Projections
                    .Where(p => p.ProductId == productId)
                    .Select(p => p.Width_cm)
                    .Distinct()
                    .ToListAsync();
        }

        public async Task<List<int>> GetProjectionWidthsForProductAsync(int productId)
        {
            return await _context.Projections
                    .Where(p => p.ProductId == productId)
                    .Select(p => p.Projection_cm)
                    .Distinct()
                    .ToListAsync();
        }

        public async Task<decimal> GetProjectionPriceForProductAsync(int productId, int widthcm, int projectioncm)
        {
            return await _context.Projections
                     .Where(p => p.ProductId == productId && p.Width_cm == widthcm && p.Projection_cm == projectioncm)
                     .Select(p => p.Price) 
                     .FirstOrDefaultAsync();
        }

        public async Task<List<Brackets>> GeBracketsForProductAsync(int productId)
        {
            return await _context.Brackets.Where(b => b.ProductId == productId).ToListAsync();
        }

        public async Task<List<Arms>> GeArmsForProductAsync(int productId)
        {
            return await _context.Arms.Where(f => f.ProductId == productId).ToListAsync();
        }

    }
}
