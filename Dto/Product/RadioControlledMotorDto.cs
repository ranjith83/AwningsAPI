using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class RadioControlledMotorDto
    {
        public int RadioMotorId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public int WidthCm { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateRadioControlledMotorDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
        [Range(0, int.MaxValue)] public int WidthCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
    }

    public class UpdateRadioControlledMotorDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][MaxLength(300)] public string Description { get; set; }
        [Range(0, int.MaxValue)] public int WidthCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
    }
}
