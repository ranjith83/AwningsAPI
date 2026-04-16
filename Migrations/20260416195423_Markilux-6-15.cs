using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Markilux615 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "ArmTypeId",
                 table: "Motors",
                 type: "int",
                 nullable: true);

            migrationBuilder.Sql(@"
                DELETE FROM ValanceStyles
                WHERE ValanceStyleId BETWEEN 1 AND 6;
                
                SET IDENTITY_INSERT dbo.ValanceStyles ON;
                INSERT INTO ValanceStyles (ValanceStyleId, WidthCm, Price, DateCreated, CreatedBy, ProductId)
                VALUES
                (1, 250, 79,  GETDATE(), 'System', 5),
                (2, 300, 87,  GETDATE(), 'System', 5),
                (3, 350, 96,  GETDATE(), 'System', 5),
                (4, 400, 109, GETDATE(), 'System', 5),
                (5, 450, 122, GETDATE(), 'System', 5),
                (6, 500, 134, GETDATE(), 'System', 5);
                SET IDENTITY_INSERT dbo.ValanceStyles OFF;
            ");


            migrationBuilder.Sql(@"
                DELETE FROM WallSealingProfiles
                WHERE WallSealingProfileId BETWEEN 1 AND 6;

                SET IDENTITY_INSERT dbo.WallSealingProfiles ON;
                INSERT INTO WallSealingProfiles (WallSealingProfileId, WidthCm, Price, DateCreated, CreatedBy, ProductId)
                VALUES
                (1, 250, 90,  GETDATE(), 'System', 5),
                (2, 300, 104, GETDATE(), 'System', 5),
                (3, 350, 114, GETDATE(), 'System', 5),
                (4, 400, 130, GETDATE(), 'System', 5),
                (5, 450, 146, GETDATE(), 'System', 5),
                (6, 500, 162, GETDATE(), 'System', 5);
                SET IDENTITY_INSERT dbo.WallSealingProfiles OFF;
            ");

            migrationBuilder.Sql(@"
            DELETE FROM LightingCassettes
            WHERE LightingId BETWEEN 1 AND 4;

            SET IDENTITY_INSERT dbo.LightingCassettes ON;
            INSERT INTO LightingCassettes (LightingId, Description, Price, DateCreated, CreatedBy, ProductId)
            VALUES
            (1, 'Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)', 1555.00, '2026-04-07', 'System', 4),
            (2, 'Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)', 1386.00, '2026-04-07', 'System', 4)

            SET IDENTITY_INSERT dbo.LightingCassettes OFF;
        ");

            migrationBuilder.Sql(@"
            DELETE FROM Motors
            WHERE MotorId BETWEEN 1 AND 6;

            SET IDENTITY_INSERT dbo.Motors ON;
            INSERT INTO Motors (MotorId, Description, Price, DateCreated, CreatedBy, ProductId, ArmTypeId)
            VALUES
            (1, 'Surcharge for servo-assisted gear', 75, GETDATE(), 'System', 5, 2),
            (2, 'Surcharge for servo-assisted gear', 75, GETDATE(), 'System', 5, 3),
            (3, 'Surcharge for servo-assisted gear', 0,  GETDATE(), 'System', 5, 8),

            (4, 'Surcharge for hard-wired motor', 484, GETDATE(), 'System', 5, 2),
            (5, 'Surcharge for hard-wired motor', 484, GETDATE(), 'System', 5, 3),
            (6, 'Surcharge for hard-wired motor', 574, GETDATE(), 'System', 5, 8);
            SET IDENTITY_INSERT dbo.Motors OFF;
        ");

          

            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "ArmTypeId", "BracketName", "CreatedBy", "DateCreated", "DateUpdated", "PartNumber", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 101, 10, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 596m, 6, null },
                    { 102, 12, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 894m, 6, null },
                    { 103, 18, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1192m, 6, null },
                    { 104, 10, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1107m, 6, null },
                    { 105, 12, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1421m, 6, null },
                    { 106, 18, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1975m, 6, null },
                    { 107, 10, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1256m, 6, null },
                    { 108, 12, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1570m, 6, null },
                    { 109, 18, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 2198m, 6, null },
                    { 110, 10, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 794m, 6, null },
                    { 111, 12, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1191m, 6, null },
                    { 112, 18, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1588m, 6, null },
                    { 113, 10, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 950m, 6, null },
                    { 114, 12, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1424m, 6, null },
                    { 115, 18, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1899m, 6, null },
                    { 116, 10, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 6, null },
                    { 117, 12, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 6, null },
                    { 118, 18, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 536m, 6, null },
                    { 119, 1, "Surcharge for junction roller", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 6, null },
                    { 120, 1, "Surcharge for one-piece cover", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 6, null },
                    { 121, null, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 113m, 7, null },
                    { 122, null, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 369m, 7, null },
                    { 123, null, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 443m, 7, null },
                    { 124, null, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 122m, 7, null },
                    { 125, null, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 224m, 7, null },
                    { 126, null, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 7, null },
                    { 127, null, "Face fixture bracket 100 mm left / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75885", 56.40m, 7, null },
                    { 128, null, "Face fixture bracket 100 mm right / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75886", 56.40m, 7, null },
                    { 129, null, "Face fixture bracket 300 mm left / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75877", 89.50m, 7, null },
                    { 130, null, "Face fixture bracket 300 mm right / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75878", 89.50m, 7, null },
                    { 131, null, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72872", 253.50m, 7, null },
                    { 132, null, "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 300 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77968", 226.90m, 7, null },
                    { 133, null, "Top fixture bracket 100 mm left / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75887", 61m, 7, null },
                    { 134, null, "Top fixture bracket 100 mm right / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75888", 61m, 7, null },
                    { 135, null, "Eaves fixture bracket 150 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75889", 50.70m, 7, null },
                    { 136, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 7, null },
                    { 137, null, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 180m, 7, null },
                    { 138, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 7, null },
                    { 139, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 7, null },
                    { 140, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "73466", 127.70m, 7, null },
                    { 141, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "73465", 164.90m, 7, null },
                    { 142, null, "Spacer block face fixture 100x150x20 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "758831", 8.60m, 7, null },
                    { 143, null, "Spacer block face fixture 100x150x12 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "758841", 7.50m, 7, null },
                    { 144, null, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716311", 4.40m, 7, null },
                    { 145, null, "Spacer block for top fixture 90x140x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716411", 5m, 7, null },
                    { 146, null, "Cover plate 230x210x2 mm", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 7, null },
                    { 153, null, "Face fixture bracket 150 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71624", 44m, 8, null },
                    { 154, null, "Face fixture bracket 300 mm left / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "70617", 75.70m, 8, null },
                    { 155, null, "Face fixture bracket 300 mm right / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "70600", 75.70m, 8, null },
                    { 156, null, "Stand-off bkt. 80-300 mm for face fixture bracket 300 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77968", 226.90m, 8, null },
                    { 157, null, "Top fixture bracket 150 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71625", 44m, 8, null },
                    { 158, null, "Eaves fixture bracket 150mm, complete / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71669", 102.20m, 8, null },
                    { 159, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 8, null },
                    { 160, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 8, null },
                    { 161, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 8, null },
                    { 162, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75326", 127.70m, 8, null },
                    { 163, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75325", 164.90m, 8, null },
                    { 164, null, "Spacer block face or top fixt.136x150x20 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.70m, 8, null },
                    { 165, null, "Spacer block face or top fixt.136x150x12 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71644", 3.80m, 8, null },
                    { 166, null, "Cover plate 230x210x2 mm", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 8, null },
                    { 167, null, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 180m, 8, null },
                    { 174, null, "Face fixture bracket left / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72826", 109.80m, 9, null },
                    { 175, null, "Face fixture bracket right / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72827", 109.80m, 9, null },
                    { 176, null, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72872", 253.50m, 9, null },
                    { 177, null, "Top fixture bracket left / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72860", 138.70m, 9, null },
                    { 178, null, "Top fixture bracket right / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72861", 138.70m, 9, null },
                    { 179, null, "Eaves fixture bracket left 150 mm, complete / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72874", 185.30m, 9, null },
                    { 180, null, "Eaves fixture bracket right 150 mm, complete / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72875", 185.30m, 9, null },
                    { 181, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 9, null },
                    { 182, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 9, null },
                    { 183, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 9, null },
                    { 184, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72870", 186m, 9, null },
                    { 185, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "73465", 164.90m, 9, null },
                    { 186, null, "Spreader Plate C 310x130x12 mm / 6", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "72526", 186m, 9, null },
                    { 187, null, "Spacer block face fixture 100x120x20 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "718581", 14.70m, 9, null },
                    { 188, null, "Spacer block face fixture 100x120x12 mm / 3", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "718571", 14.30m, 9, null },
                    { 189, null, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716311", 4.40m, 9, null },
                    { 190, null, "Spacer block for top fixture 90x140x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716411", 5m, 9, null },
                    { 191, null, "Cover plate 230x210x2 mm", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 9, null },
                    { 192, null, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 180m, 9, null },
                    { 193, 2, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 263m, 10, null },
                    { 194, 3, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 394m, 10, null },
                    { 195, 8, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 525m, 10, null },
                    { 196, 2, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 518m, 10, null },
                    { 197, 3, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 653m, 10, null },
                    { 198, 8, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 912m, 10, null },
                    { 199, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 593m, 10, null },
                    { 200, 3, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 728m, 10, null },
                    { 201, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1024m, 10, null },
                    { 202, 2, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 326m, 10, null },
                    { 203, 3, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 489m, 10, null },
                    { 204, 8, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 652m, 10, null },
                    { 205, 2, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 404m, 10, null },
                    { 206, 3, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 605m, 10, null },
                    { 207, 8, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 807m, 10, null },
                    { 208, 2, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 10, null },
                    { 209, 3, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 10, null },
                    { 210, 8, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 10, null },
                    { 211, 2, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 121m, 10, null },
                    { 212, 3, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 121m, 10, null },
                    { 213, 8, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 177m, 10, null },
                    { 214, null, "Face fixture bracket assembly 5 - 35° / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77921", 131.20m, 10, null },
                    { 215, null, "Face fixture bracket assembly 38 - 65° / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77936", 131.20m, 10, null },
                    { 216, null, "Stand-off bkt. 80-300 mm for face fixture bkt. / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77969", 238.60m, 10, null },
                    { 217, null, "Top fixture bracket 5 - 35° / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77937", 163m, 10, null },
                    { 218, null, "Top fixture bracket 38 - 65° / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77938", 245.50m, 10, null },
                    { 219, null, "Eaves fixture bracket 150mm, complete / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77939", 201.60m, 10, null },
                    { 220, null, "Eaves fixture bracket 270 mm, complete / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77940", 231.90m, 10, null },
                    { 221, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "741290", 142.80m, 10, null },
                    { 222, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 10, null },
                    { 223, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75328", 127.70m, 10, null },
                    { 224, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "75327", 164.90m, 10, null },
                    { 225, null, "Spacer block face or top fixt.136x150x20 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.70m, 10, null },
                    { 226, null, "Spacer block face or top fixt.136x150x12 mm / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71644", 3.80m, 10, null },
                    { 227, null, "Cover plate 290x210x2 mm", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "71841", 21.10m, 10, null },
                    { 228, null, "Bottom fixture bracket 150 mm, 5 - 35° / 4", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "77941", 245.50m, 10, null },
                    { 229, 10, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 525m, 11, null },
                    { 230, 12, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 788m, 11, null },
                    { 231, 18, "Surcharge for face fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1050m, 11, null },
                    { 232, 10, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1036m, 11, null },
                    { 233, 12, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1306m, 11, null },
                    { 234, 18, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1824m, 11, null },
                    { 235, 10, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1185m, 11, null },
                    { 236, 12, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1455m, 11, null },
                    { 237, 18, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 2047m, 11, null },
                    { 238, 10, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 652m, 11, null },
                    { 239, 12, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 978m, 11, null },
                    { 240, 18, "Surcharge for top fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1304m, 11, null },
                    { 241, 10, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 807m, 11, null },
                    { 242, 12, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1210m, 11, null },
                    { 243, 18, "Surcharge for eaves fixture", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1613m, 11, null },
                    { 244, 10, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 11, null },
                    { 245, 12, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 11, null },
                    { 246, 18, "Surcharge for bespoke arms", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 536m, 11, null },
                    { 247, 10, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 234m, 11, null },
                    { 248, 12, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 234m, 11, null },
                    { 249, 18, "Surcharge for arms with bionic tendon", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 348m, 11, null },
                    { 250, 1, "Surcharge for junction roller", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 11, null },
                    { 251, 1, "Surcharge for one-piece cover", "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 291m, 11, null },
                    { 252, 2, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 79m, 12, null },
                    { 253, 4, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 130m, 12, null },
                    { 254, 6, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 156m, 12, null },
                    { 255, 9, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 195m, 12, null },
                    { 256, 2, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 335m, 12, null },
                    { 257, 4, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 394m, 12, null },
                    { 258, 6, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 424m, 12, null },
                    { 259, 9, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 591m, 12, null },
                    { 260, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 409m, 12, null },
                    { 261, 4, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 468m, 12, null },
                    { 262, 6, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 498m, 12, null },
                    { 263, 9, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 702m, 12, null },
                    { 264, 2, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 140m, 12, null },
                    { 265, 4, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 279m, 12, null },
                    { 266, 6, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 348m, 12, null },
                    { 267, 9, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 418m, 12, null },
                    { 268, 2, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 246m, 12, null },
                    { 269, 4, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 492m, 12, null },
                    { 270, 6, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 615m, 12, null },
                    { 271, 9, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 738m, 12, null },
                    { 272, 2, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 12, null },
                    { 273, 4, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 12, null },
                    { 274, 6, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 12, null },
                    { 275, 9, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 12, null },
                    { 276, null, "Face fixture bracket 200 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71966", 66.40m, 12, null },
                    { 277, null, "Face fixture bracket 100 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71964", 39.50m, 12, null },
                    { 278, null, "Face fixture bracket 60 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71955", 25.50m, 12, null },
                    { 279, null, "Stand-off bkt. 80-300 mm for face fixture for face fixture bracket 60 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77967", 226.90m, 12, null },
                    { 280, null, "Top fixture bracket 90 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77967", 69.60m, 12, null },
                    { 281, null, "Top fixture bracket 200 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71652", 138.30m, 12, null },
                    { 282, null, "Eaves fixture bracket 150mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71898", 123m, 12, null },
                    { 283, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 12, null },
                    { 284, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 12, null },
                    { 285, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 12, null },
                    { 286, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75324", 127.70m, 12, null },
                    { 287, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75323", 164.90m, 12, null },
                    { 288, null, "Spacer block face fixture 100x150x20 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "758831", 8.60m, 12, null },
                    { 289, null, "Spacer block face fixture 60x140x20 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716321", 4.10m, 12, null },
                    { 290, null, "Spacer block face fixture 60x140x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71642", 4.10m, 12, null },
                    { 291, null, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716311", 4.40m, 12, null },
                    { 292, null, "Cover plate 230x210x2 mm for face fixture bracket 100 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 12, null },
                    { 295, 11, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 158m, 13, null },
                    { 296, 13, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 260m, 13, null },
                    { 297, 16, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 311m, 13, null },
                    { 298, 19, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 390m, 13, null },
                    { 299, 11, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 669m, 13, null },
                    { 300, 13, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 788m, 13, null },
                    { 301, 16, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 847m, 13, null },
                    { 302, 19, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1181m, 13, null },
                    { 303, 11, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 818m, 13, null },
                    { 304, 13, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 936m, 13, null },
                    { 305, 16, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 996m, 13, null },
                    { 306, 19, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1404m, 13, null },
                    { 307, 11, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 279m, 13, null },
                    { 308, 13, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 557m, 13, null },
                    { 309, 16, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 696m, 13, null },
                    { 310, 19, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 836m, 13, null },
                    { 311, 11, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 492m, 13, null },
                    { 312, 13, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 984m, 13, null },
                    { 313, 16, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1230m, 13, null },
                    { 314, 19, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1476m, 13, null },
                    { 315, 11, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 13, null },
                    { 316, 13, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 13, null },
                    { 317, 16, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 361m, 13, null },
                    { 318, 19, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 536m, 13, null },
                    { 319, 2, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 146m, 14, null },
                    { 320, 5, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 196m, 14, null },
                    { 321, 8, "Surcharge for face fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 14, null },
                    { 322, 2, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 402m, 14, null },
                    { 323, 5, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 455m, 14, null },
                    { 324, 8, "Surcharge for face fixture incl. spreader plate A", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 656m, 14, null },
                    { 325, 2, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 476m, 14, null },
                    { 326, 5, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 530m, 14, null },
                    { 327, 8, "Surcharge for face fixture incl. spreader plate B", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 767m, 14, null },
                    { 328, 2, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 196m, 14, null },
                    { 329, 5, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 260m, 14, null },
                    { 330, 8, "Surcharge for top fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 358m, 14, null },
                    { 331, 2, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 290m, 14, null },
                    { 332, 5, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 435m, 14, null },
                    { 333, 8, "Surcharge for eaves fixture", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 580m, 14, null },
                    { 334, 2, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 14, null },
                    { 335, 5, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 183m, 14, null },
                    { 336, 8, "Surcharge for bespoke arms", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, 269m, 14, null },
                    { 337, null, "Face fixture bracket 100 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "70867", 72.90m, 14, null },
                    { 338, null, "Face fixture bracket 45 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71813", 50.10m, 14, null },
                    { 339, null, "Stand-off bracket 80-300 mm for face fixture bracket 100 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77969", 226.90m, 14, null },
                    { 340, null, "Stand-off bracket 80-300 mm for 45 mm face fixture bracket", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "77967", 226.90m, 14, null },
                    { 341, null, "Top fixture bracket 90 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "70868", 97.70m, 14, null },
                    { 342, null, "Top fixture bracket 45 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71818", 64.30m, 14, null },
                    { 343, null, "Eaves fixture bracket 150mm, complete / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "70871", 144.90m, 14, null },
                    { 344, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 14, null },
                    { 345, null, "Adjustable eaves fixture bracket / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71198", 139.90m, 14, null },
                    { 346, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 14, null },
                    { 347, null, "Vertical fixture rail incl. fixing material 624291", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62421", 180m, 14, null },
                    { 348, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 14, null },
                    { 349, null, "Spreader plate A 430x160x12 mm / 8", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75326", 127.70m, 14, null },
                    { 350, null, "Spreader plate B 300x400x12 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75325", 164.90m, 14, null },
                    { 351, null, "Spacer block face fixture 100x150x20 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "758831", 8.60m, 14, null },
                    { 352, null, "Spacer block face fixture 45x150x20 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "718251", 4m, 14, null },
                    { 353, null, "Spacer block face fixture 100x150x12 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "758841", 7.50m, 14, null },
                    { 354, null, "Spacer block face fixture 45x150x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71826", 3.50m, 14, null },
                    { 355, null, "Spacer block for top fixture 90x140x20 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716311", 4.40m, 14, null },
                    { 356, null, "Spacer block for top fixture 45x140x20 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716261", 3.40m, 14, null },
                    { 357, null, "Cover plate 230x210x2 mm for face fixture bracket 100 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71843", 17m, 14, null },
                    { 358, null, "Cover plate 210x230x2 mm for face fixture bracket 45 mm", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71844", 15.80m, 14, null },
                    { 365, null, "Face fixture bracket right / top bracket left / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62247", 99.10m, 15, null },
                    { 366, null, "Face fixture bracket left / Top bracket right / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "62248", 99.10m, 15, null },
                    { 367, null, "Eaves fixture bracket 150 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75889", 50.70m, 15, null },
                    { 368, null, "Eaves fixture bracket 270 mm / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71659", 79.30m, 15, null },
                    { 369, null, "Spacer block face or top fixt.136x150x20 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716331", 5.70m, 15, null },
                    { 370, null, "Spacer block face or top fixt.136x150x12 mm / 3", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "71644", 3.80m, 15, null },
                    { 371, null, "Angle and plate for eaves fixture (machine finish) / 4", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "716620", 128.90m, 15, null },
                    { 372, null, "Additional eaves fixture plate 60x260x12 mm / 2", "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "75383", 43.90m, 15, null }
                });

            migrationBuilder.InsertData(
                table: "LightingCassettes",
                columns: new[] { "LightingId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)", 1555m, 7, null },
                    { 4, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)", 1386m, 7, null }
                });

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 1,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 2,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 3,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 4,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 5,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 6,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 7,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 8,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 9,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 10,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 11,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 12,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 13,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 14,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 15,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 16,
                column: "ArmTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 17,
                column: "ArmTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 18,
                column: "ArmTypeId",
                value: 8);

            migrationBuilder.InsertData(
                table: "Motors",
                columns: new[] { "MotorId", "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy" },
                values: new object[,]
                {
                    { 19, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 6, null },
                    { 20, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 6, null },
                    { 21, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 682m, 6, null },
                    { 22, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 6, null },
                    { 23, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 6, null },
                    { 24, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 916m, 6, null },
                    { 25, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 6, null },
                    { 26, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 6, null },
                    { 27, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 798m, 6, null },
                    { 28, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 7, null },
                    { 29, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 7, null },
                    { 30, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 7, null },
                    { 31, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 7, null },
                    { 32, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 8, null },
                    { 33, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 8, null },
                    { 34, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 8, null },
                    { 35, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 8, null },
                    { 36, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1115m, 8, null },
                    { 37, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 997m, 8, null },
                    { 38, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 9, null },
                    { 39, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 9, null },
                    { 40, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 9, null },
                    { 41, null, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 9, null },
                    { 42, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 10, null },
                    { 43, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 10, null },
                    { 44, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 0m, 10, null },
                    { 45, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 10, null },
                    { 46, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 10, null },
                    { 47, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 10, null },
                    { 48, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 10, null },
                    { 49, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 10, null },
                    { 50, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 10, null },
                    { 51, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 10, null },
                    { 52, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 10, null },
                    { 53, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 10, null },
                    { 54, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1115m, 10, null },
                    { 55, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 1115m, 10, null },
                    { 56, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 0m, 10, null },
                    { 57, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 997m, 10, null },
                    { 58, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 997m, 10, null },
                    { 59, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 0m, 10, null },
                    { 60, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 11, null },
                    { 61, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 11, null },
                    { 62, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 682m, 11, null },
                    { 63, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 11, null },
                    { 64, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 11, null },
                    { 65, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 916m, 11, null },
                    { 66, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 11, null },
                    { 67, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 11, null },
                    { 68, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 798m, 11, null },
                    { 69, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 12, null },
                    { 70, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 12, null },
                    { 71, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 12, null },
                    { 72, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 0m, 12, null },
                    { 73, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 75m, 12, null },
                    { 74, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 75m, 12, null },
                    { 75, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 75m, 12, null },
                    { 76, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 0m, 12, null },
                    { 77, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 12, null },
                    { 78, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 12, null },
                    { 79, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 12, null },
                    { 80, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 12, null },
                    { 81, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 12, null },
                    { 82, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 12, null },
                    { 83, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 12, null },
                    { 84, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 12, null },
                    { 85, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 361m, 13, null },
                    { 86, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 361m, 13, null },
                    { 87, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 361m, 13, null },
                    { 88, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 536m, 13, null },
                    { 89, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 574m, 13, null },
                    { 90, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 574m, 13, null },
                    { 91, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 574m, 13, null },
                    { 92, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 682m, 13, null },
                    { 93, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 13, null },
                    { 94, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 13, null },
                    { 95, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 13, null },
                    { 96, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 798m, 13, null },
                    { 97, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 14, null },
                    { 98, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 75m, 14, null },
                    { 99, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for servo-assisted gear", 0m, 14, null },
                    { 100, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 14, null },
                    { 101, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 14, null },
                    { 102, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 574m, 14, null },
                    { 103, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 14, null },
                    { 104, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 14, null },
                    { 105, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 809m, 14, null },
                    { 106, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 14, null },
                    { 107, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 14, null },
                    { 108, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 691m, 14, null },
                    { 109, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 721m, 14, null },
                    { 110, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 721m, 14, null },
                    { 111, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override + 1 ch. transmitter", 809m, 14, null },
                    { 112, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 603m, 14, null },
                    { 113, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 603m, 14, null },
                    { 114, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io with manual override w/o transmitter", 691m, 14, null },
                    { 115, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for hard-wired motor", 484m, 15, null },
                    { 116, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io + 1 ch. transmitter", 721m, 15, null },
                    { 117, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for radio-contr. motor io w/o transmitter", 603m, 15, null }
                });

            migrationBuilder.InsertData(
                table: "Projections",
                columns: new[] { "ProjectionId", "ArmTypeId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "Projection_cm", "UpdatedBy", "Width_cm" },
                values: new object[,]
                {
                    { 203, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5644m, 6, 150, null, 500 },
                    { 204, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5953m, 6, 150, null, 600 },
                    { 205, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6335m, 6, 150, null, 700 },
                    { 206, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6726m, 6, 150, null, 800 },
                    { 207, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7148m, 6, 150, null, 900 },
                    { 208, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7557m, 6, 150, null, 1000 },
                    { 209, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8069m, 6, 150, null, 1100 },
                    { 210, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9025m, 6, 150, null, 1200 },
                    { 211, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10094m, 6, 150, null, 1300 },
                    { 212, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11662m, 6, 150, null, 1390 },
                    { 213, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6317m, 6, 200, null, 600 },
                    { 214, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6739m, 6, 200, null, 700 },
                    { 215, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7146m, 6, 200, null, 800 },
                    { 216, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7576m, 6, 200, null, 900 },
                    { 217, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8010m, 6, 200, null, 1000 },
                    { 218, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8512m, 6, 200, null, 1100 },
                    { 219, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9454m, 6, 200, null, 1200 },
                    { 220, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10606m, 6, 200, null, 1300 },
                    { 221, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12208m, 6, 200, null, 1390 },
                    { 222, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7112m, 6, 250, null, 700 },
                    { 223, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7524m, 6, 250, null, 800 },
                    { 224, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8003m, 6, 250, null, 900 },
                    { 225, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8517m, 6, 250, null, 1000 },
                    { 226, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9076m, 6, 250, null, 1100 },
                    { 227, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9947m, 6, 250, null, 1200 },
                    { 228, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11235m, 6, 250, null, 1300 },
                    { 229, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12798m, 6, 250, null, 1390 },
                    { 230, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7890m, 6, 300, null, 800 },
                    { 231, 10, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8382m, 6, 300, null, 900 },
                    { 232, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8936m, 6, 300, null, 1000 },
                    { 233, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9832m, 6, 300, null, 1100 },
                    { 234, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10466m, 6, 300, null, 1200 },
                    { 235, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11729m, 6, 300, null, 1300 },
                    { 236, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13335m, 6, 300, null, 1390 },
                    { 237, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9143m, 6, 350, null, 900 },
                    { 238, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9779m, 6, 350, null, 1000 },
                    { 239, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10623m, 6, 350, null, 1100 },
                    { 240, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11303m, 6, 350, null, 1200 },
                    { 241, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12866m, 6, 350, null, 1300 },
                    { 242, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13634m, 6, 350, null, 1390 },
                    { 243, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11070m, 6, 400, null, 1000 },
                    { 244, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11948m, 6, 400, null, 1100 },
                    { 245, 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12452m, 6, 400, null, 1200 },
                    { 246, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2304m, 7, 150, null, 250 },
                    { 247, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2488m, 7, 150, null, 300 },
                    { 248, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2740m, 7, 150, null, 350 },
                    { 249, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2945m, 7, 150, null, 400 },
                    { 250, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3139m, 7, 150, null, 450 },
                    { 251, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3355m, 7, 150, null, 500 },
                    { 252, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3570m, 7, 150, null, 550 },
                    { 253, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3796m, 7, 150, null, 600 },
                    { 254, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2431m, 7, 200, null, 250 },
                    { 255, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2635m, 7, 200, null, 300 },
                    { 256, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2854m, 7, 200, null, 350 },
                    { 257, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3082m, 7, 200, null, 400 },
                    { 258, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3273m, 7, 200, null, 450 },
                    { 259, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3494m, 7, 200, null, 500 },
                    { 260, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3717m, 7, 200, null, 550 },
                    { 261, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3949m, 7, 200, null, 600 },
                    { 262, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2765m, 7, 250, null, 300 },
                    { 263, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2988m, 7, 250, null, 350 },
                    { 264, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3228m, 7, 250, null, 400 },
                    { 265, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3437m, 7, 250, null, 450 },
                    { 266, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3653m, 7, 250, null, 500 },
                    { 267, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3874m, 7, 250, null, 550 },
                    { 268, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4114m, 7, 250, null, 600 },
                    { 269, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3130m, 7, 300, null, 350 },
                    { 270, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3379m, 7, 300, null, 400 },
                    { 271, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3571m, 7, 300, null, 450 },
                    { 272, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3792m, 7, 300, null, 500 },
                    { 273, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4016m, 7, 300, null, 550 },
                    { 274, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4259m, 7, 300, null, 600 },
                    { 275, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3581m, 7, 350, null, 400 },
                    { 276, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3791m, 7, 350, null, 450 },
                    { 277, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4017m, 7, 350, null, 500 },
                    { 278, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1928m, 8, 150, null, 250 },
                    { 279, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2082m, 8, 150, null, 300 },
                    { 280, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2294m, 8, 150, null, 350 },
                    { 281, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2467m, 8, 150, null, 400 },
                    { 282, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2629m, 8, 150, null, 450 },
                    { 283, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2810m, 8, 150, null, 500 },
                    { 284, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2037m, 8, 200, null, 250 },
                    { 285, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2208m, 8, 200, null, 300 },
                    { 286, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2387m, 8, 200, null, 350 },
                    { 287, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2581m, 8, 200, null, 400 },
                    { 288, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2742m, 8, 200, null, 450 },
                    { 289, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2925m, 8, 200, null, 500 },
                    { 290, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2314m, 8, 250, null, 300 },
                    { 291, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2502m, 8, 250, null, 350 },
                    { 292, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2704m, 8, 250, null, 400 },
                    { 293, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2880m, 8, 250, null, 450 },
                    { 294, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3057m, 8, 250, null, 500 },
                    { 295, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2621m, 8, 300, null, 350 },
                    { 296, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2830m, 8, 300, null, 400 },
                    { 297, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2989m, 8, 300, null, 450 },
                    { 298, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3174m, 8, 300, null, 500 },
                    { 299, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2923m, 9, 150, null, 250 },
                    { 300, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3156m, 9, 150, null, 300 },
                    { 301, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3475m, 9, 150, null, 350 },
                    { 302, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3742m, 9, 150, null, 400 },
                    { 303, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3987m, 9, 150, null, 450 },
                    { 304, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4259m, 9, 150, null, 500 },
                    { 305, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4562m, 9, 150, null, 550 },
                    { 306, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4864m, 9, 150, null, 600 },
                    { 307, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3037m, 9, 200, null, 250 },
                    { 308, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3347m, 9, 200, null, 300 },
                    { 309, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3625m, 9, 200, null, 350 },
                    { 310, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3914m, 9, 200, null, 400 },
                    { 311, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4158m, 9, 200, null, 450 },
                    { 312, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4438m, 9, 200, null, 500 },
                    { 313, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4748m, 9, 200, null, 550 },
                    { 314, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5059m, 9, 200, null, 600 },
                    { 315, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3433m, 9, 250, null, 300 },
                    { 316, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3859m, 9, 250, null, 350 },
                    { 317, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4125m, 9, 250, null, 400 },
                    { 318, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4369m, 9, 250, null, 450 },
                    { 319, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4637m, 9, 250, null, 500 },
                    { 320, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3980m, 9, 300, null, 350 },
                    { 321, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4290m, 9, 300, null, 400 },
                    { 322, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4536m, 9, 300, null, 450 },
                    { 323, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4813m, 9, 300, null, 500 },
                    { 324, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5143m, 9, 300, null, 550 },
                    { 325, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5472m, 9, 300, null, 600 },
                    { 326, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4485m, 9, 350, null, 400 },
                    { 327, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4737m, 9, 350, null, 450 },
                    { 328, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5032m, 9, 350, null, 500 },
                    { 329, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2683m, 10, 150, null, 250 },
                    { 330, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2828m, 10, 150, null, 300 },
                    { 331, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3014m, 10, 150, null, 350 },
                    { 332, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3164m, 10, 150, null, 400 },
                    { 333, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3361m, 10, 150, null, 450 },
                    { 334, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3556m, 10, 150, null, 500 },
                    { 335, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3795m, 10, 150, null, 550 },
                    { 336, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4138m, 10, 150, null, 600 },
                    { 337, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4474m, 10, 150, null, 650 },
                    { 338, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5530m, 10, 150, null, 700 },
                    { 339, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2845m, 10, 200, null, 250 },
                    { 340, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3003m, 10, 200, null, 300 },
                    { 341, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3199m, 10, 200, null, 350 },
                    { 342, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3360m, 10, 200, null, 400 },
                    { 343, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3563m, 10, 200, null, 450 },
                    { 344, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3766m, 10, 200, null, 500 },
                    { 345, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4040m, 10, 200, null, 550 },
                    { 346, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4370m, 10, 200, null, 600 },
                    { 347, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4778m, 10, 200, null, 650 },
                    { 348, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5791m, 10, 200, null, 700 },
                    { 349, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3177m, 10, 250, null, 300 },
                    { 350, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3376m, 10, 250, null, 350 },
                    { 351, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3536m, 10, 250, null, 400 },
                    { 352, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3763m, 10, 250, null, 450 },
                    { 353, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4004m, 10, 250, null, 500 },
                    { 354, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4242m, 10, 250, null, 550 },
                    { 355, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4582m, 10, 250, null, 600 },
                    { 356, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4999m, 10, 250, null, 650 },
                    { 357, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6073m, 10, 250, null, 700 },
                    { 358, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3492m, 10, 300, null, 350 },
                    { 359, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3713m, 10, 300, null, 400 },
                    { 360, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3937m, 10, 300, null, 450 },
                    { 361, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4228m, 10, 300, null, 500 },
                    { 362, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4568m, 10, 300, null, 550 },
                    { 363, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4891m, 10, 300, null, 600 },
                    { 364, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5295m, 10, 300, null, 650 },
                    { 365, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6320m, 10, 300, null, 700 },
                    { 366, 2, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4012m, 10, 350, null, 400 },
                    { 367, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4299m, 10, 350, null, 450 },
                    { 368, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4595m, 10, 350, null, 500 },
                    { 369, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4901m, 10, 350, null, 550 },
                    { 370, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5263m, 10, 350, null, 600 },
                    { 371, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6192m, 10, 350, null, 650 },
                    { 372, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6470m, 10, 350, null, 700 },
                    { 373, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4816m, 10, 400, null, 450 },
                    { 374, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5054m, 10, 400, null, 500 },
                    { 375, 3, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5435m, 10, 400, null, 550 },
                    { 376, 8, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6774m, 10, 400, null, 700 },
                    { 377, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5353m, 11, 150, null, 500 },
                    { 378, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5643m, 11, 150, null, 600 },
                    { 379, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6013m, 11, 150, null, 700 },
                    { 380, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6313m, 11, 150, null, 800 },
                    { 381, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6713m, 11, 150, null, 900 },
                    { 382, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7099m, 11, 150, null, 1000 },
                    { 383, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7580m, 11, 150, null, 1100 },
                    { 384, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8271m, 11, 150, null, 1200 },
                    { 385, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8936m, 11, 150, null, 1300 },
                    { 386, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 11051m, 11, 150, null, 1390 },
                    { 387, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5681m, 11, 200, null, 500 },
                    { 388, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5989m, 11, 200, null, 600 },
                    { 389, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6386m, 11, 200, null, 700 },
                    { 390, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6709m, 11, 200, null, 800 },
                    { 391, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7113m, 11, 200, null, 900 },
                    { 392, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7521m, 11, 200, null, 1000 },
                    { 393, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8072m, 11, 200, null, 1100 },
                    { 394, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8731m, 11, 200, null, 1200 },
                    { 395, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9548m, 11, 200, null, 1300 },
                    { 396, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 11575m, 11, 200, null, 1390 },
                    { 397, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6346m, 11, 250, null, 600 },
                    { 398, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6745m, 11, 250, null, 700 },
                    { 399, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7066m, 11, 250, null, 800 },
                    { 400, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7515m, 11, 250, null, 900 },
                    { 401, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7994m, 11, 250, null, 1000 },
                    { 402, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8477m, 11, 250, null, 1100 },
                    { 403, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9149m, 11, 250, null, 1200 },
                    { 404, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9989m, 11, 250, null, 1300 },
                    { 405, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12129m, 11, 250, null, 1390 },
                    { 406, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6971m, 11, 300, null, 700 },
                    { 407, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7415m, 11, 300, null, 800 },
                    { 408, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 7868m, 11, 300, null, 900 },
                    { 409, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8451m, 11, 300, null, 1000 },
                    { 410, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9131m, 11, 300, null, 1100 },
                    { 411, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9774m, 11, 300, null, 1200 },
                    { 412, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10580m, 11, 300, null, 1300 },
                    { 413, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12631m, 11, 300, null, 1390 },
                    { 414, 10, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8007m, 11, 350, null, 800 },
                    { 415, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8586m, 11, 350, null, 900 },
                    { 416, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9179m, 11, 350, null, 1000 },
                    { 417, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9792m, 11, 350, null, 1100 },
                    { 418, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10520m, 11, 350, null, 1200 },
                    { 419, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12373m, 11, 350, null, 1300 },
                    { 420, 18, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12930m, 11, 350, null, 1390 },
                    { 421, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9620m, 11, 400, null, 900 },
                    { 422, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10101m, 11, 400, null, 1000 },
                    { 423, 12, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10855m, 11, 400, null, 1100 },
                    { 424, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3247m, 12, 150, null, 250 },
                    { 425, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3492m, 12, 150, null, 300 },
                    { 426, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3832m, 12, 150, null, 350 },
                    { 427, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4125m, 12, 150, null, 400 },
                    { 428, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4451m, 12, 150, null, 450 },
                    { 429, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4738m, 12, 150, null, 500 },
                    { 430, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4994m, 12, 150, null, 550 },
                    { 431, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5245m, 12, 150, null, 600 },
                    { 432, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5901m, 12, 150, null, 650 },
                    { 433, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6560m, 12, 150, null, 700 },
                    { 434, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3411m, 12, 200, null, 250 },
                    { 435, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3696m, 12, 200, null, 300 },
                    { 436, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4029m, 12, 200, null, 350 },
                    { 437, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4308m, 12, 200, null, 400 },
                    { 438, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4635m, 12, 200, null, 450 },
                    { 439, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4876m, 12, 200, null, 500 },
                    { 440, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5176m, 12, 200, null, 550 },
                    { 441, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5470m, 12, 200, null, 600 },
                    { 442, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6117m, 12, 200, null, 650 },
                    { 443, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6810m, 12, 200, null, 700 },
                    { 444, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3934m, 12, 250, null, 300 },
                    { 445, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4252m, 12, 250, null, 350 },
                    { 446, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4583m, 12, 250, null, 400 },
                    { 447, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4929m, 12, 250, null, 450 },
                    { 448, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5166m, 12, 250, null, 500 },
                    { 449, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5484m, 12, 250, null, 550 },
                    { 450, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5745m, 12, 250, null, 600 },
                    { 451, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6376m, 12, 250, null, 650 },
                    { 452, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7118m, 12, 250, null, 700 },
                    { 453, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4552m, 12, 300, null, 350 },
                    { 454, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4877m, 12, 300, null, 400 },
                    { 455, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5148m, 12, 300, null, 450 },
                    { 456, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5469m, 12, 300, null, 500 },
                    { 457, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5778m, 12, 300, null, 550 },
                    { 458, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6031m, 12, 300, null, 600 },
                    { 459, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6647m, 12, 300, null, 650 },
                    { 460, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7543m, 12, 300, null, 700 },
                    { 461, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5405m, 12, 350, null, 400 },
                    { 462, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5731m, 12, 350, null, 450 },
                    { 463, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6060m, 12, 350, null, 500 },
                    { 464, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6360m, 12, 350, null, 550 },
                    { 465, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6647m, 12, 350, null, 600 },
                    { 466, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7647m, 12, 350, null, 650 },
                    { 467, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7953m, 12, 350, null, 700 },
                    { 468, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6309m, 12, 400, null, 450 },
                    { 469, 4, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6660m, 12, 400, null, 500 },
                    { 470, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6973m, 12, 400, null, 550 },
                    { 471, 6, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7269m, 12, 400, null, 600 },
                    { 472, 9, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8517m, 12, 400, null, 700 },
                    { 473, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6548m, 13, 150, null, 500 },
                    { 474, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7039m, 13, 150, null, 600 },
                    { 475, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7707m, 13, 150, null, 700 },
                    { 476, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8298m, 13, 150, null, 800 },
                    { 477, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8960m, 13, 150, null, 900 },
                    { 478, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9532m, 13, 150, null, 1000 },
                    { 479, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10050m, 13, 150, null, 1100 },
                    { 480, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10554m, 13, 150, null, 1200 },
                    { 481, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11862m, 13, 150, null, 1300 },
                    { 482, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13171m, 13, 150, null, 1390 },
                    { 483, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 6880m, 13, 200, null, 500 },
                    { 484, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7451m, 13, 200, null, 600 },
                    { 485, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8114m, 13, 200, null, 700 },
                    { 486, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8667m, 13, 200, null, 800 },
                    { 487, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9318m, 13, 200, null, 900 },
                    { 488, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9806m, 13, 200, null, 1000 },
                    { 489, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10408m, 13, 200, null, 1100 },
                    { 490, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10993m, 13, 200, null, 1200 },
                    { 491, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12285m, 13, 200, null, 1300 },
                    { 492, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13684m, 13, 200, null, 1390 },
                    { 493, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 7925m, 13, 250, null, 600 },
                    { 494, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 8553m, 13, 250, null, 700 },
                    { 495, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9213m, 13, 250, null, 800 },
                    { 496, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9915m, 13, 250, null, 900 },
                    { 497, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10385m, 13, 250, null, 1000 },
                    { 498, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11012m, 13, 250, null, 1100 },
                    { 499, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11546m, 13, 250, null, 1200 },
                    { 500, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12814m, 13, 250, null, 1300 },
                    { 501, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14290m, 13, 250, null, 1390 },
                    { 502, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9159m, 13, 300, null, 700 },
                    { 503, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 9803m, 13, 300, null, 800 },
                    { 504, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10351m, 13, 300, null, 900 },
                    { 505, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10987m, 13, 300, null, 1000 },
                    { 506, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11606m, 13, 300, null, 1100 },
                    { 507, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12114m, 13, 300, null, 1200 },
                    { 508, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13337m, 13, 300, null, 1300 },
                    { 509, 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15143m, 13, 300, null, 1390 },
                    { 510, 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 10856m, 13, 350, null, 800 },
                    { 511, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 11520m, 13, 350, null, 900 },
                    { 512, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12174m, 13, 350, null, 1000 },
                    { 513, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 12778m, 13, 350, null, 1100 },
                    { 514, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13348m, 13, 350, null, 1200 },
                    { 515, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15289m, 13, 350, null, 1300 },
                    { 516, 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 15958m, 13, 350, null, 1390 },
                    { 517, 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 13371m, 13, 400, null, 1000 },
                    { 518, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14005m, 13, 400, null, 1100 },
                    { 519, 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 14590m, 13, 400, null, 1200 },
                    { 520, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2177m, 14, 150, null, 250 },
                    { 521, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2304m, 14, 150, null, 300 },
                    { 522, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2461m, 14, 150, null, 350 },
                    { 523, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2606m, 14, 150, null, 400 },
                    { 524, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2757m, 14, 150, null, 450 },
                    { 525, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2894m, 14, 150, null, 500 },
                    { 526, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3074m, 14, 150, null, 550 },
                    { 527, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3217m, 14, 150, null, 600 },
                    { 528, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3439m, 14, 150, null, 650 },
                    { 529, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3976m, 14, 150, null, 700 },
                    { 530, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2309m, 14, 200, null, 250 },
                    { 531, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2457m, 14, 200, null, 300 },
                    { 532, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2620m, 14, 200, null, 350 },
                    { 533, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2788m, 14, 200, null, 400 },
                    { 534, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2923m, 14, 200, null, 450 },
                    { 535, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3095m, 14, 200, null, 500 },
                    { 536, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3288m, 14, 200, null, 550 },
                    { 537, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3449m, 14, 200, null, 600 },
                    { 538, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3679m, 14, 200, null, 650 },
                    { 539, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4254m, 14, 200, null, 700 },
                    { 540, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2905m, 14, 250, null, 300 },
                    { 541, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2758m, 14, 250, null, 350 },
                    { 542, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2934m, 14, 250, null, 400 },
                    { 543, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3107m, 14, 250, null, 450 },
                    { 544, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3328m, 14, 250, null, 500 },
                    { 545, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3502m, 14, 250, null, 550 },
                    { 546, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3736m, 14, 250, null, 600 },
                    { 547, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3922m, 14, 250, null, 650 },
                    { 548, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4551m, 14, 250, null, 700 },
                    { 549, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2905m, 14, 300, null, 350 },
                    { 550, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3099m, 14, 300, null, 400 },
                    { 551, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3288m, 14, 300, null, 450 },
                    { 552, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3523m, 14, 300, null, 500 },
                    { 553, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3769m, 14, 300, null, 550 },
                    { 554, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3957m, 14, 300, null, 600 },
                    { 555, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4165m, 14, 300, null, 650 },
                    { 556, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4833m, 14, 300, null, 700 },
                    { 557, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3262m, 14, 350, null, 400 },
                    { 558, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3522m, 14, 350, null, 450 },
                    { 559, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3732m, 14, 350, null, 500 },
                    { 560, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3992m, 14, 350, null, 550 },
                    { 561, 5, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4191m, 14, 350, null, 600 },
                    { 562, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4887m, 14, 350, null, 650 },
                    { 563, 8, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 5092m, 14, 350, null, 700 },
                    { 564, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1941m, 15, 150, null, 250 },
                    { 565, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2074m, 15, 150, null, 300 },
                    { 566, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2279m, 15, 150, null, 350 },
                    { 567, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2429m, 15, 150, null, 400 },
                    { 568, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2035m, 15, 200, null, 250 },
                    { 569, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2182m, 15, 200, null, 300 },
                    { 570, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2360m, 15, 200, null, 350 },
                    { 571, 2, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2529m, 15, 200, null, 400 }
                });

            migrationBuilder.InsertData(
                table: "ShadePlus",
                columns: new[] { "ShadePlusId", "CreatedBy", "DateCreated", "DateUpdated", "Description", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 41, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1424m, 6, null, 500 },
                    { 42, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1581m, 6, null, 600 },
                    { 43, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1726m, 6, null, 700 },
                    { 44, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1878m, 6, null, 800 },
                    { 45, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2027m, 6, null, 900 },
                    { 46, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2179m, 6, null, 1000 },
                    { 47, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2329m, 6, null, 1100 },
                    { 48, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2479m, 6, null, 1200 },
                    { 49, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2628m, 6, null, 1300 },
                    { 50, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2779m, 6, null, 1390 },
                    { 51, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3060m, 6, null, 500 },
                    { 52, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3183m, 6, null, 600 },
                    { 53, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3374m, 6, null, 700 },
                    { 54, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3546m, 6, null, 800 },
                    { 55, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3649m, 6, null, 900 },
                    { 56, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3787m, 6, null, 1000 },
                    { 57, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 3959m, 6, null, 1100 },
                    { 58, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4128m, 6, null, 1200 },
                    { 59, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4318m, 6, null, 1300 },
                    { 60, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with hard-wired motor", 4511m, 6, null, 1390 },
                    { 61, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3325m, 6, null, 500 },
                    { 62, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3449m, 6, null, 600 },
                    { 63, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3638m, 6, null, 700 },
                    { 64, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3809m, 6, null, 800 },
                    { 65, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 3913m, 6, null, 900 },
                    { 66, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4053m, 6, null, 1000 },
                    { 67, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4225m, 6, null, 1100 },
                    { 68, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4392m, 6, null, 1200 },
                    { 69, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4585m, 6, null, 1300 },
                    { 70, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with radio-controlled motor io (w/o transm.)", 4776m, 6, null, 1390 },
                    { 71, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 718m, 9, null, 250 },
                    { 72, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 797m, 9, null, 300 },
                    { 73, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 865m, 9, null, 350 },
                    { 74, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 944m, 9, null, 400 },
                    { 75, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1018m, 9, null, 450 },
                    { 76, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1095m, 9, null, 500 },
                    { 77, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1170m, 9, null, 550 },
                    { 78, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with gearbox", 1247m, 9, null, 600 },
                    { 79, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1538m, 9, null, 250 },
                    { 80, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1596m, 9, null, 300 },
                    { 81, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1693m, 9, null, 350 },
                    { 82, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1777m, 9, null, 400 },
                    { 83, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1829m, 9, null, 450 },
                    { 84, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1898m, 9, null, 500 },
                    { 85, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 1986m, 9, null, 550 },
                    { 86, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with hard-wired motor", 2069m, 9, null, 600 },
                    { 87, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1667m, 9, null, 250 },
                    { 88, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1729m, 9, null, 300 },
                    { 89, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1827m, 9, null, 350 },
                    { 90, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1911m, 9, null, 400 },
                    { 91, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 1960m, 9, null, 450 },
                    { 92, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2032m, 9, null, 500 },
                    { 93, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2119m, 9, null, 550 },
                    { 94, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 170 cm with radio-controlled motor io (w/o transm.)", 2204m, 9, null, 600 },
                    { 95, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 719m, 10, null, 250 },
                    { 96, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 798m, 10, null, 300 },
                    { 97, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 865m, 10, null, 350 },
                    { 98, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 945m, 10, null, 400 },
                    { 99, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1019m, 10, null, 450 },
                    { 100, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1096m, 10, null, 500 },
                    { 101, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1173m, 10, null, 550 },
                    { 102, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1248m, 10, null, 600 },
                    { 103, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1319m, 10, null, 650 },
                    { 104, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1396m, 10, null, 700 },
                    { 105, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1424m, 11, null, 500 },
                    { 106, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1581m, 11, null, 600 },
                    { 107, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1726m, 11, null, 700 },
                    { 108, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 1878m, 11, null, 800 },
                    { 109, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2027m, 11, null, 900 },
                    { 110, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2179m, 11, null, 1000 },
                    { 111, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2329m, 11, null, 1100 },
                    { 112, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2479m, 11, null, 1200 },
                    { 113, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2628m, 11, null, 1300 },
                    { 114, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "Surcharge for height 210 cm with gearbox", 2779m, 11, null, 1390 }
                });

            migrationBuilder.InsertData(
                table: "nonStandardRALColours",
                columns: new[] { "RALColourId", "CreatedBy", "DateCreated", "DateUpdated", "MultiplyBy", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 49, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 627m, 6, null, 500 },
                    { 50, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 651m, 6, null, 600 },
                    { 51, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 692m, 6, null, 700 },
                    { 52, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 730m, 6, null, 800 },
                    { 53, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 783m, 6, null, 900 },
                    { 54, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 880m, 6, null, 1000 },
                    { 55, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 939m, 6, null, 1100 },
                    { 56, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1009m, 6, null, 1200 },
                    { 57, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1066m, 6, null, 1300 },
                    { 58, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1274m, 6, null, 1390 },
                    { 59, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 7, null, 250 },
                    { 60, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 7, null, 300 },
                    { 61, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 7, null, 350 },
                    { 62, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 388m, 7, null, 400 },
                    { 63, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 406m, 7, null, 450 },
                    { 64, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 423m, 7, null, 500 },
                    { 65, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 439m, 7, null, 550 },
                    { 66, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 458m, 7, null, 600 },
                    { 67, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 8, null, 250 },
                    { 68, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 8, null, 300 },
                    { 69, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 8, null, 350 },
                    { 70, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 388m, 8, null, 400 },
                    { 71, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 406m, 8, null, 450 },
                    { 72, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 423m, 8, null, 500 },
                    { 73, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 9, null, 250 },
                    { 74, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 9, null, 300 },
                    { 75, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 9, null, 350 },
                    { 76, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 388m, 9, null, 400 },
                    { 77, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 406m, 9, null, 450 },
                    { 78, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 423m, 9, null, 500 },
                    { 79, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 439m, 9, null, 550 },
                    { 80, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 458m, 9, null, 600 },
                    { 81, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 316m, 10, null, 250 },
                    { 82, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 10, null, 300 },
                    { 83, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 10, null, 350 },
                    { 84, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 10, null, 400 },
                    { 85, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 396m, 10, null, 450 },
                    { 86, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 445m, 10, null, 500 },
                    { 87, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 477m, 10, null, 550 },
                    { 88, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 509m, 10, null, 600 },
                    { 89, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 539m, 10, null, 650 },
                    { 90, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 640m, 10, null, 700 },
                    { 91, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 627m, 11, null, 500 },
                    { 92, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 651m, 11, null, 600 },
                    { 93, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 692m, 11, null, 700 },
                    { 94, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 730m, 11, null, 800 },
                    { 95, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 783m, 11, null, 900 },
                    { 96, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 880m, 11, null, 1000 },
                    { 97, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 939m, 11, null, 1100 },
                    { 98, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1009m, 11, null, 1200 },
                    { 99, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1066m, 11, null, 1300 },
                    { 100, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1274m, 11, null, 1390 },
                    { 101, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 316m, 12, null, 250 },
                    { 102, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 12, null, 300 },
                    { 103, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 12, null, 350 },
                    { 104, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 12, null, 400 },
                    { 105, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 396m, 12, null, 450 },
                    { 106, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 445m, 12, null, 500 },
                    { 107, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 477m, 12, null, 550 },
                    { 108, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 509m, 12, null, 600 },
                    { 109, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 539m, 12, null, 650 },
                    { 110, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 640m, 12, null, 700 },
                    { 111, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 627m, 13, null, 500 },
                    { 112, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 651m, 13, null, 600 },
                    { 113, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 692m, 13, null, 700 },
                    { 114, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 730m, 13, null, 800 },
                    { 115, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 783m, 13, null, 900 },
                    { 116, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 880m, 13, null, 1000 },
                    { 117, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 939m, 13, null, 1100 },
                    { 118, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1009m, 13, null, 1200 },
                    { 119, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1066m, 13, null, 1300 },
                    { 120, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 1274m, 13, null, 1390 },
                    { 121, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 316m, 14, null, 250 },
                    { 122, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 329m, 14, null, 300 },
                    { 123, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 352m, 14, null, 350 },
                    { 124, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 371m, 14, null, 400 },
                    { 125, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 396m, 14, null, 450 },
                    { 126, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 445m, 14, null, 500 },
                    { 127, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 477m, 14, null, 550 },
                    { 128, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 509m, 14, null, 600 },
                    { 129, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 539m, 14, null, 650 },
                    { 130, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 640m, 14, null, 700 },
                    { 131, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 312m, 15, null, 250 },
                    { 132, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 335m, 15, null, 300 },
                    { 133, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 353m, 15, null, 350 },
                    { 134, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2.5m, 369m, 15, null, 400 }
                });

            migrationBuilder.InsertData(
                table: "valanceStyles",
                columns: new[] { "ValanceStyleId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 138m, 6, null, 500 },
                    { 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 159m, 6, null, 600 },
                    { 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 182m, 6, null, 700 },
                    { 14, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 209m, 6, null, 800 },
                    { 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 230m, 6, null, 900 },
                    { 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 254m, 6, null, 1000 },
                    { 17, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 275m, 6, null, 1100 },
                    { 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 302m, 6, null, 1200 },
                    { 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 321m, 6, null, 1300 },
                    { 20, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 347m, 6, null, 1390 },
                    { 21, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 79m, 8, null, 250 },
                    { 22, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 87m, 8, null, 300 },
                    { 23, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 96m, 8, null, 350 },
                    { 24, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 109m, 8, null, 400 },
                    { 25, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 122m, 8, null, 450 },
                    { 26, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 134m, 8, null, 500 },
                    { 27, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 79m, 10, null, 250 },
                    { 28, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 87m, 10, null, 300 },
                    { 29, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 96m, 10, null, 350 },
                    { 30, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 109m, 10, null, 400 },
                    { 31, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 122m, 10, null, 450 },
                    { 32, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 134m, 10, null, 500 },
                    { 33, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 145m, 10, null, 550 },
                    { 34, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 158m, 10, null, 600 },
                    { 35, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 166m, 10, null, 650 },
                    { 36, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 10, null, 700 },
                    { 37, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 138m, 11, null, 500 },
                    { 38, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 159m, 11, null, 600 },
                    { 39, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 182m, 11, null, 700 },
                    { 40, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 209m, 11, null, 800 },
                    { 41, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 230m, 11, null, 900 },
                    { 42, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 254m, 11, null, 1000 },
                    { 43, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 275m, 11, null, 1100 },
                    { 44, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 302m, 11, null, 1200 },
                    { 45, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 321m, 11, null, 1300 },
                    { 46, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 347m, 11, null, 1390 },
                    { 47, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 79m, 12, null, 250 },
                    { 48, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 87m, 12, null, 300 },
                    { 49, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 96m, 12, null, 350 },
                    { 50, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 109m, 12, null, 400 },
                    { 51, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 122m, 12, null, 450 },
                    { 52, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 134m, 12, null, 500 },
                    { 53, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 145m, 12, null, 550 },
                    { 54, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 158m, 12, null, 600 },
                    { 55, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 166m, 12, null, 650 },
                    { 56, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 12, null, 700 },
                    { 57, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 138m, 13, null, 500 },
                    { 58, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 159m, 13, null, 600 },
                    { 59, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 182m, 13, null, 700 },
                    { 60, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 209m, 13, null, 800 },
                    { 61, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 230m, 13, null, 900 },
                    { 62, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 254m, 13, null, 1000 },
                    { 63, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 275m, 13, null, 1100 },
                    { 64, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 302m, 13, null, 1200 },
                    { 65, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 321m, 13, null, 1300 },
                    { 66, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 347m, 13, null, 1390 }
                });

            migrationBuilder.InsertData(
                table: "wallSealingProfiles",
                columns: new[] { "WallSealingProfileId", "CreatedBy", "DateCreated", "DateUpdated", "Price", "ProductId", "UpdatedBy", "WidthCm" },
                values: new object[,]
                {
                    { 11, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 170m, 6, null, 500 },
                    { 12, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 197m, 6, null, 600 },
                    { 13, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 223m, 6, null, 700 },
                    { 14, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 253m, 6, null, 800 },
                    { 15, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 281m, 6, null, 900 },
                    { 16, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 313m, 6, null, 1000 },
                    { 17, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 347m, 6, null, 1100 },
                    { 18, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 378m, 6, null, 1200 },
                    { 19, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 406m, 6, null, 1300 },
                    { 20, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 435m, 6, null, 1390 },
                    { 21, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 90m, 7, null, 250 },
                    { 22, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 104m, 7, null, 300 },
                    { 23, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 114m, 7, null, 350 },
                    { 24, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 130m, 7, null, 400 },
                    { 25, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 146m, 7, null, 450 },
                    { 26, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 162m, 7, null, 500 },
                    { 27, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 7, null, 550 },
                    { 28, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 193m, 7, null, 600 },
                    { 29, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 90m, 8, null, 250 },
                    { 30, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 104m, 8, null, 300 },
                    { 31, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 114m, 8, null, 350 },
                    { 32, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 130m, 8, null, 400 },
                    { 33, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 146m, 8, null, 450 },
                    { 34, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 162m, 8, null, 500 },
                    { 35, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 90m, 10, null, 250 },
                    { 36, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 104m, 10, null, 300 },
                    { 37, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 114m, 10, null, 350 },
                    { 38, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 130m, 10, null, 400 },
                    { 39, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 146m, 10, null, 450 },
                    { 40, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 162m, 10, null, 500 },
                    { 41, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 10, null, 550 },
                    { 42, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 193m, 10, null, 600 },
                    { 43, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 207m, 10, null, 650 },
                    { 44, "System", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 223m, 10, null, 700 },
                    { 45, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 170m, 11, null, 500 },
                    { 46, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 197m, 11, null, 600 },
                    { 47, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 223m, 11, null, 700 },
                    { 48, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 253m, 11, null, 800 },
                    { 49, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 281m, 11, null, 900 },
                    { 50, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 313m, 11, null, 1000 },
                    { 51, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 347m, 11, null, 1100 },
                    { 52, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 378m, 11, null, 1200 },
                    { 53, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 406m, 11, null, 1300 },
                    { 54, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 435m, 11, null, 1390 },
                    { 55, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 65m, 12, null, 250 },
                    { 56, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 74m, 12, null, 300 },
                    { 57, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 79m, 12, null, 350 },
                    { 58, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 86m, 12, null, 400 },
                    { 59, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 93m, 12, null, 450 },
                    { 60, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 103m, 12, null, 500 },
                    { 61, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 109m, 12, null, 550 },
                    { 62, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 116m, 12, null, 600 },
                    { 63, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 123m, 12, null, 650 },
                    { 64, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 134m, 12, null, 700 },
                    { 65, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 121m, 13, null, 500 },
                    { 66, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 134m, 13, null, 600 },
                    { 67, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 144m, 13, null, 700 },
                    { 68, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 158m, 13, null, 800 },
                    { 69, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 173m, 13, null, 900 },
                    { 70, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 193m, 13, null, 1000 },
                    { 71, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 206m, 13, null, 1100 },
                    { 72, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 225m, 13, null, 1200 },
                    { 73, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 236m, 13, null, 1300 },
                    { 74, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 258m, 13, null, 1390 },
                    { 75, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 90m, 14, null, 250 },
                    { 76, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 104m, 14, null, 300 },
                    { 77, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 114m, 14, null, 350 },
                    { 78, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 130m, 14, null, 400 },
                    { 79, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 146m, 14, null, 450 },
                    { 80, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 162m, 14, null, 500 },
                    { 81, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 177m, 14, null, 550 },
                    { 82, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 193m, 14, null, 600 },
                    { 83, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 207m, 14, null, 650 },
                    { 84, "System", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 223m, 14, null, 700 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Brackets",
                keyColumn: "BracketId",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LightingCassettes",
                keyColumn: "LightingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Motors",
                keyColumn: "MotorId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 383);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 384);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 385);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 386);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 387);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 388);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 389);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 390);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 391);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 392);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 393);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 394);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 395);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 396);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 397);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 398);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 399);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 408);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 409);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 410);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 412);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 413);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 414);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 415);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 416);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 417);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 418);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 419);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 420);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 424);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 425);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 426);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 427);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 428);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 429);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 430);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 431);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 432);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 433);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 434);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 435);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 436);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 437);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 438);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 439);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 440);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 441);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 442);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 443);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 444);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 445);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 446);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 447);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 448);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 449);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 450);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 451);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 452);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 453);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 454);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 455);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 456);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 457);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 458);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 459);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 460);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 461);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 462);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 463);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 464);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 465);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 466);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 467);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 468);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 469);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 470);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 471);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 472);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 473);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 474);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 475);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 476);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 477);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 478);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 479);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 480);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 481);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 482);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 483);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 484);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 485);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 486);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 487);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 488);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 489);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 490);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 491);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 492);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 493);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 494);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 495);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 496);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 497);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 498);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 499);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 538);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 539);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 547);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 548);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 549);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 550);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 551);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 552);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 553);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 554);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 555);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 556);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 557);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 558);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 559);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 560);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 561);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 562);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 563);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 564);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 565);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 566);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 567);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 568);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 569);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 570);

            migrationBuilder.DeleteData(
                table: "Projections",
                keyColumn: "ProjectionId",
                keyValue: 571);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "ShadePlus",
                keyColumn: "ShadePlusId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "nonStandardRALColours",
                keyColumn: "RALColourId",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "valanceStyles",
                keyColumn: "ValanceStyleId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "wallSealingProfiles",
                keyColumn: "WallSealingProfileId",
                keyValue: 84);

            migrationBuilder.DropColumn(
                name: "ArmTypeId",
                table: "Motors");
        }
    }
}
