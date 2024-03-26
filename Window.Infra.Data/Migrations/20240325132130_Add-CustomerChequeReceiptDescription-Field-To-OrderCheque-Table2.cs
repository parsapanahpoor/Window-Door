using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerChequeReceiptDescriptionFieldToOrderChequeTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChequeReceipt",
                table: "orderCheques",
                newName: "ChequeReceiptFileName");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4282));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4302));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4365));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4382));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4168));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4212));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4234));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 25, 16, 51, 27, 690, DateTimeKind.Local).AddTicks(4252));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChequeReceiptFileName",
                table: "orderCheques",
                newName: "ChequeReceipt");

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
    }
}
