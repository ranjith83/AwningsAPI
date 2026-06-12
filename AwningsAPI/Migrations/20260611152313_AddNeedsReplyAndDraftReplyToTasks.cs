using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNeedsReplyAndDraftReplyToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DraftReply",
                table: "Tasks",
                type: "nvarchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsReply",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DraftReply",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "NeedsReply",
                table: "Tasks");
        }
    }
}
