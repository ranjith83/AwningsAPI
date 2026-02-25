using AwningsAPI.Model.Email;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IEmailReaderService
    {
        /// <summary>
        /// Retrieves unread emails from the specified mailbox
        /// </summary>
        Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 50);

        /// <summary>
        /// Downloads all attachments for a specific email
        /// </summary>
        Task<List<EmailAttachment>> DownloadAttachmentsAsync(string mailboxEmail, string emailId);

        /// <summary>
        /// Marks an email as read in Outlook
        /// </summary>
        Task MarkEmailAsReadAsync(string mailboxEmail, string emailId);

        /// <summary>
        /// Moves email to a specific folder
        /// </summary>
        Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName);

        /// <summary>
        /// Gets a complete email with all attachments
        /// </summary>
        Task<IncomingEmail> GetCompleteEmailAsync(string mailboxEmail, string emailId);

        Task SendEmailAsync(string mailboxEmail, string toEmail, string toName, string subject, string bodyHtml, string? replyToEmailId = null);

    }
}