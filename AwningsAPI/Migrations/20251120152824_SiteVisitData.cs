using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class SiteVisitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteVisitValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteVisitValues", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SiteVisitValues",
                columns: new[] { "Id", "Category", "CreatedBy", "DateCreated", "DateUpdated", "DisplayOrder", "IsActive", "UpdatedBy", "Value" },
                values: new object[,]
                {
                    { 1, "Model", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Renson" },
                    { 2, "Model", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Practic" },
                    { 3, "Model", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Markilux" },
                    { 4, "Structure", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Free Standing" },
                    { 5, "Structure", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Mounted to Building" },
                    { 6, "WallType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Red Brick" },
                    { 7, "WallType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Block" },
                    { 8, "WallType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "External Insulation" },
                    { 9, "ExternalInsulation", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 10, "ExternalInsulation", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 11, "WallFinish", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Rendered" },
                    { 12, "WallFinish", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Smooth" },
                    { 13, "WallFinish", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Pebbledash" },
                    { 14, "FlashingRequired", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 15, "FlashingRequired", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 16, "StandOfBrackets", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 17, "StandOfBrackets", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 18, "Electrician", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Ours" },
                    { 19, "Electrician", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Own" },
                    { 20, "ElectricalConnection", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Plug in" },
                    { 21, "ElectricalConnection", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Hard Wired" },
                    { 22, "FixtureType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Face Fix" },
                    { 23, "FixtureType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Top Fix" },
                    { 24, "FixtureType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Recess" },
                    { 25, "Operation", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Manual" },
                    { 26, "Operation", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Motorised" },
                    { 27, "OperationSide", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Right" },
                    { 28, "OperationSide", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Left" },
                    { 29, "ValanceChoice", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 30, "ValanceChoice", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 31, "WindSensor", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Vibrabox" },
                    { 32, "WindSensor", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Anemometer" },
                    { 33, "ShadePlusRequired", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 34, "ShadePlusRequired", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 35, "ShadeType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Manual" },
                    { 36, "ShadeType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Motorised" },
                    { 37, "Lights", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 38, "Lights", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 39, "LightsType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Spot Lights" },
                    { 40, "LightsType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "LED Line" },
                    { 41, "LightsType", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Other" },
                    { 42, "Heater", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Yes" },
                    { 43, "Heater", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "No" },
                    { 44, "HeaterManufacturer", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Markilux" },
                    { 45, "HeaterManufacturer", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Bromic" },
                    { 46, "HeaterManufacturer", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Other" },
                    { 47, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "2kw" },
                    { 48, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "2.5kw" },
                    { 49, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "3kw" },
                    { 50, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4, true, null, "4kw" },
                    { 51, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5, true, null, "6kw" },
                    { 52, "HeaterOutput", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6, true, null, "Other" },
                    { 53, "RemoteControl", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "Handheld" },
                    { 54, "RemoteControl", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Wall Mounted" },
                    { 55, "ControllerBox", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "On" },
                    { 56, "ControllerBox", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Off" },
                    { 57, "ControllerBox", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "Dimmable" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteVisitValues_Category",
                table: "SiteVisitValues",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SiteVisitValues_Category_Value",
                table: "SiteVisitValues",
                columns: new[] { "Category", "Value" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteVisitValues");
        }
    }
}
