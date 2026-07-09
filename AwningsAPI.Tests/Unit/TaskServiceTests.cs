using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Tasks;
using AwningsAPI.Services.Tasks;
using AwningsAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AwningsAPI.Tests.Unit;

public class TaskServiceTests
{
    private static TaskService Build(AwningsAPI.Database.AppDbContext ctx)
    {
        var autoReply = new Mock<IEmailAutoReplyService>();
        var config = new ConfigurationBuilder().Build();

        return new TaskService(ctx, NullLogger<TaskService>.Instance, autoReply.Object, config);
    }

    // ── GetPendingSiteVisitTaskByWorkflowIdAsync ────────────────────────────────

    [Fact]
    public async Task GetPendingSiteVisitTaskByWorkflowIdAsync_WhenNoTaskExists_ReturnsNull()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        var result = await svc.GetPendingSiteVisitTaskByWorkflowIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPendingSiteVisitTaskByWorkflowIdAsync_WhenPendingTaskExists_ReturnsIt()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Tasks.Add(new AppTask
        {
            WorkflowId = 5,
            SourceType = TaskSourceType.SiteVisit.ToString(),
            Status = TaskStatusValue.New,
            DateCreated = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.GetPendingSiteVisitTaskByWorkflowIdAsync(5);

        result.Should().NotBeNull();
        result!.WorkflowId.Should().Be(5);
    }

    [Fact]
    public async Task GetPendingSiteVisitTaskByWorkflowIdAsync_IgnoresCompletedTasks()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Tasks.Add(new AppTask
        {
            WorkflowId = 7,
            SourceType = TaskSourceType.SiteVisit.ToString(),
            Status = TaskStatusValue.Completed,
            DateCreated = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.GetPendingSiteVisitTaskByWorkflowIdAsync(7);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPendingSiteVisitTaskByWorkflowIdAsync_IgnoresOtherSourceTypes()
    {
        using var ctx = DbContextFactory.Create();
        ctx.Tasks.Add(new AppTask
        {
            WorkflowId = 9,
            SourceType = TaskSourceType.Email.ToString(),
            Status = TaskStatusValue.New,
            DateCreated = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var svc = Build(ctx);
        var result = await svc.GetPendingSiteVisitTaskByWorkflowIdAsync(9);

        result.Should().BeNull();
    }

    // ── CreateTaskAsync + CompleteTaskAsync (used by SiteVisitController) ──────

    [Fact]
    public async Task CreateTaskAsync_ThenCompleteTaskAsync_SetsStatusCompletedWithDate()
    {
        using var ctx = DbContextFactory.Create();
        var svc = Build(ctx);

        var created = await svc.CreateTaskAsync(new CreateTaskDto
        {
            SourceType = TaskSourceType.SiteVisit.ToString(),
            Title = "Site Visit – Test",
            Category = "Site Visit",
            WorkflowId = 10,
        }, "tester");

        created.Status.Should().Be(TaskStatusValue.New);

        var completed = await svc.CompleteTaskAsync(created.TaskId, "Site survey completed", "tester");

        completed.Status.Should().Be(TaskStatusValue.Completed);
        completed.CompletedDate.Should().NotBeNull();
    }
}
