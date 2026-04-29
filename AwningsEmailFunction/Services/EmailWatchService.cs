using AwningsEmailFunction.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AwningsEmailFunction.Services;

public class EmailWatchService : IEmailWatchService
{
    private readonly IEmailProcessorService _emailProcessorService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailWatchService> _logger;

    public EmailWatchService(
        IEmailProcessorService emailProcessorService,
        IConfiguration configuration,
        ILogger<EmailWatchService> logger)
    {
        _emailProcessorService = emailProcessorService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SaveEmailAsync(string messageId)
    {
        var mailboxEmail = _configuration["AzureAd:OrganizerEmail"]
            ?? throw new InvalidOperationException("AzureAd:OrganizerEmail is not configured.");

        _logger.LogInformation("Incoming email notification received — MessageId: {MessageId}", messageId);

        await _emailProcessorService.ProcessIncomingEmailAsync(messageId, mailboxEmail);
    }
}
