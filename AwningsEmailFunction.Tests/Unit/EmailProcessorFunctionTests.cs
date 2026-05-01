namespace AwningsEmailFunction.Tests.Unit;

public class EmailProcessorFunctionTests
{
    [Fact]
    public async Task Run_CallsSaveEmailAsyncWithMessageId()
    {
        var mockWatchSvc = new Mock<IEmailWatchService>();
        var fn           = new EmailProcessorFunction(mockWatchSvc.Object, NullLogger<EmailProcessorFunction>.Instance);

        await fn.Run("msg-queue-001");

        mockWatchSvc.Verify(s => s.SaveEmailAsync("msg-queue-001"), Times.Once);
    }

    [Fact]
    public async Task Run_WhenSaveEmailThrows_PropagatesForRetry()
    {
        var mockWatchSvc = new Mock<IEmailWatchService>();
        mockWatchSvc.Setup(s => s.SaveEmailAsync(It.IsAny<string>()))
                    .ThrowsAsync(new Exception("DB unavailable"));

        var fn = new EmailProcessorFunction(mockWatchSvc.Object, NullLogger<EmailProcessorFunction>.Instance);

        // Must propagate — Azure Functions retries queue messages on exception
        await fn.Invoking(f => f.Run("msg-fail"))
                .Should().ThrowAsync<Exception>()
                .WithMessage("DB unavailable");
    }
}
