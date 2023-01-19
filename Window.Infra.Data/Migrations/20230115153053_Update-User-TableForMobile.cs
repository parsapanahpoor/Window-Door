using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateUserTableForMobile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireMobileSMSDateTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMobileConfirm",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MobileActivationCode",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3145));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3167));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3182));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3217));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3231));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(2985));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3073));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3088));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 19, 0, 53, 345, DateTimeKind.Local).AddTicks(3114));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireMobileSMSDateTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMobileConfirm",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MobileActivationCode",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4460));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4472));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4481));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4439));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 15, 18, 47, 4, 682, DateTimeKind.Local).AddTicks(4447));
        }
    }
}
