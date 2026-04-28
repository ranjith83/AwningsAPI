using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    /// <summary>
    /// Full product-type DTO used by both the workflow API and the configuration API.
    /// </summary>
    public class ProductTypeDto
    {
        public int ProductTypeId { get; set; }

        public string Description { get; set; }

        /// <summary>The supplier this product type belongs to.</summary>
        public int SupplierId { get; set; }

        public DateTime DateCreated { get; set; }

        public string CreatedBy { get; set; }
    }

    public class CreateProductTypeDto
    {
        [Required][Range(1, int.MaxValue)] public int SupplierId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
    }

    public class UpdateProductTypeDto
    {
        [Required][Range(1, int.MaxValue)] public int SupplierId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
    }
}