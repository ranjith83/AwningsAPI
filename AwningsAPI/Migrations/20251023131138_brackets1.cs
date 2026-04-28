using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class brackets1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Projections",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                column: "Price",
                value: 1873m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                column: "Price",
                value: 2023m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                column: "Price",
                value: 2229m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                column: "Price",
                value: 2397m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "Price",
                value: 2554m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "Price",
                value: 2730m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "Price",
                value: 1979m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "Price",
                value: 2145m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "Price",
                value: 2319m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "Price",
                value: 2508m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                column: "Price",
                value: 2664m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                column: "Price",
                value: 2842m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                column: "Price",
                value: 2248m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "Price",
                value: 2431m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "Price",
                value: 2627m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "Price",
                value: 2798m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "Price",
                value: 2970m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "Price",
                value: 2547m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "Price",
                value: 2750m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                column: "Price",
                value: 2904m);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                column: "Price",
                value: 3084m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Projections",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                column: "Price",
                value: 1873.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                column: "Price",
                value: 2023.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                column: "Price",
                value: 2229.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                column: "Price",
                value: 2397.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "Price",
                value: 2554.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "Price",
                value: 2730.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "Price",
                value: 1979.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "Price",
                value: 2145.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "Price",
                value: 2319.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "Price",
                value: 2508.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                column: "Price",
                value: 2664.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                column: "Price",
                value: 2842.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                column: "Price",
                value: 2248.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "Price",
                value: 2431.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "Price",
                value: 2627.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "Price",
                value: 2798.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "Price",
                value: 2970.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "Price",
                value: 2547.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "Price",
                value: 2750.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                column: "Price",
                value: 2904.0);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                column: "Price",
                value: 3084.0);
        }
    }
}
