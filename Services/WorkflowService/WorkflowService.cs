using AwningsAPI.Database;
using AwningsAPI.Dto.Auth;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Auth;
using AwningsAPI.Model.Products;
using AwningsAPI.Model.Suppliers;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;

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

        // ════════════════════════════════════════════════════════════════════
        // WORKFLOW
        // ════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Returns all workflows for a customer, with per-stage completion flags
        /// computed from real activity records (enquiries, quotes, site visits, invoices).
        /// </summary>
        public async Task<IEnumerable<WorkflowDto>> GetAllWorfflowsForCustomerAsync(int customerId)
        {
            // Load workflows with their product
            var workflows = await _context.WorkflowStarts
                .Include(w => w.Product)
                .Where(w => w.CustomerId == customerId)
                .ToListAsync();

            if (!workflows.Any())
                return Enumerable.Empty<WorkflowDto>();

            var workflowIds = workflows.Select(w => w.WorkflowId).ToList();

            // ── Bulk-load activity counts per workflow in a single query each ──
            // InitialEnquiry — has at least one enquiry record
            var enquiryIds = await _context.InitialEnquiries
                .Where(e => workflowIds.Contains(e.WorkflowId))
                .Select(e => e.WorkflowId)
                .Distinct()
                .ToListAsync();

            // Quote — has at least one quote record
            var quoteIds = await _context.Quotes
                .Where(q => workflowIds.Contains(q.WorkflowId))
                .Select(q => q.WorkflowId)
                .Distinct()
                .ToListAsync();

            // ShowroomInvite — has at least one showroom invite record
            var showroomIds = await _context.ShowroomInvites
                .Where(s => workflowIds.Contains(s.WorkflowId))
                .Select(s => s.WorkflowId)
                .Distinct()
                .ToListAsync();

            // SiteVisit — has at least one site visit record
            var siteVisitIds = await _context.SiteVisits
                .Where(v => workflowIds.Contains(v.WorkflowId))
                .Select(v => v.WorkflowId)
                .Distinct()
                .ToListAsync();

            // Invoice — has at least one invoice record
            var invoiceIds = await _context.Invoices
                .Where(i => workflowIds.Contains(i.WorkflowId))
                .Select(i => i.WorkflowId)
                .Distinct()
                .ToListAsync();

            // ── Build DTOs ────────────────────────────────────────────────────
            return workflows.Select(w => new WorkflowDto
            {
                WorkflowId = w.WorkflowId,
                WorkflowName = w.WorkflowName,
                ProductName = w.Product.Description,
                Description = w.Description,

                // Enabled flags (user-controlled)
                InitialEnquiry = w.InitialEnquiry,
                CreateQuotation = w.CreateQuote,
                InviteShowRoomVisit = w.InviteShowRoom,
                SetupSiteVisit = w.SetupSiteVisit,
                InvoiceSent = w.InvoiceSent,

                // Completed flags (computed from real activity)
                InitialEnquiryCompleted = enquiryIds.Contains(w.WorkflowId),
                CreateQuotationCompleted = quoteIds.Contains(w.WorkflowId),
                InviteShowRoomCompleted = showroomIds.Contains(w.WorkflowId),
                SetupSiteVisitCompleted = siteVisitIds.Contains(w.WorkflowId),
                InvoiceSentCompleted = invoiceIds.Contains(w.WorkflowId),

                DateAdded = w.DateCreated,
                AddedBy = w.CreatedBy,
                CompanyId = w.CompanyId,
                SupplierId = w.SupplierId,
                ProductId = w.ProductId,
                ProductTypeId = w.ProductTypeId,
                CustomerId = w.CustomerId
            }).ToList();
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

            if (dto.TaskId.HasValue && dto.TaskId.Value > 0)
                await TryCreateInitialEnquiryFromTaskAsync(workflow.WorkflowId, dto.TaskId.Value, currentUser);

            return workflow;
        }

        private async Task TryCreateInitialEnquiryFromTaskAsync(int workflowId, int taskId, string currentUser)
        {
            try
            {
                var task = await _context.EmailTasks.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TaskId == taskId);
                if (task == null) return;

                var cat = (task.TaskType ?? task.Category ?? string.Empty).ToLowerInvariant();
                bool isInitialEnquiry = cat is "initial_enquiry" or "initial enquiry" or "general_inquiry";
                if (!isInitialEnquiry) return;

                bool alreadyExists = await _context.InitialEnquiries
                    .AnyAsync(e => e.WorkflowId == workflowId && e.TaskId == taskId);
                if (alreadyExists) return;

                var bodyPreview = task.EmailBody ?? string.Empty;
                if (bodyPreview.Length > 400) bodyPreview = bodyPreview[..400] + "…";

                var comments = string.IsNullOrWhiteSpace(task.Subject)
                    ? bodyPreview : $"Subject: {task.Subject} {bodyPreview}";

                _context.InitialEnquiries.Add(new InitialEnquiry
                {
                    WorkflowId = workflowId,
                    Comments = comments.Trim(),
                    Email = task.FromEmail ?? string.Empty,
                    Images = null,
                    Signature = null,
                    TaskId = taskId,
                    IncomingEmailId = task.IncomingEmailId,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"[WorkflowService] TryCreateInitialEnquiryFromTaskAsync failed for task {taskId}: {ex.Message}");
            }
        }

        public async Task<WorkflowStart> UpdateWorkflow(WorkflowDto dto, string currentUser)
        {
            var existing = await _context.WorkflowStarts.FindAsync(dto.WorkflowId)
                ?? throw new Exception("Workflow not found");

            existing.WorkflowName = dto.WorkflowName;
            existing.Description = dto.Description;
            existing.InitialEnquiry = dto.InitialEnquiry;
            existing.CreateQuote = dto.CreateQuotation;
            existing.InviteShowRoom = dto.InviteShowRoomVisit;
            existing.SetupSiteVisit = dto.SetupSiteVisit;
            existing.InvoiceSent = dto.InvoiceSent;
            existing.SupplierId = dto.SupplierId;
            existing.CustomerId = dto.CustomerId;
            existing.ProductId = dto.ProductId;
            existing.ProductTypeId = dto.ProductTypeId;
            existing.DateUpdated = DateTime.UtcNow;
            existing.UpdatedBy = currentUser;

            _context.WorkflowStarts.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteWorkflowAsync(int workflowId)
        {
            var workflow = await _context.WorkflowStarts.FindAsync(workflowId);
            if (workflow == null) return false;
            _context.WorkflowStarts.Remove(workflow);
            await _context.SaveChangesAsync();
            return true;
        }

        // ════════════════════════════════════════════════════════════════════
        // INITIAL ENQUIRY
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<InitialEnquiry>> GetInitialEnquiryForWorkflowAsync(int workflowId) =>
            await _context.InitialEnquiries.Where(w => w.WorkflowId == workflowId).ToListAsync();

        public async Task<InitialEnquiry> AddInitialEnquiry(InitialEnquiryDto dto, string currentUser)
        {
            var enquiry = new InitialEnquiry
            {
                WorkflowId = dto.WorkflowId,
                Comments = dto.Comments,
                Email = dto.Email,
                Images = dto.Images,
                Signature = dto.Signature,
                TaskId = dto.TaskId,
                IncomingEmailId = dto.IncomingEmailId,
                DateCreated = DateTime.UtcNow,
                CreatedBy = currentUser
            };
            _context.InitialEnquiries.Add(enquiry);
            await _context.SaveChangesAsync();
            await _followUpService.DismissActiveForWorkflowAsync(dto.WorkflowId, currentUser);
            return enquiry;
        }

        public async Task<InitialEnquiry> UpdateInitialEnquiry(InitialEnquiryDto dto, string currentUser)
        {
            var existing = await _context.InitialEnquiries.FindAsync(dto.EnquiryId)
                ?? throw new Exception("Enquiry not found");

            existing.Comments = dto.Comments;
            existing.Email = dto.Email;
            existing.Images = dto.Images;
            existing.Signature = dto.Signature;
            existing.DateUpdated = DateTime.UtcNow;
            existing.UpdatedBy = currentUser;

            _context.InitialEnquiries.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        // ════════════════════════════════════════════════════════════════════
        // PRODUCT / PRICING
        // ════════════════════════════════════════════════════════════════════

        public async Task<List<int>> GetStandardWidthsForProductAsync(int productId) =>
            await _context.Projections.Where(p => p.ProductId == productId).Select(p => p.Width_cm).Distinct().ToListAsync();

        public async Task<List<int>> GetProjectionWidthsForProductAsync(int productId) =>
            await _context.Projections.Where(p => p.ProductId == productId).Select(p => p.Projection_cm).Distinct().ToListAsync();

        public async Task<decimal> GetProjectionPriceForProductAsync(int productId, int widthcm, int projectioncm) =>
            await _context.Projections
                .Where(p => p.ProductId == productId && p.Width_cm == widthcm && p.Projection_cm == projectioncm)
                .Select(p => p.Price).FirstOrDefaultAsync();

        public async Task<List<Brackets>> GeBracketsForProductAsync(int productId) =>
            await _context.Brackets.Where(b => b.ProductId == productId).ToListAsync();

        public async Task<List<Arms>> GeArmsForProductAsync(int productId) =>
            await _context.Arms.Where(f => f.ProductId == productId).ToListAsync();

        public async Task<List<Motors>> GeMotorsForProductAsync(int productId) =>
            await _context.Motors.Where(f => f.ProductId == productId).ToListAsync();

        public async Task<decimal> GeValanceStylePriceForProductAsync(int productId, int widthcm) =>
            await _context.valanceStyles.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        public async Task<decimal> GeNonStandardRALColourPriceForProductAsync(int productId, int widthcm) =>
            await _context.nonStandardRALColours.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        public async Task<decimal> GeWallSealingProfilerPriceForProductAsync(int productId, int widthcm) =>
            await _context.wallSealingProfiles.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        public async Task<List<Heaters>> GeHeatersForProductAsync(int productId) =>
            await _context.Heaters.Where(f => f.ProductId == productId).ToListAsync();

        // ════════════════════════════════════════════════════════════════════
        // USER SIGNATURES  (moved from UserSignatureController)
        // ════════════════════════════════════════════════════════════════════

        /// <summary>Returns all signatures owned by <paramref name="username"/>,
        /// default first then alphabetical.</summary>
        public async Task<IEnumerable<UserSignatureDto>> GetSignaturesAsync(string username)
        {
            return await _context.UserSignatures
                .Where(s => s.Username == username)
                .OrderByDescending(s => s.IsDefault)
                .ThenBy(s => s.Label)
                .Select(s => MapSigToDto(s))
                .ToListAsync();
        }

        /// <summary>Creates a new signature. Clears any existing default first
        /// if <see cref="UserSignatureDto.IsDefault"/> is true.</summary>
        public async Task<UserSignatureDto> CreateSignatureAsync(UserSignatureDto dto, string username)
        {
            if (dto.IsDefault)
                await ClearSignatureDefaultAsync(username);

            var entity = MapDtoToSig(new UserSignature(), dto);
            entity.Username = username;
            entity.DateCreated = DateTime.UtcNow;

            _context.UserSignatures.Add(entity);
            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        /// <summary>Updates label, contact fields, format options and rendered text.</summary>
        public async Task<UserSignatureDto> UpdateSignatureAsync(
            int signatureId, UserSignatureDto dto, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username)
                ?? throw new Exception("Signature not found.");

            if (dto.IsDefault && !entity.IsDefault)
                await ClearSignatureDefaultAsync(username);

            MapDtoToSig(entity, dto);
            entity.DateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        /// <summary>Promotes one signature to default, demoting all others.</summary>
        public async Task<UserSignatureDto> SetDefaultSignatureAsync(int signatureId, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username)
                ?? throw new Exception("Signature not found.");

            await ClearSignatureDefaultAsync(username);
            entity.IsDefault = true;
            entity.DateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        /// <summary>Deletes a signature. Returns false if not found.</summary>
        public async Task<bool> DeleteSignatureAsync(int signatureId, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username);

            if (entity == null) return false;

            _context.UserSignatures.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Signature private helpers ─────────────────────────────────────────

        /// <summary>Sets IsDefault = false for every signature belonging to
        /// <paramref name="username"/>. Caller must SaveChanges afterward.</summary>
        private async Task ClearSignatureDefaultAsync(string username)
        {
            var defaults = await _context.UserSignatures
                .Where(s => s.Username == username && s.IsDefault)
                .ToListAsync();

            foreach (var s in defaults)
            {
                s.IsDefault = false;
                s.DateUpdated = DateTime.UtcNow;
            }
            // SaveChanges is called by the caller that invokes this method
        }

        private static UserSignature MapDtoToSig(UserSignature e, UserSignatureDto d)
        {
            e.Label = d.Label.Trim();
            e.FullName = d.FullName?.Trim();
            e.JobTitle = d.JobTitle?.Trim();
            e.Company = d.Company?.Trim();
            e.Phone = d.Phone?.Trim();
            e.Mobile = d.Mobile?.Trim();
            e.Email = d.Email?.Trim();
            e.Website = d.Website?.Trim();
            e.GreetingText = (d.GreetingText ?? "Kindest regards,").Trim();
            e.SeparatorStyle = (d.SeparatorStyle ?? "blank_line").Trim();
            e.LayoutOrder = (d.LayoutOrder ?? "name_first").Trim();
            e.FontFamily = (d.FontFamily ?? "georgia").Trim();
            e.SignatureText = d.SignatureText.Trim();
            e.IsDefault = d.IsDefault;
            return e;
        }

        private static UserSignatureDto MapSigToDto(UserSignature s) => new()
        {
            SignatureId = s.SignatureId,
            Label = s.Label,
            FullName = s.FullName,
            JobTitle = s.JobTitle,
            Company = s.Company,
            Phone = s.Phone,
            Mobile = s.Mobile,
            Email = s.Email,
            Website = s.Website,
            GreetingText = s.GreetingText,
            SeparatorStyle = s.SeparatorStyle,
            LayoutOrder = s.LayoutOrder,
            FontFamily = s.FontFamily,
            SignatureText = s.SignatureText,
            IsDefault = s.IsDefault,
            DateCreated = s.DateCreated,
            DateUpdated = s.DateUpdated
        };
    }
}
