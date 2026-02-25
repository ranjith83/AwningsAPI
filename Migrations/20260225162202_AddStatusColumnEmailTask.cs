using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnEmailTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousAssignedToUserId",
                table: "EmailTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousAssignedToUserName",
                table: "EmailTasks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousAssignedToUserId",
                table: "EmailTasks");

            migrationBuilder.DropColumn(
                name: "PreviousAssignedToUserName",
                table: "EmailTasks");
        }
    }
}
