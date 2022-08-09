using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Window.Infra.Data.Migrations
{
    public partial class SegmentsDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2039));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2057));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2065));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(1968));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(1994));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2004));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2013));

            migrationBuilder.InsertData(
                table: "Segments",
                columns: new[] { "Id", "Aluminum", "CreateDate", "Door", "IsDelete", "Keshoie", "Lolaie", "SegmentName", "UPVC", "Window" },
                values: new object[,]
                {
                    { 1m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2085), true, false, true, true, "فریم", true, true },
                    { 2m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2142), true, false, true, true, "زهوار دوجداره", true, true },
                    { 3m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2154), true, false, true, true, "لنگه", true, true },
                    { 4m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2164), true, false, true, true, "یراق ملغی", true, true },
                    { 5m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2174), true, false, true, true, "یراق تک حالته", true, true },
                    { 6m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2185), true, false, true, true, "لنگه ی بازشوی پنجره", true, true },
                    { 7m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2194), true, false, true, true, "مولیون لولایی", true, true },
                    { 8m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2204), true, false, true, true, "گالوانیزه ی فریم", true, true },
                    { 10m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2213), true, false, true, true, "گالوانیزه ی لنگه", true, true },
                    { 11m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2224), true, false, true, true, "گالوانیزه ی مولیون", true, true },
                    { 12m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2233), true, false, true, true, "لنگه ی درب", true, true },
                    { 13m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2243), true, false, true, true, "گالوانیزه ی دربی", true, true },
                    { 14m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2252), true, false, true, true, "یراق درب سویئچی", true, true },
                    { 15m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2262), true, false, true, true, "یراق درب سرویسی", true, true },
                    { 16m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2271), true, false, true, true, "گالوانیزه ی لولایی", true, true },
                    { 17m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2281), true, false, true, true, "پنل", true, true },
                    { 18m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2291), true, false, true, true, "گالوانیزه ی مولیون لولایی", true, true },
                    { 19m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2301), true, false, true, true, "لنگه ی کشویی", true, true },
                    { 20m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2311), true, false, true, true, "گالوانیزه ی لنگه ی کشویی", true, true },
                    { 21m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2321), true, false, true, true, "نوار مویی", true, true },
                    { 22m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2330), true, false, true, true, "کاور لنگه ی کشویی", true, true },
                    { 23m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2340), true, false, true, true, "مولوین کشویی", true, true },
                    { 24m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2350), true, false, true, true, "گالوانیزه ی مولوین کشویی", true, true },
                    { 25m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2359), true, false, true, true, "کاور مولوین کشویی", true, true },
                    { 26m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2369), true, false, true, true, "کاور بارانگیر", true, true },
                    { 27m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2379), true, false, true, true, "ریل کشویی", true, true },
                    { 28m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2388), true, false, true, true, "یراق کشویی", true, true },
                    { 29m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2397), true, false, true, true, "ریل کشویی", true, true },
                    { 30m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2444), true, false, true, true, "لاستیک فشاری", true, true },
                    { 31m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2455), true, false, true, true, "اینترلاک", true, true },
                    { 32m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2464), true, false, true, true, "فریم لولایی", true, true },
                    { 33m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2474), true, false, true, true, "وادار لولایی", true, true },
                    { 34m, true, new DateTime(2022, 8, 9, 14, 4, 43, 583, DateTimeKind.Local).AddTicks(2484), true, false, true, true, "قفل چهارلنگه", true, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 1m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 2m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 3m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 4m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 5m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 6m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 7m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 8m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 10m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 11m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 12m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 13m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 14m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 15m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 16m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 17m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 18m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 19m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 20m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 21m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 22m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 23m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 24m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 25m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 26m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 27m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 28m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 29m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 30m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 31m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 32m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 33m);

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 34m);

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7974));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7988));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7998));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(8008));

            migrationBuilder.UpdateData(
                table: "ProductTypeCategories",
                keyColumn: "Id",
                keyValue: 5m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(8018));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7911));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7938));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7949));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4m,
                column: "CreateDate",
                value: new DateTime(2022, 7, 28, 17, 28, 50, 548, DateTimeKind.Local).AddTicks(7960));
        }
    }
}
