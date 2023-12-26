using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddSellerUserIdToShopProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellerUserId",
                table: "ShopProducts",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8812));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8828));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8838));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8846));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8739));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8777));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8788));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 16, 2, 922, DateTimeKind.Local).AddTicks(8796));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerUserId",
                table: "ShopProducts");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2668));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2693));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2701));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2615));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2637));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2645));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 12, 26, 17, 4, 56, 839, DateTimeKind.Local).AddTicks(2652));
        }
    }
}
