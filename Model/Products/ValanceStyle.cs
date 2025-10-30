using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class ValanceStyle
    {
        [Key]
        public int ValanceStyleId { get; set; } 
        public int WidthCm { get; set; }
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        //Navigation Properties
        public int ProductId { get; set; }
    }
}
