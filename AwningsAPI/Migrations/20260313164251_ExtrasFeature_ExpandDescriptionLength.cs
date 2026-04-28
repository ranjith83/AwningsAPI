using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExtrasFeature_ExpandDescriptionLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 7);

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

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for face fixture", "", 86m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for face fixture incl. spreader plate A", "", 334m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for face fixture incl. spreader plate B", "", 406m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for top fixture", "", 86m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for eaves fixture", "", 199m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for arms with bionic tendon", "", 117m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Surcharge for bespoke arms", "", 177m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Arms",
                columns: new[] { "ArmId", "ArmTypeId", "BfId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, 1, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for face fixture", 86m, 6, null },
                    { 2, 1, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for face fixture incl. spreader plate A", 334m, 6, null },
                    { 3, 1, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for face fixture incl. spreader plate B", 406m, 6, null },
                    { 4, 1, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for top fixture", 86m, 6, null },
                    { 5, 1, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for eaves fixture", 199m, 6, null },
                    { 6, 1, 0, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for arms with bionic tendon", 117m, 6, null },
                    { 7, 1, 0, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for bespoke arms", 177m, 6, null }
                });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Face fixture bracket 150 mm / 3", "71624", 42.70m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Face fixture bracket 300 mm left / 4", "70617", 73.50m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Face fixture bracket 300 mm right / 4", "70600", 73.50m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 300 mm / 4", "77968", 220.50m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Top fixture bracket 150 mm / 4", "71625", 42.70m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Eaves fixture bracket 150mm, complete / 4", "71669", 99.30m });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                columns: new[] { "BracketName", "PartNumber", "Price" },
                values: new object[] { "Eaves fixture bracket 270 mm /4", "71659", 77.00m });

            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 8, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 125.20m, 6, null },
                    { 9, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 42.60m, 6, null },
                    { 10, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75326", 124.10m, 6, null },
                    { 11, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75325", 160.20m, 6, null },
                    { 12, "Spacer block face or top fixt 136x150x20 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.50m, 6, null },
                    { 13, "Spacer block face or top fixt 136x150x12 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71644", 3.60m, 6, null },
                    { 14, "Cover plate 230x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 16.50m, 6, null },
                    { 15, "Cover plate 290x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71841", 20.50m, 6, null },
                    { 16, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 174.90m, 6, null }
                });
        }
    }
}
