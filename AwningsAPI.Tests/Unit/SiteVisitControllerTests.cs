using AwningsAPI.Controllers;
using AwningsAPI.Dto.SiteVisit;
using AwningsAPI.Dto.Tasks;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AwningsAPI.Tests.Unit;

public class SiteVisitControllerTests
{
    private static (SiteVisitController Controller, Mock<ISiteVisitService> SiteVisitService, Mock<ITaskService> TaskService) Build()
    {
        var siteVisitService = new Mock<ISiteVisitService>();
        var taskService = new Mock<ITaskService>();
        var controller = new SiteVisitController(siteVisitService.Object, taskService.Object, NullLogger<SiteVisitController>.Instance);
        return (controller, siteVisitService, taskService);
    }

    private static AwningsAPI.Model.SiteVisit.SiteVisit MakeSiteVisit(int id, int workflowId) => new()
    {
        SiteVisitId = id,
        WorkflowId = workflowId,
        Model = "Test Model",
        DateCreated = DateTime.UtcNow,
        CreatedBy = "tester",
    };

    private static AppTaskDto MakeTaskDto(int taskId, string status = TaskStatusValue.New) => new()
    {
        TaskId = taskId,
        Status = status,
    };

    [Fact]
    public async Task CreateSiteVisit_WithNoPendingTask_CreatesAndCompletesNewTask()
    {
        var (controller, siteVisitService, taskService) = Build();
        var dto = new CreateSiteVisitDto { WorkflowId = 10, CustomerName = "Jane Doe" };

        siteVisitService.Setup(s => s.CreateSiteVisitAsync(dto, It.IsAny<string>()))
            .ReturnsAsync(MakeSiteVisit(1, 10));
        taskService.Setup(t => t.GetPendingSiteVisitTaskByWorkflowIdAsync(10))
            .ReturnsAsync((AppTaskDto?)null);
        taskService.Setup(t => t.CreateTaskAsync(It.IsAny<CreateTaskDto>(), It.IsAny<string>()))
            .ReturnsAsync(MakeTaskDto(100));
        taskService.Setup(t => t.CompleteTaskAsync(100, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(MakeTaskDto(100, TaskStatusValue.Completed));

        var result = await controller.CreateSiteVisit(dto);

        taskService.Verify(t => t.CreateTaskAsync(It.IsAny<CreateTaskDto>(), It.IsAny<string>()), Times.Once);
        taskService.Verify(t => t.StoreSiteVisitLinkAsync(100, 1, It.IsAny<string>()), Times.Once);
        taskService.Verify(t => t.CompleteTaskAsync(100, It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().BeOfType<CreateSiteVisitResponseDto>()
            .Which.TaskId.Should().Be(100);
    }

    [Fact]
    public async Task CreateSiteVisit_WithExistingPendingTask_LinksAndCompletesIt_DoesNotCreateNewTask()
    {
        var (controller, siteVisitService, taskService) = Build();
        var dto = new CreateSiteVisitDto { WorkflowId = 20, CustomerName = "John Smith" };

        siteVisitService.Setup(s => s.CreateSiteVisitAsync(dto, It.IsAny<string>()))
            .ReturnsAsync(MakeSiteVisit(2, 20));
        taskService.Setup(t => t.GetPendingSiteVisitTaskByWorkflowIdAsync(20))
            .ReturnsAsync(MakeTaskDto(200));
        taskService.Setup(t => t.CompleteTaskAsync(200, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(MakeTaskDto(200, TaskStatusValue.Completed));

        var result = await controller.CreateSiteVisit(dto);

        taskService.Verify(t => t.CreateTaskAsync(It.IsAny<CreateTaskDto>(), It.IsAny<string>()), Times.Never);
        taskService.Verify(t => t.StoreSiteVisitLinkAsync(200, 2, It.IsAny<string>()), Times.Once);
        taskService.Verify(t => t.CompleteTaskAsync(200, It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().BeOfType<CreateSiteVisitResponseDto>()
            .Which.TaskId.Should().Be(200);
    }

    [Fact]
    public async Task GetScheduledSiteVisits_ReturnsPagedItemsFromSiteVisitService()
    {
        var (controller, siteVisitService, _) = Build();
        var items = new List<ScheduledShowroomInviteDto>
        {
            new() { ShowroomInviteId = 1, WorkflowId = 30, CustomerName = "Alice" }
        };
        siteVisitService.Setup(s => s.GetUpcomingShowroomInvitesAsync(1, 20))
            .ReturnsAsync((items, 1));

        var result = await controller.GetScheduledSiteVisits(1, 20);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(new
        {
            page = 1,
            pageSize = 20,
            totalCount = 1,
            totalPages = 1,
            items
        });
    }

    [Fact]
    public async Task GetPendingCount_ReturnsCountFromSiteVisitService()
    {
        var (controller, siteVisitService, _) = Build();
        siteVisitService.Setup(s => s.GetUpcomingShowroomInviteCountAsync()).ReturnsAsync(4);

        var result = await controller.GetPendingCount();

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeOfType<SiteVisitPendingCountDto>()
            .Which.Count.Should().Be(4);
    }
}
