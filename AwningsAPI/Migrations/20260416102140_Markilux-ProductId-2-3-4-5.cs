using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class MarkiluxProductId2345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "ArmTypeId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 30, 10, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 88m, 3, null },
                    { 31, 15, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 88m, 3, null },
                    { 32, 18, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 132m, 3, null },
                    { 33, 10, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 659m, 3, null },
                    { 34, 15, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 677m, 3, null },
                    { 35, 18, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1007m, 3, null },
                    { 36, 10, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 3, null },
                    { 37, 15, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 3, null },
                    { 38, 18, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 536m, 3, null },
                    { 39, 1, "Surcharge for junction roller", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 3, null },
                    { 40, 1, "Surcharge for one-piece cover", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 3, null },
                    { 41, 2, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 220m, 4, null },
                    { 42, 2, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 592m, 4, null },
                    { 43, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 550m, 4, null },
                    { 44, 2, "Surcharge for face fixture incl. spreader plate C", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 592m, 4, null },
                    { 45, 2, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 287m, 4, null },
                    { 46, 2, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 371m, 4, null },
                    { 47, 2, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 4, null },
                    { 48, 2, "Surcharge for the two-tone housing, markilux \"MX- colour\" in the colour combinations 1-10", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 300m, 4, null },
                    { 49, 2, "Face fixture bracket left / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "72826", 109.80m, 4, null },
                    { 50, 2, "Face fixture bracket right / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "72827", 109.80m, 4, null },
                    { 51, 2, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "72872", 253.50m, 4, null },
                    { 52, 2, "Top fixture bracket left / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "60523", 143.20m, 4, null },
                    { 53, 2, "Top fixture bracket right / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "60524", 143.20m, 4, null },
                    { 54, 2, "Eaves fixture bracket left 150 mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "60603", 185.30m, 4, null },
                    { 55, 2, "Eaves fixture bracket right 150 mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "60604", 185.30m, 4, null },
                    { 56, 2, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 4, null },
                    { 57, 2, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 4, null },
                    { 58, 2, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 4, null },
                    { 59, 2, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "72870", 186m, 4, null },
                    { 60, 2, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "73465", 164.90m, 4, null },
                    { 61, 2, "Spreader Plate C 310x130x12 mm / 6", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "72526", 186m, 4, null },
                    { 62, 2, "Spacer block face fixture 100x120x20 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "718581", 14.70m, 4, null },
                    { 63, 2, "Spacer block face fixture 100x120x12 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "718571", 14.30m, 4, null },
                    { 64, 2, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716311", 4.40m, 4, null },
                    { 65, 2, "Spacer block for top fixture 90x140x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716411", 5m, 4, null },
                    { 66, 2, "Cover plate 230x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 4, null },
                    { 67, 2, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 180m, 4, null },
                    { 68, 2, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 298m, 5, null },
                    { 69, 3, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 447m, 5, null },
                    { 70, 8, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 596m, 5, null },
                    { 71, 2, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 554m, 5, null },
                    { 72, 3, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 711m, 5, null },
                    { 73, 8, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 988m, 5, null },
                    { 74, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 628m, 5, null },
                    { 75, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 785m, 5, null },
                    { 76, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1099m, 5, null },
                    { 77, 2, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 397m, 5, null },
                    { 78, 3, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 596m, 5, null },
                    { 79, 8, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 794m, 5, null },
                    { 80, 2, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 475m, 5, null },
                    { 81, 3, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 712m, 5, null },
                    { 82, 8, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 950m, 5, null },
                    { 83, 2, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 5, null },
                    { 84, 3, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 5, null },
                    { 85, 8, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 5, null },
                    { 86, null, "Face fixture bracket assembly 5 - 35° / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74909", 148.90m, 5, null },
                    { 87, null, "Face fixture bracket assembly 36 - 70° / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74928", 148.90m, 5, null },
                    { 88, null, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77970", 243.90m, 5, null },
                    { 89, null, "Top fixture bracket 5 - 35° / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74903", 198.40m, 5, null },
                    { 90, null, "Top fixture bracket 36 - 70° / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74905", 198.40m, 5, null },
                    { 91, null, "Eaves fixture bracket 150mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74944", 237.30m, 5, null },
                    { 92, null, "Eaves fixture bracket 270 mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74970", 276.40m, 5, null },
                    { 93, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "741290", 142.80m, 5, null },
                    { 94, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 5, null },
                    { 95, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75328", 127.70m, 5, null },
                    { 96, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75327", 164.90m, 5, null },
                    { 97, null, "Spacer block face fixture 180x150x20 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "749881", 11.20m, 5, null },
                    { 98, null, "Spacer block for top fixture 136x150x20 mm /4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.70m, 5, null },
                    { 99, null, "Spacer block face fixture 180x150x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74989", 8.40m, 5, null },
                    { 100, null, "Cover plate 320x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71842", 21.90m, 5, null }
                });

            migrationBuilder.UpdateData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 1,
                columns: new[] { "DateCreated", "ProductId" },
                values: new object[] { new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.UpdateData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 2,
                columns: new[] { "DateCreated", "ProductId" },
                values: new object[] { new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 75m, 5 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 2,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for servo-assisted gear", 75m, 5 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 3,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for servo-assisted gear", 0m, 5 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 4,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for hard-wired motor", 484m, 5 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 5,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for hard-wired motor", 484m, 5 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 6,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for hard-wired motor", 574m, 5 });

            migrationBuilder.InsertData(
                table: "Motors",
                columns: new[] { "MotorId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 7, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 5, null },
                    { 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 5, null },
                    { 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 5, null },
                    { 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 5, null },
                    { 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 5, null },
                    { 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 5, null },
                    { 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1115m, 5, null },
                    { 14, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1115m, 5, null },
                    { 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 0m, 5, null },
                    { 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 997m, 5, null },
                    { 17, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 997m, 5, null },
                    { 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 0m, 5, null }
                });

            migrationBuilder.Sql("DELETE FROM dbo.Projections WHERE ProjectionId > 85;");

            migrationBuilder.InsertData(
                table: "Projections",
                columns: new[] { "ProjectionId", "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "Projection_cm", "UpdatedBy", "Width_cm" },
                values: new object[,]
                {
                    { 86, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9884m, 3, 150, null, 500 },
                    { 87, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10284m, 3, 150, null, 600 },
                    { 88, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10784m, 3, 150, null, 700 },
                    { 89, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11290m, 3, 150, null, 800 },
                    { 90, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11701m, 3, 150, null, 900 },
                    { 91, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12990m, 3, 150, null, 1000 },
                    { 92, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13790m, 3, 150, null, 1100 },
                    { 93, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14650m, 3, 150, null, 1200 },
                    { 94, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 16025m, 3, 150, null, 1300 },
                    { 95, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 17821m, 3, 150, null, 1400 },
                    { 96, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10760m, 3, 200, null, 600 },
                    { 97, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11298m, 3, 200, null, 700 },
                    { 98, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11829m, 3, 200, null, 800 },
                    { 99, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12253m, 3, 200, null, 900 },
                    { 100, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13579m, 3, 200, null, 1000 },
                    { 101, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14379m, 3, 200, null, 1100 },
                    { 102, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15205m, 3, 200, null, 1200 },
                    { 103, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 16694m, 3, 200, null, 1300 },
                    { 104, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 18492m, 3, 200, null, 1400 },
                    { 105, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11790m, 3, 250, null, 700 },
                    { 106, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12321m, 3, 250, null, 800 },
                    { 107, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12800m, 3, 250, null, 900 },
                    { 108, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14252m, 3, 250, null, 1000 },
                    { 109, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15128m, 3, 250, null, 1100 },
                    { 110, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15840m, 3, 250, null, 1200 },
                    { 111, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 17341m, 3, 250, null, 1300 },
                    { 112, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 19220m, 3, 250, null, 1400 },
                    { 113, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12793m, 3, 300, null, 800 },
                    { 114, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13292m, 3, 300, null, 900 },
                    { 115, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14813m, 3, 300, null, 1000 },
                    { 116, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15691m, 3, 300, null, 1100 },
                    { 117, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 16357m, 3, 300, null, 1200 },
                    { 118, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 17978m, 3, 300, null, 1300 },
                    { 119, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 19878m, 3, 300, null, 1400 },
                    { 120, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14664m, 3, 350, null, 900 },
                    { 121, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15786m, 3, 350, null, 1000 },
                    { 122, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 16715m, 3, 350, null, 1100 },
                    { 123, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 17594m, 3, 350, null, 1200 },
                    { 124, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 20940m, 3, 350, null, 1400 },
                    { 125, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 17248m, 3, 400, null, 1000 },
                    { 126, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 18127m, 3, 400, null, 1100 },
                    { 127, 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 18884m, 3, 400, null, 1200 },
                    { 128, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3949m, 4, 150, null, 250 },
                    { 129, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4198m, 4, 150, null, 300 },
                    { 130, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4537m, 4, 150, null, 350 },
                    { 131, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4819m, 4, 150, null, 400 },
                    { 132, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5078m, 4, 150, null, 450 },
                    { 133, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5367m, 4, 150, null, 500 },
                    { 134, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5689m, 4, 150, null, 550 },
                    { 135, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6007m, 4, 150, null, 600 },
                    { 136, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4070m, 4, 200, null, 250 },
                    { 137, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4399m, 4, 200, null, 300 },
                    { 138, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4694m, 4, 200, null, 350 },
                    { 139, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5000m, 4, 200, null, 400 },
                    { 140, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5259m, 4, 200, null, 450 },
                    { 141, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5557m, 4, 200, null, 500 },
                    { 142, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5884m, 4, 200, null, 550 },
                    { 143, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6214m, 4, 200, null, 600 },
                    { 144, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4492m, 4, 250, null, 300 },
                    { 145, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4942m, 4, 250, null, 350 },
                    { 146, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5223m, 4, 250, null, 400 },
                    { 147, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5483m, 4, 250, null, 450 },
                    { 148, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5769m, 4, 250, null, 500 },
                    { 149, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6109m, 4, 250, null, 550 },
                    { 150, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6445m, 4, 250, null, 600 },
                    { 151, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5071m, 4, 300, null, 350 },
                    { 152, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5400m, 4, 300, null, 400 },
                    { 153, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5659m, 4, 300, null, 450 },
                    { 154, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5955m, 4, 300, null, 500 },
                    { 155, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6303m, 4, 300, null, 550 },
                    { 156, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6654m, 4, 300, null, 600 },
                    { 157, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5606m, 4, 350, null, 400 },
                    { 158, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5873m, 4, 350, null, 450 },
                    { 159, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6187m, 4, 350, null, 500 },
                    { 160, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2826m, 5, 150, null, 250 },
                    { 161, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2979m, 5, 150, null, 350 },
                    { 162, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3174m, 5, 150, null, 400 },
                    { 163, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3372m, 5, 150, null, 450 },
                    { 164, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3579m, 5, 150, null, 500 },
                    { 165, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4039m, 5, 150, null, 550 },
                    { 166, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4516m, 5, 150, null, 600 },
                    { 167, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5050m, 5, 150, null, 650 },
                    { 168, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5838m, 5, 150, null, 700 },
                    { 169, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3165m, 5, 150, null, 300 },
                    { 170, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3375m, 5, 150, null, 350 },
                    { 171, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3577m, 5, 150, null, 400 },
                    { 172, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3793m, 5, 150, null, 450 },
                    { 173, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4012m, 5, 150, null, 500 },
                    { 174, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4260m, 5, 150, null, 550 },
                    { 175, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4733m, 5, 150, null, 600 },
                    { 176, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5304m, 5, 150, null, 650 },
                    { 177, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6110m, 5, 150, null, 700 },
                    { 178, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3564m, 5, 150, null, 350 },
                    { 179, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3769m, 5, 150, null, 400 },
                    { 180, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4005m, 5, 150, null, 450 },
                    { 181, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4263m, 5, 150, null, 500 },
                    { 182, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4543m, 5, 150, null, 550 },
                    { 183, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4980m, 5, 150, null, 600 },
                    { 184, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5624m, 5, 150, null, 650 },
                    { 185, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6404m, 5, 150, null, 700 },
                    { 186, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3952m, 5, 150, null, 400 },
                    { 187, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4196m, 5, 150, null, 450 },
                    { 188, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4474m, 5, 150, null, 500 },
                    { 189, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4922m, 5, 150, null, 550 },
                    { 190, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5237m, 5, 150, null, 600 },
                    { 191, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5870m, 5, 150, null, 650 },
                    { 192, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6673m, 5, 150, null, 700 },
                    { 193, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4581m, 5, 150, null, 450 },
                    { 194, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4895m, 5, 150, null, 500 },
                    { 195, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5316m, 5, 150, null, 550 },
                    { 196, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5656m, 5, 150, null, 600 },
                    { 197, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6439m, 5, 150, null, 650 },
                    { 198, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6822m, 5, 150, null, 700 },
                    { 199, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5541m, 5, 150, null, 500 },
                    { 200, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5978m, 5, 150, null, 550 },
                    { 201, 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6231m, 5, 150, null, 600 },
                    { 202, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7146m, 5, 150, null, 700 }
                });

            migrationBuilder.InsertData(
                table: "ShadePlus",
                columns: new[] { "ShadePlusId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 719m, 5, null, 250 },
                    { 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 798m, 5, null, 300 },
                    { 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 865m, 5, null, 350 },
                    { 14, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 945m, 5, null, 400 },
                    { 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1019m, 5, null, 450 },
                    { 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1096m, 5, null, 500 },
                    { 17, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1173m, 5, null, 550 },
                    { 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1248m, 5, null, 600 },
                    { 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1319m, 5, null, 650 },
                    { 20, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1396m, 5, null, 700 },
                    { 21, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1538m, 5, null, 250 },
                    { 22, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1596m, 5, null, 300 },
                    { 23, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1693m, 5, null, 350 },
                    { 24, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1778m, 5, null, 400 },
                    { 25, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1830m, 5, null, 450 },
                    { 26, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1898m, 5, null, 500 },
                    { 27, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1986m, 5, null, 550 },
                    { 28, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2070m, 5, null, 600 },
                    { 29, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2163m, 5, null, 650 },
                    { 30, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2262m, 5, null, 700 },
                    { 31, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1669m, 5, null, 250 },
                    { 32, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1729m, 5, null, 300 },
                    { 33, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1828m, 5, null, 350 },
                    { 34, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1911m, 5, null, 400 },
                    { 35, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1961m, 5, null, 450 },
                    { 36, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2032m, 5, null, 500 },
                    { 37, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2119m, 5, null, 550 },
                    { 38, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2204m, 5, null, 600 },
                    { 39, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2297m, 5, null, 650 },
                    { 40, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2395m, 5, null, 700 }
                });

            migrationBuilder.InsertData(
                table: "nonStandardRALColours",
                columns: new[] { "RALColourId", "CreatedBy", "DateCreated", "DateUpdated", "MultiplyBy", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 21, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 625m, 3, null, 500 },
                    { 22, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 650m, 3, null, 600 },
                    { 23, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 692m, 3, null, 700 },
                    { 24, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 730m, 3, null, 800 },
                    { 25, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 783m, 3, null, 900 },
                    { 26, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 879m, 3, null, 1000 },
                    { 27, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 939m, 3, null, 1100 },
                    { 28, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1008m, 3, null, 1200 },
                    { 29, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1066m, 3, null, 1300 },
                    { 30, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1274m, 3, null, 1400 },
                    { 31, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 4, null, 250 },
                    { 32, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 4, null, 300 },
                    { 33, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 4, null, 350 },
                    { 34, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 388m, 4, null, 400 },
                    { 35, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 406m, 4, null, 450 },
                    { 36, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 423m, 4, null, 500 },
                    { 37, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 439m, 4, null, 550 },
                    { 38, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 458m, 4, null, 600 },
                    { 39, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 316m, 5, null, 250 },
                    { 40, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 5, null, 300 },
                    { 41, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 5, null, 350 },
                    { 42, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 5, null, 400 },
                    { 43, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 396m, 5, null, 450 },
                    { 44, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 445m, 5, null, 500 },
                    { 45, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 477m, 5, null, 550 },
                    { 46, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 509m, 5, null, 600 },
                    { 47, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 539m, 5, null, 650 },
                    { 48, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 640m, 5, null, 700 }
                });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 79m, 5 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 2,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 87m, 5 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 3,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 96m, 5 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 4,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 109m, 5 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 5,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 122m, 5 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 6,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 134m, 5 });

            migrationBuilder.InsertData(
                table: "valanceStyles",
                columns: new[] { "ValanceStyleId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 7, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 145m, 5, null, 550 },
                    { 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 158m, 5, null, 600 },
                    { 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 166m, 5, null, 650 },
                    { 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 5, null, 700 }
                });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 90m, 5 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 2,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 104m, 5 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 3,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 114m, 5 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 4,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 130m, 5 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 5,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 146m, 5 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 6,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 162m, 5 });

            migrationBuilder.InsertData(
                table: "wallSealingProfiles",
                columns: new[] { "WallSealingProfileId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 7, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 5, null, 550 },
                    { 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 193m, 5, null, 600 },
                    { 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 207m, 5, null, 650 },
                    { 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 223m, 5, null, 700 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 77);

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
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 1,
                columns: new[] { "DateCreated", "ProductId" },
                values: new object[] { new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 });

            migrationBuilder.UpdateData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 2,
                columns: new[] { "DateCreated", "ProductId" },
                values: new object[] { new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 });

            migrationBuilder.InsertData(
                table: "LightingCassettes",
                columns: new[] { "LightingId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", 1555.00m, 3, null },
                    { 4, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", 1386.00m, 3, null }
                });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 72m, 6 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 2,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for hard-wired motor", 470m, 6 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 3,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for radio-contr. motor io/RTS + 1 ch. transmitter", 700m, 6 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 4,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for radio-contr. motor io/RTS w/o transmitter", 586m, 6 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 5,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1082m, 6 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 6,
                columns: new[] { "Description", "Price", "ProductId" },
                values: new object[] { "Surcharge for radio-contr. motor io with manual override w/o transmitter", 968m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 76m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 2,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 84m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 3,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 93m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 4,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 105m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 5,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 118m, 6 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 6,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 130m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 1,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 87m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 2,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 101m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 3,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 110m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 4,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 126m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 5,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 141m, 6 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 6,
                columns: new[] { "Price", "ProductId" },
                values: new object[] { 157m, 6 });
        }
    }
}
