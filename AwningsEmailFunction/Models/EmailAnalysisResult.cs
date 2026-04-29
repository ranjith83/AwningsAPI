namespace AwningsEmailFunction.Models;

public class EmailAnalysisResult
{
    public string Category { get; set; } = string.Empty;
    public string TaskType { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public Dictionary<string, object> ExtractedData { get; set; } = new();
    public double Confidence { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public string CompanyNumber { get; set; } = string.Empty;
    public List<string> RequiredActions { get; set; } = new();
    public Dictionary<string, string> CustomerInfo { get; set; } = new();
    public bool IsSpam { get; set; }
    public string Sentiment { get; set; } = "Neutral";
    public List<string> Warnings { get; set; } = new();
}
