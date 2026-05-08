using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FrameColour
    {
        [Key]
        public int FrameColourId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>0 = white/light (extra charge applies), 1 = black/dark (no charge)</summary>
        public int ColorValue { get; set; }

        public decimal Price { get; set; }

        public int SortOrder { get; set; }

        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
