using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AwningsAPI.Services.WorkflowService
{
    public class WorkflowService : IWorkflowService
    {
        private readonly AppDbContext _context;
        private readonly FollowUpService _followUpService;

        public WorkflowService(AppDbContext context, FollowUpService followUpService)
        {
            _context = context;
            _followUpService = followUpService;
        }
        public async Task<IEnumerable<WorkflowStart>> GetAllWorfflowsForCustomerAsync(int CustomerId)
        {
            return await _context.WorkflowStarts.Include(p => p.Product).Where(w => w.CustomerId == CustomerId).ToListAsync();
        }

        public async Task<WorkflowStart> CreateWorkflow(WorkflowDto dto, string currentUser)
        {
            var workflow = new WorkflowStart
            {
                WorkflowName = dto.WorkflowName,
                Description = dto.Description,
                InitialEnquiry = dto.InitialEnquiry,
                CreateQuote = dto.CreateQuotation,
                InviteShowRoom = dto.InviteShowRoomVisit,
                SetupSiteVisit = dto.SetupSiteVisit,
                InvoiceSent = dto.InvoiceSent,
                SupplierId = dto.SupplierId,
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                ProductTypeId = dto.ProductTypeId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };

            _context.WorkflowStarts.Add(workflow);
            await _context.SaveChangesAsync();

            // ── Auto-create InitialEnquiry when workflow originates from an initial_enquiry email ──
            // If the caller passes a TaskId, look up the task. If its category is
            // "initial_enquiry" (or the display label "Initial Enquiry"), create a linked
            // InitialEnquiry record so the follow-up timer starts immediately.
            if (dto.TaskId.HasValue && dto.TaskId.Value > 0)
            {
                await TryCreateInitialEnquiryFromTaskAsync(
                    workflowId: workflow.WorkflowId,
                    taskId: dto.TaskId.Value,
                    currentUser: currentUser);
            }

            return workflow;
        }

        /// <summary>
        /// Called by CreateWorkflow when a TaskId is provided.
        /// Loads the EmailTask and, if the category is initial_enquiry, inserts a
        /// matching InitialEnquiry record so it immediately appears in the enquiry
        /// history and starts the 3-day follow-up clock.
        ///
        /// Non-fatal: any error is logged and execution continues so the workflow
        /// is always saved even if the enquiry record fails.
        /// </summary>
        private async Task TryCreateInitialEnquiryFromTaskAsync(
            int workflowId, int taskId, string currentUser)
        {
            try
            {
                var task = await _context.EmailTasks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);

                if (task == null) return;

                // Match both the raw category key and the display label
                var cat = (task.TaskType ?? task.Category ?? string.Empty).ToLowerInvariant();
                bool isInitialEnquiry =
                    cat == "initial_enquiry" ||
                    cat == "initial enquiry" || cat == "general_inquiry";


                if (!isInitialEnquiry) return;

                // Guard: don't create a duplicate if one already exists for this workflow+task
                bool alreadyExists = await _context.InitialEnquiries
                    .AnyAsync(e => e.WorkflowId == workflowId && e.TaskId == taskId);

                if (alreadyExists) return;

                // Build the comments summary from subject + email body preview
                var bodyPreview = (task.EmailBody ?? string.Empty);
                if (bodyPreview.Length > 400) bodyPreview = bodyPreview[..400] + "…";

                var comments = string.IsNullOrWhiteSpace(task.Subject)
                    ? bodyPreview: $"Subject: {task.Subject} { bodyPreview} ";

                var enquiry = new InitialEnquiry
                {
                    WorkflowId = workflowId,
                    Comments = comments.Trim(),
                    Email = task.FromEmail ?? string.Empty,
                    Images = "",
                    TaskId = taskId,
                    IncomingEmailId = task.IncomingEmailId,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser
                };

                _context.InitialEnquiries.Add(enquiry);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Non-fatal: log but do not bubble up so the workflow save succeeds
                Console.Error.WriteLine(
                    $"[WorkflowService] TryCreateInitialEnquiryFromTaskAsync failed for " +
                    $"task {taskId}: {ex.Message}");
            }
        }

        public async Task<WorkflowStart> UpdateWorkflow(WorkflowDto dto, string currentUser)
        {
            var existingWorkflow = await _context.WorkflowStarts.FindAsync(dto.WorkflowId);

            if (existingWorkflow == null)
            {
                throw new Exception("Workflow not found");
            }

            existingWorkflow.WorkflowName = dto.WorkflowName;
            existingWorkflow.Description = dto.Description;
            existingWorkflow.InitialEnquiry = dto.InitialEnquiry;
            existingWorkflow.CreateQuote = dto.CreateQuotation;
            existingWorkflow.InviteShowRoom = dto.InviteShowRoomVisit;
            existingWorkflow.SetupSiteVisit = dto.SetupSiteVisit;
            existingWorkflow.InvoiceSent = dto.InvoiceSent;
            existingWorkflow.SupplierId = dto.SupplierId;
            existingWorkflow.CustomerId = dto.CustomerId;
            existingWorkflow.ProductId = dto.ProductId;
            existingWorkflow.ProductTypeId = dto.ProductTypeId;
            existingWorkflow.DateUpdated = DateTime.UtcNow;
            existingWorkflow.UpdatedBy = currentUser;

            _context.WorkflowStarts.Update(existingWorkflow);
            await _context.SaveChangesAsync();

            return existingWorkflow;
        }

        public async Task<IEnumerable<InitialEnquiry>> GetInitialEnquiryForWorkflowAsync(int WorkflowId)
        {
            return await _context.InitialEnquiries.Where(w => w.WorkflowId == WorkflowId).ToListAsync();
        }

        public async Task<InitialEnquiry> AddInitialEnquiry(InitialEnquiryDto dto, string currentUser)
        {
            var initialEnquiry = new InitialEnquiry
            {
                WorkflowId = dto.WorkflowId,
                Comments = dto.Comments,
                Email = dto.Email,
                Images = dto.Images,
                // Link to the originating email task/incoming email when available
                TaskId = dto.TaskId,
                IncomingEmailId = dto.IncomingEmailId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.InitialEnquiries.Add(initialEnquiry);
            await _context.SaveChangesAsync();

            // ── Timer reset: dismiss any active follow-up for this workflow ──
            // A new enquiry resets the 3-day clock. The old follow-up row
            // disappears from the grid; a new one will appear after 3 days
            // if still no quote is raised.
            await _followUpService.DismissActiveForWorkflowAsync(dto.WorkflowId, currentUser);

            return initialEnquiry;
        }

        public async Task<InitialEnquiry> UpdateInitialEnquiry(InitialEnquiryDto dto, string currentUser)
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
            existingInquiry.UpdatedBy = currentUser;

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

        public async Task<List<Motors>> GeMotorsForProductAsync(int productId)
        {
            return await _context.Motors.Where(f => f.ProductId == productId).ToListAsync();
        }
        public async Task<decimal> GeValanceStylePriceForProductAsync(int productId, int widthcm)
        {
            return await _context.valanceStyles
                     .Where(p => p.ProductId == productId && p.WidthCm == widthcm)
                     .Select(p => p.Price)
                     .FirstOrDefaultAsync();
        }

        public async Task<decimal> GeNonStandardRALColourPriceForProductAsync(int productId, int widthcm)
        {
            return await _context.nonStandardRALColours
                     .Where(p => p.ProductId == productId && p.WidthCm == widthcm)
                     .Select(p => p.Price)
                     .FirstOrDefaultAsync();
        }

        public async Task<decimal> GeWallSealingProfilerPriceForProductAsync(int productId, int widthcm)
        {
            return await _context.wallSealingProfiles
                     .Where(p => p.ProductId == productId && p.WidthCm == widthcm)
                     .Select(p => p.Price)
                     .FirstOrDefaultAsync();
        }

        public async Task<List<Heaters>> GeHeatersForProductAsync(int productId)
        {
            return await _context.Heaters.Where(f => f.ProductId == productId).ToListAsync();
        }

        public async Task<bool> DeleteWorkflowAsync(int workflowId)
        {
            var workflow = await _context.WorkflowStarts.FindAsync(workflowId);

            if (workflow == null)
                return false;

            _context.WorkflowStarts.Remove(workflow);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}