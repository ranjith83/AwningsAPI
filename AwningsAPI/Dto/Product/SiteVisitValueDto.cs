using System.ComponentModel.DataAnnotations;

namespace AwningsAPI.Dto.Product
{
    public class SiteVisitValueDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateSiteVisitValueDto
    {
        [Required][MaxLength(100)] public string Category { get; set; }
        [Required][MaxLength(200)] public string Value { get; set; }
        [Range(1, int.MaxValue)] public int DisplayOrder { get; set; } = 1;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateSiteVisitValueDto
    {
        [Required][MaxLength(100)] public string Category { get; set; }
        [Required][MaxLength(200)] public string Value { get; set; }
        [Range(1, int.MaxValue)] public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
