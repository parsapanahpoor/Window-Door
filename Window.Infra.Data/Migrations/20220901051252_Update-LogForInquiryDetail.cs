using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateLogForInquiryDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KatibeSize",
                table: "logInquiryForUserDetails",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3952));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3969));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3991));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4002));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3829));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(3904));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4038));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4049));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4060));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4071));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4083));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4093));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4121));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4133));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4144));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4154));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4165));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4176));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4187));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4197));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4208));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4219));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4230));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4240));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4251));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4272));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4293));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4304));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4314));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4335));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4346));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4356));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4366));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 1, 9, 42, 51, 447, DateTimeKind.Local).AddTicks(4377));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KatibeSize",
                table: "logInquiryForUserDetails");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2041));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2056));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2067));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2078));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2090));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(1973));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(1998));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2011));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2023));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2110));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2125));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2137));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2158));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2170));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2203));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2215));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2226));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2237));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2247));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2258));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2269));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2310));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2321));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2363));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2374));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2407));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2428));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 10, 37, 49, 434, DateTimeKind.Local).AddTicks(2471));
        }
    }
}
