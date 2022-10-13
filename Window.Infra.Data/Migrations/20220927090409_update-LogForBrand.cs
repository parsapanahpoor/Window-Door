using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class updateLogForBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CityId",
                table: "LogForBrands",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CountryId",
                table: "LogForBrands",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StateId",
                table: "LogForBrands",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8513));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8527));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8547));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8557));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8472));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8495));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "LogForBrands");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "LogForBrands");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "LogForBrands");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1189));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1202));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1233));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1152));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1162));
        }
    }
}
