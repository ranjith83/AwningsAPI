using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    public class EmailReaderService : IEmailReaderService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly ILogger<EmailReaderService> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// UTC timestamp captured when the service is first constructed.
        /// Used to filter emails so only messages received AFTER app startup are processed.
        /// Prevents reprocessing old unread emails sitting in the inbox before the watcher started.
        /// </summary>
        private static readonly DateTimeOffset AppStartedAt = DateTimeOffset.UtcNow;

        /// <summary>
        /// Hard upper bound: Graph allows at most 1000 items per request.
        /// Configurable via EmailReader:MaxEmailsPerBatch in appsettings.json.
        /// Falls back to 50 when the setting is absent or invalid.
        /// </summary>
        private int MaxEmailsPerBatch
        {
            get
            {
                var configured = _configuration.GetValue<int?>("EmailReader:MaxEmailsPerBatch");
                if (configured is null or <= 0 or > 1000)
                    return 50;
                return configured.Value;
            }
        }

        public EmailReaderService(
            GraphServiceClient graphClient,
            ILogger<EmailReaderService> logger,
            AppDbContext context,
            IConfiguration configuration)
        {
            _graphClient = graphClient;
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 0)
        {
            try
            {
                // Resolve effective limit:
                //   caller override (>0)  →  EmailReader:MaxEmailsPerBatch config  →  50
                //   Pass 0 (or omit) to use the configured / default limit.
                var effectiveLimit = (maxResults > 0 && maxResults <= 1000)
                    ? maxResults
                    : MaxEmailsPerBatch;

                // Only fetch emails received on or after the moment this app instance started.
                // Graph OData datetime format requires ISO 8601 with no milliseconds.
                var sinceUtc = AppStartedAt.ToString("yyyy-MM-ddTHH:mm:ssZ");
                var filter = $"isRead eq false and receivedDateTime ge {sinceUtc}";

                _logger.LogInformation(
                    "Fetching up to {Limit} unread emails from {Mailbox} since {Since}",
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
                    _logger.LogInformation("No unread emails found");
                    return new List<IncomingEmail>();
                }

                var incomingEmails = new List<IncomingEmail>();

                foreach (var message in messages.Value)
                {
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

                    incomingEmails.Add(incomingEmail);
                }

                _logger.LogInformation($"Retrieved {incomingEmails.Count} unread emails");
                return incomingEmails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread emails");
                throw;
            }
        }

        public async Task<List<EmailAttachment>> DownloadAttachmentsAsync(string mailboxEmail, string emailId)
        {
            try
            {
                _logger.LogInformation($"Downloading attachments for email {emailId}");

                var attachments = await _graphClient.Users[mailboxEmail]
                    .Messages[emailId]
                    .Attachments
                    .GetAsync();

                if (attachments?.Value == null || !attachments.Value.Any())
                {
                    _logger.LogInformation("No attachments found");
                    return new List<EmailAttachment>();
                }

                var emailAttachments = new List<EmailAttachment>();

                foreach (var attachment in attachments.Value)
                {
                    if (attachment is FileAttachment fileAttachment)
                    {
                        var emailAttachment = new EmailAttachment
                        {
                            AttachmentId = fileAttachment.Id ?? string.Empty,
                            FileName = fileAttachment.Name ?? "unknown",
                            ContentType = fileAttachment.ContentType ?? "application/octet-stream",
                            Size = fileAttachment.Size ?? 0,
                            IsInline = fileAttachment.IsInline ?? false,
                            Base64Content = Convert.ToBase64String(fileAttachment.ContentBytes ?? Array.Empty<byte>())
                        };

                        emailAttachments.Add(emailAttachment);

                        _logger.LogInformation($"Downloaded attachment: {emailAttachment.FileName} ({emailAttachment.Size} bytes)");
                    }
                }

                _logger.LogInformation($"Downloaded {emailAttachments.Count} attachments");
                return emailAttachments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading attachments for email {emailId}");
                throw;
            }
        }

        public async Task<IncomingEmail> GetCompleteEmailAsync(string mailboxEmail, string emailId)
        {
            try
            {
                _logger.LogInformation("GetCompleteEmailAsync called. MailboxEmail={Mailbox}, EmailId={EmailId}, Length={Length}",
                         mailboxEmail, emailId, emailId?.Length);

                _logger.LogInformation($"Fetching complete email by ID: {emailId}");
                Message? message;

                // Fetch the message directly by its Graph message ID.
                // The emailId comes from the Graph change notification resource path,
                // so it is already the exact message ID — no filter needed.
                // Detect if it's an email address or a Graph message ID
                if (emailId.Contains('@'))
                {
                    // Search by sender — returns most recent unread from that address
                    _logger.LogInformation("Searching by sender email: {Sender}", emailId);

                    var results = await _graphClient.Users[mailboxEmail]
                        .Messages
                        .GetAsync(req =>
                        {
                            req.QueryParameters.Filter = $"from/emailAddress/address eq '{emailId}'";
                            req.QueryParameters.Top = 1;
                            //req.QueryParameters.Orderby = new[] { "receivedDateTime DESC" };
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
                    // Normal path — bare Graph message ID
                    var cleanId = emailId.Contains('/')
                        ? emailId.Split('/').Last()
                        : Uri.UnescapeDataString(emailId);

                    _logger.LogInformation("Fetching email by Graph ID: {Id}", cleanId);

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
                {
                    throw new Exception($"Graph returned null for message ID: {emailId}");
                }

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

                // Download attachments if any
                if (incomingEmail.HasAttachments)
                {
                    var attachments = await DownloadAttachmentsAsync(mailboxEmail, message.Id);
                    incomingEmail.Attachments = attachments;
                }

                _logger.LogInformation($"Retrieved complete email: {incomingEmail.Subject}");
                return incomingEmail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving complete email {emailId}");
                throw;
            }
        }

        public async Task MarkEmailAsReadAsync(string mailboxEmail, string emailId)
        {
            try
            {
                var message = new Message
                {
                    IsRead = true
                };

                await _graphClient.Users[mailboxEmail]
                    .Messages[emailId]
                    .PatchAsync(message);

                _logger.LogInformation($"Marked email {emailId} as read");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking email {emailId} as read");
                throw;
            }
        }

        /// <summary>
        /// Send an email reply (or new email) via Microsoft Graph on behalf of the monitored mailbox.
        /// Saves a copy to Sent Items automatically.
        /// Optionally attaches one or more files supplied as base64-encoded content.
        /// </summary>
        /// <param name="attachments">
        /// Optional list of file attachments.  Each tuple: (fileName, base64Content, contentType)
        /// e.g. ("Quote-001.pdf", "JVBERi0x...", "application/pdf")
        /// </param>
        public async Task SendEmailAsync(
            string mailboxEmail,
            string toEmail,
            string toName,
            string subject,
            string bodyHtml,
            string? replyToEmailId = null,
            IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments = null)
        {
            try
            {
                _logger.LogInformation($"Sending email to {toEmail}, subject: {subject}");

                var message = new Message
                {
                    Subject = subject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = bodyHtml
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new Microsoft.Graph.Models.EmailAddress
                            {
                                Address = toEmail,
                                Name    = toName
                            }
                        }
                    }
                };

                // Attach files when supplied
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
                        _logger.LogInformation($"Attaching {fileName} ({contentType})");
                    }
                }

                if (!string.IsNullOrEmpty(replyToEmailId))
                {
                    // Reply in the same thread — Graph creates the reply and sends it
                    await _graphClient.Users[mailboxEmail]
                        .Messages[replyToEmailId]
                        .Reply
                        .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Reply.ReplyPostRequestBody
                        {
                            Message = message
                        });

                    _logger.LogInformation($"Sent reply to thread {replyToEmailId}");
                }
                else
                {
                    // Fresh email — send directly
                    await _graphClient.Users[mailboxEmail]
                        .SendMail
                        .PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                        {
                            Message = message,
                            SaveToSentItems = true
                        });

                    _logger.LogInformation($"Sent new email to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {toEmail}");
                throw;
            }
        }

        public async Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName)
        {
            try
            {
                // First, find or create the folder
                var folders = await _graphClient.Users[mailboxEmail]
                    .MailFolders
                    .GetAsync(requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.Filter = $"displayName eq '{folderName}'";
                    });

                string folderId;

                if (folders?.Value == null || !folders.Value.Any())
                {
                    // Create the folder if it doesn't exist
                    var newFolder = new MailFolder
                    {
                        DisplayName = folderName
                    };

                    var createdFolder = await _graphClient.Users[mailboxEmail]
                        .MailFolders
                        .PostAsync(newFolder);

                    folderId = createdFolder?.Id ?? throw new Exception("Failed to create folder");
                }
                else
                {
                    folderId = folders.Value.First().Id ?? throw new Exception("Folder ID is null");
                }

                // Move the email to the folder
                await _graphClient.Users[mailboxEmail]
                    .Messages[emailId]
                    .Move
                    .PostAsync(new Microsoft.Graph.Users.Item.Messages.Item.Move.MovePostRequestBody
                    {
                        DestinationId = folderId
                    });

                _logger.LogInformation($"Moved email {emailId} to folder {folderName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error moving email {emailId} to folder {folderName}");
                throw;
            }
        }
    }
}