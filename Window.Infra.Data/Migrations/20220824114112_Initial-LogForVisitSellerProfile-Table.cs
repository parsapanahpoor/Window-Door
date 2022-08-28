using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialLogForVisitSellerProfileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogForVisitSellerProfiles",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogForVisitSellerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogForVisitSellerProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5409));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5422));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5432));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5442));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5453));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5343));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5487));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5497));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5508));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5528));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5548));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5557));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5578));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5588));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5598));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5613));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5624));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5634));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5655));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5664));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5684));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5693));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5703));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5713));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5723));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5733));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5742));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5751));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5761));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5781));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5791));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 16, 11, 11, 993, DateTimeKind.Local).AddTicks(5801));

            migrationBuilder.CreateIndex(
                name: "IX_LogForVisitSellerProfiles_UserId",
                table: "LogForVisitSellerProfiles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogForVisitSellerProfiles");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2603));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2613));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2623));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2474));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2496));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2663));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2684));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2694));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2706));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2716));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2737));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2748));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2759));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2769));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2780));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2790));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2801));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2819));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2841));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2861));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2882));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2903));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2913));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2923));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2936));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2958));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2971));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2981));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(2995));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 54, 39, 8, DateTimeKind.Local).AddTicks(3007));
        }
    }
}
