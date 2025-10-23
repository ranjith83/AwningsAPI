using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 1,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 2,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 3,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 4,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 5,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 6,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 7,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 8,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 9,
                column: "SupplierId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 10,
                column: "SupplierId",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ProductTypes");
        }
    }
}
