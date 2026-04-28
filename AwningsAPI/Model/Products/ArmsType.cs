using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class ArmsType
    {
        [Key]
        public int ArmTypeId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
