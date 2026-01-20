using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedArmTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArmTypeId",
                table: "Projections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArmTypeId",
                table: "Arms",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmTypeId",
                table: "Projections");

            migrationBuilder.DropColumn(
                name: "ArmTypeId",
                table: "Arms");
        }
    }
}
