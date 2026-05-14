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

        /// <summary>
        /// Resolves the mailbox, optionally looks up brochures by product ID, and sends
        /// a fresh outbound email. Handles test/prod email redirection from configuration.
        /// </summary>
        Task SendDirectEmailAsync(
            string toEmail,
            string? toName,
            string subject,
            string body,
            bool attachBrochure,
            List<int>? productIds,
            IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments = null);

        Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName);

        /// <summary>
        /// Orchestrates the full send-task-email flow: resolves the mailbox from config,
        /// looks up the task if ToEmail is empty, maps attachment DTOs, sends the email,
        /// and auto-dismisses any active follow-up for the task's workflow.
        /// </summary>
        Task SendTaskEmailAsync(
            int taskId,
            string? toEmail,
            string? toName,
            string subject,
            string body,
            string? originalEmailGraphId,
            IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments,
            string currentUser);
    }
}