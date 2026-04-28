using AwningsAPI.Model.Email;

namespace AwningsAPI.Interfaces
{
    public interface IEmailAnalysisService
    {
        Task<EmailAnalysisResult> AnalyzeEmailAsync(IncomingEmail email);
        Task<string> ExtractTextFromImageAsync(byte[] imageContent);
        Task<string> ExtractTextFromPdfAsync(byte[] pdfContent);
    }
}
