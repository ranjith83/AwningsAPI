namespace AwningsAPI.Dto.Auth
{
    public class UserSignatureDto
    {
        public int? SignatureId { get; set; }

        /// <summary>Friendly label, e.g. "Formal".</summary>
        public string Label { get; set; } = string.Empty;

        // ── Contact fields ────────────────────────────────────────────────────
        public string? FullName { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        // ── Format choices ────────────────────────────────────────────────────
        /// <summary>"Kindest regards," | "Best wishes," | "Thanks," | custom</summary>
        public string GreetingText { get; set; } = "Kindest regards,";

        /// <summary>"blank_line" | "single_dash" | "double_dash" | "none"</summary>
        public string SeparatorStyle { get; set; } = "blank_line";

        /// <summary>"name_first" | "company_first"</summary>
        public string LayoutOrder { get; set; } = "name_first";

        // ── Final rendered plain-text (built by Angular, stored & used for email) ──
        public string SignatureText { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;

        // ── Read-only ─────────────────────────────────────────────────────────
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}