using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Models;
using Microsoft.EntityFrameworkCore;
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
    private readonly EmailFunctionDbContext _context;

    private static readonly TimeSpan RenewalBuffer = TimeSpan.FromMinutes(30);

    public GraphSubscriptionService(
        GraphServiceClient graphClient,
        IConfiguration configuration,
        ILogger<GraphSubscriptionService> logger,
        EmailFunctionDbContext context)
    {
        _graphClient = graphClient;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    public async Task EnsureSubscriptionAsync()
    {
        try
        {
            var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                ?? _configuration["AzureAd:OrganizerEmail"]
                ?? throw new InvalidOperationException("Monitored mailbox not configured.");

            var notificationUrl = _configuration["GraphSubscription:NotificationUrl"]
                ?? throw new InvalidOperationException("GraphSubscription:NotificationUrl not configured.");

            var clientState = _configuration["GraphSubscription:ClientState"] ?? "AwningsEmailWatcher";

            var persisted = await _context.GraphSubscriptions.FirstOrDefaultAsync();

            // Still valid — nothing to do
            if (persisted != null && DateTimeOffset.UtcNow < persisted.ExpiryDateTime - RenewalBuffer)
            {
                _logger.LogInformation(
                    "Graph subscription {Id} is still valid (expires {Expiry}). No action needed.",
                    persisted.SubscriptionId, persisted.ExpiryDateTime);
                return;
            }

            // Try to renew existing
            if (persisted != null)
            {
                try
                {
                    _logger.LogInformation("Renewing Graph subscription {Id}...", persisted.SubscriptionId);
                    var newExpiry = DateTimeOffset.UtcNow.AddMinutes(4230);
                    await _graphClient.Subscriptions[persisted.SubscriptionId]
                        .PatchAsync(new Subscription { ExpirationDateTime = newExpiry });

                    persisted.ExpiryDateTime = newExpiry;
                    persisted.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Graph subscription renewed until {Expiry}.", newExpiry);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not renew subscription {Id} — will recreate.", persisted.SubscriptionId);
                    _context.GraphSubscriptions.Remove(persisted);
                    await _context.SaveChangesAsync();
                    persisted = null;
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

            if (created?.Id == null)
                throw new Exception("Graph returned a null subscription ID.");

            _context.GraphSubscriptions.Add(new GraphSubscription
            {
                SubscriptionId = created.Id,
                ExpiryDateTime = created.ExpirationDateTime ?? DateTimeOffset.UtcNow.AddMinutes(4230),
                UpdatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            _logger.LogInformation("Graph subscription created and saved to DB. ID={Id}, Expires={Expiry}",
                created.Id, created.ExpirationDateTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create/renew Graph mail subscription.");
            throw;
        }
    }

    public string? GetActiveSubscriptionId()
    {
        return _context.GraphSubscriptions
            .AsNoTracking()
            .OrderByDescending(s => s.UpdatedAt)
            .Select(s => s.SubscriptionId)
            .FirstOrDefault();
    }

    public async Task DeleteSubscriptionAsync()
    {
        var persisted = await _context.GraphSubscriptions.FirstOrDefaultAsync();
        if (persisted == null) return;

        try
        {
            await _graphClient.Subscriptions[persisted.SubscriptionId].DeleteAsync();
            _logger.LogInformation("Graph subscription {Id} deleted.", persisted.SubscriptionId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not delete Graph subscription {Id}.", persisted.SubscriptionId);
        }
        finally
        {
            _context.GraphSubscriptions.Remove(persisted);
            await _context.SaveChangesAsync();
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

            // Clear DB record too
            var dbRecords = await _context.GraphSubscriptions.ToListAsync();
            if (dbRecords.Any())
            {
                _context.GraphSubscriptions.RemoveRange(dbRecords);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error fetching stale subscriptions — proceeding anyway.");
        }
    }
}
