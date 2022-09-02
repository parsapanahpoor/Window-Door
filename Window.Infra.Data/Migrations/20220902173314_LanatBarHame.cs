using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class LanatBarHame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks");

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
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks",
                column: "MarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2429));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2378));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2519));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2530));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2539));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2549));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2559));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2569));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2588));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2599));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2608));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2618));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2627));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2637));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2656));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2665));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2676));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2685));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2695));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2704));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2714));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2723));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2742));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2751));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2760));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2776));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2786));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2795));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2804));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2814));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 14, 2, 59, 816, DateTimeKind.Local).AddTicks(2823));

            migrationBuilder.CreateIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks",
                column: "MarketId",
                unique: true);
        }
    }
}
