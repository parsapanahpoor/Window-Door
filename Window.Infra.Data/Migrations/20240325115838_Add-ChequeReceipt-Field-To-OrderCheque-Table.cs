using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChequeReceiptFieldToOrderChequeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChequeReceipt",
                table: "orderCheques",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChequeReceipt",
                table: "orderCheques");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1959));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1976));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1991));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(2008));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(2040));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1859));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1915));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 10, 12, 26, 7, 626, DateTimeKind.Local).AddTicks(1931));
        }
    }
}
