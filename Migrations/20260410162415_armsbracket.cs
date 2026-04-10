using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class armsbracket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brackets_Products_ProductId",
                table: "Brackets");

            migrationBuilder.DropIndex(
                name: "IX_Brackets_ProductId",
                table: "Brackets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Brackets_ProductId",
                table: "Brackets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brackets_Products_ProductId",
                table: "Brackets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
