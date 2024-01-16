using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShopProductFeatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopProductFeature",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    FeatureTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeatureValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProductFeature", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3195));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3215));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3232));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3249));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3265));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3096));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3132));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3150));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 16, 22, 18, 19, 294, DateTimeKind.Local).AddTicks(3166));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopProductFeature");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2236));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2265));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2304));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2359));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2089));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2144));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 1, 15, 21, 4, 8, 100, DateTimeKind.Local).AddTicks(2198));
        }
    }
}
