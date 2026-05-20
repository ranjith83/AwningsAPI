namespace AwningsAPI.Dto.Common
{
    public class OptionLookupDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int DisplayOrder { get; set; }
    }
}
