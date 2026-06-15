using Anthropic;
using Anthropic.Models.Messages;
using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using GraphMessage = Microsoft.Graph.Models.Message;
using GraphRecipient = Microsoft.Graph.Models.Recipient;
using GraphEmailAddress = Microsoft.Graph.Models.EmailAddress;
using GraphItemBody = Microsoft.Graph.Models.ItemBody;
using GraphBodyType = Microsoft.Graph.Models.BodyType;

namespace AwningsAPI.Services.OutlookService
{
    public class EmailAutoReplyService : IEmailAutoReplyService
    {
        private readonly AnthropicClient _anthropicClient;
        private readonly GraphServiceClient _graphClient;
        private readonly ILogger<EmailAutoReplyService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        // Stable system prompt — cached so it is not reprocessed on every call
        private const string AutoReplySystemPrompt =
            "You are a professional sales assistant for Awnings of Ireland, a company specialising " +
            "in awnings, pergolas, canopies, and outdoor shade solutions.\n\n" +
            "Write a warm, concise auto-reply to acknowledge a customer's initial enquiry. The reply must:\n" +
            "- Open with a personalised greeting using the customer's first name if available\n" +
            "- Thank them for their interest and briefly acknowledge the subject of their enquiry\n" +
            "- Assure them a team member will be in touch within 1-2 business days\n" +
            "- Invite them to call or visit the showroom if they need an immediate response\n" +
            "- Close professionally with 'Kind regards, The Awnings of Ireland Team'\n\n" +
            "Keep the reply to 3-4 short paragraphs. Return only the email body — no subject line, " +
            "no 'From:', no 'To:', no metadata.";

        // ── Task draft-reply generation (used for tasks flagged NeedsReply) ─────

        private const string TaskReplyPreamble =
            "You are a professional customer service assistant for Awnings of Ireland, a company specialising " +
            "in awnings, pergolas, canopies, and outdoor shade solutions.\n\n";

        private const string TaskReplyCommonGuidance =
            "Write a warm, concise reply to the customer below. The reply must:\n" +
            "- Open with a personalised greeting using the customer's first name if available\n" +
            "{0}\n" +
            "- Close professionally with 'Kind regards, The Awnings of Ireland Team'\n\n" +
            "Keep the reply to 3-4 short paragraphs. Return only the email body — no subject line, " +
            "no 'From:', no 'To:', no metadata.";

        private static readonly Dictionary<string, string> TaskCategoryGuidance = new()
        {
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
            AnthropicClient anthropicClient,
            GraphServiceClient graphClient,
            ILogger<EmailAutoReplyService> logger,
            IConfiguration configuration,
            AppDbContext context)
        {
            _anthropicClient = anthropicClient;
            _graphClient = graphClient;
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task<(string DraftId, string Content)> GenerateAndSaveDraftAsync(
            IncomingEmail email,
            string mailboxEmail)
        {
            var replyContent = await GenerateReplyAsync(email);
            var draftId = await CreateGraphDraftAsync(email, replyContent, mailboxEmail);
            return (draftId, replyContent);
        }

        public async Task<(string DraftId, string Content)> GenerateAutoReplyForEnquiryAsync(int enquiryId)
        {
            var enquiry = await _context.InitialEnquiries.FindAsync(enquiryId)
                ?? throw new KeyNotFoundException($"Enquiry {enquiryId} not found.");

            if (enquiry.IncomingEmailId == null)
                throw new InvalidOperationException("This enquiry has no linked email to reply to.");

            var email = await _context.IncomingEmails.FindAsync(enquiry.IncomingEmailId.Value)
                ?? throw new KeyNotFoundException("Linked email not found.");

            var mailbox = _configuration["AzureAd:OrganizerEmail"] ?? "";
            var (draftId, content) = await GenerateAndSaveDraftAsync(email, mailbox);

            enquiry.AutoReplyDraftId = draftId;
            enquiry.AutoReplyContent = content;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Auto-reply draft generated for enquiry {EnquiryId}", enquiryId);
            return (draftId, content);
        }

        public async Task<string> GenerateDraftReplyForTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId)
                ?? throw new KeyNotFoundException($"Task {taskId} not found.");

            var bodyContent = await GetTaskEmailBodyAsync(task);

            var userMessage =
                $"Customer name: {task.FromName}\n" +
                $"Customer email: {task.FromEmail}\n" +
                $"Subject: {task.Subject}\n\n" +
                $"Their message:\n{bodyContent}";

