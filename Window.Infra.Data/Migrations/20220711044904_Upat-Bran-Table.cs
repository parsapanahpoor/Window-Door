using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpatBranTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_YaraghBrandId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_MainBrandId",
                table: "ProductMainBrandPrices");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9543));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9588));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9503));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9515));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 19, 4, 0, DateTimeKind.Local).AddTicks(9525));

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_YaraghBrandId",
                table: "ProductYaraghBrandPrices",
                column: "YaraghBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_MainBrandId",
                table: "ProductMainBrandPrices",
                column: "MainBrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_YaraghBrandId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_MainBrandId",
                table: "ProductMainBrandPrices");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(423));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(372));

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_YaraghBrandId",
                table: "ProductYaraghBrandPrices",
                column: "YaraghBrandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_MainBrandId",
                table: "ProductMainBrandPrices",
                column: "MainBrandId",
                unique: true);
        }
    }
}
