using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class EmailTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingEmailId = table.Column<int>(type: "int", nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedToUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedByUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CompanyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WorkflowId = table.Column<int>(type: "int", nullable: true),
                    EmailBody = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    HasAttachments = table.Column<bool>(type: "bit", nullable: false),
                    SelectedAction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateProcessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompletionNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    ExtractedData = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    AIConfidence = table.Column<double>(type: "float", nullable: true),
                    AIReasoning = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTasks", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "TaskAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    EmailAttachmentId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BlobUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtractedText = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAttachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_TaskAttachments_EmailTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmailTasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_TaskComments_EmailTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmailTasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_TaskHistories_EmailTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "EmailTasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_AssignedToUserId",
                table: "EmailTasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_Category",
                table: "EmailTasks",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_CustomerId",
                table: "EmailTasks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_DateAdded",
                table: "EmailTasks",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_IncomingEmailId",
                table: "EmailTasks",
                column: "IncomingEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_Status",
                table: "EmailTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTasks_Status_DateAdded",
                table: "EmailTasks",
                columns: new[] { "Status", "DateAdded" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_EmailAttachmentId",
                table: "TaskAttachments",
                column: "EmailAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAttachments_TaskId",
                table: "TaskAttachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_DateCreated",
                table: "TaskComments",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_UserId",
                table: "TaskComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistories_DateCreated",
                table: "TaskHistories",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistories_TaskId",
                table: "TaskHistories",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistories_TaskId_DateCreated",
                table: "TaskHistories",
                columns: new[] { "TaskId", "DateCreated" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAttachments");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "TaskHistories");

            migrationBuilder.DropTable(
                name: "EmailTasks");
        }
    }
}
