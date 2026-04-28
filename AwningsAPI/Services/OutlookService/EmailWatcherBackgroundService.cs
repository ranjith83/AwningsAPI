using AwningsAPI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    /// <summary>
    /// Long-running background service that:
    ///   1. Creates the Microsoft Graph inbox subscription on startup.
    ///   2. Re-checks every hour and renews the subscription before it expires.
    ///
    /// Register in Program.cs / Startup.cs:
    ///   builder.Services.AddHostedService&lt;EmailWatcherBackgroundService&gt;();
    ///   builder.Services.AddScoped&lt;IGraphSubscriptionService, GraphSubscriptionService&gt;();
    /// </summary>
    public class EmailWatcherBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailWatcherBackgroundService> _logger;

        // How often we check whether the subscription needs renewing.
        private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(1);

        /// <summary>
        /// The exact UTC moment this background service started.
        /// Logged on startup so you can confirm which emails will be in scope.
        /// </summary>
        public static readonly DateTimeOffset StartedAt = DateTimeOffset.UtcNow;

        public EmailWatcherBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<EmailWatcherBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "EmailWatcherBackgroundService starting. " +
                "Only emails received on or after {StartedAt} will be processed.",
                StartedAt.ToString("yyyy-MM-dd HH:mm:ss UTC"));

            // Give the host a moment to finish startup before creating the subscription.
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var subscriptionService = scope.ServiceProvider
                        .GetRequiredService<IGraphSubscriptionService>();

                    await subscriptionService.EnsureSubscriptionAsync();
                }
                catch (Exception ex)
                {
                    // Log but keep running — we'll retry on the next interval.
                    _logger.LogError(ex,
                        "Error ensuring Graph subscription. Will retry in {Interval}.",
                        CheckInterval);
                }

                await Task.Delay(CheckInterval, stoppingToken);
            }

            // Clean shutdown: remove the subscription so Graph stops sending notifications.
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var subscriptionService = scope.ServiceProvider
                    .GetRequiredService<IGraphSubscriptionService>();

                await subscriptionService.DeleteSubscriptionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not delete Graph subscription during shutdown.");
            }

            _logger.LogInformation("EmailWatcherBackgroundService stopped.");
        }
    }
}