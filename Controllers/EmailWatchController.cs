using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // CRITICAL: Graph posts anonymously — must not require auth
    public class EmailWatchController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailWatchController> _logger;

        public EmailWatchController(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<EmailWatchController> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }

        // POST /api/EmailWatch/notify
        [HttpPost("notify")]
        public async Task<IActionResult> Notify()
        {
            // ── 1. Validation handshake ───────────────────────────────────────
            if (Request.Query.TryGetValue("validationToken", out var validationToken))
            {
                _logger.LogInformation("✅ Graph validation handshake received and echoed.");
                return Content(validationToken.ToString(), "text/plain");
            }

            // ── 2. Read body safely ───────────────────────────────────────────
            Request.EnableBuffering();
            string body;
            using (var reader = new StreamReader(Request.Body, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
            }
            Request.Body.Position = 0;

            // Log raw payload — remove after debugging is done
            _logger.LogInformation("📨 Raw Graph notification body: {Body}", body);

            if (string.IsNullOrWhiteSpace(body))
            {
                _logger.LogWarning("⚠️ Graph notification received but body was empty.");
                return Accepted();
            }

            // Return 202 immediately — Graph retries if we don't respond quickly.
            // Create a new DI scope inside Task.Run so AppDbContext is NOT the
            // disposed request-scoped instance — this fixes ObjectDisposedException.
            _ = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IEmailProcessorService>();
                try { await HandleNotificationAsync(body, processor); }
                catch (Exception ex) { _logger.LogError(ex, "❌ Unhandled error in notification handler."); }
            });

            return Accepted();
        }

        private async Task HandleNotificationAsync(string body, IEmailProcessorService processor)
        {
            JObject? payload;
            try
            {
                payload = JObject.Parse(body);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "❌ Failed to parse Graph notification JSON. Body: {Body}", body);
                return;
            }

            var notifications = payload["value"] as JArray;
            if (notifications == null || !notifications.Any())
            {
                _logger.LogWarning("⚠️ Notification payload had no items. Payload: {Payload}", body);
                return;
            }

            _logger.LogInformation("📬 Graph batch received: {Count} notification(s)", notifications.Count);

            var expectedClientState = _configuration["GraphSubscription:ClientState"] ?? "";

            foreach (var notification in notifications)
            {
                try
                {
                    var receivedClientState = notification["clientState"]?.ToString();
                    var changeType = notification["changeType"]?.ToString();
                    var resource = notification["resource"]?.ToString();
                    var subscriptionId = notification["subscriptionId"]?.ToString();

                    // Log all fields so you can diagnose exactly what arrived
                    _logger.LogInformation(
                        "🔔 Notification — subscriptionId: {SubId} | changeType: {ChangeType} | resource: {Resource} | clientState: {ClientState}",
                        subscriptionId, changeType, resource, receivedClientState);

                    // ── clientState check ─────────────────────────────────────
                    // Logs a warning but does NOT drop the notification during dev.
                    // Uncomment the 'continue' below once confirmed working in prod.
                    if (!string.IsNullOrEmpty(expectedClientState) &&
                        receivedClientState != expectedClientState)
                    {
                        _logger.LogWarning(
                            "⚠️ clientState mismatch — Expected: '{Expected}' | Got: '{Actual}'. " +
                            "Tip: delete and recreate the subscription after changing ClientState in appsettings.",
                            expectedClientState, receivedClientState);

                        // continue; // ← uncomment to enforce strict validation in production
                    }

                    // ── Only handle new messages ──────────────────────────────
                    if (!string.Equals(changeType, "created", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation("⏭️ Skipping — changeType '{ChangeType}' is not 'created'.", changeType);
                        continue;
                    }

                    // ── Extract message ID from resource ──────────────────────
                    // Format: "Users/{userId}/Messages/{messageId}"
                    if (string.IsNullOrEmpty(resource))
                    {
                        _logger.LogWarning("⚠️ Notification missing resource field. Full item: {N}", notification);
                        continue;
                    }

                    var messageId = resource
                        .Split('/', StringSplitOptions.RemoveEmptyEntries)
                        .LastOrDefault();

                    if (string.IsNullOrEmpty(messageId))
                    {
                        _logger.LogWarning("⚠️ Could not parse messageId from resource: {Resource}", resource);
                        continue;
                    }

                    // ── Trigger processing ────────────────────────────────────
                    _logger.LogInformation("⚡ Triggering ProcessSingleEmailAsync for messageId: {MessageId}", messageId);
                    await processor.ProcessSingleEmailAsync(messageId);
                    _logger.LogInformation("✅ ProcessSingleEmailAsync completed for messageId: {MessageId}", messageId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error handling notification for resource: {Resource}",
                        notification["resource"]?.ToString() ?? "unknown");
                }
            }
        }

        // GET /api/EmailWatch/status
        [HttpGet("status")]
        public IActionResult Status([FromServices] IGraphSubscriptionService subscriptionService)
        {
            var id = subscriptionService.GetActiveSubscriptionId();
            return Ok(new { watching = !string.IsNullOrEmpty(id), subscriptionId = id, timestamp = DateTimeOffset.UtcNow });
        }

        // GET /api/EmailWatch/test-log  — confirms controller is reachable
        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            _logger.LogInformation("✅ EmailWatchController is reachable.");
            return Ok(new { message = "EmailWatchController is reachable", timestamp = DateTimeOffset.UtcNow });
        }
    }
}