using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class ProjectionDto
    {
        public int ProjectionId { get; set; }
        public int ProductId { get; set; }
        public int WidthCm { get; set; }
        public int ProjectionCm { get; set; }
        public decimal Price { get; set; }
        public int ArmTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateProjectionDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WidthCm { get; set; }
        [Required][Range(1, int.MaxValue)] public int ProjectionCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
        [Range(0, int.MaxValue)] public int ArmTypeId { get; set; }
    }

    public class UpdateProjectionDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WidthCm { get; set; }
        [Required][Range(1, int.MaxValue)] public int ProjectionCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
        [Range(0, int.MaxValue)] public int ArmTypeId { get; set; }
    }
}
