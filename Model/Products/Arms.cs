using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Model.Products
{
    public class Arms
    {
        [Key]
        public int ArmId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }

        //Navigation Property
        public int BfId { get; set; }   
        public int ProductId { get; set; }  
    }
}
