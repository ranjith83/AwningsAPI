using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedArmTypeIdNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArmsType",
                columns: table => new
                {
                    ArmTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmsType", x => x.ArmTypeId);
                });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 1,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 4,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.InsertData(
                table: "ArmsType",
                columns: new[] { "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "2-0-2", null },
                    { 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "2-1-3", null },
                    { 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "3-2-4", null }
                });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                column: "ArmTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                column: "ArmTypeId",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArmsType");

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 1,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 4,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                column: "ArmTypeId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                column: "ArmTypeId",
                value: 0);
        }
    }
}
