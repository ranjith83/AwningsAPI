using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Serialization.Json;

namespace AwningsEmailFunction.Tests.Unit;

public class GraphSubscriptionServiceTests
{
    private static IConfiguration BuildConfig() =>
        new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AzureAd:MonitoredMailbox"]          = "inbox@test.com",
            ["GraphSubscription:NotificationUrl"] = "https://func.azurewebsites.net/api/EmailWatch/notify",
            ["GraphSubscription:ClientState"]     = "AwningsEmailWatcher",
        }).Build();

    private static GraphServiceClient BuildGraphClient(Mock<IRequestAdapter> mockAdapter)
    {
        mockAdapter.SetupProperty(a => a.BaseUrl, "https://graph.microsoft.com/v1.0");
        // Use the real JSON factory so the Graph SDK can serialize request bodies (POST/PATCH)
        mockAdapter.Setup(a => a.SerializationWriterFactory)
                   .Returns(new JsonSerializationWriterFactory());
        return new GraphServiceClient(mockAdapter.Object);
    }

    private static GraphSubscriptionService Build(
        EmailFunctionDbContext ctx,
        GraphServiceClient graphClient,
        IConfiguration? config = null) =>
        new(graphClient, config ?? BuildConfig(),
            NullLogger<GraphSubscriptionService>.Instance, ctx);

    // ── EnsureSubscriptionAsync — still valid ─────────────────────────────────

    [Fact]
    public async Task EnsureSubscription_WhenDbRecordValidAndGraphConfirms_DoesNothing()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.GraphSubscriptions.Add(new GraphSubscription
        {
            SubscriptionId = "sub-123",
            ExpiryDateTime = DateTimeOffset.UtcNow.AddHours(2), // well within buffer
            UpdatedAt      = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var mockAdapter = new Mock<IRequestAdapter>();
        BuildGraphClient(mockAdapter); // configure base props

        // Graph confirms subscription still exists
        mockAdapter.Setup(a => a.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<Subscription>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { Id = "sub-123" });

        var svc = Build(ctx, BuildGraphClient(mockAdapter));
        await svc.EnsureSubscriptionAsync();

        // DB record unchanged, no new subscription created
        ctx.GraphSubscriptions.Should().HaveCount(1);
        ctx.GraphSubscriptions.First().SubscriptionId.Should().Be("sub-123");
    }

    [Fact]
    public async Task EnsureSubscription_WhenDbRecordValidButGraphReturns404_Recreates()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.GraphSubscriptions.Add(new GraphSubscription
        {
            SubscriptionId = "stale-sub",
            ExpiryDateTime = DateTimeOffset.UtcNow.AddHours(2),
            UpdatedAt      = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var mockAdapter = new Mock<IRequestAdapter>();

        // First call (GET to verify) throws — Graph has no such subscription
        // Subsequent call (POST to create) returns a new subscription
        var callCount = 0;
        mockAdapter.Setup(a => a.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<Subscription>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount == 1) throw new Exception("404 Not Found");
                return new Subscription { Id = "new-sub-456", ExpirationDateTime = DateTimeOffset.UtcNow.AddMinutes(4230) };
            });

        // DELETE existing stale subscriptions (SendNoContentAsync)
        mockAdapter.Setup(a => a.SendNoContentAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = Build(ctx, BuildGraphClient(mockAdapter));
        await svc.EnsureSubscriptionAsync();

        // Stale record removed, new subscription saved
        ctx.GraphSubscriptions.Should().HaveCount(1);
        ctx.GraphSubscriptions.First().SubscriptionId.Should().Be("new-sub-456");
    }

    // ── EnsureSubscriptionAsync — within renewal buffer ───────────────────────

    [Fact]
    public async Task EnsureSubscription_WhenWithinRenewalBuffer_RenewsSubscription()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.GraphSubscriptions.Add(new GraphSubscription
        {
            SubscriptionId = "sub-renew",
            ExpiryDateTime = DateTimeOffset.UtcNow.AddMinutes(20), // within 30-min buffer
            UpdatedAt      = DateTime.UtcNow,
        });
        await ctx.SaveChangesAsync();

        var mockAdapter = new Mock<IRequestAdapter>();
        // PATCH renewal returns updated subscription
        mockAdapter.Setup(a => a.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<Subscription>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { Id = "sub-renew", ExpirationDateTime = DateTimeOffset.UtcNow.AddMinutes(4230) });

        var svc = Build(ctx, BuildGraphClient(mockAdapter));
        await svc.EnsureSubscriptionAsync();

        var record = ctx.GraphSubscriptions.First();
        record.ExpiryDateTime.Should().BeAfter(DateTimeOffset.UtcNow.AddHours(1));
    }

    // ── EnsureSubscriptionAsync — no DB record ────────────────────────────────

    [Fact]
    public async Task EnsureSubscription_WhenNoPriorRecord_CreatesNewSubscription()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();

        var mockAdapter = new Mock<IRequestAdapter>();
        mockAdapter.Setup(a => a.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<Subscription>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { Id = "brand-new-sub", ExpirationDateTime = DateTimeOffset.UtcNow.AddMinutes(4230) });

        mockAdapter.Setup(a => a.SendNoContentAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = Build(ctx, BuildGraphClient(mockAdapter));
        await svc.EnsureSubscriptionAsync();

        ctx.GraphSubscriptions.Should().HaveCount(1);
        ctx.GraphSubscriptions.First().SubscriptionId.Should().Be("brand-new-sub");
    }

    // ── GetActiveSubscriptionId ───────────────────────────────────────────────

    [Fact]
    public void GetActiveSubscriptionId_WhenRecordExists_ReturnsId()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        ctx.GraphSubscriptions.Add(new GraphSubscription
        {
            SubscriptionId = "active-sub",
            ExpiryDateTime = DateTimeOffset.UtcNow.AddHours(1),
            UpdatedAt      = DateTime.UtcNow,
        });
        ctx.SaveChanges();

        var mockAdapter = new Mock<IRequestAdapter>();
        var svc = Build(ctx, BuildGraphClient(mockAdapter));

        svc.GetActiveSubscriptionId().Should().Be("active-sub");
    }

    [Fact]
    public void GetActiveSubscriptionId_WhenNoRecord_ReturnsNull()
    {
        using var ctx = EmailFunctionDbContextFactory.Create();
        var mockAdapter = new Mock<IRequestAdapter>();
        var svc = Build(ctx, BuildGraphClient(mockAdapter));

        svc.GetActiveSubscriptionId().Should().BeNull();
    }
}
