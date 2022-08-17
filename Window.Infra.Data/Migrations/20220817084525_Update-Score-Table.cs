using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateScoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreForMarkets_MarketPersonalInfo_MarketPersonalInfoId",
                table: "ScoreForMarkets");

            migrationBuilder.RenameColumn(
                name: "MarketPersonalInfoId",
                table: "ScoreForMarkets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreForMarkets_MarketPersonalInfoId",
                table: "ScoreForMarkets",
                newName: "IX_ScoreForMarkets_UserId");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4772));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4781));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4722));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4731));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4825));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4942));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4952));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4961));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4991));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5066));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5076));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5096));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5105));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5124));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5133));

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreForMarkets_Users_UserId",
                table: "ScoreForMarkets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreForMarkets_Users_UserId",
                table: "ScoreForMarkets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ScoreForMarkets",
                newName: "MarketPersonalInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreForMarkets_UserId",
                table: "ScoreForMarkets",
                newName: "IX_ScoreForMarkets_MarketPersonalInfoId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreForMarkets_MarketPersonalInfo_MarketPersonalInfoId",
                table: "ScoreForMarkets",
                column: "MarketPersonalInfoId",
                principalTable: "MarketPersonalInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
