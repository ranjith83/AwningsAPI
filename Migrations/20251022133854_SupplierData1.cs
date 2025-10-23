using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class SupplierData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 10,
                column: "DateCreated",
                value: new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2,
                column: "SupplierName",
                value: "Rensen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 910, DateTimeKind.Local).AddTicks(6635));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1250));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1273));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1292));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1295));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1296));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1298));

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 10,
                column: "DateCreated",
                value: new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2,
                column: "SupplierName",
                value: "Supplier2");
        }
    }
}
