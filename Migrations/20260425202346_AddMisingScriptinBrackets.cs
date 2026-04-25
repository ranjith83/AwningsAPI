    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMisingScriptinBrackets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql(@"
                 SET IDENTITY_INSERT Brackets ON;

                INSERT INTO Brackets(BracketId, ArmTypeId, BracketName, CreatedBy, DateCreated, Price, ProductId)" +
                " VALUES " +
                "(398, 1, 'Surcharge for junction roller', 'System', '2023-01-01T12:00:00', 291, 13)," +
                "(399, 1, 'Surcharge for one-piece cover', 'System', '2023-01-01T12:00:00', 291, 13)" +
                " SET IDENTITY_INSERT Brackets OFF;");


            migrationBuilder.Sql(@"update Motors set Price = 484 where MotorId in (73,74,75);
                    Update Motors Set Price = 574 where MotorId = 76");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
