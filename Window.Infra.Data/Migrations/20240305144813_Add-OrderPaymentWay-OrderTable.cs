using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderPaymentWayOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderState",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentWay",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3914));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3934));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3950));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3966));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3800));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3855));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3882));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentWay",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderState",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8584));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8604));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8620));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8661));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8478));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8518));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8536));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 3, 11, 24, 39, 288, DateTimeKind.Local).AddTicks(8554));
        }
    }
}
