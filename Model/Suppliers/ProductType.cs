using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Suppliers
{
    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string Description { get; set; } 
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }   
        public string? UpdatedBy { get; set; }

        //Navigation property: One ProductType has many Products
        public int SupplierId { get; set; } 
        public ICollection<Product> Products { get; set; }
    }
}
