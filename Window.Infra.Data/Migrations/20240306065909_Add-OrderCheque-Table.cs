using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderChequeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orderCheques",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CustomerUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SellerUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ChequeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderChequeAdminState = table.Column<int>(type: "int", nullable: false),
                    OrderChequeSellerState = table.Column<int>(type: "int", nullable: false),
                    SellerRejectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminRejectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderCheques", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4400));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4423));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4442));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4485));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4258));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4316));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4346));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 6, 10, 29, 8, 324, DateTimeKind.Local).AddTicks(4369));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderCheques");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3914));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3934));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3950));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3966));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3800));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3855));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2024, 3, 5, 18, 18, 10, 801, DateTimeKind.Local).AddTicks(3882));
        }
    }
}
