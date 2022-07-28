using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialProductYaraghBrandPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductMainBrandPrices",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MainBrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SegmentId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMainBrandPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMainBrandPrices_MainBrands_MainBrandId",
                        column: x => x.MainBrandId,
                        principalTable: "MainBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMainBrandPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMainBrandPrices_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductYaraghBrandPrices",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    YaraghBrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SegmentId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductYaraghBrandPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductYaraghBrandPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductYaraghBrandPrices_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductYaraghBrandPrices_YaraghBrands_YaraghBrandId",
                        column: x => x.YaraghBrandId,
                        principalTable: "YaraghBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(423));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 56, 14, 659, DateTimeKind.Local).AddTicks(372));

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_MainBrandId",
                table: "ProductMainBrandPrices",
                column: "MainBrandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_ProductId",
                table: "ProductMainBrandPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMainBrandPrices_SegmentId",
                table: "ProductMainBrandPrices",
                column: "SegmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_ProductId",
                table: "ProductYaraghBrandPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_SegmentId",
                table: "ProductYaraghBrandPrices",
                column: "SegmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductYaraghBrandPrices_YaraghBrandId",
                table: "ProductYaraghBrandPrices",
                column: "YaraghBrandId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMainBrandPrices");

            migrationBuilder.DropTable(
                name: "ProductYaraghBrandPrices");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8837));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8858));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8867));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8876));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8775));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8823));

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeCategoryId",
                table: "Products",
                column: "ProductTypeCategoryId");
        }
    }
}
