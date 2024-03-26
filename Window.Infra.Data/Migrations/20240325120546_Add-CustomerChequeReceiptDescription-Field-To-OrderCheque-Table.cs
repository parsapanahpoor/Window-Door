using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerChequeReceiptDescriptionFieldToOrderChequeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerChequeReceiptDescription",
                table: "orderCheques",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9521));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9587));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9625));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 35, 45, 154, DateTimeKind.Local).AddTicks(9482));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerChequeReceiptDescription",
                table: "orderCheques");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6762));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6639));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6713));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 15, 28, 34, 609, DateTimeKind.Local).AddTicks(6731));
        }
    }
}
