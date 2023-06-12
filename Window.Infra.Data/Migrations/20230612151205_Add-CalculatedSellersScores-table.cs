using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddCalculatedSellersScorestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatedSellersScores",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSellerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CalculatedScore = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedSellersScores", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4713));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4723));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4737));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4744));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4655));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 12, 18, 42, 4, 424, DateTimeKind.Local).AddTicks(4698));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedSellersScores");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8710));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8718));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8726));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8733));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8621));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8650));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8659));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2023, 6, 11, 22, 41, 9, 472, DateTimeKind.Local).AddTicks(8667));
        }
    }
}
