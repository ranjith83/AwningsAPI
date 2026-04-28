using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Workflow
{
    public class ProductItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string CreatedBy { get; set; }
    }
}
