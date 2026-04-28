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
    ///   is older than 3 days AND there is no Quote record in the database for
    ///   that workflow yet.
    ///
    ///   Note: we deliberately check for an actual Quote row rather than the
    ///   workflow's CreateQuote stage flag. The flag represents a UI toggle, not
    ///   whether a quote has been issued. Using the Quotes table means the
    ///   follow-up disappears as soon as a real quote exists — regardless of
    ///   how the stage toggles are set.
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
        ///   1. For every workflow that does NOT yet have a Quote record in the
        ///      Quotes table, find its most-recent InitialEnquiry.
        ///   2. If that enquiry's DateCreated &lt; (UtcNow - 3 days) AND there is no
        ///      active (non-dismissed) follow-up for that workflow → create one.
        ///
        /// Note: We check for an actual Quote row in the database rather than the
        /// workflow's CreateQuote boolean flag. The flag reflects the stage toggle
        /// setting, not whether a quote has actually been issued. Using the Quotes
        /// table means a follow-up is suppressed as soon as a real quote exists.
        /// </summary>
        public async Task<int> GeneratePendingFollowUpsAsync()
        {
            var cutoff = DateTime.UtcNow - FollowUpThreshold;

            // ── Workflows that have NO quote record yet ────────────────────────
            // Get all workflow IDs that DO have at least one quote
            var workflowIdsWithQuotes = await _context.Quotes
                .Select(q => q.WorkflowId)
                .Distinct()
                .ToHashSetAsync();

            // All workflow IDs — then exclude those with quotes
            var allWorkflowIds = await _context.WorkflowStarts
                .Select(w => w.WorkflowId)
                .ToListAsync();

            var workflowIds = allWorkflowIds
                .Where(id => !workflowIdsWithQuotes.Contains(id))
                .ToList();

            if (!workflowIds.Any())
                return 0;

            // ── KEY FIX ────────────────────────────────────────────────────────
            // Group FIRST to get the most-recent enquiry per workflow, THEN check
            // whether that latest enquiry is older than the threshold.
            //
            // The previous query applied the DateCreated <= cutoff filter BEFORE
            // grouping. This meant: if a user replied yesterday (new enquiry),
            // that reply was filtered out, leaving only the old original enquiry,
            // which then incorrectly triggered a new follow-up — making old emails
            // reappear even after they had been replied to.
            //
            // Correct rule: "The MOST-RECENT enquiry on this workflow is older than
            // 3 days." If the user replied yesterday, the most-recent enquiry is
            // 1 day old → no follow-up, regardless of older enquiries.
            var latestEnquiryPerWorkflow = await _context.InitialEnquiries
                .Where(e => workflowIds.Contains(e.WorkflowId))
                .GroupBy(e => e.WorkflowId)
                .Select(g => g.OrderByDescending(e => e.DateCreated).First())
                .ToListAsync();

            // Only raise a follow-up if the LATEST reply/enquiry is itself stale
            var staleEnquiries = latestEnquiryPerWorkflow
                .Where(e => e.DateCreated <= cutoff)
                .ToList();

            // ── Clean-up pass: dismiss active follow-ups whose latest enquiry is now fresh ──
            // This handles a gap: if a new enquiry is saved via a path that does NOT
            // call DismissActiveForWorkflowAsync (e.g. auto-created from an email task),
            // the active follow-up would persist until the next scan.
            // Here we compare every active follow-up's EnquiryId against the latest
            // enquiry for that workflow. If they differ, a reply has come in and the
            // old follow-up must be dismissed.
            var freshWorkflowIds = latestEnquiryPerWorkflow
                .Where(e => e.DateCreated > cutoff)  // reply within threshold — no follow-up needed
                .Select(e => e.WorkflowId)
                .ToHashSet();

            var staleFollowUps = await _context.WorkflowFollowUps
                .Where(f => !f.IsDismissed && freshWorkflowIds.Contains(f.WorkflowId))
                .ToListAsync();

            if (staleFollowUps.Any())
            {
                foreach (var fu in staleFollowUps)
                {
                    fu.IsDismissed = true;
                    fu.ResolvedDate = DateTime.UtcNow;
                    fu.ResolvedBy = "System";
                    fu.DismissReason = "Replied";
                    fu.Notes = "Auto-dismissed — a more recent enquiry/reply exists for this workflow.";
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation(
                    "GeneratePendingFollowUps: dismissed {Count} stale follow-up(s) — reply found.",
                    staleFollowUps.Count);
            }

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
                fu.DismissReason = "Replied";
                fu.Notes = "Automatically dismissed — a new enquiry/reply was submitted.";
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