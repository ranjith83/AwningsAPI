using AwningsEmailFunction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AwningsEmailFunction.Functions;

public class EmailProcessorFunction
{
    private readonly IEmailWatchService _emailWatchService;
    private readonly ILogger<EmailProcessorFunction> _logger;

    public EmailProcessorFunction(IEmailWatchService emailWatchService, ILogger<EmailProcessorFunction> logger)
    {
        _emailWatchService = emailWatchService;
        _logger = logger;
    }

    // Dequeues a messageId written by GraphWebhookFunction.Notify.
    // Azure Functions retries automatically on failure (up to 5 times with backoff).
    [Function("EmailProcessor")]
    public async Task Run(
        [QueueTrigger("email-processing", Connection = "AzureWebJobsStorage")] string messageId)
    {
        _logger.LogInformation("⚡ Processing email messageId: {MessageId}", messageId);
        await _emailWatchService.SaveEmailAsync(messageId);
        _logger.LogInformation("✅ Email processing complete for messageId: {MessageId}", messageId);
    }
}
