using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialKhadamatePasAzForoshTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhadamatePasAzForosh",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhadamatePasAzForosh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhadamatePasAzForosh_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasokhGoie",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasokhGoie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasokhGoie_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SehateEtelaAt",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SehateEtelaAt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SehateEtelaAt_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaAhodeZamaneTahvil",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaAhodeZamaneTahvil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaAhodeZamaneTahvil_Users_UserId",
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
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2376));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2399));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2416));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2310));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2349));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 11, 1, 20, 22, 48, 142, DateTimeKind.Local).AddTicks(2359));

            migrationBuilder.CreateIndex(
                name: "IX_KhadamatePasAzForosh_UserId",
                table: "KhadamatePasAzForosh",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasokhGoie_UserId",
                table: "PasokhGoie",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SehateEtelaAt_UserId",
                table: "SehateEtelaAt",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaAhodeZamaneTahvil_UserId",
                table: "TaAhodeZamaneTahvil",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KhadamatePasAzForosh");

            migrationBuilder.DropTable(
                name: "PasokhGoie");

            migrationBuilder.DropTable(
                name: "SehateEtelaAt");

            migrationBuilder.DropTable(
                name: "TaAhodeZamaneTahvil");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7623));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7688));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7838));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7368));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7444));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7497));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 10, 30, 18, 16, 6, 746, DateTimeKind.Local).AddTicks(7559));
        }
    }
}