            var guidance = TaskCategoryGuidance.TryGetValue(task.TaskType ?? "", out var g)
                ? g
                : TaskCategoryGuidance["general"];

            var systemPrompt = TaskReplyPreamble + string.Format(TaskReplyCommonGuidance, guidance);

            var parameters = new MessageCreateParams
            {
                Model = _configuration["Claude:Model"] ?? "claude-haiku-4-5",
                MaxTokens = 512,
                System = new List<TextBlockParam>
                {
                    new()
                    {
                        Text = systemPrompt,
                        CacheControl = new CacheControlEphemeral(),
                    }
                },
                Messages = [new() { Role = Role.User, Content = userMessage }]
            };

            var response = await _anthropicClient.Messages.Create(parameters);

            var draftReply = response.Content
                .Select(b => b.Value)
                .OfType<TextBlock>()
                .FirstOrDefault()?.Text
                ?? "Thank you for your message. A member of our team will follow up shortly.";

            task.DraftReply = draftReply;
            task.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Draft reply generated for task {TaskId} (category: {Category})", taskId, task.TaskType);

            return draftReply;
        }

        private async Task<string?> GetTaskEmailBodyAsync(Model.Tasks.AppTask task)
        {
            if (!string.IsNullOrEmpty(task.EmailBody))
                return task.EmailBody;

            if (task.IncomingEmailId == null)
                return null;

            var email = await _context.IncomingEmails.FindAsync(task.IncomingEmailId.Value);
            return email?.BodyContent ?? email?.BodyPreview;
        }

        // ── Claude reply generation ───────────────────────────────────────────

        private async Task<string> GenerateReplyAsync(IncomingEmail email)
        {
            var userMessage =
                $"Customer name: {email.FromName}\n" +
                $"Customer email: {email.FromEmail}\n" +
                $"Subject: {email.Subject}\n\n" +
                $"Their message:\n{email.BodyContent}";

            var parameters = new MessageCreateParams
            {
                Model = _configuration["Claude:Model"] ?? "claude-haiku-4-5",
                MaxTokens = 512,
                System = new List<TextBlockParam>
                {
                    new()
                    {
                        Text = AutoReplySystemPrompt,
                        CacheControl = new CacheControlEphemeral(),
                    }
                },
                Messages = [new() { Role = Role.User, Content = userMessage }]
            };

            var response = await _anthropicClient.Messages.Create(parameters);

            return response.Content
                .Select(b => b.Value)
                .OfType<TextBlock>()
                .FirstOrDefault()?.Text
                ?? "Thank you for your enquiry. A member of our team will be in touch shortly.";
        }

        // ── Microsoft Graph draft send ────────────────────────────────────────

        public async Task SendDraftAsync(string draftId, string mailboxEmail)
        {
            await _graphClient.Users[mailboxEmail].Messages[draftId].Send.PostAsync();
            _logger.LogInformation("Draft {DraftId} sent from mailbox {Mailbox}", draftId, mailboxEmail);
        }

        // ── Microsoft Graph draft creation ────────────────────────────────────

        private async Task<string> CreateGraphDraftAsync(
            IncomingEmail email,
            string replyContent,
            string mailboxEmail)
        {
            var htmlBody = replyContent
                .Replace("\n\n", "</p><p>")
                .Replace("\n", "<br/>")
                .Trim();

            var draft = new GraphMessage
            {
                Subject = email.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase)
                    ? email.Subject
                    : $"Re: {email.Subject}",
                Body = new GraphItemBody
                {
                    ContentType = GraphBodyType.Html,
                    Content = $"<p>{htmlBody}</p>"
                },
                ToRecipients = new List<GraphRecipient>
                {
                    new()
                    {
                        EmailAddress = new GraphEmailAddress
                        {
                            Name = email.FromName,
                            Address = email.FromEmail
                        }
                    }
                }
            };

            // POST to /users/{mailbox}/messages creates a draft in the Drafts folder
            var created = await _graphClient.Users[mailboxEmail].Messages.PostAsync(draft);

            if (created?.Id is null)
                throw new InvalidOperationException("Graph API returned a null draft message ID.");

            _logger.LogInformation(
                "Auto-reply draft created for email {EmailId}, draft ID {DraftId}",
                email.EmailId, created.Id);

            return created.Id;
        }
    }
}
