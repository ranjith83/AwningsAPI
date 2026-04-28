using AwningsEmailFunction.Interfaces;
using AwningsEmailFunction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AwningsEmailFunction.Services;

public class EmailReaderService : IEmailReaderService
{
    private readonly GraphServiceClient _graphClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailReaderService> _logger;

    private static readonly DateTimeOffset AppStartedAt = DateTimeOffset.UtcNow;

    private int MaxEmailsPerBatch
    {
        get
        {
            var configured = _configuration.GetValue<int?>("EmailReader:MaxEmailsPerBatch");
            if (configured is null or <= 0 or > 1000) return 50;
            return configured.Value;
        }
    }

    public EmailReaderService(
        GraphServiceClient graphClient,
        IConfiguration configuration,
        ILogger<EmailReaderService> logger)
    {
        _graphClient = graphClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 0)
    {
        try
        {
            var effectiveLimit = (maxResults > 0 && maxResults <= 1000) ? maxResults : MaxEmailsPerBatch;
            var sinceUtc = AppStartedAt.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var filter = $"isRead eq false and receivedDateTime ge {sinceUtc}";

            _logger.LogInformation("Fetching up to {Limit} unread emails from {Mailbox} since {Since}",
                effectiveLimit, mailboxEmail, sinceUtc);

            var messages = await _graphClient.Users[mailboxEmail]
                .Messages
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Filter = filter;
                    requestConfiguration.QueryParameters.Top = effectiveLimit;
                    requestConfiguration.QueryParameters.Orderby = new[] { "receivedDateTime DESC" };
                    requestConfiguration.QueryParameters.Select = new[]
                    {
                        "id", "subject", "from", "body", "bodyPreview",
                        "receivedDateTime", "hasAttachments", "importance", "isRead"
                    };
                });

            if (messages?.Value == null || !messages.Value.Any())
            {
                _logger.LogInformation("No unread emails found.");
                return new List<IncomingEmail>();
            }

            var incomingEmails = messages.Value.Select(message => new IncomingEmail
            {
                EmailId = message.Id ?? string.Empty,
                Subject = message.Subject ?? "No Subject",
                FromEmail = message.From?.EmailAddress?.Address ?? string.Empty,
                FromName = message.From?.EmailAddress?.Name ?? string.Empty,
                BodyPreview = message.BodyPreview ?? string.Empty,
                BodyContent = message.Body?.Content ?? string.Empty,
                IsHtml = message.Body?.ContentType == BodyType.Html,
                ReceivedDateTime = message.ReceivedDateTime?.DateTime ?? DateTime.UtcNow,
                HasAttachments = message.HasAttachments ?? false,
                Importance = message.Importance?.ToString() ?? "Normal",
                ProcessingStatus = "Pending"
            }).ToList();

