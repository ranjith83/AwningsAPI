using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;

namespace AwningsEmailFunction.Tests.Unit;

/// <summary>
/// Tests for GraphWebhookFunction.ExtractMessageIds (internal).
/// Verifies the JSON parsing and notification filtering logic without
/// spinning up an HTTP server.
/// </summary>
public class WebhookNotificationParserTests
{
    private static GraphWebhookFunction BuildFunction()
    {
        var mockAdapter = new Mock<IRequestAdapter>();
        mockAdapter.SetupProperty(a => a.BaseUrl, "https://graph.microsoft.com/v1.0");
        mockAdapter.Setup(a => a.SerializationWriterFactory)
                   .Returns(new Mock<ISerializationWriterFactory>().Object);

        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["GraphSubscription:ClientState"] = "AwningsEmailWatcher",
        }).Build();

        return new GraphWebhookFunction(
            config,
            NullLogger<GraphWebhookFunction>.Instance,
            new Mock<IGraphSubscriptionService>().Object);
    }

    private static string MakePayload(string changeType, string resource, string clientState = "AwningsEmailWatcher") => $$"""
        {
          "value": [
            {
              "subscriptionId": "sub-123",
              "changeType": "{{changeType}}",
              "resource": "{{resource}}",
              "clientState": "{{clientState}}"
            }
          ]
        }
        """;

    // ── Valid notification ────────────────────────────────────────────────────

    [Fact]
    public void ExtractMessageIds_ValidCreatedNotification_ReturnsMessageId()
    {
        var fn  = BuildFunction();
        var ids = fn.ExtractMessageIds(MakePayload("created",
            "users/inbox@test.com/mailFolders/inbox/messages/AAMsgId123=="));

        ids.Should().ContainSingle().Which.Should().Be("AAMsgId123==");
    }

    [Fact]
    public void ExtractMessageIds_BatchOfNotifications_ReturnsAllMessageIds()
    {
        var fn      = BuildFunction();
        var payload = """
            {
              "value": [
                { "changeType": "created", "resource": "users/u/mailFolders/inbox/messages/MSG1", "clientState": "AwningsEmailWatcher" },
                { "changeType": "created", "resource": "users/u/mailFolders/inbox/messages/MSG2", "clientState": "AwningsEmailWatcher" }
              ]
            }
            """;

        var ids = fn.ExtractMessageIds(payload);
        ids.Should().BeEquivalentTo(["MSG1", "MSG2"]);
    }

    // ── Non-created changeType skipped ────────────────────────────────────────

    [Fact]
    public void ExtractMessageIds_UpdatedChangeType_ReturnsEmpty()
    {
        var fn  = BuildFunction();
        var ids = fn.ExtractMessageIds(MakePayload("updated",
            "users/u/mailFolders/inbox/messages/MSG1"));

        ids.Should().BeEmpty();
    }

    [Fact]
    public void ExtractMessageIds_DeletedChangeType_ReturnsEmpty()
    {
        var fn  = BuildFunction();
        var ids = fn.ExtractMessageIds(MakePayload("deleted",
            "users/u/mailFolders/inbox/messages/MSG1"));

        ids.Should().BeEmpty();
    }

    // ── Missing or malformed resource ────────────────────────────────────────

    [Fact]
    public void ExtractMessageIds_EmptyResource_ReturnsEmpty()
    {
        var fn      = BuildFunction();
        var payload = """
            {
              "value": [
                { "changeType": "created", "resource": "", "clientState": "AwningsEmailWatcher" }
              ]
            }
            """;

        fn.ExtractMessageIds(payload).Should().BeEmpty();
    }

    [Fact]
    public void ExtractMessageIds_MissingValueArray_ReturnsEmpty()
    {
        var fn = BuildFunction();
        fn.ExtractMessageIds("""{ "other": "data" }""").Should().BeEmpty();
    }

    // ── Bad JSON ──────────────────────────────────────────────────────────────

    [Fact]
    public void ExtractMessageIds_InvalidJson_ReturnsEmpty()
    {
        var fn = BuildFunction();
        fn.ExtractMessageIds("not-json-at-all").Should().BeEmpty();
    }

    // ── Empty body ────────────────────────────────────────────────────────────

    [Fact]
    public void ExtractMessageIds_EmptyString_ReturnsEmpty()
    {
        var fn = BuildFunction();
        fn.ExtractMessageIds("{}").Should().BeEmpty();
    }
}
