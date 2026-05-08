using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlterFrameColourStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WidthCm",
                table: "FrameColours",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "FrameColours",
                newName: "ColorValue");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FrameColours",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Seed 11 frame colour options.
            // ColorValue: 1 = black/dark (no surcharge), 0 = white/light (surcharge applies).
            // Prices are placeholders — update via configuration screen.
            migrationBuilder.InsertData(
                table: "FrameColours",
                columns: new[] { "FrameColourId", "Description", "ColorValue", "Price", "SortOrder", "DateCreated", "CreatedBy" },
                values: new object[,]
                {
                    { 1,  "Anthracite",           1, 0m,   1,  new DateTime(2026, 5, 8), "System" },
                    { 2,  "Jet Black",             1, 0m,   2,  new DateTime(2026, 5, 8), "System" },
                    { 3,  "White",                 0, 100m, 3,  new DateTime(2026, 5, 8), "System" },
                    { 4,  "Traffic White",         0, 100m, 4,  new DateTime(2026, 5, 8), "System" },
                    { 5,  "Cream White",           0, 100m, 5,  new DateTime(2026, 5, 8), "System" },
                    { 6,  "Silver Grey",           0, 100m, 6,  new DateTime(2026, 5, 8), "System" },
                    { 7,  "Light Grey",            0, 100m, 7,  new DateTime(2026, 5, 8), "System" },
                    { 8,  "Bronze Brown",          0, 100m, 8,  new DateTime(2026, 5, 8), "System" },
                    { 9,  "Beige",                 0, 100m, 9,  new DateTime(2026, 5, 8), "System" },
                    { 10, "Moss Green",            0, 100m, 10, new DateTime(2026, 5, 8), "System" },
                    { 11, "Custom RAL",            0, 150m, 11, new DateTime(2026, 5, 8), "System" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "FrameColours", keyColumn: "FrameColourId", keyValues: new object[] { 1,2,3,4,5,6,7,8,9,10,11 });

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FrameColours");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "FrameColours",
                newName: "WidthCm");

            migrationBuilder.RenameColumn(
                name: "ColorValue",
                table: "FrameColours",
                newName: "ProductId");
        }
    }
}