            _logger.LogInformation("Retrieved {Count} unread emails.", incomingEmails.Count);
            return incomingEmails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread emails.");
            throw;
        }
    }

    public async Task<IncomingEmail> GetCompleteEmailAsync(string mailboxEmail, string emailId)
    {
        try
        {
            _logger.LogInformation("Fetching complete email. MailboxEmail={Mailbox}, EmailId={EmailId}",
                mailboxEmail, emailId);

            Message? message;

            if (emailId.Contains('@'))
            {
                var results = await _graphClient.Users[mailboxEmail]
                    .Messages
                    .GetAsync(req =>
                    {
                        req.QueryParameters.Filter = $"from/emailAddress/address eq '{emailId}'";
                        req.QueryParameters.Top = 1;
                        req.QueryParameters.Select = new[]
                        {
                            "id", "subject", "from", "body", "bodyPreview",
                            "receivedDateTime", "hasAttachments", "importance", "isRead"
                        };
                    });

                message = results?.Value?.FirstOrDefault()
                    ?? throw new Exception($"No email found from sender: {emailId}");
            }
            else
            {
                var cleanId = emailId.Contains('/')
                    ? emailId.Split('/').Last()
                    : Uri.UnescapeDataString(emailId);

                message = await _graphClient.Users[mailboxEmail]
                    .Messages[cleanId]
                    .GetAsync(req =>
                    {
                        req.QueryParameters.Select = new[]
                        {
                            "id", "subject", "from", "body", "bodyPreview",
                            "receivedDateTime", "hasAttachments", "importance", "isRead"
                        };
                    });
            }

            if (message == null)
                throw new Exception($"Graph returned null for message ID: {emailId}");

            var incomingEmail = new IncomingEmail
            {
                EmailId = message.Id ?? string.Empty,
                Subject = message.Subject ?? "No Subject",
                FromEmail = message.From?.EmailAddress?.Address ?? string.Empty,
                FromName = message.From?.EmailAddress?.Name ?? string.Empty,
                BodyPreview = message.BodyPreview ?? string.Empty,
                BodyContent = message.Body?.Content ?? string.Empty,
                IsHtml = message.Body?.ContentType == BodyType.Html,
                ReceivedDateTime = message.ReceivedDateTime?.DateTime ?? DateTime.UtcNow,
                HasAttachments = message.HasAttachments ?? false,
                Importance = message.Importance?.ToString() ?? "Normal",
                ProcessingStatus = "Pending"
            };

            if (incomingEmail.HasAttachments)
                incomingEmail.Attachments = await DownloadAttachmentsAsync(mailboxEmail, message.Id!);

            _logger.LogInformation("Retrieved complete email: {Subject}", incomingEmail.Subject);
            return incomingEmail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving complete email {EmailId}.", emailId);
            throw;
        }
    }

    public async Task<List<EmailAttachment>> DownloadAttachmentsAsync(string mailboxEmail, string emailId)
    {
        try
        {
            var attachments = await _graphClient.Users[mailboxEmail]
                .Messages[emailId]
                .Attachments
                .GetAsync();

            if (attachments?.Value == null || !attachments.Value.Any())
                return new List<EmailAttachment>();

            var result = new List<EmailAttachment>();

            foreach (var attachment in attachments.Value)
            {
                if (attachment is not FileAttachment fileAttachment) continue;

                result.Add(new EmailAttachment
                {
                    AttachmentId = fileAttachment.Id ?? string.Empty,
                    FileName = fileAttachment.Name ?? "unknown",
                    ContentType = fileAttachment.ContentType ?? "application/octet-stream",
                    Size = fileAttachment.Size ?? 0,
                    IsInline = fileAttachment.IsInline ?? false,
                    Base64Content = Convert.ToBase64String(fileAttachment.ContentBytes ?? Array.Empty<byte>())
                });
            }

            _logger.LogInformation("Downloaded {Count} attachments for email {EmailId}.", result.Count, emailId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading attachments for email {EmailId}.", emailId);
            throw;
        }
    }

    public async Task MarkEmailAsReadAsync(string mailboxEmail, string emailId)
    {
        try
        {
            await _graphClient.Users[mailboxEmail]
                .Messages[emailId]
                .PatchAsync(new Message { IsRead = true });

            _logger.LogInformation("Marked email {EmailId} as read.", emailId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking email {EmailId} as read.", emailId);
            throw;
        }
    }

    public async Task SendEmailAsync(
        string mailboxEmail, string toEmail, string toName,
        string subject, string bodyHtml,
        string? replyToEmailId = null,
        IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments = null)
    {
        try
        {
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody { ContentType = BodyType.Html, Content = bodyHtml },
                ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new Microsoft.Graph.Models.EmailAddress
                        {
                            Address = toEmail,
                            Name = toName
                        }
                    }
                }
            };

            if (attachments != null)
            {
                message.Attachments = new List<Microsoft.Graph.Models.Attachment>();
                foreach (var (fileName, base64Content, contentType) in attachments)
                {
                    if (string.IsNullOrWhiteSpace(base64Content)) continue;
                    message.Attachments.Add(new FileAttachment
                    {
                        Name = fileName,
                        ContentType = contentType,
                        ContentBytes = Convert.FromBase64String(base64Content)
                    });
                }
            }

            if (!string.IsNullOrEmpty(replyToEmailId))
            {
                await _graphClient.Users[mailboxEmail]
                    .Messages[replyToEmailId]
                    .Reply
                    .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Reply.ReplyPostRequestBody
                    {
                        Message = message
                    });
            }
            else
            {
                await _graphClient.Users[mailboxEmail]
                    .SendMail
                    .PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                    {
                        Message = message,
                        SaveToSentItems = true
                    });
            }

            _logger.LogInformation("Email sent to {ToEmail}.", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {ToEmail}.", toEmail);
            throw;
        }
    }

    public async Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName)
    {
        try
        {
            var folders = await _graphClient.Users[mailboxEmail]
                .MailFolders
                .GetAsync(req =>
                {
                    req.QueryParameters.Filter = $"displayName eq '{folderName}'";
                });

            string folderId;

            if (folders?.Value == null || !folders.Value.Any())
            {
                var created = await _graphClient.Users[mailboxEmail]
                    .MailFolders
                    .PostAsync(new MailFolder { DisplayName = folderName });
                folderId = created?.Id ?? throw new Exception("Failed to create folder.");
            }
            else
            {
                folderId = folders.Value.First().Id ?? throw new Exception("Folder ID is null.");
            }

            await _graphClient.Users[mailboxEmail]
                .Messages[emailId]
                .Move
                .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Move.MovePostRequestBody
                {
                    DestinationId = folderId
                });

            _logger.LogInformation("Moved email {EmailId} to folder {FolderName}.", emailId, folderName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error moving email {EmailId} to folder {FolderName}.", emailId, folderName);
            throw;
        }
    }
}
