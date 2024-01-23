using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ShopProducts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9353));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9386));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9418));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9438));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9294));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 23, 17, 59, 9, 159, DateTimeKind.Local).AddTicks(9316));

            migrationBuilder.CreateIndex(
                name: "IX_ShopProductSelectedCategories_ShopCategoryId",
                table: "ShopProductSelectedCategories",
                column: "ShopCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProductSelectedCategories_ShopProductId",
                table: "ShopProductSelectedCategories",
                column: "ShopProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductSelectedCategories_ShopCategories_ShopCategoryId",
                table: "ShopProductSelectedCategories",
                column: "ShopCategoryId",
                principalTable: "ShopCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProductSelectedCategories_ShopProducts_ShopProductId",
                table: "ShopProductSelectedCategories",
                column: "ShopProductId",
                principalTable: "ShopProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductSelectedCategories_ShopCategories_ShopCategoryId",
                table: "ShopProductSelectedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopProductSelectedCategories_ShopProducts_ShopProductId",
                table: "ShopProductSelectedCategories");

            migrationBuilder.DropIndex(
                name: "IX_ShopProductSelectedCategories_ShopCategoryId",
                table: "ShopProductSelectedCategories");

            migrationBuilder.DropIndex(
                name: "IX_ShopProductSelectedCategories_ShopProductId",
                table: "ShopProductSelectedCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3382));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3406));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3452));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3254));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3323));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 36, 3, 760, DateTimeKind.Local).AddTicks(3342));
        }
    }
}
