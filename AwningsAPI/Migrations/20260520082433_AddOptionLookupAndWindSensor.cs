using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionLookupAndWindSensor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WindSensorOption",
                table: "Quotes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WindSensorOption",
                table: "Invoices",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OptionLookups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionLookups", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OptionLookups",
                columns: new[] { "Id", "Category", "DisplayOrder", "IsActive", "Label", "Price", "Value" },
                values: new object[,]
                {
                    { 1, "WindSensor", 1, true, "No", null, "No" },
                    { 2, "WindSensor", 2, true, "Yes - €250", 250m, "Yes" },
                    { 3, "WindSensor", 3, true, "Free of charge", 0m, "Free" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionLookups");

            migrationBuilder.DropColumn(
                name: "WindSensorOption",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "WindSensorOption",
                table: "Invoices");
        }
    }
}
