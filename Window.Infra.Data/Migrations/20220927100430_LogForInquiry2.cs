using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class LogForInquiry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogForInquiry_MainBrands_BrandId",
                table: "LogForInquiry");

            migrationBuilder.DropIndex(
                name: "IX_LogForInquiry_BrandId",
                table: "LogForInquiry");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "LogForInquiry");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9936));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9968));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 934, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 934, DateTimeKind.Local).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 34, 29, 933, DateTimeKind.Local).AddTicks(9845));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BrandId",
                table: "LogForInquiry",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5639));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5656));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5665));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5683));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5614));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 13, 25, 55, 14, DateTimeKind.Local).AddTicks(5624));

            migrationBuilder.CreateIndex(
                name: "IX_LogForInquiry_BrandId",
                table: "LogForInquiry",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogForInquiry_MainBrands_BrandId",
                table: "LogForInquiry",
                column: "BrandId",
                principalTable: "MainBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
