using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class LanatBarHame2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9519));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9915));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9972));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9993));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(54));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(94));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(113));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(133));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(171));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(190));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(282));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(348));

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle",
                column: "MarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2570));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2582));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2602));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2531));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2542));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2552));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2628));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2640));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2661));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2681));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2691));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2701));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2710));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2721));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2731));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2740));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2750));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2786));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2807));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2816));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2835));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2863));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2882));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2891));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2900));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2909));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2918));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2928));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2939));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 3, 14, 159, DateTimeKind.Local).AddTicks(2949));

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle",
                column: "MarketId",
                unique: true);
        }
    }
}
