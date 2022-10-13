using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageFile",
                table: "TicketMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1189));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1202));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1233));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1152));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 26, 20, 58, 38, 868, DateTimeKind.Local).AddTicks(1162));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageFile",
                table: "TicketMessages");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5415));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5427));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5448));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5458));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5344));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 24, 22, 32, 39, 598, DateTimeKind.Local).AddTicks(5395));
        }
    }
}
