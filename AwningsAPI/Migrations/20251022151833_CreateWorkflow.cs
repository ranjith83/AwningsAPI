using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowStart",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialEnquiry = table.Column<bool>(type: "bit", nullable: false),
                    CreateQuote = table.Column<bool>(type: "bit", nullable: false),
                    InviteShowRoom = table.Column<bool>(type: "bit", nullable: false),
                    SetupSiteVisit = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceSent = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStart", x => x.WorkflowId);
                });

            migrationBuilder.InsertData(
                table: "WorkflowStart",
                columns: new[] { "WorkflowId", "CreateQuote", "CreatedBy", "DateCreated", "DateUpdated", "Description", "InitialEnquiry", "InviteShowRoom", "InvoiceSent", "ProductId", "ProductTypeId", "SetupSiteVisit", "SupplierId", "UpdatedBy" },
                values: new object[] { 1, false, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Markilux 990 for outside garden", false, false, false, 6, 1, false, 1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowStart");
        }
    }
}
