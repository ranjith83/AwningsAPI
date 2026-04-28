using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "UserSignatures",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "UserSignatures",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserSignatures",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserSignatures",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GreetingText",
                table: "UserSignatures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "Kindest regards,");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "UserSignatures",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LayoutOrder",
                table: "UserSignatures",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "name_first");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "UserSignatures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "UserSignatures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeparatorStyle",
                table: "UserSignatures",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "blank_line");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "UserSignatures",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSignatures_Username",
                table: "UserSignatures",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSignatures_Username",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "GreetingText",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "LayoutOrder",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "SeparatorStyle",
                table: "UserSignatures");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "UserSignatures");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "UserSignatures",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
