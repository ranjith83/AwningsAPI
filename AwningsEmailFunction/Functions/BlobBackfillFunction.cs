using AwningsEmailFunction.Database;
using AwningsEmailFunction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AwningsEmailFunction.Functions;

public class BlobBackfillFunction
{
    private readonly EmailFunctionDbContext _context;
    private readonly IBlobEmailStorageService _blobService;
    private readonly ILogger<BlobBackfillFunction> _logger;

    private const int BatchSize = 50;

    public BlobBackfillFunction(
        EmailFunctionDbContext context,
        IBlobEmailStorageService blobService,
        ILogger<BlobBackfillFunction> logger)
    {
        _context = context;
        _blobService = blobService;
        _logger = logger;
    }

    // POST /api/backfill/run — manually trigger a batch
    [Function("BlobBackfillRun")]
    public async Task<HttpResponseData> RunManual(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "backfill/run")] HttpRequestData req)
    {
        _logger.LogInformation("Manual blob backfill triggered.");
        var result = await RunBackfillBatchAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }

    // GET /api/backfill/status — check how many emails still need migrating
    [Function("BlobBackfillStatus")]
    public async Task<HttpResponseData> Status(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "backfill/status")] HttpRequestData req)
    {
        var pendingEmails = await GetRemainingCountAsync();
        var pendingAttachments = await GetRemainingAttachmentCountAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            pendingEmailBodies = pendingEmails,
            pendingAttachments = pendingAttachments,
            isComplete = pendingEmails == 0 && pendingAttachments == 0,
            timestamp = DateTimeOffset.UtcNow
        });
        return response;
    }

    private async Task<BackfillResult> RunBackfillBatchAsync()
    {
        var result = new BackfillResult();

        // --- Backfill email bodies ---
        var emails = await _context.IncomingEmails
            .Where(e => e.BodyBlobUrl == null && e.BodyContent != null && e.BodyContent != "")
            .OrderBy(e => e.Id)
            .Take(BatchSize)
            .ToListAsync();

        foreach (var email in emails)
        {
            try
            {
                var url = await _blobService.UploadEmailBodyAsync(email.EmailId, email.BodyContent!, email.IsHtml);
                email.BodyBlobUrl = url;
                email.BodyContent = null;
                result.BodiesMigrated++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to migrate body for email {Id} ({EmailId})", email.Id, email.EmailId);
                result.BodiesFailed++;
            }
        }

        if (emails.Count > 0)
            await _context.SaveChangesAsync();

        // --- Backfill attachments ---
        var attachments = await _context.EmailAttachments
            .Where(a => a.BlobStorageUrl == null && a.Base64Content != null && a.Base64Content != "")
            .OrderBy(a => a.Id)
            .Take(BatchSize)
            .Include(a => a.IncomingEmail)
            .ToListAsync();

        foreach (var att in attachments)
        {
            try
            {
                var bytes = Convert.FromBase64String(att.Base64Content!);
                var url = await _blobService.UploadAttachmentAsync(
                    att.IncomingEmail.EmailId, att.AttachmentId, att.FileName, bytes, att.ContentType);

                att.BlobStorageUrl = url;
                att.Base64Content = null;
                result.AttachmentsMigrated++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to migrate attachment {Id} ({FileName})", att.Id, att.FileName);
                result.AttachmentsFailed++;
            }
        }

        if (attachments.Count > 0)
            await _context.SaveChangesAsync();

        result.RemainingEmailBodies = await GetRemainingCountAsync();
        result.RemainingAttachments = await GetRemainingAttachmentCountAsync();

        _logger.LogInformation(
            "Backfill batch complete — Bodies: {BodiesMigrated} migrated, {BodiesFailed} failed | " +
            "Attachments: {AttsMigrated} migrated, {AttsFailed} failed | " +
            "Remaining: {RemBodies} bodies, {RemAtts} attachments",
            result.BodiesMigrated, result.BodiesFailed,
            result.AttachmentsMigrated, result.AttachmentsFailed,
            result.RemainingEmailBodies, result.RemainingAttachments);

        return result;
    }

    private Task<int> GetRemainingCountAsync() =>
        _context.IncomingEmails.CountAsync(e => e.BodyBlobUrl == null && e.BodyContent != null && e.BodyContent != "");

    private Task<int> GetRemainingAttachmentCountAsync() =>
        _context.EmailAttachments.CountAsync(a => a.BlobStorageUrl == null && a.Base64Content != null && a.Base64Content != "");

    private class BackfillResult
    {
        public int BodiesMigrated { get; set; }
        public int BodiesFailed { get; set; }
        public int AttachmentsMigrated { get; set; }
        public int AttachmentsFailed { get; set; }
        public int RemainingEmailBodies { get; set; }
        public int RemainingAttachments { get; set; }
    }
}
