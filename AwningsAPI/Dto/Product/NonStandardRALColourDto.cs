using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class NonStandardRALColourDto
    {
        public int RALColourId { get; set; }
        public int ProductId { get; set; }
        public int WidthCm { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateNonStandardRALColourDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WidthCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
    }

    public class UpdateNonStandardRALColourDto
    {
        [Required][Range(1, int.MaxValue)] public int ProductId { get; set; }
        [Required][Range(1, int.MaxValue)] public int WidthCm { get; set; }
        [Range(0, double.MaxValue)] public decimal Price { get; set; }
    }
}
