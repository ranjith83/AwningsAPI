using AwningsAPI.Model.Workflow;
using AwningsAPI.Services.WorkflowService;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace AwningsAPI.Tests.Unit;

public class FollowUpServiceTests
{
    private static FollowUpService Build(AwningsAPI.Database.AppDbContext ctx) =>
        new(ctx, NullLogger<FollowUpService>.Instance);

    private static WorkflowStart MakeWorkflow(int id) =>
        new()
        {
            WorkflowId    = id,
            WorkflowName  = $"Workflow {id}",
            Description   = "Test",
            CustomerId    = 100,
            SupplierId    = 1,
            ProductId     = 1,
            ProductTypeId = 1,
            DateCreated   = DateTime.UtcNow,
            CreatedBy     = "tester",
        };

    private static InitialEnquiry MakeEnquiry(int id, int workflowId, DateTime dateCreated) =>
        new()
        {
            EnquiryId   = id,
            WorkflowId  = workflowId,
            Comments    = $"Enquiry {id}",
            Email       = $"e{id}@test.com",
            DateCreated = dateCreated,
            CreatedBy   = "tester",
        };

    private static WorkflowFollowUp MakeFollowUp(int id, int workflowId, int enquiryId, bool dismissed = false) =>
        new()
        {
            FollowUpId      = id,
            WorkflowId      = workflowId,
            EnquiryId       = enquiryId,
            LastEnquiryDate = DateTime.UtcNow.AddDays(-5),
            Subject         = $"Follow-up {id}",
            IsDismissed     = dismissed,
            DateAdded       = DateTime.UtcNow,
            CreatedBy       = "System",
        };

    // ── DismissActiveForWorkflow ──────────────────────────────────────────────

    [Fact]
    public async Task DismissActiveForWorkflowAsync_WithActiveFollowUps_DismissesAllAndSetsReason()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowFollowUps.AddRange(
            MakeFollowUp(1, workflowId: 1, enquiryId: 10),
            MakeFollowUp(2, workflowId: 1, enquiryId: 11));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        await svc.DismissActiveForWorkflowAsync(1, "testuser");

