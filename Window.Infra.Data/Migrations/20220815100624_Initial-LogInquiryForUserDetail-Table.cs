using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialLogInquiryForUserDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "LogInquiryForUsers");

            migrationBuilder.DropColumn(
                name: "SampleId",
                table: "LogInquiryForUsers");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "LogInquiryForUsers");

            migrationBuilder.CreateTable(
                name: "logInquiryForUserDetails",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogInquiryForUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SampleId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logInquiryForUserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_logInquiryForUserDetails_LogInquiryForUsers_LogInquiryForUserId",
                        column: x => x.LogInquiryForUserId,
                        principalTable: "LogInquiryForUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_logInquiryForUserDetails_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5084));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5098));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5107));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5117));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5006));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5046));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5067));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5164));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5175));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5186));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5197));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5208));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5219));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5229));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5250));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5260));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5270));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5300));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5320));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5401));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5411));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5421));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5432));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5442));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5452));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5461));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5471));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5481));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5491));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5511));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 15, 14, 36, 24, 229, DateTimeKind.Local).AddTicks(5520));

            migrationBuilder.CreateIndex(
                name: "IX_logInquiryForUserDetails_LogInquiryForUserId",
                table: "logInquiryForUserDetails",
                column: "LogInquiryForUserId");

            migrationBuilder.CreateIndex(
                name: "IX_logInquiryForUserDetails_SampleId",
                table: "logInquiryForUserDetails",
                column: "SampleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logInquiryForUserDetails");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "LogInquiryForUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SampleId",
                table: "LogInquiryForUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "LogInquiryForUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(285));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(312));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(331));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(170));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(423));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(442));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(480));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(501));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(537));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(575));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(644));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(665));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(682));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(718));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(736));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(754));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(773));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(790));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(808));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(841));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(858));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(876));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(911));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(930));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(948));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(965));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(982));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(1000));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(1018));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 14, 17, 25, 9, 384, DateTimeKind.Local).AddTicks(1034));
        }
    }
}
