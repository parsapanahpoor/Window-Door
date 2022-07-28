using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class updatesomerelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_ProductId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_SegmentId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_ProductId",
                table: "ProductMainBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_SegmentId",
                table: "ProductMainBrandPrices");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6750));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6760));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6769));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6778));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6697));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6707));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 11, 9, 31, 6, 761, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_ProductId",
                table: "ProductYaraghBrandPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_SegmentId",
                table: "ProductYaraghBrandPrices",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_ProductId",
                table: "ProductMainBrandPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_SegmentId",
                table: "ProductMainBrandPrices",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_ProductId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductYaraghBrandPrices_SegmentId",
                table: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_ProductId",
                table: "ProductMainBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductMainBrandPrices_SegmentId",
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
                name: "IX_ProductYaraghBrandPrices_ProductId",
                table: "ProductYaraghBrandPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_SegmentId",
                table: "ProductYaraghBrandPrices",
                column: "SegmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_ProductId",
                table: "ProductMainBrandPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_SegmentId",
                table: "ProductMainBrandPrices",
                column: "SegmentId",
                unique: true);
        }
    }
}
