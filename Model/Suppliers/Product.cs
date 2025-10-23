using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Suppliers
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Description { get; set; } 
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        // Navigation property: One ProductType has many Products
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey("ProductTypeId")]
        public int ProductTypeId { get; set; }
    }
}
