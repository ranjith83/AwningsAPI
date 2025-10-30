using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class Motors
    {
        [Key]
        public int MotorId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        //Navigation Properties
        public int ProductId { get; set; }
    }
}
