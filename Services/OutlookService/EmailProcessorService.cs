using AwningsAPI.Database;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Email;
using AwningsAPI.Services.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwningsAPI.Services.Email
{
    public class EmailProcessorService : IEmailProcessorService
    {
        private readonly IEmailReaderService _emailReaderService;
        private readonly IEmailAnalysisService _emailAnalysisService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailProcessorService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITaskService _taskService;

        public EmailProcessorService(
            IEmailReaderService emailReaderService,
            IEmailAnalysisService emailAnalysisService,
            ITaskService taskService,
            AppDbContext context,
            IConfiguration configuration,
            ILogger<EmailProcessorService> logger,
            HttpClient httpClient)
        {
            _emailReaderService = emailReaderService;
            _emailAnalysisService = emailAnalysisService;
            _taskService = taskService;
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task ProcessIncomingEmailsAsync()
        {
            try
            {
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new Exception("Monitored mailbox not configured");

                _logger.LogInformation($"Starting email processing for mailbox: {mailboxEmail}");

                // Get unread emails
                var emails = await _emailReaderService.GetUnreadEmailsAsync(mailboxEmail);

                if (!emails.Any())
                {
                    _logger.LogInformation("No unread emails to process");
                    return;
                }

                _logger.LogInformation($"Found {emails.Count} unread emails to process");

                foreach (var email in emails)
                {
                    try
                    {
                        await ProcessEmailAsync(email, mailboxEmail);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing email: {email.Subject}");

                        // Save error to database
                        email.ProcessingStatus = "Failed";
                        email.ErrorMessage = ex.Message;
                        email.DateProcessed = DateTime.UtcNow;

                        _context.IncomingEmails.Add(email);
                        await _context.SaveChangesAsync();
                    }
                }

                _logger.LogInformation("Email processing completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in email processing cycle");
                throw;
            }
        }

        public async Task ProcessSingleEmailAsync(string emailId)
        {
            try
            {
                var mailboxEmail = _configuration["AzureAd:MonitoredMailbox"]
                    ?? _configuration["AzureAd:OrganizerEmail"]
                    ?? throw new Exception("Monitored mailbox not configured");

                _logger.LogInformation($"Processing single email: {emailId}");

                var email = await _emailReaderService.GetCompleteEmailAsync(mailboxEmail, emailId);
                await ProcessEmailAsync(email, mailboxEmail);

                _logger.LogInformation($"Email {emailId} processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing email {emailId}");
                throw;
            }
        }

        private async Task ProcessEmailAsync(IncomingEmail email, string mailboxEmail)
        {
            try
            {
                _logger.LogInformation($"Processing email: {email.Subject} from {email.FromEmail}");

                // Update status
                email.ProcessingStatus = "Processing";

                // Download attachments if present
                if (email.HasAttachments && !email.Attachments.Any())
                {
                    var attachments = await _emailReaderService.DownloadAttachmentsAsync(mailboxEmail, email.EmailId);
                    email.Attachments = attachments;

                    // Extract text from attachments
                    await ExtractTextFromAttachmentsAsync(email);
                }

                // Analyze email with AI
                var analysisResult = await _emailAnalysisService.AnalyzeEmailAsync(email);

                // Update email with analysis results
                email.Category = analysisResult.Category;
                email.CategoryConfidence = analysisResult.Confidence;
                email.ExtractedData = JsonConvert.SerializeObject(analysisResult.ExtractedData);
                email.ProcessingStatus = "Completed";
                email.DateProcessed = DateTime.UtcNow;

                // Save to database
                _context.IncomingEmails.Add(email);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Email categorized as: {email.Category} (confidence: {email.CategoryConfidence:P})");

                // Route to appropriate API based on category
                //await RouteEmailAsync(email, analysisResult);

                // Create task from email
                await CreateTaskFromEmail(email, analysisResult);

                // Mark as read and move to processed folder
                await _emailReaderService.MarkEmailAsReadAsync(mailboxEmail, email.EmailId);
                await _emailReaderService.MoveEmailToFolderAsync(mailboxEmail, email.EmailId, $"Processed/{email.Category}");

                _logger.LogInformation($"Email {email.EmailId} processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing email {email.EmailId}");
                email.ProcessingStatus = "Failed";
                email.ErrorMessage = ex.Message;
                email.DateProcessed = DateTime.UtcNow;

                if (email.Id == 0) // Not yet in database
                {
                    _context.IncomingEmails.Add(email);
                }

                await _context.SaveChangesAsync();
                throw;
            }
        }

        private async Task CreateTaskFromEmail(IncomingEmail email, EmailAnalysisResult analysisResult)
        {
            try
            {
                _logger.LogInformation($"Creating task from email: {email.Subject}");

                // Create task using the service
                var task = await _taskService.CreateTaskFromEmailAsync(email.Id, "System");

                if (task != null)
                {
                    _logger.LogInformation($"Task created successfully: TaskId={task.TaskId}, Category={task.Category}, Status={task.Status}");
                }
                else
                {
                    _logger.LogWarning($"Task creation returned null for email {email.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating task from email {email.Id}");
                // Don't throw - email is still marked as processed
            }
        }

        private async Task ExtractTextFromAttachmentsAsync(IncomingEmail email)
        {
            foreach (var attachment in email.Attachments)
            {
                try
                {
                    if (string.IsNullOrEmpty(attachment.Base64Content))
                        continue;

                    var content = Convert.FromBase64String(attachment.Base64Content);

                    if (attachment.ContentType.Contains("pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"Extracting text from PDF: {attachment.FileName}");
                        attachment.ExtractedText = await _emailAnalysisService.ExtractTextFromPdfAsync(content);
                    }
                    else if (attachment.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"Extracting text from image: {attachment.FileName}");
                        attachment.ExtractedText = await _emailAnalysisService.ExtractTextFromImageAsync(content);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error extracting text from attachment: {attachment.FileName}");
                }
            }
        }

        private async Task RouteEmailAsync(IncomingEmail email, EmailAnalysisResult analysisResult)
        {
            try
            {
                var routingConfig = _configuration.GetSection($"EmailRouting:{email.Category}");
                var apiEndpoint = routingConfig["ApiEndpoint"];

                if (string.IsNullOrEmpty(apiEndpoint))
                {
                    _logger.LogWarning($"No API endpoint configured for category: {email.Category}");
                    return;
                }

                _logger.LogInformation($"Routing email to: {apiEndpoint}");

                // Prepare payload
                var payload = new
                {
                    emailId = email.Id,
                    category = email.Category,
                    confidence = email.CategoryConfidence,
                    subject = email.Subject,
                    from = new
                    {
                        name = email.FromName,
                        email = email.FromEmail
                    },
                    bodyPreview = email.BodyPreview,
                    bodyContent = email.BodyContent,
                    receivedDateTime = email.ReceivedDateTime,
                    importance = email.Importance,
                    hasAttachments = email.HasAttachments,
                    attachments = email.Attachments.Select(a => new
                    {
                        fileName = a.FileName,
                        contentType = a.ContentType,
                        size = a.Size,
                        extractedText = a.ExtractedText
                    }).ToList(),
                    extractedData = analysisResult.ExtractedData,
                    requiredActions = analysisResult.RequiredActions,
                    priority = analysisResult.Priority,
                    reasoning = analysisResult.Reasoning
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add custom headers if configured
                var headers = routingConfig.GetSection("Headers");
                foreach (var header in headers.GetChildren())
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                var response = await _httpClient.PostAsync(apiEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully routed email to {apiEndpoint}. Response: {response.StatusCode}");
                }
                else
                {
                    _logger.LogError($"Failed to route email to {apiEndpoint}. Status: {response.StatusCode}, Error: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error routing email to API for category: {email.Category}");
            }
        }
    }
}