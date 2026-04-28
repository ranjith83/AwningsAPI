using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Controls",
                columns: table => new
                {
                    ControlId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controls", x => x.ControlId);
                    table.ForeignKey(
                        name: "FK_Controls_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LightingCassettes",
                columns: table => new
                {
                    LightingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightingCassettes", x => x.LightingId);
                    table.ForeignKey(
                        name: "FK_LightingCassettes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShadePlus",
                columns: table => new
                {
                    ShadePlusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WidthCm = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShadePlus", x => x.ShadePlusId);
                    table.ForeignKey(
                        name: "FK_ShadePlus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Controls",
                columns: new[] { "ControlId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "markilux io-5 designcontrol transmitter - 5 channel", "8272099", 154.00m, 2, null },
                    { 2, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Somfy TaHoma Switch", "8272377", 362.00m, 2, null }
                });

            migrationBuilder.InsertData(
                table: "LightingCassettes",
                columns: new[] { "LightingId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", 1555.00m, 5, null },
                    { 2, "System", new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", 1386.00m, 5, null },
                    { 3, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", 1555.00m, 3, null },
                    { 4, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", 1386.00m, 3, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "ProductTypeId", "SupplierId", "UpdatedBy" },
                values: new object[,]
                {
                    { 12, "System", new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 3300 Semi", 1, 1, null },
                    { 13, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 6000", 1, 1, null },
                    { 14, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 779", 1, 1, null },
                    { 15, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 8800", 1, 1, null },
                    { 16, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 6000 XXL", 1, 1, null }
                });

            migrationBuilder.InsertData(
                table: "ShadePlus",
                columns: new[] { "ShadePlusId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1592.00m, 1, null, 250 },
                    { 2, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1650.00m, 1, null, 300 },
                    { 3, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1727.00m, 1, null, 350 },
                    { 4, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1815.00m, 1, null, 400 },
                    { 5, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1875.00m, 1, null, 450 },
                    { 6, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 1951.00m, 1, null, 500 },
                    { 7, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 2044.00m, 1, null, 550 },
                    { 8, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 2128.00m, 1, null, 600 },
                    { 9, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 2206.00m, 1, null, 650 },
                    { 10, "System", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm - radio-controlled motor", 2284.00m, 1, null, 700 },
                    { 11, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1424.00m, 4, null, 500 },
                    { 12, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1581.00m, 4, null, 600 },
                    { 13, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1726.00m, 4, null, 700 },
                    { 14, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1878.00m, 4, null, 800 },
                    { 15, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2027.00m, 4, null, 900 },
                    { 16, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2179.00m, 4, null, 1000 },
                    { 17, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2329.00m, 4, null, 1100 },
                    { 18, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2479.00m, 4, null, 1200 },
                    { 19, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2628.00m, 4, null, 1300 },
                    { 20, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2779.00m, 4, null, 1390 },
                    { 21, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3060.00m, 4, null, 500 },
                    { 22, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3183.00m, 4, null, 600 },
                    { 23, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3374.00m, 4, null, 700 },
                    { 24, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3546.00m, 4, null, 800 },
                    { 25, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3649.00m, 4, null, 900 },
                    { 26, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3787.00m, 4, null, 1000 },
                    { 27, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3959.00m, 4, null, 1100 },
                    { 28, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4128.00m, 4, null, 1200 },
                    { 29, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4318.00m, 4, null, 1300 },
                    { 30, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4511.00m, 4, null, 1390 },
                    { 31, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3325.00m, 4, null, 500 },
                    { 32, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3449.00m, 4, null, 600 },
                    { 33, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3638.00m, 4, null, 700 },
                    { 34, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3809.00m, 4, null, 800 },
                    { 35, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3913.00m, 4, null, 900 },
                    { 36, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4053.00m, 4, null, 1000 },
                    { 37, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4225.00m, 4, null, 1100 },
                    { 38, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4392.00m, 4, null, 1200 },
                    { 39, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4585.00m, 4, null, 1300 },
                    { 40, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4776.00m, 4, null, 1390 },
                    { 71, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 718.00m, 7, null, 250 },
                    { 72, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 797.00m, 7, null, 300 },
                    { 73, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 865.00m, 7, null, 350 },
                    { 74, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 944.00m, 7, null, 400 },
                    { 75, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1018.00m, 7, null, 450 },
                    { 76, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1095.00m, 7, null, 500 },
                    { 77, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1170.00m, 7, null, 550 },
                    { 78, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1247.00m, 7, null, 600 },
                    { 79, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1538.00m, 7, null, 250 },
                    { 80, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1596.00m, 7, null, 300 },
                    { 81, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1693.00m, 7, null, 350 },
                    { 82, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1777.00m, 7, null, 400 },
                    { 83, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1829.00m, 7, null, 450 },
                    { 84, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1898.00m, 7, null, 500 },
                    { 85, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1986.00m, 7, null, 550 },
                    { 86, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 2069.00m, 7, null, 600 },
                    { 87, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1667.00m, 7, null, 250 },
                    { 88, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1729.00m, 7, null, 300 },
                    { 89, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1827.00m, 7, null, 350 },
                    { 90, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1911.00m, 7, null, 400 },
                    { 91, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1960.00m, 7, null, 450 },
                    { 92, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2032.00m, 7, null, 500 },
                    { 93, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2119.00m, 7, null, 550 },
                    { 94, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2204.00m, 7, null, 600 },
                    { 95, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 719.00m, 8, null, 250 },
                    { 96, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 798.00m, 8, null, 300 },
                    { 97, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 865.00m, 8, null, 350 },
                    { 98, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 945.00m, 8, null, 400 },
                    { 99, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1019.00m, 8, null, 450 },
                    { 100, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1096.00m, 8, null, 500 },
                    { 101, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1173.00m, 8, null, 550 },
                    { 102, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1248.00m, 8, null, 600 },
                    { 103, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1319.00m, 8, null, 650 },
                    { 104, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1396.00m, 8, null, 700 }
                });

            migrationBuilder.InsertData(
                table: "armsTypes",
                columns: new[] { "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "UpdatedBy" },
                values: new object[,]
                {
                    { 4, "System", new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), null, "4-0-4", null },
                    { 5, "System", new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), null, "4-0-6", null },
                    { 6, "System", new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), null, "4-2-6", null },
                    { 7, "System", new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), null, "6-4-8", null },
                    { 8, "System", new DateTime(2026, 4, 8, 19, 35, 51, 0, DateTimeKind.Unspecified), null, "2-0-3", null },
                    { 9, "System", new DateTime(2026, 4, 9, 9, 27, 27, 0, DateTimeKind.Unspecified), null, "2-0-4", null },
                    { 10, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "3-2-6", null },
                    { 11, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2-1-5", null },
                    { 12, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "4-0-5", null },
                    { 13, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "4-0-7", null },
                    { 14, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "4-2-9", null },
                    { 15, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "6-4-11", null }
                });

            migrationBuilder.InsertData(
                table: "Controls",
                columns: new[] { "ControlId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "markilux io-1 designcontrol transmitter - 1 channel", "8272087", 118.00m, 14, null },
                    { 4, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "markilux io-5 designcontrol transmitter - 5 channel", "8272099", 154.00m, 14, null }
                });

            migrationBuilder.InsertData(
                table: "ShadePlus",
                columns: new[] { "ShadePlusId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 41, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 719.00m, 14, null, 250 },
                    { 42, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 798.00m, 14, null, 300 },
                    { 43, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 865.00m, 14, null, 350 },
                    { 44, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 945.00m, 14, null, 400 },
                    { 45, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1019.00m, 14, null, 450 },
                    { 46, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1096.00m, 14, null, 500 },
                    { 47, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1173.00m, 14, null, 550 },
                    { 48, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1248.00m, 14, null, 600 },
                    { 49, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1319.00m, 14, null, 650 },
                    { 50, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1396.00m, 14, null, 700 },
                    { 51, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1538.00m, 14, null, 250 },
                    { 52, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1596.00m, 14, null, 300 },
                    { 53, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1693.00m, 14, null, 350 },
                    { 54, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1778.00m, 14, null, 400 },
                    { 55, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1830.00m, 14, null, 450 },
                    { 56, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1898.00m, 14, null, 500 },
                    { 57, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 1986.00m, 14, null, 550 },
                    { 58, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2070.00m, 14, null, 600 },
                    { 59, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2163.00m, 14, null, 650 },
                    { 60, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 2262.00m, 14, null, 700 },
                    { 61, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1669.00m, 14, null, 250 },
                    { 62, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1729.00m, 14, null, 300 },
                    { 63, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1828.00m, 14, null, 350 },
                    { 64, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1911.00m, 14, null, 400 },
                    { 65, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 1961.00m, 14, null, 450 },
                    { 66, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2032.00m, 14, null, 500 },
                    { 67, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2119.00m, 14, null, 550 },
                    { 68, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2204.00m, 14, null, 600 },
                    { 69, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2297.00m, 14, null, 650 },
                    { 70, "System", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 2395.00m, 14, null, 700 },
                    { 105, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1424.00m, 15, null, 500 },
                    { 106, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1581.00m, 15, null, 600 },
                    { 107, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1726.00m, 15, null, 700 },
                    { 108, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1878.00m, 15, null, 800 },
                    { 109, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2027.00m, 15, null, 900 },
                    { 110, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2179.00m, 15, null, 1000 },
                    { 111, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2329.00m, 15, null, 1100 },
                    { 112, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2479.00m, 15, null, 1200 },
                    { 113, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2628.00m, 15, null, 1300 },
                    { 114, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2779.00m, 15, null, 1390 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Controls_ProductId",
                table: "Controls",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LightingCassettes_ProductId",
                table: "LightingCassettes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShadePlus_ProductId",
                table: "ShadePlus",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controls");

            migrationBuilder.DropTable(
                name: "LightingCassettes");

            migrationBuilder.DropTable(
                name: "ShadePlus");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 15);
        }
    }
}
