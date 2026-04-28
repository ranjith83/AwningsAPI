using AwningsEmailFunction.Models;

namespace AwningsEmailFunction.Interfaces;

public interface IEmailReaderService
{
    Task<List<IncomingEmail>> GetUnreadEmailsAsync(string mailboxEmail, int maxResults = 0);
    Task<IncomingEmail> GetCompleteEmailAsync(string mailboxEmail, string emailId);
    Task<List<EmailAttachment>> DownloadAttachmentsAsync(string mailboxEmail, string emailId);
    Task MarkEmailAsReadAsync(string mailboxEmail, string emailId);
    Task SendEmailAsync(
        string mailboxEmail, string toEmail, string toName,
        string subject, string bodyHtml,
        string? replyToEmailId = null,
        IEnumerable<(string FileName, string Base64Content, string ContentType)>? attachments = null);
    Task MoveEmailToFolderAsync(string mailboxEmail, string emailId, string folderName);
}
