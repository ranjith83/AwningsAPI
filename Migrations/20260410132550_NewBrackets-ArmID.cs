using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewBracketsArmID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "Brackets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ArmTypeId",
                table: "Brackets",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 10,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 11,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 12,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 13,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 14,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 15,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 16,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 17,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 18,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 19,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 20,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 21,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 22,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 23,
                column: "ArmTypeId",
                value: null);

            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "ArmTypeId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 78, 1, "Surcharge for face fixture", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 220.00m, 7, null },
                    { 79, 1, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 592.00m, 7, null },
                    { 80, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 550.00m, 7, null },
                    { 81, 1, "Surcharge for face fixture incl. spreader plate C", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 592.00m, 7, null },
                    { 82, 1, "Surcharge for top fixture", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 278.00m, 7, null },
                    { 83, 1, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 6, 13, 41, 50, 820, DateTimeKind.Unspecified), null, null, 371.00m, 7, null },
                    { 110, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 183.00m, 1, null },
                    { 111, 2, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 183.00m, 1, null },
                    { 112, 3, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 269.00m, 1, null },
                    { 113, 1, "Surcharge for face fixture A", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 22.00m, 1, null },
                    { 114, 2, "Surcharge for face fixture A", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 22.00m, 1, null },
                    { 115, 3, "Surcharge for face fixture A", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 32.00m, 1, null },
                    { 116, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 330.00m, 1, null },
                    { 117, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 339.00m, 1, null },
                    { 118, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 19, 20, 47, 870, DateTimeKind.Unspecified), null, null, 504.00m, 1, null },
                    { 119, 1, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 44.00m, 2, null },
                    { 120, 2, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 44.00m, 2, null },
                    { 121, 3, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 66.00m, 2, null },
                    { 122, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 330.00m, 2, null },
                    { 123, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 339.00m, 2, null },
                    { 124, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 504.00m, 2, null },
                    { 125, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 183.00m, 2, null },
                    { 126, 2, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 183.00m, 2, null },
                    { 127, 3, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 18, 15, 206, DateTimeKind.Unspecified), null, null, 269.00m, 2, null },
                    { 135, 4, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 88.00m, 13, null },
                    { 136, 6, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 88.00m, 13, null },
                    { 137, 7, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 132.00m, 13, null },
                    { 138, 4, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 659.00m, 13, null },
                    { 139, 6, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 677.00m, 13, null },
                    { 140, 7, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 1007.00m, 13, null },
                    { 141, 4, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 361.00m, 13, null },
                    { 142, 6, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 361.00m, 13, null },
                    { 143, 7, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 20, 49, 38, 263, DateTimeKind.Unspecified), null, null, 536.00m, 13, null },
                    { 146, 1, "Surcharge for face fixture", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 220.00m, 3, null },
                    { 147, 1, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 592.00m, 3, null },
                    { 148, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 550.00m, 3, null },
                    { 149, 1, "Surcharge for face fixture incl. spreader plate C", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 592.00m, 3, null },
                    { 150, 1, "Surcharge for top fixture", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 287.00m, 3, null },
                    { 151, 1, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 371.00m, 3, null },
                    { 152, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 183.00m, 3, null },
                    { 153, 1, "Surcharge for the two-tone housing, markilux \"MX- colour\" in the colour combinations 1-10", "System", new DateTime(2026, 4, 7, 21, 7, 50, 610, DateTimeKind.Unspecified), null, null, 300.00m, 3, null },
                    { 154, 1, "Face fixture bracket left / 3", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "72826", 109.80m, 3, null },
                    { 155, 1, "Face fixture bracket right / 3", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "72827", 109.80m, 3, null },
                    { 156, 1, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "72872", 253.50m, 3, null },
                    { 157, 1, "Top fixture bracket left / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "60523", 143.20m, 3, null },
                    { 158, 1, "Top fixture bracket right / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "60524", 143.20m, 3, null },
                    { 159, 1, "Eaves fixture bracket left 150 mm, complete / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "60603", 185.30m, 3, null },
                    { 160, 1, "Eaves fixture bracket right 150 mm, complete / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "60604", 185.30m, 3, null },
                    { 161, 1, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "71659", 79.30m, 3, null },
                    { 162, 1, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "716620", 128.90m, 3, null },
                    { 163, 1, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "75383", 43.90m, 3, null },
                    { 164, 1, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "72870", 186.00m, 3, null },
                    { 165, 1, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "73465", 164.90m, 3, null },
                    { 166, 1, "Spreader Plate C 310x130x12 mm / 6", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "72526", 186.00m, 3, null },
                    { 167, 1, "Spacer block face fixture 100x120x20 mm / 3", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "718581", 14.70m, 3, null },
                    { 168, 1, "Spacer block face fixture 100x120x12 mm / 3", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "718571", 14.30m, 3, null },
                    { 169, 1, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "716311", 4.40m, 3, null },
                    { 170, 1, "Spacer block for top fixture 90x140x12 mm / 4", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "716411", 5.00m, 3, null },
                    { 171, 1, "Cover plate 230x210x2 mm", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "71843", 17.00m, 3, null },
                    { 172, 1, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2026, 4, 7, 21, 19, 27, 960, DateTimeKind.Unspecified), null, "62421", 180.00m, 3, null },
                    { 173, 4, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 596.00m, 4, null },
                    { 174, 5, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 894.00m, 4, null },
                    { 175, 7, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1192.00m, 4, null },
                    { 176, 4, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1107.00m, 4, null },
                    { 177, 5, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1421.00m, 4, null },
                    { 178, 7, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1975.00m, 4, null },
                    { 179, 4, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1256.00m, 4, null },
                    { 180, 5, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1570.00m, 4, null },
                    { 181, 7, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 2198.00m, 4, null },
                    { 182, 4, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 794.00m, 4, null },
                    { 183, 5, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1191.00m, 4, null },
                    { 184, 7, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1588.00m, 4, null },
                    { 185, 4, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 950.00m, 4, null },
                    { 186, 5, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1424.00m, 4, null },
                    { 187, 7, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 1899.00m, 4, null },
                    { 188, 4, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 361.00m, 4, null },
                    { 189, 5, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 361.00m, 4, null },
                    { 190, 7, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 17, 43, 59, 706, DateTimeKind.Unspecified), null, null, 536.00m, 4, null },
                    { 193, 1, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 298.00m, 14, null },
                    { 194, 8, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 447.00m, 14, null },
                    { 195, 3, "Surcharge for face fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 596.00m, 14, null },
                    { 196, 1, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 554.00m, 14, null },
                    { 197, 8, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 711.00m, 14, null },
                    { 198, 3, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 988.00m, 14, null },
                    { 199, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 628.00m, 14, null },
                    { 200, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 785.00m, 14, null },
                    { 201, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 1099.00m, 14, null },
                    { 202, 1, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 397.00m, 14, null },
                    { 203, 8, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 596.00m, 14, null },
                    { 204, 3, "Surcharge for top fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 794.00m, 14, null },
                    { 205, 1, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 475.00m, 14, null },
                    { 206, 8, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 712.00m, 14, null },
                    { 207, 3, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 950.00m, 14, null },
                    { 208, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 183.00m, 14, null },
                    { 209, 8, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 183.00m, 14, null },
                    { 210, 3, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 8, 19, 48, 32, 840, DateTimeKind.Unspecified), null, null, 269.00m, 14, null },
                    { 226, 1, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 263.00m, 8, null },
                    { 227, 8, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 394.00m, 8, null },
                    { 228, 3, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 525.00m, 8, null },
                    { 229, 1, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 518.00m, 8, null },
                    { 230, 8, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 653.00m, 8, null },
                    { 231, 3, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 912.00m, 8, null },
                    { 232, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 593.00m, 8, null },
                    { 233, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 728.00m, 8, null },
                    { 234, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 1024.00m, 8, null },
                    { 235, 1, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 326.00m, 8, null },
                    { 236, 8, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 489.00m, 8, null },
                    { 237, 3, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 652.00m, 8, null },
                    { 238, 1, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 404.00m, 8, null },
                    { 239, 8, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 605.00m, 8, null },
                    { 240, 3, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 807.00m, 8, null },
                    { 241, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 183.00m, 8, null },
                    { 242, 8, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 183.00m, 8, null },
                    { 243, 3, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 269.00m, 8, null },
                    { 244, 1, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 121.00m, 8, null },
                    { 245, 8, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 121.00m, 8, null },
                    { 246, 3, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 8, 23, 24, 643, DateTimeKind.Unspecified), null, null, 177.00m, 8, null },
                    { 262, 4, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 525.00m, 15, null },
                    { 263, 5, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 788.00m, 15, null },
                    { 264, 7, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1050.00m, 15, null },
                    { 265, 4, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1036.00m, 15, null },
                    { 266, 5, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1306.00m, 15, null },
                    { 267, 7, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1824.00m, 15, null },
                    { 268, 4, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1185.00m, 15, null },
                    { 269, 5, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1455.00m, 15, null },
                    { 270, 7, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 2047.00m, 15, null },
                    { 271, 4, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 652.00m, 15, null },
                    { 272, 5, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 978.00m, 15, null },
                    { 273, 7, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1304.00m, 15, null },
                    { 274, 4, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 807.00m, 15, null },
                    { 275, 5, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1210.00m, 15, null },
                    { 276, 7, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 1613.00m, 15, null },
                    { 277, 4, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 361.00m, 15, null },
                    { 278, 5, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 361.00m, 15, null },
                    { 279, 7, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 536.00m, 15, null },
                    { 280, 4, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 234.00m, 15, null },
                    { 281, 5, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 234.00m, 15, null },
                    { 282, 7, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 9, 9, 15, 44, 326, DateTimeKind.Unspecified), null, null, 348.00m, 15, null },
                    { 305, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 51, 21, 433, DateTimeKind.Unspecified), null, null, 183.00m, 15, null },
                    { 306, 9, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 51, 21, 433, DateTimeKind.Unspecified), null, null, 183.00m, 15, null },
                    { 307, 11, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 51, 21, 433, DateTimeKind.Unspecified), null, null, 183.00m, 15, null },
                    { 308, 10, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 51, 21, 433, DateTimeKind.Unspecified), null, null, 269.00m, 15, null },
                    { 309, 1, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 79.00m, 9, null },
                    { 310, 9, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 130.00m, 9, null },
                    { 311, 11, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 156.00m, 9, null },
                    { 312, 10, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 195.00m, 9, null },
                    { 313, 1, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 335.00m, 9, null },
                    { 314, 9, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 394.00m, 9, null },
                    { 315, 11, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 424.00m, 9, null },
                    { 316, 10, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 591.00m, 9, null },
                    { 317, 1, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 409.00m, 9, null },
                    { 318, 9, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 468.00m, 9, null },
                    { 319, 11, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 498.00m, 9, null },
                    { 320, 10, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 702.00m, 9, null },
                    { 321, 1, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 140.00m, 9, null },
                    { 322, 9, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 279.00m, 9, null },
                    { 323, 11, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 348.00m, 9, null },
                    { 324, 10, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 418.00m, 9, null },
                    { 325, 1, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 246.00m, 9, null },
                    { 326, 9, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 492.00m, 9, null },
                    { 327, 11, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 615.00m, 9, null },
                    { 328, 10, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 738.00m, 9, null },
                    { 329, 1, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 183.00m, 9, null },
                    { 330, 9, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 183.00m, 9, null },
                    { 331, 11, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 183.00m, 9, null },
                    { 332, 10, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 9, 52, 58, 963, DateTimeKind.Unspecified), null, null, 269.00m, 9, null },
                    { 351, 12, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 158.00m, 16, null },
                    { 352, 13, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 260.00m, 16, null },
                    { 353, 14, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 311.00m, 16, null },
                    { 354, 15, "Surcharge for face fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 390.00m, 16, null },
                    { 355, 12, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 669.00m, 16, null },
                    { 356, 13, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 788.00m, 16, null },
                    { 357, 14, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 847.00m, 16, null },
                    { 358, 15, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 1181.00m, 16, null },
                    { 359, 12, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 818.00m, 16, null },
                    { 360, 13, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 936.00m, 16, null },
                    { 361, 14, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 996.00m, 16, null },
                    { 362, 15, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 1404.00m, 16, null },
                    { 363, 12, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 279.00m, 16, null },
                    { 364, 13, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 557.00m, 16, null },
                    { 365, 14, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 696.00m, 16, null },
                    { 366, 15, "Surcharge for top fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 836.00m, 16, null },
                    { 367, 12, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 492.00m, 16, null },
                    { 368, 13, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 984.00m, 16, null },
                    { 369, 14, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 1230.00m, 16, null },
                    { 370, 15, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 1476.00m, 16, null },
                    { 371, 12, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 361.00m, 16, null },
                    { 372, 13, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 361.00m, 16, null },
                    { 373, 14, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 361.00m, 16, null },
                    { 374, 15, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 9, 10, 25, 31, 153, DateTimeKind.Unspecified), null, null, 536.00m, 16, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 374);

            migrationBuilder.DropColumn(
                name: "ArmTypeId",
                table: "Brackets");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "Brackets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
