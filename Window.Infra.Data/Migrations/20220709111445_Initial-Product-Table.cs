using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypeCategories",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CountryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StateId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CityId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerType = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    ProductKind = table.Column<int>(type: "int", nullable: false),
                    ProductTypeCategoryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypeCategories_ProductTypeCategoryId",
                        column: x => x.ProductTypeCategoryId,
                        principalTable: "ProductTypeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ProductTypeCategories",
                columns: new[] { "Id", "CreateDate", "IsDelete", "Name", "ProductType" },
                values: new object[,]
                {
                    { 1m, new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8837), false, "کشویی", 0 },
                    { 2m, new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8850), false, "کتیبه", 0 },
                    { 3m, new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8858), false, "کتیبه", 1 },
                    { 4m, new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8867), false, "درب", 1 },
                    { 5m, new DateTime(2022, 7, 9, 15, 44, 45, 511, DateTimeKind.Local).AddTicks(8876), false, "لولایی", 1 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypeCategories");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 14, 48, 7, 961, DateTimeKind.Local).AddTicks(7804));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 14, 48, 7, 961, DateTimeKind.Local).AddTicks(7831));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 14, 48, 7, 961, DateTimeKind.Local).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 9, 14, 48, 7, 961, DateTimeKind.Local).AddTicks(7851));
        }
    }
}
