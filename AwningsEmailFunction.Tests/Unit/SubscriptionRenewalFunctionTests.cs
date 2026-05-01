using Microsoft.Azure.Functions.Worker;

namespace AwningsEmailFunction.Tests.Unit;

public class SubscriptionRenewalFunctionTests
{
    [Fact]
    public async Task Run_CallsEnsureSubscriptionAsync()
    {
        var mockSvc    = new Mock<IGraphSubscriptionService>();
        var fn         = new SubscriptionRenewalFunction(mockSvc.Object, NullLogger<SubscriptionRenewalFunction>.Instance);
        var timerInfo  = new Mock<TimerInfo>();

        await fn.Run(timerInfo.Object);

        mockSvc.Verify(s => s.EnsureSubscriptionAsync(), Times.Once);
    }

    [Fact]
    public async Task Run_WhenEnsureThrows_DoesNotPropagate()
    {
        var mockSvc = new Mock<IGraphSubscriptionService>();
        mockSvc.Setup(s => s.EnsureSubscriptionAsync())
               .ThrowsAsync(new Exception("Graph unavailable"));

        var fn        = new SubscriptionRenewalFunction(mockSvc.Object, NullLogger<SubscriptionRenewalFunction>.Instance);
        var timerInfo = new Mock<TimerInfo>();

        // Should not throw — timer functions must not crash on error
        await fn.Invoking(f => f.Run(timerInfo.Object))
                .Should().NotThrowAsync();
    }
}
