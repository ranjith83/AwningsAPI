using AwningsEmailFunction.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AwningsEmailFunction.Services;

public class GraphSubscriptionService : IGraphSubscriptionService
{
    private readonly GraphServiceClient _graphClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GraphSubscriptionService> _logger;

    private static string? _subscriptionId;
    private static DateTimeOffset _subscriptionExpiry = DateTimeOffset.MinValue;
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

    public async Task EnsureSubscriptionAsync()
    {
        try
        {
            var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                ?? _configuration["AzureAd:OrganizerEmail"]
                ?? throw new InvalidOperationException("Monitored mailbox not configured.");

            var notificationUrl = _configuration["GraphSubscription:NotificationUrl"]
                ?? throw new InvalidOperationException(
                    "GraphSubscription:NotificationUrl not configured.");

            var clientState = _configuration["GraphSubscription:ClientState"] ?? "AwningsEmailWatcher";

            // ── Recover from restart: static fields are null but subscription may still exist in Graph ──
            if (string.IsNullOrEmpty(_subscriptionId))
            {
                await TryRestoreFromGraphAsync(notificationUrl);
            }

            // Still valid — nothing to do
            if (!string.IsNullOrEmpty(_subscriptionId) &&
                DateTimeOffset.UtcNow < _subscriptionExpiry - RenewalBuffer)
            {
                _logger.LogInformation(
                    "Graph subscription {Id} is still valid (expires {Expiry}). No action needed.",
                    _subscriptionId, _subscriptionExpiry);
                return;
            }

            // Renew existing
            if (!string.IsNullOrEmpty(_subscriptionId))
            {
                try
                {
                    _logger.LogInformation("Renewing Graph subscription {Id}...", _subscriptionId);
                    var newExpiry = DateTimeOffset.UtcNow.AddMinutes(4230);
                    await _graphClient.Subscriptions[_subscriptionId]
                        .PatchAsync(new Subscription { ExpirationDateTime = newExpiry });
                    _subscriptionExpiry = newExpiry;
                    _logger.LogInformation("Graph subscription renewed until {Expiry}.", _subscriptionExpiry);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not renew subscription {Id} — will recreate.", _subscriptionId);
                    _subscriptionId = null;
                    _subscriptionExpiry = DateTimeOffset.MinValue;
                }
            }

            // Create new
            _logger.LogInformation("Cleaning up stale Graph subscriptions before creating new one...");
            await DeleteAllSubscriptionsAsync();

            _logger.LogInformation("Creating new Graph mail subscription for {Mailbox}...", mailboxEmail);

            var subscription = new Subscription
            {
                ChangeType = "created",
                NotificationUrl = notificationUrl,
                Resource = $"users/{mailboxEmail}/mailFolders/inbox/messages",
                ExpirationDateTime = DateTimeOffset.UtcNow.AddMinutes(4230),
                ClientState = clientState,
                LatestSupportedTlsVersion = "v1_2"
            };

            var created = await _graphClient.Subscriptions.PostAsync(subscription);

            _subscriptionId = created?.Id ?? throw new Exception("Graph returned a null subscription ID.");
            _subscriptionExpiry = created.ExpirationDateTime ?? DateTimeOffset.UtcNow.AddMinutes(4230);

            _logger.LogInformation("Graph subscription created. ID={Id}, Expires={Expiry}",
                _subscriptionId, _subscriptionExpiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create/renew Graph mail subscription.");
            throw;
        }
    }

    public string? GetActiveSubscriptionId() => _subscriptionId;

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
    /// Called when _subscriptionId is null (e.g. after a restart).
    /// Queries Graph to find an existing valid subscription matching our notification URL
    /// and restores the in-memory state — avoiding unnecessary deletion and recreation.
    /// </summary>
    private async Task TryRestoreFromGraphAsync(string notificationUrl)
    {
        try
        {
            _logger.LogInformation("Static subscription state is empty — querying Graph to restore...");

            var existing = await _graphClient.Subscriptions.GetAsync();
            if (existing?.Value == null || !existing.Value.Any())
            {
                _logger.LogInformation("No existing Graph subscriptions found.");
                return;
            }

            var valid = existing.Value.FirstOrDefault(s =>
                s.NotificationUrl == notificationUrl &&
                s.ExpirationDateTime > DateTimeOffset.UtcNow + RenewalBuffer);

            if (valid != null)
            {
                _subscriptionId = valid.Id;
                _subscriptionExpiry = valid.ExpirationDateTime ?? DateTimeOffset.MinValue;
                _logger.LogInformation(
                    "Restored subscription {Id} from Graph (expires {Expiry}).",
                    _subscriptionId, _subscriptionExpiry);
            }
            else
            {
                _logger.LogInformation(
                    "No valid matching subscription found on Graph — will create a new one.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not query Graph subscriptions during restore — will proceed to create.");
        }
    }

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

            _logger.LogInformation("Found {Count} stale subscription(s) — deleting all.", existing.Value.Count);

            foreach (var sub in existing.Value)
            {
                try
                {
                    await _graphClient.Subscriptions[sub.Id].DeleteAsync();
                    _logger.LogInformation("Deleted stale subscription {Id}.", sub.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not delete stale subscription {Id}.", sub.Id);
                }
            }

            _subscriptionId = null;
            _subscriptionExpiry = DateTimeOffset.MinValue;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error fetching stale subscriptions — proceeding anyway.");
            _subscriptionId = null;
            _subscriptionExpiry = DateTimeOffset.MinValue;
        }
    }
}
