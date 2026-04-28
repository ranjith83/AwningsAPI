using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class BracketProjectionMIssing : Migration
    {
        private const string CreatedDate = "2026-04-18 12:00:00";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Fix Projection_cm for Product 5 (6000 Single) ────────────────────
            // All blocks 2-6 were incorrectly set to 150; correct values are 200-400.

            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 200 WHERE ProjectionId IN (169,170,171,172,173,174,175,176,177)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 250 WHERE ProjectionId IN (178,179,180,181,182,183,184,185)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 300 WHERE ProjectionId IN (186,187,188,189,190,191,192)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 350 WHERE ProjectionId IN (193,194,195,196,197,198)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 400 WHERE ProjectionId IN (199,200,201,202)");

            // ── Fix junction-roller bracket ArmTypeId (should be null = universal) ─
            // Product 3 (MX-4 Coupler): brackets 39, 40
            // migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = NULL WHERE BracketId IN (39, 40)");
            // Product 6 (6000 Coupler): brackets 119, 120
            migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = NULL WHERE BracketId IN (119, 120)");
            // Product 11 (5010 Coupler): brackets 250, 251
            migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = NULL WHERE BracketId IN (250, 251)");

            // ── New surcharge brackets for Product 8 (990) ───────────────────────
            migrationBuilder.Sql($@"
SET IDENTITY_INSERT Brackets ON;
INSERT INTO Brackets (BracketId, ProductId, BracketName, PartNumber, Price, ArmTypeId, DateCreated, CreatedBy)
VALUES
(373, 8, 'Face fixture', NULL, 88.00, NULL, '{CreatedDate}', 'System'),
(374, 8, 'Face fixture incl. spreader A', NULL, 344.00, NULL, '{CreatedDate}', 'System'),
(375, 8, 'Face fixture incl. spreader B', NULL, 418.00, NULL, '{CreatedDate}', 'System'),
(376, 8, 'Top fixture', NULL, 88.00, NULL, '{CreatedDate}', 'System'),
(377, 8, 'Eaves fixture', NULL, 205.00, NULL, '{CreatedDate}', 'System'),
(378, 8, 'Arms with bionic tendon', NULL, 121.00, NULL, '{CreatedDate}', 'System');
SET IDENTITY_INSERT Brackets OFF;");

            // ── New surcharge brackets for Product 9 (970) ───────────────────────
            migrationBuilder.Sql($@"
SET IDENTITY_INSERT Brackets ON;
INSERT INTO Brackets (BracketId, ProductId, BracketName, PartNumber, Price, ArmTypeId, DateCreated, CreatedBy)
VALUES
(379, 9, 'Face fixture', NULL, 220.00, NULL, '{CreatedDate}', 'System'),
(380, 9, 'Face fixture incl. spreader A', NULL, 592.00, NULL, '{CreatedDate}', 'System'),
(381, 9, 'Face fixture incl. spreader B', NULL, 550.00, NULL, '{CreatedDate}', 'System'),
(382, 9, 'Face fixture incl. spreader C', NULL, 592.00, NULL, '{CreatedDate}', 'System'),
(383, 9, 'Top fixture', NULL, 278.00, NULL, '{CreatedDate}', 'System'),
(384, 9, 'Eaves fixture', NULL, 371.00, NULL, '{CreatedDate}', 'System');
SET IDENTITY_INSERT Brackets OFF;");

            // ── New cover plate bracket for Product 12 (3300 Single) ─────────────
            migrationBuilder.Sql($@"
SET IDENTITY_INSERT Brackets ON;
INSERT INTO Brackets (BracketId, ProductId, BracketName, PartNumber, Price, ArmTypeId, DateCreated, CreatedBy)
VALUES
(385, 12, 'Cover plate 210x230x2 mm for face fixture bracket 60 mm', '71844', 15.80, 1, '{CreatedDate}', 'System');
SET IDENTITY_INSERT Brackets OFF;");

            // ── New LightingCassettes for Product 9 (970) ────────────────────────
            migrationBuilder.Sql($@"
SET IDENTITY_INSERT LightingCassettes ON;
INSERT INTO LightingCassettes (LightingId, ProductId, Description, Price, DateCreated, CreatedBy)
VALUES
(5, 9, 'Surcharge for LED Line RGB-WW Radio-controlled io - dimmable (without remote control)', 1555.00, '{CreatedDate}', 'System'),
(6, 9, 'Surcharge for LED Line RGB-WW Zigbee radio control - dimmable (without transmitter)', 1386.00, '{CreatedDate}', 'System');
SET IDENTITY_INSERT LightingCassettes OFF;");

            // ── New Controls for Product 2 MX-4 Single & Product 14─────────────────────────
            migrationBuilder.Sql($@"
            SET IDENTITY_INSERT Controls ON;
                   INSERT INTO Controls (ControlId, Description, PartNumber, Price, DateCreated, CreatedBy, ProductId)
        VALUES 
        (1, 'markilux io-5 designcontrol transmitter - 5 channel', '8272099', 154.00, '2026-04-07', 'System', 2),
        (2, 'Somfy TaHoma Switch', '8272377', 362.00, '2026-04-07', 'System', 2);
            SET IDENTITY_INSERT Controls OFF;");

            // ── New Controls for Product 5 (6000 Single) ─────────────────────────
            migrationBuilder.Sql($@"
          SET IDENTITY_INSERT Controls ON;
                   INSERT INTO Controls (ControlId, Description, PartNumber, Price, DateCreated, CreatedBy, ProductId)
        VALUES 
        (3, 'markilux io-1 designcontrol transmitter - 1 channel', '8272087', 118.00, '2026-04-08', 'System', 5),
        (4, 'markilux io-5 designcontrol transmitter - 5 channel', '8272099', 154.00, '2026-04-08', 'System', 5);
    SET IDENTITY_INSERT Controls OFF;");

            // ── New Controls for Product 15 (900) ────────────────────────────────
            migrationBuilder.Sql($@"
                SET IDENTITY_INSERT Controls ON;

                INSERT INTO Controls (ControlId, ProductId, PartNumber, Description, Price, DateCreated, CreatedBy)
                VALUES
                    (7,  15, '8272087', 'markilux io-1 designcontrol transmitter - 1 channel', 118.00, '{CreatedDate}', 'System'),
                    (8,  15, '8272366', 'Vibrabox Eolis 3D WireFree io - traffic grey, RAL 7043', 241.00, '{CreatedDate}', 'System'),
                    (9,  15, '8272367', 'Vibrabox Eolis 3D WireFree io - traffic white, RAL 9016', 241.00, '{CreatedDate}', 'System'),
                    (10, 15, '8272370', 'Soliris Smoove Uno Pure White Kit incl. wind / light sensor + frame', 420.00, '{CreatedDate}', 'System');

                SET IDENTITY_INSERT Controls OFF;
                ");

            migrationBuilder.Sql(@"
            UPDATE Projections SET Projection_cm = 200 WHERE ProjectionId IN (169,170,171,172,173,174,175,176,177);

            UPDATE Projections SET Projection_cm = 250 WHERE ProjectionId IN (178,179,180,181,182,183,184,185);

            UPDATE Projections SET Projection_cm = 300 WHERE ProjectionId IN (186,187,188,189,190,191,192);

            UPDATE Projections SET Projection_cm = 350 WHERE ProjectionId IN (193,194,195,196,197,198);

            UPDATE Projections SET Projection_cm = 400 WHERE ProjectionId IN (199,200,201,202);
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert Projection_cm for Product 5
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 150 WHERE ProjectionId IN (169,170,171,172,173,174,175,176,177)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 150 WHERE ProjectionId IN (178,179,180,181,182,183,184,185)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 150 WHERE ProjectionId IN (186,187,188,189,190,191,192)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 150 WHERE ProjectionId IN (193,194,195,196,197,198)");
            migrationBuilder.Sql("UPDATE Projections SET Projection_cm = 150 WHERE ProjectionId IN (199,200,201,202)");

            // Revert bracket ArmTypeId fixes
            migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = 1 WHERE BracketId IN (39, 40)");
            migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = 1 WHERE BracketId IN (119, 120)");
            migrationBuilder.Sql("UPDATE Brackets SET ArmTypeId = 1 WHERE BracketId IN (250, 251)");

            // Remove inserted brackets
            migrationBuilder.Sql("DELETE FROM Brackets WHERE BracketId IN (373,374,375,376,377,378,379,380,381,382,383,384,385)");

            // Remove inserted LightingCassettes
            migrationBuilder.Sql("DELETE FROM LightingCassettes WHERE LightingId IN (5, 6)");

            // Remove inserted Controls
            migrationBuilder.Sql("DELETE FROM Controls WHERE ControlId IN (5,6,7,8,9,10)");
        }
    }
}
