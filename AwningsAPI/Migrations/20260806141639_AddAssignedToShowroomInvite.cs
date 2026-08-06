using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedToShowroomInvite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "ShowroomInvites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserName",
                table: "ShowroomInvites",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "ShowroomInvites");

            migrationBuilder.DropColumn(
                name: "AssignedToUserName",
                table: "ShowroomInvites");
        }
    }
}
