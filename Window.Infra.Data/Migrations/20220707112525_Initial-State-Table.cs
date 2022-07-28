using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "MarketPersonalInfo");

            migrationBuilder.DropColumn(
                name: "State",
                table: "MarketPersonalInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "CityId",
                table: "MarketPersonalInfo",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CountryId",
                table: "MarketPersonalInfo",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StateId",
                table: "MarketPersonalInfo",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ParentId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_State_ParentId",
                        column: x => x.ParentId,
                        principalTable: "State",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 15, 55, 24, 673, DateTimeKind.Local).AddTicks(299));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 15, 55, 24, 673, DateTimeKind.Local).AddTicks(326));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 15, 55, 24, 673, DateTimeKind.Local).AddTicks(337));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 15, 55, 24, 673, DateTimeKind.Local).AddTicks(346));

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_CityId",
                table: "MarketPersonalInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_CountryId",
                table: "MarketPersonalInfo",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_StateId",
                table: "MarketPersonalInfo",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_State_ParentId",
                table: "State",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_State_CityId",
                table: "MarketPersonalInfo",
                column: "CityId",
                principalTable: "State",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_State_CountryId",
                table: "MarketPersonalInfo",
                column: "CountryId",
                principalTable: "State",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_State_StateId",
                table: "MarketPersonalInfo",
                column: "StateId",
                principalTable: "State",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_State_CityId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_State_CountryId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_State_StateId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_CityId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_CountryId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropIndex(
                name: "IX_MarketPersonalInfo_StateId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "MarketPersonalInfo");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MarketPersonalInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "MarketPersonalInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 5, 10, 1, 57, 521, DateTimeKind.Local).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 5, 10, 1, 57, 521, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 5, 10, 1, 57, 521, DateTimeKind.Local).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 5, 10, 1, 57, 521, DateTimeKind.Local).AddTicks(1470));
        }
    }
}
