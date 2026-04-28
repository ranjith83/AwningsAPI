using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class RadioControlledMotors
    {
        [Key]
        public int RadioMotorId { get; set; }
        public string Description { get; set; }
        public int Width_cm { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation property
        public int ProductId { get; set; }  
    }
}
