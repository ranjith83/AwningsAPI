using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class radioMotors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArmsType",
                table: "ArmsType");

            migrationBuilder.RenameTable(
                name: "ArmsType",
                newName: "armsTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_armsTypes",
                table: "armsTypes",
                column: "ArmTypeId");

            migrationBuilder.CreateTable(
                name: "radioControlledMotors",
                columns: table => new
                {
                    RadioMotorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width_cm = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radioControlledMotors", x => x.RadioMotorId);
                });

            migrationBuilder.InsertData(
                table: "radioControlledMotors",
                columns: new[] { "RadioMotorId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy", "Width_cm" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1547m, 1, null, 250 },
                    { 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1603m, 1, null, 300 },
                    { 3, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1678m, 1, null, 350 },
                    { 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1763m, 1, null, 400 },
                    { 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1822m, 1, null, 450 },
                    { 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1896m, 1, null, 500 },
                    { 7, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 1986m, 1, null, 550 },
                    { 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 2068m, 1, null, 600 },
                    { 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 2143m, 1, null, 650 },
                    { 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "RadioControlled Motor", 2219m, 1, null, 700 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "radioControlledMotors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_armsTypes",
                table: "armsTypes");

            migrationBuilder.RenameTable(
                name: "armsTypes",
                newName: "ArmsType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArmsType",
                table: "ArmsType",
                column: "ArmTypeId");
        }
    }
}
