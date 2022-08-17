using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialScoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreForMarkets",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketPersonalInfoId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreForMarkets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreForMarkets_MarketPersonalInfo_MarketPersonalInfoId",
                        column: x => x.MarketPersonalInfoId,
                        principalTable: "MarketPersonalInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1177));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1189));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1201));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1082));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1115));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1129));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1142));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1253));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1267));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1287));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1338));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1375));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1388));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1400));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1413));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1425));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1449));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1463));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1475));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1487));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1499));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1511));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1537));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1549));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1561));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1586));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1599));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1617));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1630));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1644));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 12, 16, 35, 324, DateTimeKind.Local).AddTicks(1656));

            migrationBuilder.CreateIndex(
                name: "IX_ScoreForMarkets_MarketPersonalInfoId",
                table: "ScoreForMarkets",
                column: "MarketPersonalInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreForMarkets");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5192));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5204));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5214));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5223));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5232));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5152));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5163));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5173));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5251));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5263));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5274));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5283));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5303));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5312));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5331));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5404));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5414));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5433));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5443));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5452));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5464));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5492));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5511));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5520));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5549));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5558));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5577));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5587));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5596));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 43, 49, 984, DateTimeKind.Local).AddTicks(5606));
        }
    }
}
