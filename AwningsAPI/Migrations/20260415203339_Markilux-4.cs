using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Markilux4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "ArmTypeId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 14, 2, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 44m, 2, null },
                    { 15, 5, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 44m, 2, null },
                    { 16, 8, "Surcharge face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 66m, 2, null },
                    { 17, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 330m, 2, null },
                    { 18, 5, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 339m, 2, null },
                    { 19, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 504m, 2, null },
                    { 20, 2, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 2, null },
                    { 21, 5, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 2, null },
                    { 22, 8, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 2, null },
                    { 23, 1, "Face fixture bracket 200 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62143", 66.40m, 2, null },
                    { 24, 1, "Face fixture bracket A 300 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "60775", 88.40m, 2, null },
                    { 25, 1, "Spreader plate B 250 x 23 x 49 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75327", 164.90m, 2, null },
                    { 26, 1, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77970", 243.90m, 2, null },
                    { 27, 1, "Spacer block face fixture 180x150x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "74989", 8.40m, 2, null },
                    { 28, 1, "Spacer block face fixture 180x150x20 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "749881", 11.20m, 2, null },
                    { 29, 1, "Cover plate 320x210x2 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71842", 21.90m, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Projections",
                columns: new[] { "ProjectionId", "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "Projection_cm", "UpdatedBy", "Width_cm" },
                values: new object[,]
                {
                    { 44, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4931m, 2, 150, null, 250 },
                    { 45, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5132m, 2, 150, null, 300 },
                    { 46, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5382m, 2, 150, null, 350 },
                    { 47, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5634m, 2, 150, null, 400 },
                    { 48, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5840m, 2, 150, null, 450 },
                    { 49, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6484m, 2, 150, null, 500 },
                    { 50, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6885m, 2, 150, null, 550 },
                    { 51, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7315m, 2, 150, null, 600 },
                    { 52, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8002m, 2, 150, null, 650 },
                    { 53, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8899m, 2, 150, null, 700 },
                    { 54, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5369m, 2, 200, null, 300 },
                    { 55, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5638m, 2, 200, null, 350 },
                    { 56, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5904m, 2, 200, null, 400 },
                    { 57, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6116m, 2, 200, null, 450 },
                    { 58, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6780m, 2, 200, null, 500 },
                    { 59, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7179m, 2, 200, null, 550 },
                    { 60, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7591m, 2, 200, null, 600 },
                    { 61, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8336m, 2, 200, null, 650 },
                    { 62, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9235m, 2, 200, null, 700 },
                    { 63, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5884m, 2, 250, null, 350 },
                    { 64, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6150m, 2, 250, null, 400 },
                    { 65, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6390m, 2, 250, null, 450 },
                    { 66, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7116m, 2, 250, null, 500 },
                    { 67, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7553m, 2, 250, null, 550 },
                    { 68, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7909m, 2, 250, null, 600 },
                    { 69, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8661m, 2, 250, null, 650 },
                    { 70, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9599m, 2, 250, null, 700 },
                    { 71, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6386m, 2, 300, null, 400 },
                    { 72, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6635m, 2, 300, null, 450 },
                    { 73, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7396m, 2, 300, null, 500 },
                    { 74, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7834m, 2, 300, null, 550 },
                    { 75, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8168m, 2, 300, null, 600 },
                    { 76, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8979m, 2, 300, null, 650 },
                    { 77, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9928m, 2, 300, null, 700 },
                    { 78, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7322m, 2, 350, null, 450 },
                    { 79, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7883m, 2, 350, null, 500 },
                    { 80, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8347m, 2, 350, null, 550 },
                    { 81, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8787m, 2, 350, null, 600 },
                    { 82, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10215m, 2, 350, null, 700 },
                    { 83, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8586m, 2, 400, null, 500 },
                    { 84, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9027m, 2, 400, null, 550 },
                    { 85, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9431m, 2, 400, null, 600 }
                });

            migrationBuilder.InsertData(
                table: "nonStandardRALColours",
                columns: new[] { "RALColourId", "CreatedBy", "DateCreated", "DateUpdated", "MultiplyBy", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 315m, 2, null, 250 },
                    { 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 328m, 2, null, 300 },
                    { 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 351m, 2, null, 350 },
                    { 14, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 2, null, 400 },
                    { 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 396m, 2, null, 450 },
                    { 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 444m, 2, null, 500 },
                    { 17, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 476m, 2, null, 550 },
                    { 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 508m, 2, null, 600 },
                    { 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 539m, 2, null, 650 },
                    { 20, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 640m, 2, null, 700 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 20);
        }
    }
}
