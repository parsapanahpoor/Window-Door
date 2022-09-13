using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class RemoeFieldProductTypeProductKind3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypeCategories_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6115));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6144));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6188));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(5984));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6033));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6057));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6080));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6272));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6295));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6317));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6337));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6397));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6418));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6439));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6464));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6484));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6507));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6528));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6550));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6571));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6593));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6622));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6648));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6670));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6692));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6712));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6754));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6817));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6839));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6881));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6903));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6925));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6947));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 28, 41, 21, DateTimeKind.Local).AddTicks(6969));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProductTypeCategoryId",
                table: "Products",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6508));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6519));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6530));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6540));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6446));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6578));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6589));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6599));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6610));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6623));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6634));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6645));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6662));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6674));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6685));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6696));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6706));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6729));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6751));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6763));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6805));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6816));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6826));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6836));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6846));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6856));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6867));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6877));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6888));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6898));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6908));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 22, 23, 26, 366, DateTimeKind.Local).AddTicks(6919));

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypeCategories_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId",
                principalTable: "ProductTypeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
