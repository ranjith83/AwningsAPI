using System.Threading.Tasks;

namespace AwningsAPI.Interfaces
{
    public interface IEmailProcessorService
    {
        /// <summary>
        /// Fetches and processes all unread emails from the monitored mailbox.
        /// </summary>
        /// <param name="maxEmails">
        /// Maximum number of unread emails to fetch in this run.
        /// 0 (default) = use EmailReader:MaxEmailsPerBatch from appsettings (fallback 50).
        /// Valid range: 1–1000. Values outside this range are ignored and the default is used.
        /// </param>
        Task ProcessIncomingEmailsAsync();

        /// <summary>
        /// Fetches and processes a single email by its Microsoft Graph message ID.
        /// </summary>
        Task ProcessSingleEmailAsync(string emailId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //Task ProcessIncomingEmailsAsync();
    }
}