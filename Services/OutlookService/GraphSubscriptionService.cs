using AwningsAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    /// <summary>
    /// Manages a Microsoft Graph change-notification subscription on the monitored mailbox.
    /// When a new message arrives Graph POSTs a notification to /api/EmailWatch/notify,
    /// which triggers immediate processing — no polling required.
    /// </summary>
    public class GraphSubscriptionService : IGraphSubscriptionService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GraphSubscriptionService> _logger;

        // In-memory store for active subscription ID and expiry.
        // Replace with DB/cache storage for multi-instance deployments.
        private static string? _subscriptionId;
        private static DateTimeOffset _subscriptionExpiry = DateTimeOffset.MinValue;

        // Graph subscriptions expire after a maximum of 4230 minutes (~3 days) for mail.
        // We renew 30 minutes before expiry to avoid any gap.
        private static readonly TimeSpan RenewalBuffer = TimeSpan.FromMinutes(30);

        public GraphSubscriptionService(
            GraphServiceClient graphClient,
            IConfiguration configuration,
            ILogger<GraphSubscriptionService> logger)
        {
            _graphClient = graphClient;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Creates or renews the inbox subscription.
        /// Call this from a background service on startup and periodically.
        /// </summary>
        public async Task EnsureSubscriptionAsync()
        {
            try
            {
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new InvalidOperationException("Monitored mailbox not configured.");

                // Notification URL must be publicly reachable (HTTPS).
                // Use ngrok / Azure App Service URL in development.
                var notificationUrl = _configuration["GraphSubscription:NotificationUrl"]
                    ?? throw new InvalidOperationException(
                        "GraphSubscription:NotificationUrl not configured. " +
                        "Set it to your public HTTPS endpoint, e.g. https://yourapp.azurewebsites.net/api/EmailWatch/notify");

                var clientState = _configuration["GraphSubscription:ClientState"] ?? "AwningsEmailWatcher";

                // Renew if subscription is still active but close to expiry
                if (!string.IsNullOrEmpty(_subscriptionId) &&
                    DateTimeOffset.UtcNow < _subscriptionExpiry - RenewalBuffer)
                {
                    _logger.LogInformation(
                        "Graph subscription {Id} is still valid (expires {Expiry}). No action needed.",
                        _subscriptionId, _subscriptionExpiry);
                    return;
                }

                // Renew existing subscription when it exists but is near expiry
                if (!string.IsNullOrEmpty(_subscriptionId))
                {
                    _logger.LogInformation("Renewing Graph subscription {Id}...", _subscriptionId);

                    var newExpiry = DateTimeOffset.UtcNow.AddMinutes(4230);

                    await _graphClient.Subscriptions[_subscriptionId]
                        .PatchAsync(new Subscription { ExpirationDateTime = newExpiry });

                    _subscriptionExpiry = newExpiry;
                    _logger.LogInformation("Graph subscription renewed until {Expiry}.", _subscriptionExpiry);
                    return;
                }

                // ── Clean up ALL stale subscriptions before creating a new one ──
                // Previous app restarts leave orphaned subscriptions in Graph that
                // all fire for the same email, causing duplicate processing.
                _logger.LogInformation("Cleaning up any stale Graph subscriptions before creating new one...");
                await DeleteAllSubscriptionsAsync();

                // Create a brand-new subscription
                _logger.LogInformation("Creating new Graph mail subscription for {Mailbox}...", mailboxEmail);

                var subscription = new Subscription
                {
                    ChangeType = "created",                                      // Fire on new emails only
                    NotificationUrl = notificationUrl,
                    Resource = $"users/{mailboxEmail}/mailFolders/inbox/messages", // Watch Inbox
                    ExpirationDateTime = DateTimeOffset.UtcNow.AddMinutes(4230),
                    ClientState = clientState,                                   // Validated on every notification
                    LatestSupportedTlsVersion = "v1_2"
                };

                var created = await _graphClient.Subscriptions.PostAsync(subscription);

                _subscriptionId = created?.Id
                    ?? throw new Exception("Graph returned a null subscription ID.");
                _subscriptionExpiry = created.ExpirationDateTime
                    ?? DateTimeOffset.UtcNow.AddMinutes(4230);

                _logger.LogInformation(
                    "Graph subscription created. ID={Id}, Expires={Expiry}",
                    _subscriptionId, _subscriptionExpiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create/renew Graph mail subscription.");
                throw;
            }
        }

        /// <summary>Returns the active subscription ID, or null if none exists.</summary>
        public string? GetActiveSubscriptionId() => _subscriptionId;

        /// <summary>Deletes the active subscription (e.g. on app shutdown).</summary>
        public async Task DeleteSubscriptionAsync()
        {
            if (string.IsNullOrEmpty(_subscriptionId)) return;

            try
            {
                await _graphClient.Subscriptions[_subscriptionId].DeleteAsync();
                _logger.LogInformation("Graph subscription {Id} deleted.", _subscriptionId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not delete Graph subscription {Id}.", _subscriptionId);
            }
            finally
            {
                _subscriptionId = null;
                _subscriptionExpiry = DateTimeOffset.MinValue;
            }
        }

        /// <summary>
        /// Fetches ALL active subscriptions from Graph and deletes every one.
        /// Called on startup to remove orphaned subscriptions left by previous
        /// app restarts, which would otherwise cause duplicate notifications.
        /// </summary>
        private async Task DeleteAllSubscriptionsAsync()
        {
            try
            {
                var existing = await _graphClient.Subscriptions.GetAsync();

                if (existing?.Value == null || !existing.Value.Any())
                {
                    _logger.LogInformation("No stale Graph subscriptions found.");
                    _subscriptionId = null;
                    _subscriptionExpiry = DateTimeOffset.MinValue;
                    return;
                }

                _logger.LogInformation(
                    "Found {Count} stale subscription(s) — deleting all before registering new one.",
                    existing.Value.Count);

                foreach (var sub in existing.Value)
                {
                    try
                    {
                        await _graphClient.Subscriptions[sub.Id].DeleteAsync();
                        _logger.LogInformation("🗑️ Deleted stale subscription {Id}.", sub.Id);
                    }
                    catch (Exception ex)
                    {
                        // Log but continue — a failed delete shouldn't block startup
                        _logger.LogWarning(ex, "Could not delete stale subscription {Id}.", sub.Id);
                    }
                }

                _subscriptionId = null;
                _subscriptionExpiry = DateTimeOffset.MinValue;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching stale subscriptions — proceeding with new subscription anyway.");
                _subscriptionId = null;
                _subscriptionExpiry = DateTimeOffset.MinValue;
            }
        }
    }
}