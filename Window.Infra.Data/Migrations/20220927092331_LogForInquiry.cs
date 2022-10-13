using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class LogForInquiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogForInquiry",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    StateId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CityId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerType = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogForInquiry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogForInquiry_MainBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "MainBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogForInquiry_States_CityId",
                        column: x => x.CityId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogForInquiry_States_CountryId",
                        column: x => x.CountryId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogForInquiry_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8580));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8603));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8612));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8622));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8503));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8549));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 53, 30, 928, DateTimeKind.Local).AddTicks(8559));

            migrationBuilder.CreateIndex(
                name: "IX_LogForInquiry_BrandId",
                table: "LogForInquiry",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_LogForInquiry_CityId",
                table: "LogForInquiry",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_LogForInquiry_CountryId",
                table: "LogForInquiry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LogForInquiry_StateId",
                table: "LogForInquiry",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogForInquiry");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8513));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8527));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8547));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8557));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8472));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 9, 27, 12, 34, 8, 962, DateTimeKind.Local).AddTicks(8495));
        }
    }
}
