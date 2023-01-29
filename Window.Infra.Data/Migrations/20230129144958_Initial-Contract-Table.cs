using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialContractTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestForContracts",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    RequestForContractType = table.Column<int>(type: "int", nullable: false),
                    RequestForContractStatus = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForContracts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9908));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9917));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 1, 29, 18, 19, 57, 659, DateTimeKind.Local).AddTicks(9868));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestForContracts");

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
    }
}
