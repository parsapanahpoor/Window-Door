using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddShopProductSelectedCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopProductSelectedCategories",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ShopCategoryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProductSelectedCategories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5670));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5682));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5690));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5697));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5705));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5608));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5636));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 43, 59, 221, DateTimeKind.Local).AddTicks(5652));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopProductSelectedCategories");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3307));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3329));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3342));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3356));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3371));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3150));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3244));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 2, 11, 25, 18, 21, DateTimeKind.Local).AddTicks(3274));
        }
    }
}
