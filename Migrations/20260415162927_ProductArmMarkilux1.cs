using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductArmMarkilux1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Description",
                value: "Markilux MX-4 Single");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "Description",
                value: "Markilux MX-4 Coupler");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "Description",
                value: "Markilux MX-2");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Description",
                value: "Markilux 6000 Single");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                column: "Description",
                value: "Markilux 6000 Coupler");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                column: "Description",
                value: "Markilux MX-3");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                column: "Description",
                value: "Markilux 990");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "Description",
                value: "Markilux 970");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "Description",
                value: "Markilux 5010 Single");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "Description",
                value: "Markilux 5010 Coupler");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "Description",
                value: "Markilux 3300 Single");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                column: "Description",
                value: "Markilux 3300 Coupler");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                column: "Description",
                value: "Markilux 1710");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                column: "Description",
                value: "Markilux 900");

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 22,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 23,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 24,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 25,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 26,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 27,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 29,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 30,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 31,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 32,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 33,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 34,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 35,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 36,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 37,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 38,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 39,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 40,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 41,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 42,
                column: "ArmTypeId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 43,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 1,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 2, 3, 796, DateTimeKind.Unspecified), "All" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 2,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 2, 3, 796, DateTimeKind.Unspecified), "2-0-2" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 3,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 37, 4, 580, DateTimeKind.Unspecified), "2-0-3" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 4,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 8, 51, 993, DateTimeKind.Unspecified), "2-0-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 5,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 2, 3, 796, DateTimeKind.Unspecified), "2-1-3" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 6,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 8, 51, 993, DateTimeKind.Unspecified), "2-1-5" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 7,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 10, 49, 347, DateTimeKind.Unspecified), "3-1-5" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 8,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 2, 3, 796, DateTimeKind.Unspecified), "3-2-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 9,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 8, 51, 993, DateTimeKind.Unspecified), "3-2-6" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 10,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 21, 28, 550, DateTimeKind.Unspecified), "4-0-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 11,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 9, 47, 603, DateTimeKind.Unspecified), "4-0-5" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 12,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 52, 53, 557, DateTimeKind.Unspecified), "4-0-6" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 13,
                column: "DateCreated",
                value: new DateTime(2026, 4, 15, 13, 9, 47, 603, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 14,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 13, 11, 21, 477, DateTimeKind.Unspecified), "4-0-8" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 15,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 15, 11, 21, 28, 550, DateTimeKind.Unspecified), "4-2-6" });

            migrationBuilder.InsertData(
                table: "armsTypes",
                columns: new[] { "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "UpdatedBy" },
                values: new object[,]
                {
                    { 16, "System", new DateTime(2026, 4, 15, 13, 9, 47, 603, DateTimeKind.Unspecified), null, "4-2-9", null },
                    { 17, "System", new DateTime(2026, 4, 15, 13, 11, 37, 573, DateTimeKind.Unspecified), null, "6-2-10", null },
                    { 18, "System", new DateTime(2026, 4, 15, 11, 21, 28, 550, DateTimeKind.Unspecified), null, "6-4-8", null },
                    { 19, "System", new DateTime(2026, 4, 15, 13, 9, 47, 603, DateTimeKind.Unspecified), null, "6-4-11", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Description",
                value: "Markilux MX-4");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "Description",
                value: "Markilux MX-2");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "Description",
                value: "Markilux 6000");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Description",
                value: "Markilux MX-3");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                column: "Description",
                value: "Markilux 990");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                column: "Description",
                value: "Markilux 970");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                column: "Description",
                value: "Markilux 5010");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "Description",
                value: "Markilux 3300");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "Description",
                value: "Markilux 1710");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "Description",
                value: "Markilux 900");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "Description",
                value: "Markilux 3300 Semi");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                column: "Description",
                value: "Markilux 6000");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                column: "Description",
                value: "Markilux 779");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                column: "Description",
                value: "Markilux 8800");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "ProductTypeId", "SupplierId", "UpdatedBy" },
                values: new object[] { 16, "System", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Markilux 6000 XXL", 1, 1, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 22,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 23,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 24,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 25,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 26,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 27,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 29,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 30,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 31,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 32,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 33,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 34,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 35,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 36,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 37,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 38,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 39,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 40,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 41,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 42,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 43,
                column: "ArmTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 1,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "2-0-2" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 2,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "2-1-3" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 3,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "3-2-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 4,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), "4-0-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 5,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), "4-0-6" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 6,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), "4-2-6" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 7,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 6, 13, 0, 30, 0, DateTimeKind.Unspecified), "6-4-8" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 8,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 8, 19, 35, 51, 0, DateTimeKind.Unspecified), "2-0-3" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 9,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 9, 27, 27, 0, DateTimeKind.Unspecified), "2-0-4" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 10,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "3-2-6" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 11,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "2-1-5" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 12,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "4-0-5" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 13,
                column: "DateCreated",
                value: new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 14,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "4-2-9" });

            migrationBuilder.UpdateData(
                table: "armsTypes",
                keyColumn: "ArmTypeId",
                keyValue: 15,
                columns: new[] { "DateCreated", "Description" },
                values: new object[] { new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "6-4-11" });
        }
    }
}
