using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AwningsEmailFunction.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AwningsEmailFunction.Services;

public class BlobEmailStorageService : IBlobEmailStorageService
{
    private readonly string _connectionString;
    private readonly string _containerName;
    private readonly ILogger<BlobEmailStorageService> _logger;

    public BlobEmailStorageService(IConfiguration configuration, ILogger<BlobEmailStorageService> logger)
    {
        _connectionString = configuration["BlobStorageConnectionString"]
            ?? throw new InvalidOperationException("BlobStorageConnectionString is not configured.");
        _containerName = configuration["BlobStorageContainerName"] ?? "awnings-emails";
        _logger = logger;
    }

    public async Task<string> UploadEmailBodyAsync(string emailId, string content, bool isHtml)
    {
        var extension = isHtml ? "html" : "txt";
        var contentType = isHtml ? "text/html" : "text/plain";
        var blobName = $"bodies/{emailId}.{extension}";

        var blobClient = await GetBlobClientAsync(blobName);
        var bytes = Encoding.UTF8.GetBytes(content);

        using var stream = new MemoryStream(bytes);
        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } });

        _logger.LogInformation("Uploaded email body to blob: {BlobName}", blobName);
        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadAttachmentAsync(string emailId, string attachmentId, string fileName, byte[] contentBytes, string contentType)
    {
        var blobName = $"attachments/{emailId}/{attachmentId}/{fileName}";

        var blobClient = await GetBlobClientAsync(blobName);

        using var stream = new MemoryStream(contentBytes);
        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } });

        _logger.LogInformation("Uploaded attachment to blob: {BlobName}", blobName);
        return blobClient.Uri.ToString();
    }

    public async Task<string> DownloadEmailBodyAsync(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        var path = uri.AbsolutePath.TrimStart('/');
        var slashIndex = path.IndexOf('/');
        var blobName = slashIndex >= 0 ? path[(slashIndex + 1)..] : path;

        var blobClient = new BlobClient(_connectionString, _containerName, blobName);
        var response = await blobClient.DownloadContentAsync();
        return response.Value.Content.ToString();
    }

    private async Task<BlobClient> GetBlobClientAsync(string blobName)
    {
        var containerClient = new BlobContainerClient(_connectionString, _containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        return containerClient.GetBlobClient(blobName);
    }
}
