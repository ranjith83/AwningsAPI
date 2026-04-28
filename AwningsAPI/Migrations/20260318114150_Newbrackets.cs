using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Newbrackets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 8, "Face fixture bracket 150 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71624", 42.70m, 6, null },
                    { 9, "Face fixture bracket 300 mm left", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "70617", 73.50m, 6, null },
                    { 10, "Face fixture bracket 300 mm right", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "70600", 73.50m, 6, null },
                    { 11, "Stand-off bkt. 80-300 mm for face fixture bracket 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77968", 220.50m, 6, null },
                    { 12, "Top fixture bracket 150 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71625", 42.70m, 6, null },
                    { 13, "Eaves fixture bracket 150mm, complete", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71669", 99.30m, 6, null },
                    { 14, "Eaves fixture bracket 270 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 77.00m, 6, null },
                    { 15, "Angle and plate for eaves fixture (machine finish)", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 125.20m, 6, null },
                    { 16, "Additional eaves fixture plate 60x260x12 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 42.60m, 6, null },
                    { 17, "Spreader plate A 430x160x12 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75326", 124.10m, 6, null },
                    { 18, "Spreader plate B 300x400x12 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75325", 160.20m, 6, null },
                    { 19, "Spacer block face or top fixt. 136x150x20 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.50m, 6, null },
                    { 20, "Spacer block face or top fixt. 136x150x12 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71644", 3.60m, 6, null },
                    { 21, "Cover plate 230x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 16.50m, 6, null },
                    { 22, "Cover plate 290x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71841", 20.50m, 6, null },
                    { 23, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 174.90m, 6, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 23);
        }
    }
}
