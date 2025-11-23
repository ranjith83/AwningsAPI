namespace AwningsAPI.Dto.SiteVisit
{
    public class SiteVisitDto
    {
        public int SiteVisitId { get; set; }
        public int WorkflowId { get; set; }
        public string ProductModelType { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? OtherPleaseSpecify { get; set; }

        // Product Model Section
        public string? SiteLayout { get; set; }
        public string? Structure { get; set; }
        public string? PassageHeight { get; set; }
        public string? Width { get; set; }
        public string? Projection { get; set; }
        public string? HeightAvailable { get; set; }
        public string? WallType { get; set; }
        public string? ExternalInsulation { get; set; }
        public string? WallFinish { get; set; }
        public string? WallThickness { get; set; }
        public string? SpecialBrackets { get; set; }
        public string? SideInfills { get; set; }
        public string? FlashingRequired { get; set; }
        public string? FlashingDimensions { get; set; }
        public string? StandOfBrackets { get; set; }
        public string? StandOfBracketDimension { get; set; }
        public string? Electrician { get; set; }
        public string? ElectricalConnection { get; set; }
        public string? Location { get; set; }
        public string? OtherSiteSurveyNotes { get; set; }

        // Model Details Section
        public string? FixtureType { get; set; }
        public string? Operation { get; set; }
        public string? CrankLength { get; set; }
        public string? OperationSide { get; set; }
        public string? Fabric { get; set; }
        public string? RAL { get; set; }
        public string? ValanceChoice { get; set; }
        public string? Valance { get; set; }
        public string? WindSensor { get; set; }

        // ShadePlus Section
        public string? ShadePlusRequired { get; set; }
        public string? ShadeType { get; set; }
        public string? ShadeplusFabric { get; set; }
        public string? ShadePlusAnyOtherDetail { get; set; }

        // Lights Section
        public string? Lights { get; set; }
        public string? LightsType { get; set; }
        public string? LightsAnyOtherDetails { get; set; }

        // Heater Section
        public string? Heater { get; set; }
        public string? HeaterManufacturer { get; set; }
        public string? NumberRequired { get; set; }
        public string? HeaterOutput { get; set; }
        public string? HeaterColour { get; set; }
        public string? RemoteControl { get; set; }
        public string? ControllerBox { get; set; }
        public string? HeaterAnyOtherDetails { get; set; }

        // Metadata
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? DateUpdated { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class CreateSiteVisitDto
    {
        public int WorkflowId { get; set; }
        public string ProductModelType { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? OtherPleaseSpecify { get; set; }
        public string? SiteLayout { get; set; }
        public string? Structure { get; set; }
        public string? PassageHeight { get; set; }
        public string? Width { get; set; }
        public string? Projection { get; set; }
        public string? HeightAvailable { get; set; }
        public string? WallType { get; set; }
        public string? ExternalInsulation { get; set; }
        public string? WallFinish { get; set; }
        public string? WallThickness { get; set; }
        public string? SpecialBrackets { get; set; }
        public string? SideInfills { get; set; }
        public string? FlashingRequired { get; set; }
        public string? FlashingDimensions { get; set; }
        public string? StandOfBrackets { get; set; }
        public string? StandOfBracketDimension { get; set; }
        public string? Electrician { get; set; }
        public string? ElectricalConnection { get; set; }
        public string? Location { get; set; }
        public string? OtherSiteSurveyNotes { get; set; }
        public string? FixtureType { get; set; }
        public string? Operation { get; set; }
        public string? CrankLength { get; set; }
        public string? OperationSide { get; set; }
        public string? Fabric { get; set; }
        public string? RAL { get; set; }
        public string? ValanceChoice { get; set; }
        public string? Valance { get; set; }
        public string? WindSensor { get; set; }
        public string? ShadePlusRequired { get; set; }
        public string? ShadeType { get; set; }
        public string? ShadeplusFabric { get; set; }
        public string? ShadePlusAnyOtherDetail { get; set; }
        public string? Lights { get; set; }
        public string? LightsType { get; set; }
        public string? LightsAnyOtherDetails { get; set; }
        public string? Heater { get; set; }
        public string? HeaterManufacturer { get; set; }
        public string? NumberRequired { get; set; }
        public string? HeaterOutput { get; set; }
        public string? HeaterColour { get; set; }
        public string? RemoteControl { get; set; }
        public string? ControllerBox { get; set; }
        public string? HeaterAnyOtherDetails { get; set; }
    }
}