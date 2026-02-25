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

        public EmailReaderService(
            GraphServiceClient graphClient,
            ILogger<EmailReaderService> logger,
            AppDbContext context)
        {
            _graphClient = graphClient;
            _logger = logger;
            _context = context;
        }

        public async Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 50)
        {
            try
            {
                _logger.LogInformation($"Fetching unread emails from {mailboxEmail}");

                var messages = await _graphClient.Users[mailboxEmail]
                    .Messages
                    .GetAsync(requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.Filter = "isRead eq false";
                        requestConfiguration.QueryParameters.Top = maxResults;
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
                _logger.LogInformation($"Fetching complete email {emailId}");


                var messages = await _graphClient.Users[mailboxEmail]
                    .Messages
                    .GetAsync();

                var id = messages?.Value?.First().Id;

                var safeId = Uri.EscapeDataString(id);

                // Get the email message
                var msg = await _graphClient.Users[mailboxEmail]
                    .Messages
                    .GetAsync(requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.Filter =
                            $"from/emailAddress/address eq '{emailId}'";

                        requestConfiguration.QueryParameters.Select = new[]
                        {
                           "id", "subject", "from", "body", "bodyPreview",
                           "receivedDateTime", "hasAttachments", "importance", "isRead"
                       };
                    });
                var message = msg?.Value?.FirstOrDefault();


                if (message == null)
                {
                    throw new Exception($"Email {emailId} not found");
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
        /// </summary>
        public async Task SendEmailAsync(
            string mailboxEmail,
            string toEmail,
            string toName,
            string subject,
            string bodyHtml,
            string? replyToEmailId = null)
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