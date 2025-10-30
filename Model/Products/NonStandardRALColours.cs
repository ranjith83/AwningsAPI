using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class NonStandardRALColours
    {
        [Key]
        public int RALColourId { get; set; }
        public int WidthCm { get; set; }
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        //Navigation Properties
        public int ProductId { get; set; }
    }
}
