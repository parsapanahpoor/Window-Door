using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Window.Application.Services.Services;
using Window.Data.Context;
using Window.Domain.Entities.Inquiry;

var server = args.FirstOrDefault() ?? @"localhost\MSSQLSERVER2022";
if (server is not (@"localhost\MSSQLSERVER2022" or @"(localdb)\MSSQLLocalDB"))
    throw new ArgumentException("A supported local SQL Server is required.");

var connection = new SqlConnectionStringBuilder
{
    DataSource = server,
    InitialCatalog = "Panjerehyab_InquiryTest_" + Guid.NewGuid().ToString("N"),
    IntegratedSecurity = true,
    TrustServerCertificate = true
}.ConnectionString;
var options = new DbContextOptionsBuilder<WindowDbContext>().UseSqlServer(connection).Options;
await using var fixture = new WindowDbContext(options);

try
{
    await fixture.Database.EnsureCreatedAsync();
    await fixture.Database.ExecuteSqlRawAsync("""
        DROP TABLE dbo.InquirySellerExposures;
        DROP INDEX IX_logInquiryForUserDetails_LogInquiryForUserId_ClientRequestId ON dbo.logInquiryForUserDetails;
        ALTER TABLE dbo.logInquiryForUserDetails DROP COLUMN ClientRequestId;
        ALTER TABLE dbo.LogInquiryForUsers DROP COLUMN OwnerUserId, LastCompletedAtUtc, LastCompletionHasQuote;
        CREATE TABLE dbo.__EFMigrationsHistory (
            MigrationId nvarchar(150) NOT NULL PRIMARY KEY,
            ProductVersion nvarchar(32) NOT NULL
        );
        """);
    var previousMigration = "20261107120000_AddSmsLogin";
    var upgradeScript = fixture.GetService<IMigrator>().GenerateScript(previousMigration);
    await fixture.Database.OpenConnectionAsync();
    foreach (var batch in System.Text.RegularExpressions.Regex.Split(upgradeScript, @"^GO\s*$",
        System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase))
    {
        if (!string.IsNullOrWhiteSpace(batch)) await fixture.Database.ExecuteSqlRawAsync(batch);
    }
    await fixture.Database.CloseConnectionAsync();

    const string first = "web_fixture_first";
    const string second = "web_fixture_second";
    fixture.LogInquiryForUsers.AddRange(
        new LogInquiryForUser { UserMAcAddress = first },
        new LogInquiryForUser { UserMAcAddress = second });
    await fixture.SaveChangesAsync();

    var service = new InquryService(fixture, null!);
    Check(await service.IsUnownedInquiry(first), "Guest inquiry must start unowned.");
    Check(!await service.IsUnownedInquiry("web_missing"), "Missing token must be rejected.");
    var claims = await Task.WhenAll(
        ClaimInSeparateContext(first, 101),
        ClaimInSeparateContext(first, 102));
    Check(claims.Count(accepted => accepted) == 1, "Exactly one account may claim the inquiry.");
    var owner = claims[0] ? 101UL : 102UL;
    var other = claims[0] ? 102UL : 101UL;
    Check(await service.InquiryBelongsToUser(first, owner), "Winning claim must persist.");
    Check(!await service.ClaimInquiry(first, other), "A second account cannot take ownership.");
    Check(await service.ClaimInquiry(first, owner), "Retry by the owner must be idempotent.");
    Check(!await service.IsUnownedInquiry(first), "A claimed inquiry must be unavailable to guests.");
    Check(await service.IsUnownedInquiry(second), "Claiming one inquiry must leave other drafts alone.");

    var migrations = fixture.GetService<IMigrationsAssembly>().Migrations.Keys.ToArray();
    var ownerMigration = Array.IndexOf(migrations, "20261121084957_AddInquiryOwnerUserId");
    Check(ownerMigration > 0, "Ownership migration must be in the migration chain.");
    var script = fixture.GetService<IMigrator>().GenerateScript(migrations[ownerMigration - 1], migrations[ownerMigration]);
    Check(script.Contains("ADD [OwnerUserId]"), "Migration must add the nullable ownership column.");
    Check(!script.Contains("UPDATE [Roles]") && !script.Contains("UPDATE [ProductTypeCategories]"),
        "Ownership migration must leave existing seed records alone.");

    await InquiryWorkflowChecks.RunAsync(fixture, options);

    Console.WriteLine("PASS: guest guard, competing claims, owner retry, draft isolation, and migration scope.");
}
finally
{
    await fixture.Database.EnsureDeletedAsync();
}

async Task<bool> ClaimInSeparateContext(string token, ulong owner)
{
    await using var context = new WindowDbContext(options);
    return await new InquryService(context, null!).ClaimInquiry(token, owner);
}

static void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}
