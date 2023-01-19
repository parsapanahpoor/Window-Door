using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateUserTableForEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6083));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6098));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6107));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6116));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(5980));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6031));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6043));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 4, 21, 18, 16, 307, DateTimeKind.Local).AddTicks(6052));
        }
    }
}
