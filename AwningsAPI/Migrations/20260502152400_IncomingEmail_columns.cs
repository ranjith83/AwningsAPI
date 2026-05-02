using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class IncomingEmail_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BodyContent",
                table: "IncomingEmails",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)");

            migrationBuilder.AddColumn<string>(
                name: "BodyBlobUrl",
                table: "IncomingEmails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyBlobUrl",
                table: "IncomingEmails");

            migrationBuilder.AlterColumn<string>(
                name: "BodyContent",
                table: "IncomingEmails",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);
        }
    }
}
