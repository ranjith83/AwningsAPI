using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace AwningsEmailFunction.Services;

public class EmailAutoReplyService : IEmailAutoReplyService
{
    private readonly EmailFunctionDbContext _context;
    private readonly IBlobEmailStorageService _blobService;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EmailAutoReplyService> _logger;

    private const string CommonGuidance =
        "Write a warm, concise reply to the customer below. The reply must:\n" +
        "- Open with a personalised greeting using the customer's first name if available\n" +
        "{0}\n" +
        "- Close professionally with 'Kind regards, The Awnings of Ireland Team'\n\n" +
        "Keep the reply to 3-4 short paragraphs. Return only the email body — no subject line, " +
        "no 'From:', no 'To:', no metadata.";

    private const string Preamble =
        "You are a professional customer service assistant for Awnings of Ireland, a company specialising " +
        "in awnings, pergolas, canopies, and outdoor shade solutions.\n\n";

    private static readonly Dictionary<string, string> CategoryGuidance = new()
    {
        ["enquiry"] =
            "- Thank them for their interest and briefly acknowledge the subject of their enquiry\n" +
            "- Assure them a team member will review their requirements and follow up with product information and pricing within 1-2 business days\n" +
            "- Invite them to call or visit the showroom if they need an immediate response",

        ["quote"] =
            "- Thank them for their quote request and briefly acknowledge the details or specifications they provided\n" +
            "- Confirm that the team is preparing a formal quotation and will send it within 2-3 business days\n" +
            "- Invite them to share any additional measurements, photos, or preferences that would help with the quote",

        ["showroom"] =
            "- Thank them for their interest in visiting the showroom\n" +
            "- Let them know a team member will confirm a suitable time for their visit\n" +
            "- Invite them to suggest a few preferred dates/times, or to call ahead to check opening hours before visiting",

        ["complaint"] =
            "- Sincerely apologise for the issue they have experienced and thank them for bringing it to the company's attention\n" +
            "- Briefly acknowledge the specific problem they described, using an empathetic and reassuring tone\n" +
            "- Reassure them that the team is looking into the matter and will follow up with a resolution as soon as possible\n" +
            "- If helpful, ask them to provide any additional details (e.g. order/invoice number, photos) that would help resolve the issue faster",

        ["order_status"] =
            "- Thank them for getting in touch about their order\n" +
            "- Let them know the team is checking the status/delivery schedule of their order and will come back to them with an update within 1-2 business days\n" +
            "- Invite them to call the office if the matter is urgent or if they can provide an order/invoice number to help locate their order faster",

        ["general"] =
            "- Thank them for their message and briefly acknowledge what they have asked about\n" +
            "- Let them know a team member will review their message and follow up with the relevant information as soon as possible\n" +
            "- Invite them to call the office if the matter is urgent"
    };

    public EmailAutoReplyService(
        EmailFunctionDbContext context,
        IBlobEmailStorageService blobService,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<EmailAutoReplyService> logger)
    {
        _context = context;
        _blobService = blobService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task GenerateDraftReplyAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
        {
            _logger.LogWarning("AutoResponse: task {TaskId} not found — skipping", taskId);
            return;
        }

        var bodyContent = await GetEmailBodyAsync(task);

        var apiKey = _configuration["Claude:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("Claude:ApiKey is not configured");

        var systemPrompt = BuildSystemPrompt(task.TaskType);
        var userMessage =
            $"Customer name: {task.FromName}\n" +
            $"Customer email: {task.FromEmail}\n" +
            $"Subject: {task.Subject}\n\n" +
            $"Their message:\n{bodyContent}";

        var draftReply = await CallClaudeAsync(systemPrompt, userMessage, apiKey);

        task.DraftReply = draftReply;
        task.DateUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Draft reply generated for task {TaskId} (category: {Category})", taskId, task.TaskType);
    }

    private async Task<string?> GetEmailBodyAsync(Models.AppTask task)
    {
        if (!string.IsNullOrEmpty(task.EmailBody))
            return task.EmailBody;

        if (task.IncomingEmailId == null)
            return null;

        var email = await _context.IncomingEmails.FindAsync(task.IncomingEmailId.Value);
        if (email == null)
            return null;

        if (!string.IsNullOrEmpty(email.BodyContent))
            return email.BodyContent;

        if (!string.IsNullOrEmpty(email.BodyBlobUrl))
            return await _blobService.DownloadEmailBodyAsync(email.BodyBlobUrl);

        return email.BodyPreview;
    }

    private static string BuildSystemPrompt(string? category)
    {
        var guidance = CategoryGuidance.TryGetValue(category ?? "", out var g)
            ? g
            : CategoryGuidance["general"];

        return Preamble + string.Format(CommonGuidance, guidance);
    }

    private async Task<string> CallClaudeAsync(string systemPrompt, string userMessage, string apiKey)
    {
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

        var requestBody = new
        {
            model = _configuration["Claude:Model"] ?? "claude-haiku-4-5",
            max_tokens = 512,
            system = systemPrompt,
            messages = new[] { new { role = "user", content = userMessage } }
        };

        var response = await client.PostAsync(
            "https://api.anthropic.com/v1/messages",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Claude API returned {StatusCode} when generating draft reply: {Body}", (int)response.StatusCode, json);
            throw new InvalidOperationException($"Claude API request failed with status {(int)response.StatusCode}: {json}");
        }

        var claudeResponse = JsonSerializer.Deserialize<ClaudeResponse>(json);
        var text = claudeResponse?.content?.FirstOrDefault()?.text;

        if (string.IsNullOrEmpty(text))
        {
            _logger.LogError("Claude API returned no text content when generating draft reply: {Body}", json);
            throw new InvalidOperationException("Claude API returned no text content");
        }

        return text.Trim();
    }

    private class ClaudeResponse
    {
        public ClaudeContent[] content { get; set; } = Array.Empty<ClaudeContent>();
    }

    private class ClaudeContent
    {
        public string text { get; set; } = string.Empty;
    }
}
