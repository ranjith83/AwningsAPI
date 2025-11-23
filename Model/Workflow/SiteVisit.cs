using AwningsAPI.Model.Workflow;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwningsAPI.Model.SiteVisit
{
    [Table("SiteVisits")]
    public class SiteVisit
    {
        [Key]
        public int SiteVisitId { get; set; }

        [Required]
        public int WorkflowId { get; set; }

        [ForeignKey("WorkflowId")]
        public virtual WorkflowStart? Workflow { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductModelType { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Model { get; set; }

        [MaxLength(500)]
        public string? OtherPleaseSpecify { get; set; }

        // Product Model Section
        [MaxLength(1000)]
        public string? SiteLayout { get; set; }

        [MaxLength(100)]
        public string? Structure { get; set; }

        [MaxLength(50)]
        public string? PassageHeight { get; set; }

        [MaxLength(50)]
        public string? Width { get; set; }

        [MaxLength(50)]
        public string? Projection { get; set; }

        [MaxLength(50)]
        public string? HeightAvailable { get; set; }

        [MaxLength(100)]
        public string? WallType { get; set; }

        [MaxLength(50)]
        public string? ExternalInsulation { get; set; }

        [MaxLength(100)]
        public string? WallFinish { get; set; }

        [MaxLength(50)]
        public string? WallThickness { get; set; }

        [MaxLength(500)]
        public string? SpecialBrackets { get; set; }

        [MaxLength(500)]
        public string? SideInfills { get; set; }

        [MaxLength(50)]
        public string? FlashingRequired { get; set; }

        [MaxLength(200)]
        public string? FlashingDimensions { get; set; }

        [MaxLength(50)]
        public string? StandOfBrackets { get; set; }

        [MaxLength(200)]
        public string? StandOfBracketDimension { get; set; }

        [MaxLength(50)]
        public string? Electrician { get; set; }

        [MaxLength(100)]
        public string? ElectricalConnection { get; set; }

        [MaxLength(500)]
        public string? Location { get; set; }

        [MaxLength(2000)]
        public string? OtherSiteSurveyNotes { get; set; }

        // Model Details Section
        [MaxLength(100)]
        public string? FixtureType { get; set; }

        [MaxLength(50)]
        public string? Operation { get; set; }

        [MaxLength(50)]
        public string? CrankLength { get; set; }

        [MaxLength(50)]
        public string? OperationSide { get; set; }

        [MaxLength(200)]
        public string? Fabric { get; set; }

        [MaxLength(100)]
        public string? RAL { get; set; }

        [MaxLength(50)]
        public string? ValanceChoice { get; set; }

        [MaxLength(200)]
        public string? Valance { get; set; }

        [MaxLength(100)]
        public string? WindSensor { get; set; }

        // ShadePlus Section
        [MaxLength(50)]
        public string? ShadePlusRequired { get; set; }

        [MaxLength(50)]
        public string? ShadeType { get; set; }

        [MaxLength(200)]
        public string? ShadeplusFabric { get; set; }

        [MaxLength(1000)]
        public string? ShadePlusAnyOtherDetail { get; set; }

        // Lights Section
        [MaxLength(50)]
        public string? Lights { get; set; }

        [MaxLength(100)]
        public string? LightsType { get; set; }

        [MaxLength(1000)]
        public string? LightsAnyOtherDetails { get; set; }

        // Heater Section
        [MaxLength(50)]
        public string? Heater { get; set; }

        [MaxLength(100)]
        public string? HeaterManufacturer { get; set; }

        [MaxLength(50)]
        public string? NumberRequired { get; set; }

        [MaxLength(50)]
        public string? HeaterOutput { get; set; }

        [MaxLength(100)]
        public string? HeaterColour { get; set; }

        [MaxLength(100)]
        public string? RemoteControl { get; set; }

        [MaxLength(100)]
        public string? ControllerBox { get; set; }

        [MaxLength(1000)]
        public string? HeaterAnyOtherDetails { get; set; }

        // Audit Fields
        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? DateUpdated { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }

    [Table("SiteVisitValues")]
    public class SiteVisitValues
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty; // e.g., "Structure", "WallType", "Model"

        [Required]
        [MaxLength(200)]
        public string Value { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? DateUpdated { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }

}