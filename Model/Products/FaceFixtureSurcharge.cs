using AwningsAPI.Model.Suppliers;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FaceFixtureSurcharge
    {
        [Key]
        public int FaceFixtureSurchargeId { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        //Navigation Properties
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
