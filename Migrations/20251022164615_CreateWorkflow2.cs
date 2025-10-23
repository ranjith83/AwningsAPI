using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorkflow2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitialEnquiries",
                columns: table => new
                {
                    EnquiryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialEnquiries", x => x.EnquiryId);
                });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3,
                column: "SupplierName",
                value: "Practic");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStarts_ProductId",
                table: "WorkflowStarts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowStarts_Products_ProductId",
                table: "WorkflowStarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowStarts_Products_ProductId",
                table: "WorkflowStarts");

            migrationBuilder.DropTable(
                name: "InitialEnquiries");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowStarts_ProductId",
                table: "WorkflowStarts");

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3,
                column: "SupplierName",
                value: "Supplier3");
        }
    }
}
