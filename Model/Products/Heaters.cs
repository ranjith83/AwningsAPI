using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class Heaters
    {
        [Key]
        public int HeaterId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceNonRALColour { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        //Navigation Property
        public int ProductId { get; set; }
    }
}
