using AwningsAPI.Model.Email;

namespace AwningsAPI.Interfaces
{
    public interface IEmailAutoReplyService
    {
        /// <summary>
        /// Generates a professional auto-reply for an initial enquiry using Claude,
        /// saves it as a draft via Microsoft Graph, and returns the draft ID and body.
        /// </summary>
        Task<(string DraftId, string Content)> GenerateAndSaveDraftAsync(
            IncomingEmail email,
            string mailboxEmail);

        /// <summary>
        /// Looks up the enquiry and its linked email, generates a Claude auto-reply,
        /// saves the draft via Microsoft Graph, and persists the draft ID and content
        /// back to the enquiry row.
        /// </summary>
        Task<(string DraftId, string Content)> GenerateAutoReplyForEnquiryAsync(int enquiryId);

        /// <summary>
        /// Sends an existing Outlook draft message via Microsoft Graph.
        /// </summary>
        Task SendDraftAsync(string draftId, string mailboxEmail);
    }
}
