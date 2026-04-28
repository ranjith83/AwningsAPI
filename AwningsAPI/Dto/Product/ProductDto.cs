using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    /// <summary>
    /// Full product DTO used by both the workflow/addon API and the configuration API.
    /// ProductName is the display name used throughout the UI.
    /// Description holds the same value and is kept as an alias for the configuration API.
    /// </summary>
    public class ProductDto
    {
        public int ProductId { get; set; }

        /// <summary>Display name — e.g. "Markilux 990".</summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Alias for ProductName, used by the configuration API.
        /// Both properties are populated from the same underlying Description column.
        /// </summary>
        public string Description
        {
            get => ProductName;
            set => ProductName = value;
        }

        public int ProductTypeId { get; set; }

        public int SupplierId { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }
    }

    public class CreateProductDto
    {
        [Required][MaxLength(300)] public string Description { get; set; }
        [Required][Range(1, int.MaxValue)] public int ProductTypeId { get; set; }
        [Required][Range(1, int.MaxValue)] public int SupplierId { get; set; }
    }

    public class UpdateProductDto
    {
        [Required][MaxLength(300)] public string Description { get; set; }
        [Required][Range(1, int.MaxValue)] public int ProductTypeId { get; set; }
        [Required][Range(1, int.MaxValue)] public int SupplierId { get; set; }
    }
}