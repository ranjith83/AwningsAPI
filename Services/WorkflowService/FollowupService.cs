using AwningsAPI.Database;
using AwningsAPI.Dto.Workflow;
using AwningsAPI.Model.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AwningsAPI.Services.WorkflowService
{
    /// <summary>
    /// Manages Follow-Up records that are driven by InitialEnquiry data.
    ///
    /// RULE:
    ///   A follow-up is created when the MOST-RECENT InitialEnquiry for a workflow
    ///   is older than 3 days AND the workflow still has CreateQuote = false.
    ///
    /// TIMER RESET:
    ///   When a NEW InitialEnquiry is added to a workflow
    ///   (WorkflowService.AddInitialEnquiry calls DismissActiveForWorkflowAsync),
    ///   any existing active follow-up is dismissed with DismissReason = "NewEnquiry".
    ///   After 3 more days a fresh follow-up will be generated for the new enquiry.
    ///
    /// NAVIGATION:
    ///   Follow-up rows navigate to /workflow/initial-enquiry?workflowId=&customerId=
    ///   so the user can add a new enquiry (which resets the timer).
    /// </summary>
    public class FollowUpService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FollowUpService> _logger;

        // ── 3-day threshold ────────────────────────────────────────────────────
        private static readonly TimeSpan FollowUpThreshold = TimeSpan.FromDays(3);

        public FollowUpService(AppDbContext context, ILogger<FollowUpService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ═════════════════════════════════════════════════════════════════════
        // GENERATION — called by controller on page load / scheduled job
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Scans all workflows for stale enquiries and inserts follow-up records
        /// where none exist yet. Returns the number of new records created.
        ///
        /// Algorithm:
        ///   1. For every workflow where CreateQuote = false, find its most-recent
        ///      InitialEnquiry.
        ///   2. If that enquiry's DateCreated &lt; (UtcNow - 3 days) AND there is no
        ///      active (non-dismissed) follow-up for that workflow → create one.
        /// </summary>
        public async Task<int> GeneratePendingFollowUpsAsync()
        {
            var cutoff = DateTime.UtcNow - FollowUpThreshold;

            // Workflows that haven't had a quote raised yet
            var workflowIds = await _context.WorkflowStarts
                .Where(w => w.CreateQuote == false)
                .Select(w => w.WorkflowId)
                .ToListAsync();

            if (!workflowIds.Any())
                return 0;

            // Most-recent enquiry per workflow, older than the threshold
            var staleEnquiries = await _context.InitialEnquiries
                .Where(e =>
                    workflowIds.Contains(e.WorkflowId) &&
                    e.DateCreated <= cutoff)
                .GroupBy(e => e.WorkflowId)
                .Select(g => g.OrderByDescending(e => e.DateCreated).First())
                .ToListAsync();

            _logger.LogInformation(
                "GeneratePendingFollowUps: {Count} workflow(s) with a stale enquiry",
                staleEnquiries.Count);

            if (!staleEnquiries.Any())
                return 0;

            // Workflow IDs that already have an active follow-up
            var existingWorkflowIds = await _context.WorkflowFollowUps
                .Where(f => !f.IsDismissed)
                .Select(f => f.WorkflowId)
                .ToHashSetAsync();

            int created = 0;

            foreach (var enquiry in staleEnquiries)
            {
                // Skip if there is already an active follow-up for this workflow
                if (existingWorkflowIds.Contains(enquiry.WorkflowId))
                    continue;

                // Resolve company name
                string companyName = "Unknown";
                var workflow = await _context.WorkflowStarts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.WorkflowId == enquiry.WorkflowId);

                if (workflow?.CustomerId != null)
                {
                    var customer = await _context.Customers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CustomerId == workflow.CustomerId);
                    companyName = customer?.Name ?? "Unknown";
                }

                // Build a human-readable subject from the enquiry
                var commentsPreview = string.IsNullOrWhiteSpace(enquiry.Comments)
                    ? string.Empty
                    : (enquiry.Comments.Length > 80
                        ? enquiry.Comments[..80] + "…"
                        : enquiry.Comments);

                var subject = string.IsNullOrWhiteSpace(commentsPreview)
                    ? "Follow-up required — no response to initial enquiry"
                    : $"Follow-up: {commentsPreview}";

                var followUp = new WorkflowFollowUp
                {
                    WorkflowId = enquiry.WorkflowId,
                    CustomerId = workflow?.CustomerId,
                    CompanyName = companyName,
                    EnquiryId = enquiry.EnquiryId,
                    LastEnquiryDate = enquiry.DateCreated,
                    EnquiryComments = enquiry.Comments,
                    EnquiryEmail = enquiry.Email,
                    Subject = subject,
                    Category = "Inquiry",
                    DateAdded = DateTime.UtcNow,
                    IsDismissed = false,
                    TaskId = enquiry.TaskId,
                    CreatedBy = "System"
                };

                _context.WorkflowFollowUps.Add(followUp);
                created++;
            }

            if (created > 0)
                await _context.SaveChangesAsync();

            _logger.LogInformation(
                "GeneratePendingFollowUps: created {Count} new follow-up(s)", created);
            return created;
        }

        // ═════════════════════════════════════════════════════════════════════
        // TIMER RESET — called by WorkflowService.AddInitialEnquiry
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Called automatically when a NEW InitialEnquiry is saved for a workflow.
        /// Dismisses any active follow-up with reason "NewEnquiry" so the 3-day
        /// clock restarts from the new enquiry's date.
        /// </summary>
        public async Task DismissActiveForWorkflowAsync(int workflowId, string currentUser)
        {
            var activeFollowUps = await _context.WorkflowFollowUps
                .Where(f => f.WorkflowId == workflowId && !f.IsDismissed)
                .ToListAsync();

            if (!activeFollowUps.Any())
                return;

            foreach (var fu in activeFollowUps)
            {
                fu.IsDismissed = true;
                fu.ResolvedDate = DateTime.UtcNow;
                fu.ResolvedBy = currentUser;
                fu.DismissReason = "NewEnquiry";
                fu.Notes = "Automatically dismissed — new enquiry submitted by user.";
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "DismissActiveForWorkflow: dismissed {Count} follow-up(s) for workflow {WorkflowId} (new enquiry)",
                activeFollowUps.Count, workflowId);
        }

        // ═════════════════════════════════════════════════════════════════════
        // QUERIES
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Active (non-dismissed) follow-ups, newest first.</summary>
        public async Task<List<FollowUpDto>> GetActiveFollowUpsAsync()
        {
            var items = await _context.WorkflowFollowUps
                .Where(f => !f.IsDismissed)
                .OrderByDescending(f => f.LastEnquiryDate)
                .ToListAsync();

            return items.Select(ToDto).ToList();
        }

        /// <summary>All follow-ups including dismissed, newest first.</summary>
        public async Task<List<FollowUpDto>> GetAllFollowUpsAsync()
        {
            var items = await _context.WorkflowFollowUps
                .OrderByDescending(f => f.LastEnquiryDate)
                .ToListAsync();

            return items.Select(ToDto).ToList();
        }

        /// <summary>Active follow-ups for one customer.</summary>
        public async Task<List<FollowUpDto>> GetFollowUpsByCustomerAsync(int customerId)
        {
            var items = await _context.WorkflowFollowUps
                .Where(f => f.CustomerId == customerId && !f.IsDismissed)
                .OrderByDescending(f => f.LastEnquiryDate)
                .ToListAsync();

            return items.Select(ToDto).ToList();
        }

        // ═════════════════════════════════════════════════════════════════════
        // MANUAL DISMISS — user explicitly resolves a follow-up
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>
        /// User-initiated dismiss (e.g. "called customer, they're not interested").
        /// </summary>
        public async Task<bool> DismissFollowUpAsync(int followUpId, string notes, string currentUser)
        {
            var followUp = await _context.WorkflowFollowUps.FindAsync(followUpId);
            if (followUp == null) return false;

            followUp.IsDismissed = true;
            followUp.ResolvedDate = DateTime.UtcNow;
            followUp.ResolvedBy = currentUser;
            followUp.Notes = notes;
            followUp.DismissReason = "UserDismiss";

            await _context.SaveChangesAsync();
            return true;
        }

        // ═════════════════════════════════════════════════════════════════════
        // HELPERS
        // ═════════════════════════════════════════════════════════════════════

        private static FollowUpDto ToDto(WorkflowFollowUp f) => new()
        {
            FollowUpId = f.FollowUpId,
            WorkflowId = f.WorkflowId,
            CustomerId = f.CustomerId,
            CompanyName = f.CompanyName ?? string.Empty,
            EnquiryId = f.EnquiryId,
            EnquiryEmail = f.EnquiryEmail,
            EnquiryComments = f.EnquiryComments,
            LastEnquiryDate = f.LastEnquiryDate,
            Subject = f.Subject,
            Category = f.Category,
            DateAdded = f.DateAdded,
            IsDismissed = f.IsDismissed,
            DismissReason = f.DismissReason,
            Notes = f.Notes,
            ResolvedDate = f.ResolvedDate,
            ResolvedBy = f.ResolvedBy,
            TaskId = f.TaskId
        };
    }
}