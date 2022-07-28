using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialMarketUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketUsers_Market_MarketId",
                table: "MarketUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketUsers_Roles_RoleId",
                table: "MarketUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketUsers_Users_UserId",
                table: "MarketUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MarketUsers",
                table: "MarketUsers");

            migrationBuilder.RenameTable(
                name: "MarketUsers",
                newName: "MarketUser");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUsers_UserId",
                table: "MarketUser",
                newName: "IX_MarketUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUsers_RoleId",
                table: "MarketUser",
                newName: "IX_MarketUser_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUsers_MarketId",
                table: "MarketUser",
                newName: "IX_MarketUser_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarketUser",
                table: "MarketUser",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 39, 33, 302, DateTimeKind.Local).AddTicks(5365));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 39, 33, 302, DateTimeKind.Local).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 39, 33, 302, DateTimeKind.Local).AddTicks(5402));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 39, 33, 302, DateTimeKind.Local).AddTicks(5411));

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUser_Market_MarketId",
                table: "MarketUser",
                column: "MarketId",
                principalTable: "Market",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUser_Roles_RoleId",
                table: "MarketUser",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUser_Users_UserId",
                table: "MarketUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketUser_Market_MarketId",
                table: "MarketUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketUser_Roles_RoleId",
                table: "MarketUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketUser_Users_UserId",
                table: "MarketUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MarketUser",
                table: "MarketUser");

            migrationBuilder.RenameTable(
                name: "MarketUser",
                newName: "MarketUsers");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUser_UserId",
                table: "MarketUsers",
                newName: "IX_MarketUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUser_RoleId",
                table: "MarketUsers",
                newName: "IX_MarketUsers_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_MarketUser_MarketId",
                table: "MarketUsers",
                newName: "IX_MarketUsers_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarketUsers",
                table: "MarketUsers",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 37, 57, 444, DateTimeKind.Local).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 37, 57, 444, DateTimeKind.Local).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 37, 57, 444, DateTimeKind.Local).AddTicks(9517));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 16, 1, 37, 57, 444, DateTimeKind.Local).AddTicks(9537));

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUsers_Market_MarketId",
                table: "MarketUsers",
                column: "MarketId",
                principalTable: "Market",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUsers_Roles_RoleId",
                table: "MarketUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketUsers_Users_UserId",
                table: "MarketUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
