using AwningsAPI.Dto.Workflow;
using AwningsAPI.Model.Workflow;
using AwningsAPI.Services.WorkflowService;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace AwningsAPI.Tests.Unit;

public class WorkflowServiceTests
{
    // ── Factory helpers ───────────────────────────────────────────────────────

    private static (WorkflowService svc, AwningsAPI.Database.AppDbContext ctx) Build(string? db = null)
    {
        var ctx     = DbContextFactory.Create(db);
        var followUp = new FollowUpService(ctx, NullLogger<FollowUpService>.Instance);
        var svc     = new WorkflowService(ctx, followUp);
        return (svc, ctx);
    }

    private static WorkflowStart MakeWorkflow(int id = 1) =>
        new()
        {
            WorkflowId   = id,
            WorkflowName = $"Workflow {id}",
            Description  = "Test",
            CustomerId   = 100,
            SupplierId   = 1,
            ProductId    = 1,
            ProductTypeId = 1,
            DateCreated  = DateTime.UtcNow,
            CreatedBy    = "tester",
        };

    private static InitialEnquiry MakeEnquiry(int id, int workflowId, bool isDeleted = false) =>
        new()
        {
            EnquiryId   = id,
            WorkflowId  = workflowId,
            Comments    = $"Enquiry {id}",
            Email       = $"e{id}@test.com",
            DateCreated = DateTime.UtcNow,
            CreatedBy   = "tester",
            IsDeleted   = isDeleted,
            DeletedAt   = isDeleted ? DateTime.UtcNow : null,
            DeletedBy   = isDeleted ? "tester" : null,
        };

    // ── GetInitialEnquiryForWorkflow ──────────────────────────────────────────

    [Fact]
    public async Task GetInitialEnquiryForWorkflowAsync_ReturnsOnlyMatchingWorkflow()
    {
        var (svc, ctx) = Build();
        ctx.InitialEnquiries.AddRange(
            MakeEnquiry(1, workflowId: 10),
            MakeEnquiry(2, workflowId: 20));
        await ctx.SaveChangesAsync();

        var result = (await svc.GetInitialEnquiryForWorkflowAsync(10)).ToList();

        result.Should().HaveCount(1);
        result[0].EnquiryId.Should().Be(1);
    }

    [Fact]
    public async Task GetInitialEnquiryForWorkflowAsync_ExcludesSoftDeletedRecords()
    {
        var (svc, ctx) = Build();
        ctx.InitialEnquiries.AddRange(
            MakeEnquiry(1, workflowId: 1),                    // active
            MakeEnquiry(2, workflowId: 1, isDeleted: true));  // soft-deleted
        await ctx.SaveChangesAsync();

        var result = (await svc.GetInitialEnquiryForWorkflowAsync(1)).ToList();

        result.Should().HaveCount(1);
        result[0].EnquiryId.Should().Be(1);
    }

    [Fact]
    public async Task GetInitialEnquiryForWorkflowAsync_WhenNoEnquiries_ReturnsEmpty()
    {
        var (svc, _) = Build();

        var result = await svc.GetInitialEnquiryForWorkflowAsync(999);

        result.Should().BeEmpty();
    }

    // ── AddInitialEnquiry ─────────────────────────────────────────────────────

    [Fact]
    public async Task AddInitialEnquiry_PersistsEnquiryWithAuditFields()
    {
        var (svc, ctx) = Build();

        var dto = new InitialEnquiryDto
        {
            WorkflowId = 1,
            Comments   = "New enquiry",
            Email      = "customer@test.com",
        };

        var result = await svc.AddInitialEnquiry(dto, "testuser");

        result.EnquiryId.Should().BeGreaterThan(0);
        result.CreatedBy.Should().Be("testuser");
        result.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        ctx.InitialEnquiries.Count().Should().Be(1);
    }

    [Fact]
    public async Task AddInitialEnquiry_DismissesActiveFollowUps()
    {
        var (svc, ctx) = Build();

        // Seed an active follow-up for the same workflow
        ctx.WorkflowFollowUps.Add(new WorkflowFollowUp
        {
            WorkflowId      = 1,
            EnquiryId       = 99,
            LastEnquiryDate = DateTime.UtcNow.AddDays(-5),
            Subject         = "Old follow-up",
            IsDismissed     = false,
            DateAdded       = DateTime.UtcNow,
            CreatedBy       = "System",
        });
        await ctx.SaveChangesAsync();

        await svc.AddInitialEnquiry(new InitialEnquiryDto { WorkflowId = 1, Comments = "Reply", Email = "c@test.com" }, "testuser");

        var followUp = ctx.WorkflowFollowUps.First();
        followUp.IsDismissed.Should().BeTrue();
        followUp.DismissReason.Should().Be("Replied");
    }

