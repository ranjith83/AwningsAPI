using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteVisitValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.UpdateData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 55,
                column: "Value",
                value: "On Off");

            migrationBuilder.UpdateData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 57,
                column: "DisplayOrder",
                value: 2);

            migrationBuilder.InsertData(
                table: "SiteVisitValues",
                columns: new[] { "Id", "Category", "CreatedBy", "DateCreated", "DateUpdated", "DisplayOrder", "IsActive", "UpdatedBy", "Value" },
                values: new object[,]
                {
                    { 58, "SideInfills", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, null, "P1" },
                    { 59, "SideInfills", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "P2" },
                    { 60, "SideInfills", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3, true, null, "S1" },
                    { 61, "SideInfills", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4, true, null, "S2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.UpdateData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 55,
                column: "Value",
                value: "On");

            migrationBuilder.UpdateData(
                table: "SiteVisitValues",
                keyColumn: "Id",
                keyValue: 57,
                column: "DisplayOrder",
                value: 3);

            migrationBuilder.InsertData(
                table: "SiteVisitValues",
                columns: new[] { "Id", "Category", "CreatedBy", "DateCreated", "DateUpdated", "DisplayOrder", "IsActive", "UpdatedBy", "Value" },
                values: new object[] { 56, "ControllerBox", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, true, null, "Off" });
        }
    }
}
