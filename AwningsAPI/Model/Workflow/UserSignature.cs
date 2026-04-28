using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Auth
{
    /// <summary>
    /// A named email signature belonging to one user.
    /// SignatureText is the final plain-text appended to emails.
    /// Format metadata (greeting, font, separator, etc.) lets the builder
    /// re-open a saved signature for editing faithfully.
    /// </summary>
    public class UserSignature
    {
        [Key]
        public int SignatureId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Label { get; set; } = string.Empty;

        // ── Contact fields ────────────────────────────────────────────────────
        [MaxLength(150)] public string? FullName { get; set; }
        [MaxLength(150)] public string? JobTitle { get; set; }
        [MaxLength(150)] public string? Company { get; set; }
        [MaxLength(50)] public string? Phone { get; set; }
        [MaxLength(50)] public string? Mobile { get; set; }
        [MaxLength(255)] public string? Email { get; set; }
        [MaxLength(255)] public string? Website { get; set; }

        // ── Format choices ────────────────────────────────────────────────────
        [MaxLength(100)] public string GreetingText { get; set; } = "Kindest regards,";

        /// <summary>"blank_line" | "single_dash" | "double_dash" | "none"</summary>
        [MaxLength(30)] public string SeparatorStyle { get; set; } = "blank_line";

        /// <summary>"name_first" | "company_first"</summary>
        [MaxLength(30)] public string LayoutOrder { get; set; } = "name_first";

        /// <summary>
        /// CSS font-family key chosen by the user in the builder.
        /// Stored as a short token (e.g. "georgia", "times", "arial") so the
        /// Angular preview and signature textarea can apply the correct typeface.
        /// Defaults to "georgia" — a classic email signature font.
        /// </summary>
        [MaxLength(60)] public string FontFamily { get; set; } = "georgia";

        // ── Final rendered text ───────────────────────────────────────────────
        [Required]
        public string SignatureText { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
    }
}