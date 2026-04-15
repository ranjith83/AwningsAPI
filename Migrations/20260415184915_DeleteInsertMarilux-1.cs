using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeleteInsertMarilux1 : Migration
    {

        // Migration: ReplaceProductsSeed
        // Paste the Up() and Down() methods into a new migration (create with: dotnet ef migrations add ReplaceProductsSeed)
        // Then run: dotnet ef database update

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Delete any existing seeded rows for these ProductIds to avoid duplicate-key errors
            migrationBuilder.Sql("DELETE FROM dbo.Products;");

            // 2) Insert canonical product seed rows
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Description", "ProductTypeId", "SupplierId", "DateCreated", "CreatedBy" },
                values: new object[,]
                {
            { 1,  "Markilux MX-1 compact",      1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 2,  "Markilux MX-4 Single",       1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 3,  "Markilux MX-4 Coupler",      1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 4,  "Markilux MX-2",              1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 5,  "Markilux 6000 Single",       1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 6,  "Markilux 6000 Coupler",      1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 7,  "Markilux MX-3",              1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 8,  "Markilux 990",               1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 9,  "Markilux 970",               1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 10, "Markilux 5010 Single",       1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },
            { 11, "Markilux 5010 Coupler",      1, 1, new DateTime(2026, 4, 15, 0, 0, 0), "System" },

            // explicit DateCreated values as provided
            { 12, "Markilux 3300 Single",       1, 1, new DateTime(2026, 4, 6, 0, 0, 0),  "System" },
            { 13, "Markilux 3300 Coupler",      1, 1, new DateTime(2026, 4, 7, 0, 0, 0),  "System" },
            { 14, "Markilux 1710",              1, 1, new DateTime(2026, 4, 8, 0, 0, 0),  "System" },
            { 15, "Markilux 900",               1, 1, new DateTime(2026, 4, 9, 0, 0, 0),  "System" }
                });

            // 0) Remove any existing ArmTypes rows with these IDs to avoid PK conflicts
            migrationBuilder.Sql("DELETE FROM dbo.ArmsTypes;");

            // 1) Insert ArmsTypes using raw SQL (preserve fractional seconds with datetime2)
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT dbo.ArmsTypes ON;

                INSERT INTO dbo.ArmsTypes (ArmTypeId, Description, DateCreated, CreatedBy) VALUES
                (1,  'All',      CONVERT(datetime2, '2026-04-15T11:02:03.7966667'), 'System'),
                (2,  '2-0-2',    CONVERT(datetime2, '2026-04-15T11:02:03.7966667'), 'System'),
                (3,  '2-0-3',    CONVERT(datetime2, '2026-04-15T11:37:04.5800000'), 'System'),
                (4,  '2-0-4',    CONVERT(datetime2, '2026-04-15T13:08:51.9933333'), 'System'),
                (5,  '2-1-3',    CONVERT(datetime2, '2026-04-15T11:02:03.7966667'), 'System'),
                (6,  '2-1-5',    CONVERT(datetime2, '2026-04-15T13:08:51.9933333'), 'System'),
                (7,  '3-1-5',    CONVERT(datetime2, '2026-04-15T13:10:49.3466667'), 'System'),
                (8,  '3-2-4',    CONVERT(datetime2, '2026-04-15T11:02:03.7966667'), 'System'),
                (9,  '3-2-6',    CONVERT(datetime2, '2026-04-15T13:08:51.9933333'), 'System'),
                (10, '4-0-4',    CONVERT(datetime2, '2026-04-15T11:21:28.5500000'), 'System'),
                (11, '4-0-5',    CONVERT(datetime2, '2026-04-15T13:09:47.6033333'), 'System'),
                (12, '4-0-6',    CONVERT(datetime2, '2026-04-15T11:52:53.5566667'), 'System'),
                (13, '4-0-7',    CONVERT(datetime2, '2026-04-15T13:09:47.6033333'), 'System'),
                (14, '4-0-8',    CONVERT(datetime2, '2026-04-15T13:11:21.4766667'), 'System'),
                (15, '4-2-6',    CONVERT(datetime2, '2026-04-15T11:21:28.5500000'), 'System'),
                (16, '4-2-9',    CONVERT(datetime2, '2026-04-15T13:09:47.6033333'), 'System'),
                (17, '6-2-10',   CONVERT(datetime2, '2026-04-15T13:11:37.5733333'), 'System'),
                (18, '6-4-8',    CONVERT(datetime2, '2026-04-15T11:21:28.5500000'), 'System'),
                (19, '6-4-11',   CONVERT(datetime2, '2026-04-15T13:09:47.6033333'), 'System');

                SET IDENTITY_INSERT dbo.ArmsTypes OFF;
                ");

            // 2) Remove any existing rows for ProductId = 1 to avoid PK / duplicate conflicts
            migrationBuilder.Sql("DELETE FROM dbo.Brackets;");
            migrationBuilder.Sql("DELETE FROM dbo.nonStandardRALColours;");
            migrationBuilder.Sql("DELETE FROM dbo.ShadePlus;");
            migrationBuilder.Sql("DELETE FROM dbo.Projections;");

            // 3) Insert Brackets (kept as InsertData since Brackets is likely mapped; adjust if not)
            migrationBuilder.InsertData(
                table: "Brackets",
                columns: new[] { "BracketId", "BracketName", "PartNumber", "Price", "ProductId", "DateCreated", "CreatedBy", "ArmTypeId" },
                values: new object[,]
                {
            { 1,  "Surcharge for bespoke arms",                                 null,     183.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 2 },
            { 2,  "Surcharge for bespoke arms",                                 null,     183.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 5 },
            { 3,  "Surcharge for bespoke arms",                                 null,     269.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 8 },

            { 4,  "Surcharge for face fixture A",                                null,      22.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 2 },
            { 5,  "Surcharge for face fixture A",                                null,      22.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 5 },
            { 6,  "Surcharge for face fixture A",                                null,      32.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 8 },

            { 7,  "Surcharge for face fixture incl. spreader plate B",           null,     330.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 2 },
            { 8,  "Surcharge for face fixture incl. spreader plate B",           null,     339.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 5 },
            { 9,  "Surcharge for face fixture incl. spreader plate B",           null,     504.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 8 },

            { 10, "Face fixture 280 x 180 mm",                                   "72611",   78.40m, 1, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 11, "Face fixture bracket A 300 x 180 mm",                         "72714",   89.00m, 1, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 12, "Spreader plate B 300 x 400 x 12 mm",                          "75327",  164.90m, 1, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 13, "Stand-off bkt. 80-300 mm for face fixture",                   "77970",  243.90m, 1, new DateTime(2026,4,15,12,0,0), "System", 1 }
                });

            // 4) Insert NonStandardRALColours
            migrationBuilder.InsertData(
                table: "nonStandardRALColours",
                columns: new[] { "RALColourId", "WidthCm", "Price", "DateCreated", "CreatedBy", "ProductId", "MultiplyBy" },
                values: new object[,]
                {
            { 1,  250, 316m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 2,  300, 329m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 3,  350, 352m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 4,  400, 371m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 5,  450, 396m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 6,  500, 445m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 7,  550, 477m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 8,  600, 509m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 9,  650, 539m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m },
            { 10, 700, 640m, new DateTime(2026,4,15,12,0,0), "System", 1, 2.5m }
                });

            // 5) Insert ShadePlus rows
            migrationBuilder.InsertData(
                table: "ShadePlus",
                columns: new[] { "ShadePlusId", "Description", "WidthCm", "Price", "DateCreated", "CreatedBy", "ProductId" },
                values: new object[,]
                {
            { 1,  "Surcharge for height 170 cm - radio-controlled motor", 250, 1592m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 2,  "Surcharge for height 170 cm - radio-controlled motor", 300, 1650m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 3,  "Surcharge for height 170 cm - radio-controlled motor", 350, 1727m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 4,  "Surcharge for height 170 cm - radio-controlled motor", 400, 1815m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 5,  "Surcharge for height 170 cm - radio-controlled motor", 450, 1875m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 6,  "Surcharge for height 170 cm - radio-controlled motor", 500, 1951m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 7,  "Surcharge for height 170 cm - radio-controlled motor", 550, 2044m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 8,  "Surcharge for height 170 cm - radio-controlled motor", 600, 2128m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 9,  "Surcharge for height 170 cm - radio-controlled motor", 650, 2206m, new DateTime(2026,4,15,12,0,0), "System", 1 },
            { 10, "Surcharge for height 170 cm - radio-controlled motor", 700, 2284m, new DateTime(2026,4,15,12,0,0), "System", 1 }
                });

            // 6) Insert Projections using server GETDATE()
            migrationBuilder.Sql(@"
                    INSERT INTO dbo.Projections (Width_cm, Projection_cm, Price, DateCreated, CreatedBy, ProductId, ArmTypeId) VALUES
                    (250,165,4639,GETDATE(),'System',1,2),
                    (300,165,4821,GETDATE(),'System',1,2),
                    (350,165,5050,GETDATE(),'System',1,2),
                    (400,165,5278,GETDATE(),'System',1,2),
                    (450,165,5525,GETDATE(),'System',1,5),
                    (500,165,6111,GETDATE(),'System',1,5),
                    (550,165,6414,GETDATE(),'System',1,5),
                    (600,165,6806,GETDATE(),'System',1,5),
                    (650,165,7430,GETDATE(),'System',1,5),
                    (700,165,8247,GETDATE(),'System',1,8),

                    (300,215,5037,GETDATE(),'System',1,2),
                    (350,215,5282,GETDATE(),'System',1,2),
                    (400,215,5524,GETDATE(),'System',1,2),
                    (450,215,5775,GETDATE(),'System',1,5),
                    (500,215,6379,GETDATE(),'System',1,5),
                    (550,215,6683,GETDATE(),'System',1,5),
                    (600,215,7058,GETDATE(),'System',1,5),
                    (650,215,7733,GETDATE(),'System',1,5),
                    (700,215,8551,GETDATE(),'System',1,8),

                    (350,265,5506,GETDATE(),'System',1,2),
                    (400,265,5747,GETDATE(),'System',1,2),
                    (450,265,6025,GETDATE(),'System',1,5),
                    (500,265,6686,GETDATE(),'System',1,5),
                    (550,265,7023,GETDATE(),'System',1,5),
                    (600,265,7346,GETDATE(),'System',1,5),
                    (650,265,8029,GETDATE(),'System',1,5),
                    (700,265,8883,GETDATE(),'System',1,8),

                    (400,315,5961,GETDATE(),'System',1,2),
                    (450,315,6249,GETDATE(),'System',1,5),
                    (500,315,6939,GETDATE(),'System',1,5),
                    (550,315,7278,GETDATE(),'System',1,5),
                    (600,315,7581,GETDATE(),'System',1,5),
                    (650,315,8318,GETDATE(),'System',1,5),
                    (700,315,9181,GETDATE(),'System',1,8),

                    (450,365,6872,GETDATE(),'System',1,5),
                    (500,365,7383,GETDATE(),'System',1,5),
                    (550,365,7744,GETDATE(),'System',1,5),
                    (600,365,8144,GETDATE(),'System',1,5),
                    (650,365,9007,GETDATE(),'System',1,8),
                    (700,365,9442,GETDATE(),'System',1,8),

                    (500,415,8009,GETDATE(),'System',1,5),
                    (550,415,8362,GETDATE(),'System',1,5),
                    (700,415,9712,GETDATE(),'System',1,8);
                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the seeded rows we inserted in Up()
            migrationBuilder.Sql("DELETE FROM dbo.Brackets;");
            migrationBuilder.Sql("DELETE FROM dbo.nonStandardRALColours;");
            migrationBuilder.Sql("DELETE FROM dbo.ShadePlus;");

            // Remove Projections we inserted for ProductId = 1
            migrationBuilder.Sql("DELETE FROM dbo.Projections;");

            // Remove ArmsTypes we inserted
            migrationBuilder.Sql("DELETE FROM dbo.ArmsTypes ;");

            migrationBuilder.Sql("DELETE FROM dbo.Products;");
        }


    }
}
