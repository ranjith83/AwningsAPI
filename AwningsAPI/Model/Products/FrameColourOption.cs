using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class FrameColourOption
    {
        [Key]
        public int FrameColourOptionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
