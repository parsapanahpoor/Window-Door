using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class InitialMarketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellerLinks");

            migrationBuilder.DropTable(
                name: "SellersPersonalInfo");

            migrationBuilder.DropTable(
                name: "SellerWorkSamle");

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MarketName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketPersonalsInfoState = table.Column<int>(type: "int", nullable: false),
                    ActivationTariff = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Market_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Market_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketLinks",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MarketId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LinkTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketLinks_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketLinks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketPersonalInfo",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MarketId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    NationalCode = table.Column<int>(type: "int", nullable: false),
                    PhotoOfBusinessLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoOfNationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriodTimesOfWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketPersonalsInfoState = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketPersonalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketPersonalInfo_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketPersonalInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketUsers",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MarketId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    RoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketUsers_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketWorkSamle",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MarketId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    WorkSampleImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkSampleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketWorkSamle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketWorkSamle_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Market",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketWorkSamle_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Market_RoleId",
                table: "Market",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Market_UserId",
                table: "Market",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketLinks_MarketId",
                table: "MarketLinks",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketLinks_UserId",
                table: "MarketLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_MarketId",
                table: "MarketPersonalInfo",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPersonalInfo_UserId",
                table: "MarketPersonalInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketUsers_MarketId",
                table: "MarketUsers",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketUsers_RoleId",
                table: "MarketUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketUsers_UserId",
                table: "MarketUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_MarketId",
                table: "MarketWorkSamle",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketWorkSamle_UserId",
                table: "MarketWorkSamle",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketLinks");

            migrationBuilder.DropTable(
                name: "MarketPersonalInfo");

            migrationBuilder.DropTable(
                name: "MarketUsers");

            migrationBuilder.DropTable(
                name: "MarketWorkSamle");

            migrationBuilder.DropTable(
                name: "Market");

            migrationBuilder.CreateTable(
                name: "SellerLinks",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LinkTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerLinks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellersPersonalInfo",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    NationalCode = table.Column<int>(type: "int", nullable: false),
                    PeriodTimesOfWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoOfBusinessLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoOfNationalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellersPersonalsInfoState = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellersPersonalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellersPersonalInfo_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellerWorkSamle",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    WorkSampleImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkSampleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerWorkSamle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerWorkSamle_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5467));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5489));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 6, 14, 9, 54, 55, 128, DateTimeKind.Local).AddTicks(5509));

            migrationBuilder.CreateIndex(
                name: "IX_SellerLinks_UserId",
                table: "SellerLinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SellersPersonalInfo_UserId",
                table: "SellersPersonalInfo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerWorkSamle_UserId",
                table: "SellerWorkSamle",
                column: "UserId");
        }
    }
}
