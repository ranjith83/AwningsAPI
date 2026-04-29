using AwningsEmailFunction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace AwningsEmailFunction.Functions;

public class GraphWebhookFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<GraphWebhookFunction> _logger;
    private readonly IGraphSubscriptionService _subscriptionService;
    private readonly IServiceScopeFactory _scopeFactory;

    public GraphWebhookFunction(
        IConfiguration configuration,
        ILogger<GraphWebhookFunction> logger,
        IGraphSubscriptionService subscriptionService,
        IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _subscriptionService = subscriptionService;
        _scopeFactory = scopeFactory;
    }

    // GET /api/EmailWatch/notify — Graph sends this first to verify the endpoint is real
    [Function("GraphWebhookValidate")]
    public async Task<HttpResponseData> Validate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "EmailWatch/notify")] HttpRequestData req)
    {
        var qs = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        var validationToken = qs["validationToken"];

        if (!string.IsNullOrEmpty(validationToken))
        {
            _logger.LogInformation("✅ Graph validation handshake received (GET).");
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(validationToken);
            return response;
        }

        return req.CreateResponse(HttpStatusCode.OK);
    }

    // POST /api/EmailWatch/notify — Graph posts change notifications here
    [Function("GraphWebhookNotify")]
    public async Task<HttpResponseData> Notify(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "EmailWatch/notify")] HttpRequestData req)
    {
        // Graph sometimes sends validation as POST too
        var qs = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        var validationToken = qs["validationToken"];
        if (!string.IsNullOrEmpty(validationToken))
        {
            _logger.LogInformation("✅ Graph validation handshake received (POST).");
            var valResponse = req.CreateResponse(HttpStatusCode.OK);
            valResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await valResponse.WriteStringAsync(validationToken);
            return valResponse;
        }

        string body;
        using (var reader = new StreamReader(req.Body))
            body = await reader.ReadToEndAsync();

        _logger.LogInformation("📨 Raw Graph notification body: {Body}", body);

        if (string.IsNullOrWhiteSpace(body))
        {
            _logger.LogWarning("⚠️ Empty notification body.");
            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        // Return 202 immediately — Graph retries if we don't respond within a few seconds
        // Use a new scope so the DbContext isn't disposed when the function invocation ends
        _ = Task.Run(async () =>
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var watchService = scope.ServiceProvider.GetRequiredService<IEmailWatchService>();
                await HandleNotificationAsync(body, watchService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Unhandled exception in background email processing task");
            }
        });

        return req.CreateResponse(HttpStatusCode.Accepted);
    }

    // GET /api/EmailWatch/status — check if subscription is active
    [Function("GraphWebhookStatus")]
    public async Task<HttpResponseData> Status(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "EmailWatch/status")] HttpRequestData req)
    {
        var id = _subscriptionService.GetActiveSubscriptionId();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            watching = !string.IsNullOrEmpty(id),
            subscriptionId = id,
            timestamp = DateTimeOffset.UtcNow
        });
        return response;
    }

    // POST /api/EmailWatch/subscribe — manually trigger subscription creation (useful for local testing)
    [Function("GraphWebhookSubscribe")]
    public async Task<HttpResponseData> Subscribe(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "EmailWatch/subscribe")] HttpRequestData req)
    {
        try
        {
            await _subscriptionService.EnsureSubscriptionAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                message = "Subscription ensured",
                subscriptionId = _subscriptionService.GetActiveSubscriptionId(),
                timestamp = DateTimeOffset.UtcNow
            });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to ensure subscription.");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = ex.Message });
            return response;
        }
    }

    // DELETE /api/EmailWatch/subscribe — remove the active subscription
    [Function("GraphWebhookUnsubscribe")]
    public async Task<HttpResponseData> Unsubscribe(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "EmailWatch/subscribe")] HttpRequestData req)
    {
        await _subscriptionService.DeleteSubscriptionAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { message = "Subscription deleted", timestamp = DateTimeOffset.UtcNow });
        return response;
    }

    private async Task HandleNotificationAsync(string body, IEmailWatchService emailWatchService)
    {
        _logger.LogInformation("🔄 HandleNotificationAsync started — processing background task");

        JObject? payload;
        try { payload = JObject.Parse(body); }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "❌ Failed to parse Graph notification JSON. Body: {Body}", body);
            return;
        }

        var notifications = payload["value"] as JArray;
        if (notifications == null || !notifications.Any())
        {
            _logger.LogWarning("⚠️ Notification payload had no items.");
            return;
        }

        _logger.LogInformation("📬 Graph batch received: {Count} notification(s)", notifications.Count);

        var expectedClientState = _configuration["GraphSubscription:ClientState"] ?? "";

        foreach (var notification in notifications)
        {
            try
            {
                var clientState = notification["clientState"]?.ToString();
                var changeType = notification["changeType"]?.ToString();
                var resource = notification["resource"]?.ToString();
                var subscriptionId = notification["subscriptionId"]?.ToString();

                _logger.LogInformation(
                    "🔔 Notification — subscriptionId: {SubId} | changeType: {ChangeType} | resource: {Resource} | clientState: {ClientState}",
                    subscriptionId, changeType, resource, clientState);

                if (!string.IsNullOrEmpty(expectedClientState) && clientState != expectedClientState)
                {
                    _logger.LogWarning(
                        "⚠️ clientState mismatch — Expected: '{Expected}' | Got: '{Actual}'.",
                        expectedClientState, clientState);
                    // continue; // ← uncomment to enforce strict validation in production
                }

                if (!string.Equals(changeType, "created", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("⏭️ Skipping — changeType '{ChangeType}' is not 'created'.", changeType);
                    continue;
                }

                if (string.IsNullOrEmpty(resource))
                {
                    _logger.LogWarning("⚠️ Notification missing resource field.");
                    continue;
                }

                var messageId = resource.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                if (string.IsNullOrEmpty(messageId))
                {
                    _logger.LogWarning("⚠️ Could not parse messageId from resource: {Resource}", resource);
                    continue;
                }

                _logger.LogInformation("⚡ Triggering processing for messageId: {MessageId}", messageId);

                await emailWatchService.SaveEmailAsync(messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error handling notification for resource: {Resource}",
                    notification["resource"]?.ToString() ?? "unknown");
            }
        }
    }

}
