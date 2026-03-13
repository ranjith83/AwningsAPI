using AwningsAPI.Model.Email;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IEmailReaderService
    {
        /// <summary>
        /// Fetches unread emails from the given mailbox.
        /// </summary>
        /// <param name="mailboxEmail">The monitored mailbox address.</param>
        /// <param name="maxResults">
        /// Hard limit on number of emails returned.
        /// 0 (default) = use EmailReader:MaxEmailsPerBatch from appsettings (fallback 50).
        /// Explicit values are clamped to 1–1000 (Graph API ceiling).
        /// </param>
        Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 0);

        Task<List<EmailAttachment>> DownloadAttachmentsAsync(string mailboxEmail, string emailId);

        Task<IncomingEmail> GetCompleteEmailAsync(string mailboxEmail, string emailId);

        Task MarkEmailAsReadAsync(string mailboxEmail, string emailId);

        Task SendEmailAsync(
            string mailboxEmail,
            string toEmail,
            string toName,
            string subject,
            string bodyHtml,
            string? replyToEmailId = null,
            System.Collections.Generic.IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments = null);

        Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName);
    }
}