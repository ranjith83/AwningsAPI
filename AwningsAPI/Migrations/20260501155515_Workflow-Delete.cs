using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InitialEnquiries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "InitialEnquiries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InitialEnquiries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_InitialEnquiries_IsDeleted",
                table: "InitialEnquiries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InitialEnquiries_WorkflowId",
                table: "InitialEnquiries",
                column: "WorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InitialEnquiries_IsDeleted",
                table: "InitialEnquiries");

            migrationBuilder.DropIndex(
                name: "IX_InitialEnquiries_WorkflowId",
                table: "InitialEnquiries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InitialEnquiries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "InitialEnquiries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InitialEnquiries");
        }
    }
}
