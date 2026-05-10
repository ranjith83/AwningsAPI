using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AwningsAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFrameColourOptionTableAndReseed : Migration
    {
        // ── Colour master list (FrameColourOptionId 1-11) ───────────────────────
        //  1  Traffic White RAL 9016
        //  2  Metallic Aluminium RAL 9006
        //  3  Anthracite Metallic 5204
        //  4  Stone Grey Metallic 5215
        //  5  Off-White Textured Finish 5233
        //  6  Havana Brown Textured Finish 5229
        //  7  Grey Brown Similar to RAL 8019
        //  8  Grey Aluminium RAL 9007
        //  9  Matt Iron Mica DB 703
        // 10  Anthracite Grey Textured Finish RAL 7016
        // 11  Jet Black Textured Finish RAL 9005
        //
        // IsNonStandardRAL: true = price from NonStandardRALColours, false = included
        //
        // Group A – all 11 false (included):
        //   ProductId 4  MX-2 | 5  6000 Single | 6  6000 Coupler | 8  990
        //   ProductId 10 5010 Single | 11 5010 Coupler | 12 3300 Single | 13 3300 Coupler | 14 1710
        //
        // Group B – options 1-7 false, 8-11 true:
        //   ProductId 1 MX-1 compact | 2 MX-4 Single | 3 MX-4 Coupler | 7 MX-3 | 9 970
        //
        // Group C – options 1,3,4 false; 2,5-11 true:
        //   ProductId 15 Markilux 900

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Create the master colour table
            migrationBuilder.CreateTable(
                name: "FrameColourOptions",
                columns: table => new
                {
                    FrameColourOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description  = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    DateCreated  = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy    = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdated  = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy    = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameColourOptions", x => x.FrameColourOptionId);
                });

            // 2. Seed the 11 colours
            migrationBuilder.InsertData(
                table: "FrameColourOptions",
                columns: new[] { "FrameColourOptionId", "Description", "DisplayOrder", "DateCreated", "CreatedBy" },
                values: new object[,]
                {
                    {  1, "Traffic White RAL 9016",                  1, new DateTime(2026, 5, 10), "System" },
                    {  2, "Metallic Aluminium RAL 9006",              2, new DateTime(2026, 5, 10), "System" },
                    {  3, "Anthracite Metallic 5204",                 3, new DateTime(2026, 5, 10), "System" },
                    {  4, "Stone Grey Metallic 5215",                 4, new DateTime(2026, 5, 10), "System" },
                    {  5, "Off-White Textured Finish 5233",           5, new DateTime(2026, 5, 10), "System" },
                    {  6, "Havana Brown Textured Finish 5229",        6, new DateTime(2026, 5, 10), "System" },
                    {  7, "Grey Brown Similar to RAL 8019",           7, new DateTime(2026, 5, 10), "System" },
                    {  8, "Grey Aluminium RAL 9007",                  8, new DateTime(2026, 5, 10), "System" },
                    {  9, "Matt Iron Mica DB 703",                    9, new DateTime(2026, 5, 10), "System" },
                    { 10, "Anthracite Grey Textured Finish RAL 7016", 10, new DateTime(2026, 5, 10), "System" },
                    { 11, "Jet Black Textured Finish RAL 9005",       11, new DateTime(2026, 5, 10), "System" },
                });

            // 3. Remove old placeholder rows before schema changes
            migrationBuilder.DeleteData(
                table: "FrameColours",
                keyColumn: "FrameColourId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

            // 4. Drop obsolete columns
            migrationBuilder.DropColumn(name: "ColorValue",  table: "FrameColours");
            migrationBuilder.DropColumn(name: "DateUpdated", table: "FrameColours");
            migrationBuilder.DropColumn(name: "Description", table: "FrameColours");
            migrationBuilder.DropColumn(name: "Price",       table: "FrameColours");
            migrationBuilder.DropColumn(name: "UpdatedBy",   table: "FrameColours");

            // 5. Repurpose the remaining int column: SortOrder → FrameColourOptionId
            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "FrameColours",
                newName: "FrameColourOptionId");

            // 6. Add new columns
            migrationBuilder.AddColumn<bool>(
                name: "IsNonStandardRAL",
                table: "FrameColours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "FrameColours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 7. Index + FK
            migrationBuilder.CreateIndex(
                name: "IX_FrameColours_FrameColourOptionId",
                table: "FrameColours",
                column: "FrameColourOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FrameColours_FrameColourOptions_FrameColourOptionId",
                table: "FrameColours",
                column: "FrameColourOptionId",
                principalTable: "FrameColourOptions",
                principalColumn: "FrameColourOptionId",
                onDelete: ReferentialAction.Restrict);

            // 8. Insert 165 product-colour mapping rows
            // Columns: FrameColourId, ProductId, FrameColourOptionId, IsNonStandardRAL, DateCreated, CreatedBy
            migrationBuilder.InsertData(
                table: "FrameColours",
                columns: new[] { "FrameColourId", "ProductId", "FrameColourOptionId", "IsNonStandardRAL", "DateCreated", "CreatedBy" },
                values: new object[,]
                {
                    // ── Group A: ProductId 4 (Markilux MX-2) — all included ───────────────
                    {   1,  4,  1, false, new DateTime(2026, 5, 10), "System" },
                    {   2,  4,  2, false, new DateTime(2026, 5, 10), "System" },
                    {   3,  4,  3, false, new DateTime(2026, 5, 10), "System" },
                    {   4,  4,  4, false, new DateTime(2026, 5, 10), "System" },
                    {   5,  4,  5, false, new DateTime(2026, 5, 10), "System" },
                    {   6,  4,  6, false, new DateTime(2026, 5, 10), "System" },
                    {   7,  4,  7, false, new DateTime(2026, 5, 10), "System" },
                    {   8,  4,  8, false, new DateTime(2026, 5, 10), "System" },
                    {   9,  4,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  10,  4, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  11,  4, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 5 (Markilux 6000 Single) ───────────────────────
                    {  12,  5,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  13,  5,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  14,  5,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  15,  5,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  16,  5,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  17,  5,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  18,  5,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  19,  5,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  20,  5,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  21,  5, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  22,  5, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 6 (Markilux 6000 Coupler) ──────────────────────
                    {  23,  6,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  24,  6,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  25,  6,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  26,  6,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  27,  6,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  28,  6,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  29,  6,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  30,  6,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  31,  6,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  32,  6, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  33,  6, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 8 (Markilux 990) ──────────────────────────────
                    {  34,  8,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  35,  8,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  36,  8,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  37,  8,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  38,  8,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  39,  8,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  40,  8,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  41,  8,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  42,  8,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  43,  8, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  44,  8, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 10 (Markilux 5010 Single) ──────────────────────
                    {  45, 10,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  46, 10,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  47, 10,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  48, 10,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  49, 10,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  50, 10,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  51, 10,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  52, 10,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  53, 10,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  54, 10, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  55, 10, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 11 (Markilux 5010 Coupler) ─────────────────────
                    {  56, 11,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  57, 11,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  58, 11,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  59, 11,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  60, 11,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  61, 11,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  62, 11,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  63, 11,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  64, 11,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  65, 11, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  66, 11, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 12 (Markilux 3300 Single) ──────────────────────
                    {  67, 12,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  68, 12,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  69, 12,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  70, 12,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  71, 12,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  72, 12,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  73, 12,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  74, 12,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  75, 12,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  76, 12, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  77, 12, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 13 (Markilux 3300 Coupler) ─────────────────────
                    {  78, 13,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  79, 13,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  80, 13,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  81, 13,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  82, 13,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  83, 13,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  84, 13,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  85, 13,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  86, 13,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  87, 13, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  88, 13, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group A: ProductId 14 (Markilux 1710) ────────────────────────────
                    {  89, 14,  1, false, new DateTime(2026, 5, 10), "System" },
                    {  90, 14,  2, false, new DateTime(2026, 5, 10), "System" },
                    {  91, 14,  3, false, new DateTime(2026, 5, 10), "System" },
                    {  92, 14,  4, false, new DateTime(2026, 5, 10), "System" },
                    {  93, 14,  5, false, new DateTime(2026, 5, 10), "System" },
                    {  94, 14,  6, false, new DateTime(2026, 5, 10), "System" },
                    {  95, 14,  7, false, new DateTime(2026, 5, 10), "System" },
                    {  96, 14,  8, false, new DateTime(2026, 5, 10), "System" },
                    {  97, 14,  9, false, new DateTime(2026, 5, 10), "System" },
                    {  98, 14, 10, false, new DateTime(2026, 5, 10), "System" },
                    {  99, 14, 11, false, new DateTime(2026, 5, 10), "System" },

                    // ── Group B: ProductId 1 (Markilux MX-1 compact) ──────────────────────
                    // Options 1-7 included, 8-11 IsNonStandardRAL=true
                    { 100,  1,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 101,  1,  2, false, new DateTime(2026, 5, 10), "System" },
                    { 102,  1,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 103,  1,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 104,  1,  5, false, new DateTime(2026, 5, 10), "System" },
                    { 105,  1,  6, false, new DateTime(2026, 5, 10), "System" },
                    { 106,  1,  7, false, new DateTime(2026, 5, 10), "System" },
                    { 107,  1,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 108,  1,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 109,  1, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 110,  1, 11, true,  new DateTime(2026, 5, 10), "System" },

                    // ── Group B: ProductId 2 (Markilux MX-4 Single) ───────────────────────
                    { 111,  2,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 112,  2,  2, false, new DateTime(2026, 5, 10), "System" },
                    { 113,  2,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 114,  2,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 115,  2,  5, false, new DateTime(2026, 5, 10), "System" },
                    { 116,  2,  6, false, new DateTime(2026, 5, 10), "System" },
                    { 117,  2,  7, false, new DateTime(2026, 5, 10), "System" },
                    { 118,  2,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 119,  2,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 120,  2, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 121,  2, 11, true,  new DateTime(2026, 5, 10), "System" },

                    // ── Group B: ProductId 3 (Markilux MX-4 Coupler) ──────────────────────
                    { 122,  3,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 123,  3,  2, false, new DateTime(2026, 5, 10), "System" },
                    { 124,  3,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 125,  3,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 126,  3,  5, false, new DateTime(2026, 5, 10), "System" },
                    { 127,  3,  6, false, new DateTime(2026, 5, 10), "System" },
                    { 128,  3,  7, false, new DateTime(2026, 5, 10), "System" },
                    { 129,  3,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 130,  3,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 131,  3, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 132,  3, 11, true,  new DateTime(2026, 5, 10), "System" },

                    // ── Group B: ProductId 7 (Markilux MX-3) ─────────────────────────────
                    { 133,  7,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 134,  7,  2, false, new DateTime(2026, 5, 10), "System" },
                    { 135,  7,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 136,  7,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 137,  7,  5, false, new DateTime(2026, 5, 10), "System" },
                    { 138,  7,  6, false, new DateTime(2026, 5, 10), "System" },
                    { 139,  7,  7, false, new DateTime(2026, 5, 10), "System" },
                    { 140,  7,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 141,  7,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 142,  7, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 143,  7, 11, true,  new DateTime(2026, 5, 10), "System" },

                    // ── Group B: ProductId 9 (Markilux 970) ──────────────────────────────
                    { 144,  9,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 145,  9,  2, false, new DateTime(2026, 5, 10), "System" },
                    { 146,  9,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 147,  9,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 148,  9,  5, false, new DateTime(2026, 5, 10), "System" },
                    { 149,  9,  6, false, new DateTime(2026, 5, 10), "System" },
                    { 150,  9,  7, false, new DateTime(2026, 5, 10), "System" },
                    { 151,  9,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 152,  9,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 153,  9, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 154,  9, 11, true,  new DateTime(2026, 5, 10), "System" },

                    // ── Group C: ProductId 15 (Markilux 900) ──────────────────────────────
                    // Options 1,3,4 included; 2,5-11 IsNonStandardRAL=true
                    { 155, 15,  1, false, new DateTime(2026, 5, 10), "System" },
                    { 156, 15,  2, true,  new DateTime(2026, 5, 10), "System" },
                    { 157, 15,  3, false, new DateTime(2026, 5, 10), "System" },
                    { 158, 15,  4, false, new DateTime(2026, 5, 10), "System" },
                    { 159, 15,  5, true,  new DateTime(2026, 5, 10), "System" },
                    { 160, 15,  6, true,  new DateTime(2026, 5, 10), "System" },
                    { 161, 15,  7, true,  new DateTime(2026, 5, 10), "System" },
                    { 162, 15,  8, true,  new DateTime(2026, 5, 10), "System" },
                    { 163, 15,  9, true,  new DateTime(2026, 5, 10), "System" },
                    { 164, 15, 10, true,  new DateTime(2026, 5, 10), "System" },
                    { 165, 15, 11, true,  new DateTime(2026, 5, 10), "System" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Remove all 165 mapping rows
            migrationBuilder.DeleteData(
                table: "FrameColours",
                keyColumn: "FrameColourId",
                keyValues: new object[]
                {
                    1,   2,   3,   4,   5,   6,   7,   8,   9,  10,  11,
                    12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,
                    23,  24,  25,  26,  27,  28,  29,  30,  31,  32,  33,
                    34,  35,  36,  37,  38,  39,  40,  41,  42,  43,  44,
                    45,  46,  47,  48,  49,  50,  51,  52,  53,  54,  55,
                    56,  57,  58,  59,  60,  61,  62,  63,  64,  65,  66,
                    67,  68,  69,  70,  71,  72,  73,  74,  75,  76,  77,
                    78,  79,  80,  81,  82,  83,  84,  85,  86,  87,  88,
                    89,  90,  91,  92,  93,  94,  95,  96,  97,  98,  99,
                    100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
                    111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121,
                    122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132,
                    133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
                    144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154,
                    155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165
                });

            // 2. Remove FK and index
            migrationBuilder.DropForeignKey(name: "FK_FrameColours_FrameColourOptions_FrameColourOptionId", table: "FrameColours");
            migrationBuilder.DropIndex(name: "IX_FrameColours_FrameColourOptionId", table: "FrameColours");

            // 3. Drop new columns
            migrationBuilder.DropColumn(name: "IsNonStandardRAL", table: "FrameColours");
            migrationBuilder.DropColumn(name: "ProductId",         table: "FrameColours");

            // 4. Rename back
            migrationBuilder.RenameColumn(name: "FrameColourOptionId", table: "FrameColours", newName: "SortOrder");

            // 5. Restore old columns
            migrationBuilder.AddColumn<int>(name: "ColorValue", table: "FrameColours", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<DateTime>(name: "DateUpdated", table: "FrameColours", type: "datetime2", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Description", table: "FrameColours", type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "");
            migrationBuilder.AddColumn<decimal>(name: "Price", table: "FrameColours", type: "decimal(18,4)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<string>(name: "UpdatedBy", table: "FrameColours", type: "nvarchar(max)", nullable: true);

            // 6. Re-seed original 11 placeholder rows
            migrationBuilder.InsertData(
                table: "FrameColours",
                columns: new[] { "FrameColourId", "Description", "ColorValue", "Price", "SortOrder", "DateCreated", "CreatedBy" },
                values: new object[,]
                {
                    {  1, "Anthracite",    1, 0m,    1, new DateTime(2026, 5, 8), "System" },
                    {  2, "Jet Black",     1, 0m,    2, new DateTime(2026, 5, 8), "System" },
                    {  3, "White",         0, 100m,  3, new DateTime(2026, 5, 8), "System" },
                    {  4, "Traffic White", 0, 100m,  4, new DateTime(2026, 5, 8), "System" },
                    {  5, "Cream White",   0, 100m,  5, new DateTime(2026, 5, 8), "System" },
                    {  6, "Silver Grey",   0, 100m,  6, new DateTime(2026, 5, 8), "System" },
                    {  7, "Light Grey",    0, 100m,  7, new DateTime(2026, 5, 8), "System" },
                    {  8, "Bronze Brown",  0, 100m,  8, new DateTime(2026, 5, 8), "System" },
                    {  9, "Beige",         0, 100m,  9, new DateTime(2026, 5, 8), "System" },
                    { 10, "Moss Green",    0, 100m, 10, new DateTime(2026, 5, 8), "System" },
                    { 11, "Custom RAL",    0, 150m, 11, new DateTime(2026, 5, 8), "System" },
                });

            // 7. Drop the new master colour table
            migrationBuilder.DropTable(name: "FrameColourOptions");
        }
    }
}
