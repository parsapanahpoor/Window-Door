using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class UpdateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle");

            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_MarketId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks");

            migrationBuilder.DropIndex(
                name: "IX_Market_UserId",
                table: "Market");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 14, 0, 23, 787, DateTimeKind.Local).AddTicks(1418));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 14, 0, 23, 787, DateTimeKind.Local).AddTicks(1452));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 14, 0, 23, 787, DateTimeKind.Local).AddTicks(1470));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 14, 0, 23, 787, DateTimeKind.Local).AddTicks(1500));

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle",
                column: "MarketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_MarketId",
                table: "MarketPersonalInfo",
                column: "MarketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks",
                column: "MarketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Market_UserId",
                table: "Market",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle");

            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_MarketId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks");

            migrationBuilder.DropIndex(
                name: "IX_Market_UserId",
                table: "Market");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 12, 49, 5, 81, DateTimeKind.Local).AddTicks(3160));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 12, 49, 5, 81, DateTimeKind.Local).AddTicks(3187));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 12, 49, 5, 81, DateTimeKind.Local).AddTicks(3199));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 2, 12, 49, 5, 81, DateTimeKind.Local).AddTicks(3208));

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_MarketId",
                table: "MarketPersonalInfo",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_Market_UserId",
                table: "Market",
                column: "UserId");
        }
    }
}
