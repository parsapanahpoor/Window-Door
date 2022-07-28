using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateSellerPersonalInfoRelationByUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_UserId",
                table: "MarketPersonalInfo");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9606));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 14, 41, 48, 425, DateTimeKind.Local).AddTicks(9659));

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_UserId",
                table: "MarketPersonalInfo",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_UserId",
                table: "MarketPersonalInfo");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7779));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7796));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7804));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7731));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7741));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 10, 19, 57, 117, DateTimeKind.Local).AddTicks(7749));

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_UserId",
                table: "MarketPersonalInfo",
                column: "UserId");
        }
    }
}