    // ── UpdateInitialEnquiry ──────────────────────────────────────────────────

    [Fact]
    public async Task UpdateInitialEnquiry_WhenFound_UpdatesEditableFields()
    {
        var (svc, ctx) = Build();
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1));
        await ctx.SaveChangesAsync();

        var dto = new InitialEnquiryDto
        {
            EnquiryId = 1,
            WorkflowId = 1,
            Comments  = "Updated comment",
            Email     = "updated@test.com",
        };

        var result = await svc.UpdateInitialEnquiry(dto, "editor");

        result.Comments.Should().Be("Updated comment");
        result.Email.Should().Be("updated@test.com");
        result.UpdatedBy.Should().Be("editor");
        result.DateUpdated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task UpdateInitialEnquiry_WhenNotFound_ThrowsException()
    {
        var (svc, _) = Build();

        await svc.Invoking(s => s.UpdateInitialEnquiry(
                new InitialEnquiryDto { EnquiryId = 999, WorkflowId = 1, Comments = "x", Email = "x@x.com" }, "user"))
            .Should().ThrowAsync<Exception>()
            .WithMessage("*Enquiry not found*");
    }

    // ── DeleteInitialEnquiry (soft delete) ────────────────────────────────────

    [Fact]
    public async Task DeleteInitialEnquiryAsync_WhenExists_SoftDeletesWithAuditFields()
    {
        var (svc, ctx) = Build();
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1));
        await ctx.SaveChangesAsync();

        var result = await svc.DeleteInitialEnquiryAsync(1, "admin");

        result.Should().BeTrue();

        // Bypass the query filter to inspect the soft-deleted row
        var row = await ctx.InitialEnquiries
            .IgnoreQueryFilters()
            .SingleAsync(e => e.EnquiryId == 1);

        row.IsDeleted.Should().BeTrue();
        row.DeletedBy.Should().Be("admin");
        row.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteInitialEnquiryAsync_AfterDelete_NotReturnedByGet()
    {
        var (svc, ctx) = Build();
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1));
        await ctx.SaveChangesAsync();

        await svc.DeleteInitialEnquiryAsync(1, "admin");

        var enquiries = await svc.GetInitialEnquiryForWorkflowAsync(1);
        enquiries.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteInitialEnquiryAsync_WhenNotFound_ReturnsFalse()
    {
        var (svc, _) = Build();

        var result = await svc.DeleteInitialEnquiryAsync(999, "admin");

        result.Should().BeFalse();
    }

    // ── DeleteWorkflow ────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteWorkflowAsync_WhenWorkflowNotFound_ReturnsNotDeleted()
    {
        var (svc, _) = Build();

        var result = await svc.DeleteWorkflowAsync(999);

        result.Deleted.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }

    [Fact]
    public async Task DeleteWorkflowAsync_WhenActiveEnquiriesExist_ReturnsBlockedResult()
    {
        var (svc, ctx) = Build();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1)); // active
        await ctx.SaveChangesAsync();

        var result = await svc.DeleteWorkflowAsync(1);

        result.Deleted.Should().BeFalse();
        result.BlockingDependencies.Should().Contain(d => d.Name == "Initial Enquiry");
    }

    [Fact]
    public async Task DeleteWorkflowAsync_WhenAllEnquiriesSoftDeleted_AllowsDeletion()
    {
        var (svc, ctx) = Build();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1, isDeleted: true)); // soft-deleted
        await ctx.SaveChangesAsync();

        var result = await svc.DeleteWorkflowAsync(1);

        result.Deleted.Should().BeTrue();
        ctx.WorkflowStarts.Any(w => w.WorkflowId == 1).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteWorkflowAsync_WithNoData_DeletesSuccessfully()
    {
        var (svc, ctx) = Build();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        await ctx.SaveChangesAsync();

        var result = await svc.DeleteWorkflowAsync(1);

        result.Deleted.Should().BeTrue();
        ctx.WorkflowStarts.Any(w => w.WorkflowId == 1).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteWorkflowAsync_WithMultipleDependencies_ListsAllInResponse()
    {
        var (svc, ctx) = Build();
        ctx.WorkflowStarts.Add(MakeWorkflow(1));
        ctx.InitialEnquiries.Add(MakeEnquiry(1, workflowId: 1));
        ctx.Quotes.Add(new AwningsAPI.Model.Workflow.Quote
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

        var result = await svc.DeleteWorkflowAsync(1);

        result.Deleted.Should().BeFalse();
        result.BlockingDependencies.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
