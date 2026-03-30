using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Supplier
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateSupplierDto
    {
        [Required][MaxLength(200)] public string SupplierName { get; set; }
    }

    public class UpdateSupplierDto
    {
        [Required][MaxLength(200)] public string SupplierName { get; set; }
    }
}
