using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixMotorDataProduct13And14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean up any residual history entries from previously failed migrations
            // Product 14 (Markilux 1710) — remove manual override motor options (not applicable)
            migrationBuilder.Sql("DELETE FROM Motors WHERE MotorId IN (109, 110, 111, 112, 113, 114)");

            // Product 13 (Markilux 3300 Coupler) — correct motor prices
            migrationBuilder.Sql("UPDATE Motors SET Price = 574 WHERE MotorId IN (85, 86, 87)");
            migrationBuilder.Sql("UPDATE Motors SET Price = 682 WHERE MotorId = 88");
            migrationBuilder.Sql("UPDATE Motors SET Price = 809 WHERE MotorId IN (89, 90, 91)");
            migrationBuilder.Sql("UPDATE Motors SET Price = 916 WHERE MotorId = 92");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
