using AwningsAPI.Model.Suppliers;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FixingPoints
    {
        [Key]
        public int FixingPointId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PartNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        //Navigation property   
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
