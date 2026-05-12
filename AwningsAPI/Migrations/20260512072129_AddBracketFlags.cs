using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBracketFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Brackets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPriceIgnored",
                table: "Brackets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 1,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 2,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 3,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 4,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 5,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 6,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 7,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 8,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 9,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 10,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 11,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 12,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 13,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 14,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 15,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 16,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 17,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 18,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 19,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 20,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 21,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 22,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 23,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 24,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 25,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 26,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 27,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 28,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 29,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 30,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 31,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 32,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 33,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 34,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 35,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 36,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 37,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 38,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 39,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 40,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 41,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 42,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 43,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 44,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 45,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 46,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 47,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 48,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 49,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 50,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 51,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 52,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 53,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 54,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 55,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 56,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 57,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 58,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 59,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 60,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 61,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 62,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 63,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 64,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 65,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 66,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 67,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 68,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 69,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 70,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 71,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 72,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 73,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 74,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 75,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 76,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 77,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 78,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 79,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 80,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 81,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 82,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 83,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 84,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 85,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 86,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 87,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 88,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 89,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 90,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 91,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 92,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 93,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 94,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 95,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 96,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 97,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 98,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 99,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 100,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 101,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 102,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 103,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 104,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 105,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 106,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 107,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 108,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 109,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 110,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 111,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 112,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 113,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 114,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 115,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 116,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 117,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 118,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 119,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 120,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 121,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 122,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 123,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 124,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 125,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 126,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 127,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 128,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 129,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 130,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 131,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 132,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 133,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 134,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 135,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 136,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 137,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 138,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 139,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 140,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 141,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 142,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 143,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 144,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 145,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 146,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 153,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 154,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 155,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 156,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 157,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 158,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 159,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 160,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 161,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 162,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 163,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 164,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 165,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 166,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 167,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 174,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 175,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 176,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 177,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 178,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 179,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 180,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 181,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 182,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 183,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 184,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 185,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 186,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 187,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 188,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 189,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 190,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 191,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 192,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 193,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 194,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 195,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 196,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 197,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 198,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 199,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 200,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 201,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 202,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 203,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 204,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 205,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 206,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 207,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 208,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 209,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 210,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 211,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 212,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 213,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 214,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 215,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 216,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 217,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 218,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 219,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 220,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 221,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 222,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 223,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 224,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 225,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 226,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 227,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 228,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 229,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 230,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 231,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 232,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 233,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 234,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 235,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 236,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 237,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 238,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 239,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 240,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 241,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 242,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 243,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 244,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 245,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 246,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 247,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 248,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 249,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 250,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 251,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 252,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 253,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 254,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 255,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 256,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 257,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 258,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 259,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 260,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 261,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 262,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 263,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 264,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 265,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 266,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 267,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 268,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 269,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 270,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 271,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 272,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 273,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 274,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 275,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 276,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 277,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 278,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 279,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 280,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 281,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 282,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 283,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 284,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 285,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 286,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 287,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 288,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 289,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 290,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 291,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 292,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 295,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 296,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 297,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 298,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 299,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 300,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 301,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 302,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 303,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 304,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 305,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 306,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 307,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 308,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 309,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 310,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 311,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 312,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 313,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 314,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 315,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 316,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 317,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 318,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 319,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 320,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 321,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 322,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 323,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 324,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 325,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 326,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 327,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 328,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 329,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 330,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 331,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 332,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 333,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 334,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 335,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 336,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 337,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 338,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 339,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 340,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 341,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 342,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 343,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "BrackUpdate-Database\r\nBuild started...\r\nBuild succeeded.\r\nMicrosoft.Data.SqlClient.SqlException (0x80131904): Incorrect syntax near the keyword 'WHERE'.\r\n   at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)\r\n   at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)\r\n   at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)\r\n   at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)\r\n   at Microsoft.Data.SqlClient.SqlCommand.RunExecuteNonQueryTds(String methodName, Boolean isAsync, Int32 timeout, Boolean asyncWrite)\r\n   at Microsoft.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, Boolean sendToPipe, Int32 timeout, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry, String methodName)\r\n   at Microsoft.Data.SqlClient.SqlCommand.ExecuteNonQuery()\r\n   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteNonQuery(RelationalCommandParameterObject parameterObject)\r\n   at Microsoft.EntityFrameworkCore.Migrations.MigrationCommand.ExecuteNonQuery(IRelationalConnection connection, IReadOnlyDictionary`2 parameterValues)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationCommandExecutor.Execute(IReadOnlyList`1 migrationCommands, IRelationalConnection connection, MigrationExecutionState executionState, Boolean beginTransaction, Boolean commitTransaction, Nullable`1 isolationLevel)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationCommandExecutor.<>c.<ExecuteNonQuery>b__3_1(DbContext _, ValueTuple`6 s)\r\n   at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.Execute[TState,TResult](TState state, Func`3 operation, Func`3 verifySucceeded)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationCommandExecutor.ExecuteNonQuery(IReadOnlyList`1 migrationCommands, IRelationalConnection connection, MigrationExecutionState executionState, Boolean commitTransaction, Nullable`1 isolationLevel)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.MigrateImplementation(DbContext context, String targetMigration, MigrationExecutionState state, Boolean useTransaction)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.<>c.<Migrate>b__20_1(DbContext c, ValueTuple`4 s)\r\n   at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.Execute[TState,TResult](TState state, Func`3 operation, Func`3 verifySucceeded)\r\n   at Microsoft.EntityFrameworkCore.Migrations.Internal.Migrator.Migrate(String targetMigration)\r\n   at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.UpdateDatabase(String targetMigration, String connectionString, String contextType)\r\n   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabaseImpl(String targetMigration, String connectionString, String contextType)\r\n   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.UpdateDatabase.<>c__DisplayClass0_0.<.ctor>b__0()\r\n   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)\r\nClientConnectionId:9d13e590-3671-4fb7-b324-2543b0e342eb\r\nError Number:156,State:1,Class:15\r\nIncorrect syntax near the keyword 'WHERE'.ets",
                keyColumn: "BracketId",
                keyValue: 344,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 345,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 346,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 347,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 348,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 349,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 350,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 351,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 352,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 353,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 354,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 355,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 356,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 357,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 358,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 365,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 366,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 367,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 368,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 369,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 370,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 371,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 372,
                columns: new string[0],
                values: new object[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Brackets");

            migrationBuilder.DropColumn(
                name: "IsPriceIgnored",
                table: "Brackets");
        }
    }
}
