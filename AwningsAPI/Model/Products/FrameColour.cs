using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FrameColour
    {
        [Key]
        public int FrameColourId { get; set; }

        public int ProductId { get; set; }

        public int FrameColourOptionId { get; set; }

        /// <summary>true = price from NonStandardRALColours, false = included (no extra charge)</summary>
        public bool IsNonStandardRAL { get; set; }

        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }

        public FrameColourOption FrameColourOption { get; set; }
    }
}
