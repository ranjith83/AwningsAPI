using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AwningsEmailFunction.Services;

public class EmailWatchService : IEmailWatchService
{
    private readonly IEmailReaderService _emailReaderService;
    private readonly EmailFunctionDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailWatchService> _logger;

    public EmailWatchService(
        IEmailReaderService emailReaderService,
        EmailFunctionDbContext context,
        IConfiguration configuration,
        ILogger<EmailWatchService> logger)
    {
        _emailReaderService = emailReaderService;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SaveEmailAsync(string messageId)
    {
        var mailboxEmail = _configuration["AzureAd:OrganizerEmail"]
            ?? throw new InvalidOperationException("AzureAd:OrganizerEmail is not configured.");

        _logger.LogInformation("Fetching email {MessageId} from mailbox {Mailbox}", messageId, mailboxEmail);

        var alreadyExists = await _context.IncomingEmails
            .AnyAsync(e => e.EmailId == messageId);

        if (alreadyExists)
        {
            _logger.LogInformation("Email {MessageId} already in DB — skipping.", messageId);
            return;
        }

        var email = await _emailReaderService.GetCompleteEmailAsync(mailboxEmail, messageId);

        var entity = new IncomingEmail
        {
            EmailId = email.EmailId,
            Subject = email.Subject,
            FromEmail = email.FromEmail,
            FromName = email.FromName,
            BodyPreview = email.BodyPreview,
            BodyContent = email.BodyContent,
            IsHtml = email.IsHtml,
            ReceivedDateTime = email.ReceivedDateTime,
            HasAttachments = email.HasAttachments,
            Importance = email.Importance,
            ProcessingStatus = "Pending",
            DateCreated = DateTime.UtcNow
        };

        foreach (var att in email.Attachments)
        {
            entity.Attachments.Add(new EmailAttachment
            {
                AttachmentId = att.AttachmentId,
                FileName = att.FileName,
                ContentType = att.ContentType,
                Size = att.Size,
                IsInline = att.IsInline,
                Base64Content = att.Base64Content,
                ExtractedText = att.ExtractedText,
                DateDownloaded = DateTime.UtcNow
            });
        }

        _context.IncomingEmails.Add(entity);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Saved email {MessageId} (Subject: {Subject}) to DB with Id={Id}",
            messageId, entity.Subject, entity.Id);
    }
}
