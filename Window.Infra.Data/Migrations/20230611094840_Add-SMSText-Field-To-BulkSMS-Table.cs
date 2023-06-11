using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddSMSTextFieldToBulkSMSTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SMSText",
                table: "BulkSMS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2547));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2561));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2569));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2583));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2491));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2514));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2522));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 13, 18, 39, 606, DateTimeKind.Local).AddTicks(2530));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SMSText",
                table: "BulkSMS");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1888));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1898));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1904));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1910));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1861));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1868));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 22, 9, 24, 518, DateTimeKind.Local).AddTicks(1874));
        }
    }
}
