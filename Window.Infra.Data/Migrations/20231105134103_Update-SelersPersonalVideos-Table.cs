using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateSelersPersonalVideosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SelersPersonalVideos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4201));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4214));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4222));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4236));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4243));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4168));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4176));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 17, 11, 2, 997, DateTimeKind.Local).AddTicks(4184));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "SelersPersonalVideos");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6334));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6343));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6352));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6361));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6260));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6285));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6295));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 11, 5, 13, 0, 23, 659, DateTimeKind.Local).AddTicks(6305));
        }
    }
}
