using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Auth
{
    /// <summary>
    /// A named email signature belonging to one user.
    /// The SignatureText column stores the final rendered plain-text
    /// that is appended to outgoing emails.
    /// Format metadata (greeting, separator style, etc.) is stored so
    /// the builder UI can re-open a saved signature for editing.
    /// </summary>
    public class UserSignature
    {
        [Key]
        public int SignatureId { get; set; }

        /// <summary>JWT ClaimTypes.Name of the owning user.</summary>
        [Required]
        [MaxLength(255)]
        public string Username { get; set; } = string.Empty;

        /// <summary>Friendly label, e.g. "Formal" or "Quick reply".</summary>
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
        /// <summary>Greeting line, e.g. "Kindest regards," / "Best wishes," / "Thanks,"</summary>
        [MaxLength(100)] public string GreetingText { get; set; } = "Kindest regards,";

        /// <summary>
        /// Separator style between greeting and contact block.
        /// Values: "blank_line" | "single_dash" | "double_dash" | "none"
        /// </summary>
        [MaxLength(30)] public string SeparatorStyle { get; set; } = "blank_line";

        /// <summary>
        /// Layout order of contact lines.
        /// Values: "name_first" | "company_first"
        /// </summary>
        [MaxLength(30)] public string LayoutOrder { get; set; } = "name_first";

        // ── Final rendered text (written on every save) ───────────────────────
        /// <summary>
        /// Pre-rendered plain-text signature appended to emails.
        /// Generated from the fields above by the Angular builder.
        /// </summary>
        [Required]
        public string SignatureText { get; set; } = string.Empty;

        /// <summary>Pre-selected when the enquiry form opens.</summary>
        public bool IsDefault { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
    }
}