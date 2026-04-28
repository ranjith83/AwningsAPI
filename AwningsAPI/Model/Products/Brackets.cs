using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.Products
{
    public class Brackets
    {
        [Key]
        public int BracketId { get; set; }

        public int ProductId { get; set; }

        public string BracketName { get; set; } = string.Empty;

        public string? PartNumber { get; set; } = string.Empty;

        public decimal Price { get; set; }

        /// <summary>
        /// Links this bracket to the arm type recorded on the Projections row
        /// (Projections.ArmTypeId). When set, this bracket is only shown when
        /// the selected width/projection resolves to the matching ArmTypeId.
        /// Null = bracket is universal and shown for every arm type.
        /// </summary>
        public int? ArmTypeId { get; set; }

        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }
}