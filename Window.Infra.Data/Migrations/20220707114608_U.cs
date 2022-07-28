using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class U : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_State_State_ParentId",
                table: "State");

            migrationBuilder.DropPrimaryKey(
                name: "PK_State",
                table: "State");

            migrationBuilder.RenameTable(
                name: "State",
                newName: "States");

            migrationBuilder.RenameIndex(
                name: "IX_State_ParentId",
                table: "States",
                newName: "IX_States_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_States",
                table: "States",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9520));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9548));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 7, 16, 16, 7, 867, DateTimeKind.Local).AddTicks(9570));

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_States_CityId",
                table: "MarketPersonalInfo",
                column: "CityId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_States_CountryId",
                table: "MarketPersonalInfo",
                column: "CountryId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketPersonalInfo_States_StateId",
                table: "MarketPersonalInfo",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_States_ParentId",
                table: "States",
                column: "ParentId",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_States_CityId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_States_CountryId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketPersonalInfo_States_StateId",
                table: "MarketPersonalInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_States_States_ParentId",
                table: "States");

            migrationBuilder.DropPrimaryKey(
                name: "PK_States",
                table: "States");

            migrationBuilder.RenameTable(
                name: "States",
                newName: "State");

            migrationBuilder.RenameIndex(
                name: "IX_States_ParentId",
                table: "State",
                newName: "IX_State_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_State",
                table: "State",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_State_State_ParentId",
                table: "State",
                column: "ParentId",
                principalTable: "State",
                principalColumn: "Id");
        }
    }
}
