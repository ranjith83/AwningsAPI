using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Email
{
    public class IncomingEmail
    {
        [Key]
        public int Id { get; set; }

        public string EmailId { get; set; } = string.Empty; // Microsoft Graph ID

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string FromEmail { get; set; } = string.Empty;

        public string FromName { get; set; } = string.Empty;

        public string BodyPreview { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(MAX)")]
        public string BodyContent { get; set; } = string.Empty;

        public bool IsHtml { get; set; }

        public DateTime ReceivedDateTime { get; set; }

        public bool HasAttachments { get; set; }

        public string Importance { get; set; } = "Normal";

        // Processing status
        public string ProcessingStatus { get; set; } = "Pending"; // Pending, Processing, Completed, Failed

        public string? Category { get; set; } // AI-determined category

        public double? CategoryConfidence { get; set; } // AI confidence score

        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExtractedData { get; set; } // JSON of extracted data

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateProcessed { get; set; }

        public string? ErrorMessage { get; set; }

        // Navigation properties
        public virtual ICollection<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }

    public class EmailAttachment
    {
        [Key]
        public int Id { get; set; }

        public int IncomingEmailId { get; set; }

        [Required]
        public string AttachmentId { get; set; } = string.Empty; // Microsoft Graph ID

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Size { get; set; }

        public string? BlobStorageUrl { get; set; } // If using Azure Blob Storage

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Base64Content { get; set; } // Or store as base64

        public bool IsInline { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? ExtractedText { get; set; } // Text extracted from PDF/images

        public DateTime DateDownloaded { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("IncomingEmailId")]
        public virtual IncomingEmail IncomingEmail { get; set; } = null!;
    }

    // DTO for AI analysis response
    public class EmailAnalysisResult
    {
        public string Category { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public string Reasoning { get; set; } = string.Empty;
        public Dictionary<string, object> ExtractedData { get; set; } = new();
        public List<string> RequiredActions { get; set; } = new();
        public string Priority { get; set; } = "Normal"; // Low, Normal, High, Urgent
    }

    // Categories enum for type safety
    public static class EmailCategory
    {
        public const string InvoiceCreation = "invoice_creation";
        public const string QuoteCreation = "quote_creation";
        public const string CustomerCreation = "customer_creation";
        public const string ShowroomBooking = "showroom_booking";
        public const string GeneralInquiry = "general_inquiry";
        public const string ProductInquiry = "product_inquiry";
        public const string Complaint = "complaint";
        public const string Unknown = "unknown";
    }
}