        var all = ctx.WorkflowFollowUps.Where(f => f.WorkflowId == 1).ToList();
        all.Should().AllSatisfy(f =>
        {
            f.IsDismissed.Should().BeTrue();
            f.ResolvedBy.Should().Be("testuser");
            f.DismissReason.Should().Be("Replied");
        });
    }

    [Fact]
    public async Task DismissActiveForWorkflowAsync_WhenNoActiveFollowUps_DoesNothing()
    {
        using var ctx = DbContextFactory.Create();
        // Already dismissed
        ctx.WorkflowFollowUps.Add(MakeFollowUp(1, workflowId: 1, enquiryId: 10, dismissed: true));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);

        // Should complete without exception
        await svc.DismissActiveForWorkflowAsync(1, "testuser");

        // Still dismissed, ResolvedBy unchanged
        ctx.WorkflowFollowUps.Find(1)!.ResolvedBy.Should().BeNull();
    }

    [Fact]
    public async Task DismissActiveForWorkflowAsync_OnlyDismissesTargetWorkflow()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowFollowUps.AddRange(
            MakeFollowUp(1, workflowId: 1, enquiryId: 10),  // target
            MakeFollowUp(2, workflowId: 2, enquiryId: 20)); // other workflow
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        await svc.DismissActiveForWorkflowAsync(1, "testuser");

        ctx.WorkflowFollowUps.Find(1)!.IsDismissed.Should().BeTrue();
        ctx.WorkflowFollowUps.Find(2)!.IsDismissed.Should().BeFalse();
    }

    // ── DismissFollowUp (manual) ──────────────────────────────────────────────

    [Fact]
    public async Task DismissFollowUpAsync_WithValidId_MarksAsDismissedAndReturnsTrue()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowFollowUps.Add(MakeFollowUp(1, workflowId: 1, enquiryId: 10));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.DismissFollowUpAsync(1, "Called customer — not interested", "agent");

        result.Should().BeTrue();
        var fu = ctx.WorkflowFollowUps.Find(1)!;
        fu.IsDismissed.Should().BeTrue();
        fu.DismissReason.Should().Be("UserDismiss");
        fu.Notes.Should().Contain("not interested");
        fu.ResolvedBy.Should().Be("agent");
    }

    [Fact]
    public async Task DismissFollowUpAsync_WithInvalidId_ReturnsFalse()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        var result = await svc.DismissFollowUpAsync(999, "notes", "agent");

        result.Should().BeFalse();
    }

    // ── GeneratePendingFollowUps ──────────────────────────────────────────────

    [Fact]
    public async Task GeneratePendingFollowUpsAsync_WithStaleEnquiryAndNoQuote_CreatesFollowUp()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-5)));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var count = await svc.GeneratePendingFollowUpsAsync();

        count.Should().Be(1);
        ctx.WorkflowFollowUps.Should().HaveCount(1);
        ctx.WorkflowFollowUps.First().WorkflowId.Should().Be(1);
    }

    [Fact]
    public async Task GeneratePendingFollowUpsAsync_WithRecentEnquiry_DoesNotCreateFollowUp()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-1)));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var count = await svc.GeneratePendingFollowUpsAsync();

        count.Should().Be(0);
        ctx.WorkflowFollowUps.Should().BeEmpty();
    }

    [Fact]
    public async Task GeneratePendingFollowUpsAsync_WhenWorkflowAlreadyHasQuote_SkipsWorkflow()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-5)));
        ctx.Quotes.Add(new Quote
        {
            WorkflowId   = 1,
            QuoteNumber  = "Q-001",
            QuoteDate    = DateTime.UtcNow,
            FollowUpDate = DateTime.UtcNow.AddDays(7),
            CustomerId   = 100,
            Notes        = "",
            Terms        = "",
            CreatedBy    = "tester",
        });
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var count = await svc.GeneratePendingFollowUpsAsync();

        count.Should().Be(0);
    }

    [Fact]
    public async Task GeneratePendingFollowUpsAsync_WhenFollowUpAlreadyExists_DoesNotDuplicate()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-5)));
        ctx.WorkflowFollowUps.Add(MakeFollowUp(1, workflowId: 1, enquiryId: 1));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var count = await svc.GeneratePendingFollowUpsAsync();

        count.Should().Be(0);
        ctx.WorkflowFollowUps.Should().HaveCount(1);
    }

    [Fact]
    public async Task GeneratePendingFollowUpsAsync_WithRecentReply_AutoDismissesStaleFollowUp()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        // Old enquiry (stale)
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-5)));
        // New reply (recent) — same workflow
        ctx.InitialEnquiries.Add(MakeEnquiry(2, workflowId: 1, dateCreated: DateTime.UtcNow.AddDays(-1)));
        // Active follow-up referencing the old enquiry
        ctx.WorkflowFollowUps.Add(MakeFollowUp(1, workflowId: 1, enquiryId: 1));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        await svc.GeneratePendingFollowUpsAsync();

        // The existing follow-up should be auto-dismissed because the latest enquiry is recent
        ctx.WorkflowFollowUps.Find(1)!.IsDismissed.Should().BeTrue();
        ctx.WorkflowFollowUps.Find(1)!.DismissReason.Should().Be("Replied");
    }

    // ── GetActiveFollowUps ────────────────────────────────────────────────────

    [Fact]
    public async Task GetActiveFollowUpsAsync_ReturnsOnlyNonDismissed()
    {
        using var ctx = DbContextFactory.Create();
        ctx.WorkflowFollowUps.AddRange(
            MakeFollowUp(1, workflowId: 1, enquiryId: 10, dismissed: false),
            MakeFollowUp(2, workflowId: 2, enquiryId: 20, dismissed: true));
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.GetActiveFollowUpsAsync();

        result.Should().HaveCount(1);
        result[0].FollowUpId.Should().Be(1);
    }
}
