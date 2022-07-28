using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class SeedDataForRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5467));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5489));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateDate", "IsDelete", "RoleUniqueName", "Title" },
                values: new object[] { 4m, new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5509), false, "SellerMaster", "SellerMaster" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 9, 13, 49, 33, 758, DateTimeKind.Local).AddTicks(6308));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 9, 13, 49, 33, 758, DateTimeKind.Local).AddTicks(6338));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 9, 13, 49, 33, 758, DateTimeKind.Local).AddTicks(6349));
        }
    }
}
