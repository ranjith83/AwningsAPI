using Microsoft.Extensions.Configuration;

namespace AwningsEmailFunction.Tests.Unit;

public class EmailWatchServiceTests
{
    private static IConfiguration BuildConfig(string? mailbox = "inbox@test.com") =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AzureAd:OrganizerEmail"] = mailbox,
        }).Build();

    // ── SaveEmailAsync delegates to processor ─────────────────────────────────

    [Fact]
    public async Task SaveEmailAsync_CallsProcessorWithCorrectMailbox()
    {
        var mockProcessor = new Mock<IEmailProcessorService>();
        var svc = new EmailWatchService(mockProcessor.Object, BuildConfig(), NullLogger<EmailWatchService>.Instance);

        await svc.SaveEmailAsync("msg-001");

        mockProcessor.Verify(p => p.ProcessIncomingEmailAsync("msg-001", "inbox@test.com"), Times.Once);
    }

    [Fact]
    public async Task SaveEmailAsync_WhenMailboxNotConfigured_Throws()
    {
        var mockProcessor = new Mock<IEmailProcessorService>();
        var svc = new EmailWatchService(mockProcessor.Object, BuildConfig(null), NullLogger<EmailWatchService>.Instance);

        await svc.Invoking(s => s.SaveEmailAsync("msg-001"))
                 .Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("*OrganizerEmail*");
    }
}
