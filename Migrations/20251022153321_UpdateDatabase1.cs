using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStart",
                table: "WorkflowStart");

            migrationBuilder.RenameTable(
                name: "WorkflowStart",
                newName: "WorkflowStarts");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "WorkflowStarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStarts",
                table: "WorkflowStarts",
                column: "WorkflowId");

            migrationBuilder.UpdateData(
                table: "WorkflowStarts",
                keyColumn: "WorkflowId",
                keyValue: 1,
                column: "CompanyId",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkflowStarts",
                table: "WorkflowStarts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WorkflowStarts");

            migrationBuilder.RenameTable(
                name: "WorkflowStarts",
                newName: "WorkflowStart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkflowStart",
                table: "WorkflowStart",
                column: "WorkflowId");
        }
    }
}
