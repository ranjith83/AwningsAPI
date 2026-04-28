using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameEmailTasksToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttachments_EmailTasks_TaskId",
                table: "TaskAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_EmailTasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_EmailTasks_TaskId",
                table: "TaskHistories");

            migrationBuilder.DropTable(
                name: "EmailTasks");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Email"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IncomingEmailId = table.Column<int>(type: "int", nullable: true),
                    FromName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FromEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedToUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedByUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PreviousAssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    PreviousAssignedToUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CompanyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WorkflowId = table.Column<int>(type: "int", nullable: true),
                    SiteVisitId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToUserId",
                table: "Tasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Category",
                table: "Tasks",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CustomerId",
                table: "Tasks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DateAdded",
                table: "Tasks",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IncomingEmailId",
                table: "Tasks",
                column: "IncomingEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SourceType",
                table: "Tasks",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SourceType_Status_DateAdded",
                table: "Tasks",
                columns: new[] { "SourceType", "Status", "DateAdded" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status",
                table: "Tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Status_DateAdded",
                table: "Tasks",
                columns: new[] { "Status", "DateAdded" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_Tasks_TaskId",
                table: "TaskHistories",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskHistories_Tasks_TaskId",
                table: "TaskHistories");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.CreateTable(
                name: "EmailTasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AIConfidence = table.Column<double>(type: "float", nullable: true),
                    AIReasoning = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedByUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedToUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionNotes = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateProcessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailBody = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    ExtractedData = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    FromEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HasAttachments = table.Column<bool>(type: "bit", nullable: false),
                    IncomingEmailId = table.Column<int>(type: "int", nullable: false),
                    PreviousAssignedToUserId = table.Column<int>(type: "int", nullable: true),
                    PreviousAssignedToUserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SelectedAction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkflowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTasks", x => x.TaskId);
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

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttachments_EmailTasks_TaskId",
                table: "TaskAttachments",
                column: "TaskId",
                principalTable: "EmailTasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_EmailTasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "EmailTasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskHistories_EmailTasks_TaskId",
                table: "TaskHistories",
                column: "TaskId",
                principalTable: "EmailTasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
