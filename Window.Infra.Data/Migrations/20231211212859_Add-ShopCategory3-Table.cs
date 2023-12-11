using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddShopCategory3Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCategories_ShopCategories_ShopCategoryTypeId",
                table: "ShopCategories");

            migrationBuilder.DropIndex(
                name: "IX_ShopCategories_ShopCategoryTypeId",
                table: "ShopCategories");

            migrationBuilder.DropColumn(
                name: "ShopCategoryTypeId",
                table: "ShopCategories");

            migrationBuilder.AddColumn<int>(
                name: "ShopCategoryType",
                table: "ShopCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9107));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 12, 0, 58, 58, 679, DateTimeKind.Local).AddTicks(9150));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopCategoryType",
                table: "ShopCategories");

            migrationBuilder.AddColumn<decimal>(
                name: "ShopCategoryTypeId",
                table: "ShopCategories",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2985));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2995));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(3003));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(3017));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2887));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2956));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 11, 23, 38, 16, 767, DateTimeKind.Local).AddTicks(2964));

            migrationBuilder.CreateIndex(
                name: "IX_ShopCategories_ShopCategoryTypeId",
                table: "ShopCategories",
                column: "ShopCategoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCategories_ShopCategories_ShopCategoryTypeId",
                table: "ShopCategories",
                column: "ShopCategoryTypeId",
                principalTable: "ShopCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
