using AwningsAPI.Model.Suppliers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Products
{
    [Table("LightingCassettes")]
    public class LightingCassette
    {
        [Key]
        public int LightingId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        [MaxLength(255)]
        public string? UpdatedBy { get; set; }
    }
}