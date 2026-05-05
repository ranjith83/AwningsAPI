using AwningsEmailFunction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AwningsEmailFunction.Functions;

public class SubscriptionRenewalFunction
{
    private readonly IGraphSubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionRenewalFunction> _logger;

    public SubscriptionRenewalFunction(
        IGraphSubscriptionService subscriptionService,
        ILogger<SubscriptionRenewalFunction> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    // Runs every 12 hours — Graph subscriptions for mail expire after ~3 days (4230 min).
    // Renewal happens when within 12 hours of expiry. CRON: second minute hour day month weekday
    [Function("SubscriptionRenewal")]
    public async Task Run([TimerTrigger("0 0 */12 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("🔄 Subscription renewal check started at {Time}", DateTimeOffset.UtcNow);

        if (timerInfo.ScheduleStatus is not null)
        {
            _logger.LogInformation("⏰ Next renewal scheduled at: {Next}", timerInfo.ScheduleStatus.Next);
        }

        try
        {
            await _subscriptionService.EnsureSubscriptionAsync();
            _logger.LogInformation("✅ Subscription renewal check completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during subscription renewal.");
        }
    }
}
