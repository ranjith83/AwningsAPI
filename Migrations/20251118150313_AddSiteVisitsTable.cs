using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteVisitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteVisits",
                columns: table => new
                {
                    SiteVisitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    ProductModelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OtherPleaseSpecify = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SiteLayout = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Structure = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PassageHeight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Width = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Projection = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeightAvailable = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WallType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalInsulation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WallFinish = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WallThickness = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SpecialBrackets = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SideInfills = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FlashingRequired = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FlashingDimensions = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StandOfBrackets = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandOfBracketDimension = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Electrician = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ElectricalConnection = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OtherSiteSurveyNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FixtureType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CrankLength = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OperationSide = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fabric = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RAL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ValanceChoice = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Valance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WindSensor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShadePlusRequired = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShadeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShadeplusFabric = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShadePlusAnyOtherDetail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Lights = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LightsType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LightsAnyOtherDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Heater = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaterManufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumberRequired = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaterOutput = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeaterColour = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RemoteControl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ControllerBox = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HeaterAnyOtherDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteVisits", x => x.SiteVisitId);
                    table.ForeignKey(
                        name: "FK_SiteVisits_WorkflowStarts_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "WorkflowStarts",
                        principalColumn: "WorkflowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteVisits_WorkflowId",
                table: "SiteVisits",
                column: "WorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteVisits");
        }
    }
}
