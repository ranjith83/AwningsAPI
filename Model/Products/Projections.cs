using AwningsAPI.Model.Suppliers;
using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class Projections
    {
        [Key]
        public int ProjectionId { get; set; }
        public int Width_cm { get; set; }
        public int Projection_cm { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        //Navigation property to Product
        public int ProductId { get; set; }  
        public Product Product { get; set; }

    }
}
