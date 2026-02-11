using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class EmailService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomingEmails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyPreview = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyContent = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsHtml = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasAttachments = table.Column<bool>(type: "bit", nullable: false),
                    Importance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryConfidence = table.Column<double>(type: "float", nullable: true),
                    ExtractedData = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateProcessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingEmailId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    BlobStorageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Base64Content = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    IsInline = table.Column<bool>(type: "bit", nullable: false),
                    ExtractedText = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateDownloaded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAttachments_IncomingEmails_IncomingEmailId",
                        column: x => x.IncomingEmailId,
                        principalTable: "IncomingEmails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachments_IncomingEmailId",
                table: "EmailAttachments",
                column: "IncomingEmailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAttachments");

            migrationBuilder.DropTable(
                name: "IncomingEmails");
        }
    }
}
