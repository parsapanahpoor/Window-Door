using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateSiteSetting2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SMSForDisActiveFrom15Day",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SMSForDisActiveFrom3Day",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SMSForDisActiveFromToday",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SMSForDisActiveFrom15Day",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "SMSForDisActiveFrom3Day",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "SMSForDisActiveFromToday",
                table: "SiteSettings");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1960));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1972));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1982));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2029));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2038));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1924));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1935));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(1944));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2057));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2070));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2080));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2090));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2099));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2110));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2138));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2157));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2166));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2176));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2213));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2224));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2233));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2242));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2252));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2270));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2320));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2358));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2376));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2386));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 11, 44, 4, 846, DateTimeKind.Local).AddTicks(2395));
        }
    }
}
