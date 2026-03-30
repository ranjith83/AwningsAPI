using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class ArmDto
    {
        public int ArmId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ArmTypeId { get; set; }
        public int BfId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateArmDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
        [Range(0, int.MaxValue)] public int ArmTypeId { get; set; }
        [Range(0, int.MaxValue)] public int BfId { get; set; }
    }

    public class UpdateArmDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
        [Range(0, int.MaxValue)] public int ArmTypeId { get; set; }
        [Range(0, int.MaxValue)] public int BfId { get; set; }
    }
}
