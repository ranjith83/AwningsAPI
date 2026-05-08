using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FrameColour
    {
        [Key]
        public int FrameColourId { get; set; }
        public int ProductId { get; set; }
        public int WidthCm { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
