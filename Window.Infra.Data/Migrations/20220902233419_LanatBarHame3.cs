using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class LanatBarHame3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogForBrands",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MainBrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogForBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogForBrands_MainBrands_MainBrandId",
                        column: x => x.MainBrandId,
                        principalTable: "MainBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1703));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1715));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1725));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1735));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1745));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1639));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1667));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1678));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1688));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1763));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1778));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1789));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1799));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1809));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1820));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1849));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1860));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1869));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1880));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1890));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1910));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1929));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1947));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1957));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1967));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1977));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1987));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(1996));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2007));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2027));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2036));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2046));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2056));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2065));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2075));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2085));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 3, 4, 4, 19, 78, DateTimeKind.Local).AddTicks(2094));

            migrationBuilder.CreateIndex(
                name: "IX_LogForBrands_MainBrandId",
                table: "LogForBrands",
                column: "MainBrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogForBrands");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9519));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9837));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9915));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9933));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9972));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 199, DateTimeKind.Local).AddTicks(9993));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(54));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(94));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(113));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(133));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(171));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(190));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(282));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 2, 22, 4, 49, 200, DateTimeKind.Local).AddTicks(348));
        }
    }
}
