using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddRelationBetweenProductMainBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MainBrandId",
                table: "Products",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5432));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5445));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5456));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5466));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5355));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5395));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 56, 17, 608, DateTimeKind.Local).AddTicks(5406));

            migrationBuilder.CreateIndex(
                name: "IX_Products_MainBrandId",
                table: "Products",
                column: "MainBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MainBrands_MainBrandId",
                table: "Products",
                column: "MainBrandId",
                principalTable: "MainBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MainBrands_MainBrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MainBrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainBrandId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(7985));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(8048));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(8058));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(8069));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(7912));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(7951));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 23, 17, 25, 36, 209, DateTimeKind.Local).AddTicks(7962));
        }
    }
}
