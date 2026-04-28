using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class FollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomingEmailId",
                table: "InitialEnquiries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "InitialEnquiries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkflowFollowUps",
                columns: table => new
                {
                    FollowUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EnquiryId = table.Column<int>(type: "int", nullable: false),
                    LastEnquiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnquiryComments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EnquiryEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Inquiry"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDismissed = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DismissReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowFollowUps", x => x.FollowUpId);
                    table.ForeignKey(
                        name: "FK_WorkflowFollowUps_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkflowFollowUps_InitialEnquiries_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "InitialEnquiries",
                        principalColumn: "EnquiryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowFollowUps_WorkflowStarts_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "WorkflowStarts",
                        principalColumn: "WorkflowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowFollowUps_CustomerId",
                table: "WorkflowFollowUps",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowFollowUps_DateAdded",
                table: "WorkflowFollowUps",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowFollowUps_EnquiryId",
                table: "WorkflowFollowUps",
                column: "EnquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowFollowUps_IsDismissed",
                table: "WorkflowFollowUps",
                column: "IsDismissed");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowFollowUps_WorkflowId",
                table: "WorkflowFollowUps",
                column: "WorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowFollowUps");

            migrationBuilder.DropColumn(
                name: "IncomingEmailId",
                table: "InitialEnquiries");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "InitialEnquiries");
        }
    }
}
