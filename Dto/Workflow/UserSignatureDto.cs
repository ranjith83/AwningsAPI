namespace AwningsAPI.Dto.Auth
{
    public class UserSignatureDto
    {
        public int? SignatureId { get; set; }
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
        public string GreetingText { get; set; } = "Kindest regards,";
        public string SeparatorStyle { get; set; } = "blank_line";
        public string LayoutOrder { get; set; } = "name_first";

        /// <summary>
        /// Font token chosen in the builder, e.g. "georgia", "times", "arial".
        /// Used by Angular to render the preview and signature textarea in the
        /// correct typeface. Defaults to "georgia".
        /// </summary>
        public string FontFamily { get; set; } = "georgia";

        public string SignatureText { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}