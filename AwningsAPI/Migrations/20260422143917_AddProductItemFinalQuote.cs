using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProductItemFinalQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FinalQuote",
                table: "WorkflowStarts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVoided",
                table: "Quotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoidedAt",
                table: "Quotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "QuoteItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "InvoiceItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.Id);
                });

            migrationBuilder.InsertData(
              table: "ProductItems",
              columns: new[] { "Id", "Description", "DateCreated", "CreatedBy" },
              values: new object[,]
              {
                    { 1, "Brackets",             new DateTime(2026, 4, 22), "System" },
                    { 2, "Motors",               new DateTime(2026, 4, 22), "System" },
                    { 3, "Valance",              new DateTime(2026, 4, 22), "System" },
                    { 4, "Non-Standard Rals",    new DateTime(2026, 4, 22), "System" },
                    { 5, "ShadePlus",            new DateTime(2026, 4, 22), "System" },
                    { 6, "Lighting Cassettes",   new DateTime(2026, 4, 22), "System" },
                    { 7, "Wall Sealing Profile", new DateTime(2026, 4, 22), "System" },
                    { 8, "Controls",             new DateTime(2026, 4, 22), "System" },
                    { 9, "Heaters",              new DateTime(2026, 4, 22), "System" }
              });

            migrationBuilder.UpdateData(
                table: "WorkflowStarts",
                keyColumn: "WorkflowId",
                keyValue: 1,
                column: "FinalQuote",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_QuoteItems_ProductItemId",
                table: "QuoteItems",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ProductItemId",
                table: "InvoiceItems",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_ProductItems_ProductItemId",
                table: "InvoiceItems",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuoteItems_ProductItems_ProductItemId",
                table: "QuoteItems",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // --- Product 8: Markilux 990 surcharge inserts (386–391) ---
            migrationBuilder.Sql(@"

              SET IDENTITY_INSERT Brackets ON;

                INSERT INTO Brackets (BracketId, BracketName, PartNumber, Price, DateCreated, CreatedBy, ArmTypeId, ProductId)
                VALUES
                (386, 'Surcharge for face fixture', NULL, 88, '2026-04-18 12:00:00', 'System', 2, 8),
                (387, 'Surcharge for face fixture incl. spreader plate A', NULL, 344, '2026-04-18 12:00:00', 'System', 2, 8),
                (388, 'Surcharge for face fixture incl. spreader plate B', NULL, 418, '2026-04-18 12:00:00', 'System', 2, 8),
                (389, 'Surcharge for top fixture', NULL, 88, '2026-04-18 12:00:00', 'System', 2, 8),
                (390, 'Surcharge for eaves fixture', NULL, 205, '2026-04-18 12:00:00', 'System', 2, 8),
                (391, 'Surcharge for arms with bionic tendon', NULL, 121, '2026-04-18 12:00:00', 'System', 2, 8);
               SET IDENTITY_INSERT Brackets OFF;
            ");

            // --- Product 9: Markilux 970 surcharge inserts (392–397) ---
            migrationBuilder.Sql(@"
                  SET IDENTITY_INSERT Brackets ON;
                INSERT INTO Brackets (BracketId, BracketName, PartNumber, Price, DateCreated, CreatedBy, ArmTypeId, ProductId)
                VALUES
                (392, 'Surcharge for face fixture', NULL, 220, '2026-04-18 12:00:00', 'System', 2, 9),
                (393, 'Surcharge for face fixture incl. spreader plate A', NULL, 592, '2026-04-18 12:00:00', 'System', 2, 9),
                (394, 'Surcharge for face fixture incl. spreader plate B', NULL, 550, '2026-04-18 12:00:00', 'System', 2, 9),
                (395, 'Surcharge for face fixture incl. spreader plate C', NULL, 592, '2026-04-18 12:00:00', 'System', 2, 9),
                (396, 'Surcharge for top fixture', NULL, 278, '2026-04-18 12:00:00', 'System', 2, 9),
                (397, 'Surcharge for eaves fixture', NULL, 371, '2026-04-18 12:00:00', 'System', 2, 9);
                SET IDENTITY_INSERT Brackets OFF;
            ");

            // --- Update ArmTypeId = 2 for existing Product 8 bracket IDs ---
            migrationBuilder.Sql(@"
                UPDATE Brackets
                SET ArmTypeId = 2
                WHERE BracketId IN (
                    153, 154, 155, 156, 157, 158, 159, 160,
                    161, 162, 163, 164, 165, 166, 167,
                    373, 374, 375, 376, 377, 378
                );
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_ProductItems_ProductItemId",
                table: "InvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_QuoteItems_ProductItems_ProductItemId",
                table: "QuoteItems");

            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_QuoteItems_ProductItemId",
                table: "QuoteItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_ProductItemId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "FinalQuote",
                table: "WorkflowStarts");

            migrationBuilder.DropColumn(
                name: "IsVoided",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "VoidedAt",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "QuoteItems");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "InvoiceItems");

            migrationBuilder.Sql(@"
                DELETE FROM Brackets
                WHERE BracketId BETWEEN 386 AND 391;
            ");

            // Rollback Product 9 inserts
            migrationBuilder.Sql(@"
                DELETE FROM Brackets
                WHERE BracketId BETWEEN 392 AND 397;
            ");

            // Rollback ArmTypeId updates
            migrationBuilder.Sql(@"
                UPDATE Brackets
                SET ArmTypeId = NULL
                WHERE BracketId IN (
                    153, 154, 155, 156, 157, 158, 159, 160,
                    161, 162, 163, 164, 165, 166, 167,
                    373, 374, 375, 376, 377, 378
                );
            ");
        }
    }
}
