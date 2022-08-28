using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class AddGlassToInquiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GlassId",
                table: "LogInquiryForUsers",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4348));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4358));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4369));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4213));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4240));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4250));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4259));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4417));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4428));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4437));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4447));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4458));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4468));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4477));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4487));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4534));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4563));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4573));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4583));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4593));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4661));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4671));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4690));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4701));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4710));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4720));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4739));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 28, 9, 32, 58, 769, DateTimeKind.Local).AddTicks(4758));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlassId",
                table: "LogInquiryForUsers");

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
        }
    }
}
