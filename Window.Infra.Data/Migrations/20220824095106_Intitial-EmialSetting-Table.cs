using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class IntitialEmialSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailActivationCode",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Smtp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EnableSsL = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultEmail = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EmailSettings",
                columns: new[] { "Id", "CreateDate", "DisplayName", "EnableSsL", "From", "IsDefaultEmail", "IsDelete", "Password", "Port", "Smtp", "UserName" },
                values: new object[] { 1m, new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "برنامه ی درب و پنجره", true, "parsapanahpoor77@gmail.com", true, false, "54511441", 587, "smtp.gmail.com", "برنامه ی درب و پنجره" });

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6392));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6406));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6427));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6437));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6325));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6353));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6365));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6507));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6519));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6532));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6543));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6586));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6597));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6607));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6618));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6628));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6639));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6650));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6671));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6681));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6692));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6713));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6734));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6749));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6761));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6804));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 24, 14, 21, 5, 820, DateTimeKind.Local).AddTicks(6815));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailSettings");

            migrationBuilder.DropColumn(
                name: "EmailActivationCode",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4772));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4781));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4722));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4731));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4825));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4942));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4952));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4961));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(4991));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5066));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5076));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5086));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5096));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5105));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5124));

            migrationBuilder.UpdateData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 17, 13, 15, 24, 912, DateTimeKind.Local).AddTicks(5133));
        }
    }
}
