using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddPriceToShopProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopProductsSelectedBrands");

            migrationBuilder.DropTable(
                name: "ShopProductsSelectedColors");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "ShopProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9662));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9607));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 8, 11, 18, 4, 594, DateTimeKind.Local).AddTicks(9626));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "ShopProducts");

            migrationBuilder.CreateTable(
                name: "ShopProductsSelectedBrands",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ShopBrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ShopProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProductsSelectedBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopProductsSelectedColors",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ShopProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProductsSelectedColors", x => x.Id);
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
    }
}
