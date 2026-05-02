namespace AwningsEmailFunction.Interfaces;

public interface IBlobEmailStorageService
{
    Task<string> UploadEmailBodyAsync(string emailId, string content, bool isHtml);
    Task<string> UploadAttachmentAsync(string emailId, string attachmentId, string fileName, byte[] contentBytes, string contentType);
    Task<string> DownloadEmailBodyAsync(string blobUrl);
}
