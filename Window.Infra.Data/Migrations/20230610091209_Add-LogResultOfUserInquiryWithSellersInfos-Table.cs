using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddLogResultOfUserInquiryWithSellersInfosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogResultOfUserInquiryWithSellersInfos",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LogInquiryForUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    BrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    SellerScore = table.Column<int>(type: "int", nullable: false),
                    SellerShopName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogResultOfUserInquiryWithSellersInfos", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1836));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1871));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1895));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1918));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1942));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1695));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1742));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1769));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 10, 12, 42, 9, 158, DateTimeKind.Local).AddTicks(1795));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogResultOfUserInquiryWithSellersInfos");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4585));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4530));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4541));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 9, 10, 45, 23, 467, DateTimeKind.Local).AddTicks(4550));
        }
    }
}
