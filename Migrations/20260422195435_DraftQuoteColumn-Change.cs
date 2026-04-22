using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class DraftQuoteColumnChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoidedAt",
                table: "Quotes",
                newName: "FinalizedAt");

            migrationBuilder.RenameColumn(
                name: "IsVoided",
                table: "Quotes",
                newName: "IsFinal");

            migrationBuilder.AddColumn<int>(
                name: "DraftQuoteId",
                table: "Quotes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_DraftQuoteId",
                table: "Quotes",
                column: "DraftQuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Quotes_DraftQuoteId",
                table: "Quotes",
                column: "DraftQuoteId",
                principalTable: "Quotes",
                principalColumn: "QuoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Quotes_DraftQuoteId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_DraftQuoteId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "DraftQuoteId",
                table: "Quotes");

            migrationBuilder.RenameColumn(
                name: "IsFinal",
                table: "Quotes",
                newName: "IsVoided");

            migrationBuilder.RenameColumn(
                name: "FinalizedAt",
                table: "Quotes",
                newName: "VoidedAt");
        }
    }
}
