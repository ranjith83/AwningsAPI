namespace AwningsAPI.Dto.Product
{
    /// <summary>
    /// Used by both the product addon API (read-only) and the configuration API (CRUD).
    /// All fields from the Brackets table are exposed; callers that only need the
    /// original three fields (BracketId, BracketName, Price) can simply ignore the rest.
    /// </summary>
    public class BracketDto
    {
        public int BracketId { get; set; }

        /// <summary>The product this bracket belongs to.</summary>
        public int ProductId { get; set; }

        public string BracketName { get; set; }

        /// <summary>Manufacturer part / catalogue number.  Empty string when not set.</summary>
        public string PartNumber { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }
    }
}