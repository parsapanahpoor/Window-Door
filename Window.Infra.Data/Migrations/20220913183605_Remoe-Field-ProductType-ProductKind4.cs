using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class RemoeFieldProductTypeProductKind4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductKind",
                table: "LogInquiryForUsers");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "LogInquiryForUsers");

            migrationBuilder.AddColumn<int>(
                name: "CountOfSample",
                table: "logInquiryForUserDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7378));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7393));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7404));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7329));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7467));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7479));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7490));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7500));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7512));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7523));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7545));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7558));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7569));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7579));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7590));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7601));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7612));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7622));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7633));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7645));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7656));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7673));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7695));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7717));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7730));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7741));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7752));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7796));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7808));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 13, 23, 6, 4, 733, DateTimeKind.Local).AddTicks(7819));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfSample",
                table: "logInquiryForUserDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductKind",
                table: "LogInquiryForUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "LogInquiryForUsers",
                type: "int",
                nullable: true);

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
    }
}
