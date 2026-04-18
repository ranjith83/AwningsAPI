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

        public async Task<IEnumerable<WorkflowDto>> GetAllWorfflowsForCustomerAsync(int customerId)
        {
            var workflows = await _context.WorkflowStarts
                .Include(w => w.Product)
                .Where(w => w.CustomerId == customerId)
                .ToListAsync();

            if (!workflows.Any())
                return Enumerable.Empty<WorkflowDto>();

            var workflowIds = workflows.Select(w => w.WorkflowId).ToList();

            // Bulk-load which workflow IDs have real activity for each stage
            var enquiryIds = await _context.InitialEnquiries.Where(e => workflowIds.Contains(e.WorkflowId)).Select(e => e.WorkflowId).Distinct().ToListAsync();
            var quoteIds = await _context.Quotes.Where(q => workflowIds.Contains(q.WorkflowId)).Select(q => q.WorkflowId).Distinct().ToListAsync();
            var showroomIds = await _context.ShowroomInvites.Where(s => workflowIds.Contains(s.WorkflowId)).Select(s => s.WorkflowId).Distinct().ToListAsync();
            var siteVisitIds = await _context.SiteVisits.Where(v => workflowIds.Contains(v.WorkflowId)).Select(v => v.WorkflowId).Distinct().ToListAsync();
            var invoiceIds = await _context.Invoices.Where(i => workflowIds.Contains(i.WorkflowId)).Select(i => i.WorkflowId).Distinct().ToListAsync();

            return workflows.Select(w => new WorkflowDto
            {
                WorkflowId = w.WorkflowId,
                WorkflowName = w.WorkflowName,
                ProductName = w.Product.Description,
                Description = w.Description,
                InitialEnquiry = w.InitialEnquiry,
                CreateQuotation = w.CreateQuote,
                InviteShowRoomVisit = w.InviteShowRoom,
                SetupSiteVisit = w.SetupSiteVisit,
                InvoiceSent = w.InvoiceSent,
                // Stage-completed flags
                InitialEnquiryCompleted = enquiryIds.Contains(w.WorkflowId),
                CreateQuotationCompleted = quoteIds.Contains(w.WorkflowId),
                InviteShowRoomCompleted = showroomIds.Contains(w.WorkflowId),
                SetupSiteVisitCompleted = siteVisitIds.Contains(w.WorkflowId),
                InvoiceSentCompleted = invoiceIds.Contains(w.WorkflowId),
                // Deletion guard flag (any completed stage = has dependencies)
                HasDependencies =
                    enquiryIds.Contains(w.WorkflowId) ||
                    quoteIds.Contains(w.WorkflowId) ||
                    showroomIds.Contains(w.WorkflowId) ||
                    siteVisitIds.Contains(w.WorkflowId) ||
                    invoiceIds.Contains(w.WorkflowId),
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
                var task = await _context.Tasks.AsNoTracking()
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
                Console.Error.WriteLine($"[WorkflowService] TryCreateInitialEnquiryFromTaskAsync failed for task {taskId}: {ex.Message}");
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

        /// <summary>
        /// Checks all five dependency tables before deleting.
        /// Returns a <see cref="WorkflowDeleteResult"/> with:
        ///   Deleted = true  → workflow was removed.
        ///   Deleted = false → blocked; BlockingDependencies lists what is preventing deletion.
        /// This ensures data integrity: you cannot lose enquiries, quotes, site visits,
        /// showroom visits or invoices by accidentally deleting their parent workflow.
        /// </summary>
        public async Task<WorkflowDeleteResult> DeleteWorkflowAsync(int workflowId)
        {
            var workflow = await _context.WorkflowStarts.FindAsync(workflowId);
            if (workflow == null)
                return new WorkflowDeleteResult { Deleted = false, Message = "Workflow not found." };

            // ── Check every dependency table ──────────────────────────────────
            var deps = new List<WorkflowDependency>();

            int enquiryCount = await _context.InitialEnquiries.CountAsync(e => e.WorkflowId == workflowId);
            if (enquiryCount > 0)
                deps.Add(new WorkflowDependency { Name = "Initial Enquiry", Count = enquiryCount });

            int quoteCount = await _context.Quotes.CountAsync(q => q.WorkflowId == workflowId);
            if (quoteCount > 0)
                deps.Add(new WorkflowDependency { Name = "Quote", Count = quoteCount });

            int showroomCount = await _context.ShowroomInvites.CountAsync(s => s.WorkflowId == workflowId);
            if (showroomCount > 0)
                deps.Add(new WorkflowDependency { Name = "Showroom Visit", Count = showroomCount });

            int siteVisitCount = await _context.SiteVisits.CountAsync(v => v.WorkflowId == workflowId);
            if (siteVisitCount > 0)
                deps.Add(new WorkflowDependency { Name = "Site Visit", Count = siteVisitCount });

            int invoiceCount = await _context.Invoices.CountAsync(i => i.WorkflowId == workflowId);
            if (invoiceCount > 0)
                deps.Add(new WorkflowDependency { Name = "Invoice", Count = invoiceCount });

            // ── Block if any dependencies exist ───────────────────────────────
            if (deps.Any())
            {
                var names = string.Join(", ", deps.Select(d => $"{d.Name} ({d.Count})"));
                return new WorkflowDeleteResult
                {
                    Deleted = false,
                    Message = $"Cannot delete — this workflow has linked records: {names}.",
                    BlockingDependencies = deps
                };
            }

            // ── Safe to delete ────────────────────────────────────────────────
            // Also clean up follow-ups (these are soft metadata, safe to remove)
            var followUps = await _context.WorkflowFollowUps
                .Where(f => f.WorkflowId == workflowId).ToListAsync();
            if (followUps.Any())
                _context.WorkflowFollowUps.RemoveRange(followUps);

            _context.WorkflowStarts.Remove(workflow);
            await _context.SaveChangesAsync();

            return new WorkflowDeleteResult
            {
                Deleted = true,
                Message = "Workflow deleted successfully.",
                BlockingDependencies = new List<WorkflowDependency>()
            };
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

            existing.Comments = dto.Comments; existing.Email = dto.Email;
            existing.Images = dto.Images; existing.Signature = dto.Signature;
            existing.DateUpdated = DateTime.UtcNow; existing.UpdatedBy = currentUser;

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
            await _context.Projections.Where(p => p.ProductId == productId && p.Width_cm == widthcm && p.Projection_cm == projectioncm).Select(p => p.Price).FirstOrDefaultAsync();

        /// <summary>
        /// Returns the ArmTypeId from the Projections row that matches the given
        /// product / width / projection combination.  The frontend calls this after
        /// the user picks a width so the brackets dropdown can be filtered.
        /// Returns null if no matching projection row is found.
        /// </summary>
        public async Task<int?> GetArmTypeForProjectionAsync(int productId, int widthcm, int projectioncm) =>
            await _context.Projections
                .Where(p => p.ProductId == productId && p.Width_cm == widthcm && p.Projection_cm == projectioncm)
                .Select(p => (int?)p.ArmTypeId)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Returns brackets for the product.  When <paramref name="armTypeId"/> is
        /// supplied, only brackets whose ArmTypeId matches OR whose ArmTypeId is null
        /// (universal brackets) are returned.  When null, all brackets are returned
        /// (e.g. before the user has selected a width).
        /// </summary>
        public async Task<List<Brackets>> GeBracketsForProductAsync(int productId, int? armTypeId = null)
        {
            var query = _context.Brackets.Where(b => b.ProductId == productId);

            if (armTypeId.HasValue)
                query = query.Where(b => b.ArmTypeId == null || b.ArmTypeId == armTypeId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Arms>> GeArmsForProductAsync(int productId) =>
            await _context.Arms.Where(f => f.ProductId == productId).ToListAsync();

        public async Task<List<Motors>> GeMotorsForProductAsync(int productId) =>
            await _context.Motors.Where(f => f.ProductId == productId).ToListAsync();

        public async Task<decimal> GeValanceStylePriceForProductAsync(int productId, int widthcm) =>
            await _context.valanceStyles.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        public async Task<decimal> GeNonStandardRALColourPriceForProductAsync(int productId, int widthcm) =>
            await _context.nonStandardRALColours.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        /// <summary>
        /// Returns all ShadePlus rows for the given product and width, together with a
        /// HasMultiple flag.  The frontend uses HasMultiple to decide whether to render
        /// a plain price field (single option) or a dropdown (multiple options).
        /// When multiple options exist the user picks one and its Description is stored
        /// on the quote line instead of the generic "ShadePlus" label.
        /// </summary>
        public async Task<ShadePlusOptionsDto> GetShadePlusOptionsAsync(int productId, int widthcm)
        {
            var rows = await _context.ShadePlus
                .Where(p => p.ProductId == productId ) // && p.WidthCm == widthcm)
                .OrderBy(p => p.ShadePlusId)
                .Select(p => new ShadePlusDto
                {
                    ShadePlusId = p.ShadePlusId,
                    Description = p.Description,
                    WidthCm = p.WidthCm,
                    Price = p.Price
                })
                .ToListAsync();

            return new ShadePlusOptionsDto
            {
                HasMultiple = rows.Count > 1,
                Options = rows
            };
        }

        public async Task<decimal> GeWallSealingProfilerPriceForProductAsync(int productId, int widthcm) =>
            await _context.wallSealingProfiles.Where(p => p.ProductId == productId && p.WidthCm == widthcm).Select(p => p.Price).FirstOrDefaultAsync();

        // ── Addon availability checks ─────────────────────────────────────────
        // Used by the frontend to show/hide optional addon checkboxes.

        public async Task<bool> HasNonStandardRALColoursAsync(int productId) =>
            await _context.nonStandardRALColours.AnyAsync(p => p.ProductId == productId);

        public async Task<bool> HasShadePlusAsync(int productId) =>
            await _context.ShadePlus.AnyAsync(p => p.ProductId == productId);

        public async Task<bool> HasValanceStylesAsync(int productId) =>
            await _context.valanceStyles.AnyAsync(p => p.ProductId == productId);

        public async Task<bool> HasWallSealingProfilesAsync(int productId) =>
            await _context.wallSealingProfiles.AnyAsync(p => p.ProductId == productId);

        public async Task<List<Heaters>> GeHeatersForProductAsync(int productId) =>
            await _context.Heaters.Where(f => f.ProductId == productId).ToListAsync();

        // ════════════════════════════════════════════════════════════════════
        // USER SIGNATURES
        // ════════════════════════════════════════════════════════════════════

        public async Task<IEnumerable<UserSignatureDto>> GetSignaturesAsync(string username) =>
            await _context.UserSignatures
                .Where(s => s.Username == username)
                .OrderByDescending(s => s.IsDefault).ThenBy(s => s.Label)
                .Select(s => MapSigToDto(s)).ToListAsync();

        public async Task<UserSignatureDto> CreateSignatureAsync(UserSignatureDto dto, string username)
        {
            if (dto.IsDefault) await ClearSignatureDefaultAsync(username);
            var entity = MapDtoToSig(new UserSignature(), dto);
            entity.Username = username; entity.DateCreated = DateTime.UtcNow;
            _context.UserSignatures.Add(entity);
            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        public async Task<UserSignatureDto> UpdateSignatureAsync(int signatureId, UserSignatureDto dto, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username)
                ?? throw new Exception("Signature not found.");
            if (dto.IsDefault && !entity.IsDefault) await ClearSignatureDefaultAsync(username);
            MapDtoToSig(entity, dto);
            entity.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        public async Task<UserSignatureDto> SetDefaultSignatureAsync(int signatureId, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username)
                ?? throw new Exception("Signature not found.");
            await ClearSignatureDefaultAsync(username);
            entity.IsDefault = true; entity.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return MapSigToDto(entity);
        }

        public async Task<bool> DeleteSignatureAsync(int signatureId, string username)
        {
            var entity = await _context.UserSignatures
                .FirstOrDefaultAsync(s => s.SignatureId == signatureId && s.Username == username);
            if (entity == null) return false;
            _context.UserSignatures.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ClearSignatureDefaultAsync(string username)
        {
            var defaults = await _context.UserSignatures
                .Where(s => s.Username == username && s.IsDefault).ToListAsync();
            foreach (var s in defaults) { s.IsDefault = false; s.DateUpdated = DateTime.UtcNow; }
        }

        private static UserSignature MapDtoToSig(UserSignature e, UserSignatureDto d)
        {
            e.Label = d.Label.Trim(); e.FullName = d.FullName?.Trim();
            e.JobTitle = d.JobTitle?.Trim(); e.Company = d.Company?.Trim();
            e.Phone = d.Phone?.Trim(); e.Mobile = d.Mobile?.Trim();
            e.Email = d.Email?.Trim(); e.Website = d.Website?.Trim();
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