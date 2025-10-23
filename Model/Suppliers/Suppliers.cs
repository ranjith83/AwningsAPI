using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Suppliers
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; } 
        public string SupplierName { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }  
        public DateTime Updated { get; set; }
        public int UpdatedBy { get; set; }

        //Naviagtion property: One Supplier has many Products
        public ICollection<Product> Products { get; set; }
    }
}
