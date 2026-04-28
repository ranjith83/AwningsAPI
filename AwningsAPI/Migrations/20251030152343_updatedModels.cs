using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Suppliers");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "WorkflowStarts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "WorkflowStarts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "WorkflowStarts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "wallSealingProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "wallSealingProfiles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "wallSealingProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "valanceStyles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "valanceStyles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "valanceStyles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Projections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Projections",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Projections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "ProductTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Products",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "nonStandardRALColours",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "nonStandardRALColours",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "nonStandardRALColours",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Motors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Motors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Motors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "InitialEnquiries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "InitialEnquiries",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "InitialEnquiries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Heaters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Heaters",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Heaters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "CustomerContacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "CustomerContacts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Brackets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Brackets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Brackets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "BFs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "BFs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "BFs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Arms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Arms",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Arms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 12,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 13,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 14,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 15,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 16,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "CustomerContacts",
                keyColumn: "ContactId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "System", null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "System", null });

            migrationBuilder.UpdateData(
                table: "Heaters",
                keyColumn: "HeaterId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "WorkflowStarts",
                keyColumn: "WorkflowId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { "System", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Suppliers");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "WorkflowStarts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "WorkflowStarts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "WorkflowStarts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "wallSealingProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "wallSealingProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "wallSealingProfiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "valanceStyles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "valanceStyles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "valanceStyles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Suppliers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Projections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Projections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Projections",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "ProductTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "nonStandardRALColours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "nonStandardRALColours",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "nonStandardRALColours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Motors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Motors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Motors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Invoices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "InitialEnquiries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "InitialEnquiries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "InitialEnquiries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Heaters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Heaters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Heaters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "CustomerContacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "CustomerContacts",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Brackets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Brackets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Brackets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "BFs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "BFs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "BFs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Arms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Arms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Arms",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Arms",
                keyColumn: "ArmId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "BFs",
                keyColumn: "BFId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 12,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 13,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 14,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 15,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 16,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "CustomerContacts",
                keyColumn: "ContactId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { 1, null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { 1, null });

            migrationBuilder.UpdateData(
                table: "Heaters",
                keyColumn: "HeaterId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "ProductTypes",
                keyColumn: "ProductTypeId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 7,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 8,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 9,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 10,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 11,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 12,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 13,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 14,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 15,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 16,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 17,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 18,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 19,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 20,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 21,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "Updated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "Updated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "Updated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "WorkflowStarts",
                keyColumn: "WorkflowId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 1,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 2,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 3,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 4,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 5,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });

            migrationBuilder.UpdateData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 6,
                columns: new[] { "CreatedBy", "DateUpdated", "UpdatedBy" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 });
        }
    }
}
