using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Residential = table.Column<bool>(type: "bit", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eircode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerContacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_CustomerContacts_Customers_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Customers",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CompanyId", "Address1", "Address2", "Address3", "CompanyNumber", "CountryId", "County", "CreatedBy", "DateCreated", "Eircode", "Email", "Fax", "Mobile", "Name", "Phone", "RegistrationNumber", "Residential", "TaxNumber", "UpdatedBy", "UpdatedDate", "VATNumber" },
                values: new object[] { 1, "123 Main Street", "Suite 100", null, "ACME123", 1, "Dublin", 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "D01XY12", "info@acme.ie", null, "+35387654321", "Acme Corporation", "+35312345678", "REG456789", false, "TAX123456", null, null, "VAT987654" });

            migrationBuilder.InsertData(
                table: "CustomerContacts",
                columns: new[] { "ContactId", "CompanyId", "CreatedBy", "DateCreated", "DateOfBirth", "DateUpdated", "Email", "FirstName", "LastName", "Mobile", "Phone", "UpdatedBy" },
                values: new object[] { 1, 1, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "john.doe@acme.ie", "John", "Doe", "+35387654322", "+35312345679", null });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContacts_CompanyId",
                table: "CustomerContacts",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerContacts");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
