using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Window.Data.Context;

#nullable disable

namespace Window.Infra.Data.Migrations;

[DbContext(typeof(WindowDbContext))]
[Migration("20261107120000_AddSmsLogin")]
public sealed class AddSmsLogin : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: MobileLoginKey.Name,
            schema: "dbo",
            table: "Users",
            type: "nvarchar(32)",
            maxLength: 32,
            nullable: false,
            computedColumnSql: MobileLoginKey.ComputedSql,
            stored: true);

        migrationBuilder.AddColumn<bool>(
            name: MobileLoginKey.Excluded,
            schema: "dbo",
            table: "Users",
            type: "bit",
            nullable: false,
            defaultValue: false);

        // Keep all existing accounts; do not pick an owner for a shared number.
        migrationBuilder.Sql("""
            EXEC(N'
                WITH duplicates AS (
                    SELECT [Id], COUNT_BIG(*) OVER (PARTITION BY [MobileLoginKey]) AS [Matches]
                    FROM [dbo].[Users]
                )
                UPDATE u SET [SmsLoginExcluded] = 1
                FROM [dbo].[Users] AS u
                JOIN duplicates AS d ON d.[Id] = u.[Id]
                WHERE d.[Matches] > 1;
            ');
            """);

        migrationBuilder.CreateIndex(
            name: "IX_Users_MobileLoginKey_SmsLoginExcluded",
            schema: "dbo",
            table: "Users",
            columns: new[] { MobileLoginKey.Name, MobileLoginKey.Excluded });
        migrationBuilder.CreateIndex(
            name: "IX_Users_MobileLoginKey",
            schema: "dbo",
            table: "Users",
            column: MobileLoginKey.Name,
            unique: true,
            filter: MobileLoginKey.EligibleFilter);

        migrationBuilder.CreateTable(
            name: "SmsLoginChallenges",
            schema: "dbo",
            columns: table => new
            {
                Mobile = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProtectedCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                NextSendAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                WindowStartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                SendCount = table.Column<int>(type: "int", nullable: false),
                FailedAttempts = table.Column<int>(type: "int", nullable: false),
                Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_SmsLoginChallenges", x => x.Mobile));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "SmsLoginChallenges", schema: "dbo");
        migrationBuilder.DropIndex(name: "IX_Users_MobileLoginKey", table: "Users", schema: "dbo");
        migrationBuilder.DropIndex(name: "IX_Users_MobileLoginKey_SmsLoginExcluded", table: "Users", schema: "dbo");
        migrationBuilder.DropColumn(name: MobileLoginKey.Excluded, table: "Users", schema: "dbo");
        migrationBuilder.DropColumn(name: MobileLoginKey.Name, table: "Users", schema: "dbo");
    }
}
