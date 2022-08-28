using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialSiteSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertForSellerForSeenProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2603));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2613));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2623));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2474));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2496));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2663));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2684));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2694));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2706));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2716));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2737));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2748));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2759));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2769));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2780));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2790));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2801));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2819));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2841));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2861));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2882));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2903));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2913));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2923));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2936));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2958));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2971));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2981));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2995));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(3007));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6392));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6406));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6427));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6437));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6325));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6353));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6365));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6507));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6519));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6532));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6543));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6586));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6597));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6618));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6639));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6681));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6692));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6713));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6734));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6749));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6761));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6804));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6815));
        }
    }
}
