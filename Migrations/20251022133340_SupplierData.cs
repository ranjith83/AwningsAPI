using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class SupplierData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Createded",
                table: "Suppliers",
                newName: "DateCreated");

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ProductTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ProductTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "ProductTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 10, 22, 14, 33, 39, 910, DateTimeKind.Local).AddTicks(6635), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Folding-arm Cassette Awnings", 0 },
                    { 2, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1250), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Folding-arm Semi-cassette Awnings", 0 },
                    { 3, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1269), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Open Folding-arm Awnings", 0 },
                    { 4, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1271), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stretch-Awnings", 0 },
                    { 5, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1273), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wind Protection and Privacy", 0 },
                    { 6, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1292), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Awning Systems", 0 },
                    { 7, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1295), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Free-standing awning stand systems", 0 },
                    { 8, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1296), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pergola awnings", 0 },
                    { 9, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1298), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conservatory and Glass Canopy Awnings", 0 },
                    { 10, 1, new DateTime(2025, 10, 22, 14, 33, 39, 913, DateTimeKind.Local).AddTicks(1300), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vertical Roller Blinds and Awnings", 0 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "ProductTypeId", "SupplierId", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux MX-1 compact", 1, 1, 0 },
                    { 2, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux MX-4", 1, 1, 0 },
                    { 3, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux MX-2", 1, 1, 0 },
                    { 4, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 6000", 1, 1, 0 },
                    { 5, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux MX-3", 1, 1, 0 },
                    { 6, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 990", 1, 1, 0 },
                    { 7, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 970", 1, 1, 0 },
                    { 8, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 5010", 1, 1, 0 },
                    { 9, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 3300", 1, 1, 0 },
                    { 10, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 1710", 1, 1, 0 },
                    { 11, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 900", 1, 1, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Suppliers",
                newName: "Createded");
        }
    }
}
