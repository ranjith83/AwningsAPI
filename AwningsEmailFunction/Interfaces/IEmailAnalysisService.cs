using AwningsEmailFunction.Models;

namespace AwningsEmailFunction.Interfaces;

public interface IEmailAnalysisService
{
    Task<EmailAnalysisResult> AnalyzeEmailAsync(IncomingEmail email);
}
