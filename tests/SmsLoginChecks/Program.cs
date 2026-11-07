using System.Collections.Concurrent;
using CRM.Data.Repository;
using Microsoft.Extensions.Configuration;
using Window.Domain.ViewModels.Account;
using Kavenegar.Core.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Window.Application.Security;
using Window.Application.Services;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;

var server = args.FirstOrDefault() ?? @"localhost\MSSQLSERVER2022";
if (server is not (@"localhost\MSSQLSERVER2022" or @"(localdb)\MSSQLLocalDB"))
    throw new ArgumentException("Use a supported local SQL Server instance for these checks.");

var connection = new SqlConnectionStringBuilder
{
    DataSource = server,
    InitialCatalog = "Panjerehyab_SmsLoginTest_" + Guid.NewGuid().ToString("N"),
    IntegratedSecurity = true,
    TrustServerCertificate = true
}.ConnectionString;
var options = new DbContextOptionsBuilder<WindowDbContext>().UseSqlServer(connection).Options;
var clock = new TestClock();
var sender = new OfflineSms();
var protector = new EphemeralDataProtectionProvider();
await using var fixture = new WindowDbContext(options);

try
{
    await fixture.Database.EnsureCreatedAsync();

    var legacyPassword = PasswordHelper.EncodePasswordMd5("fixture-password");
    const string adminPhone = "09100000011";
    const string blockedPhone = "09100000012";
    const string unverifiedPhone = "09100000013";
    const string deletedPhone = "09100000014";
    const string disputedPhone = "09100000015";
    const string disputedAlias = "+۹۸ ۹۱۰-۰۰۰-۰۰۱۵";
    const string newlyDisputedPhone = "09100000016";
    var admin = CreateUser("+۹۸ ۹۱۰-۰۰۰-۰۰۱۱", legacyPassword, confirmed: true);
    admin.IsAdmin = true;
    var blocked = CreateUser(blockedPhone, legacyPassword, confirmed: true);
    blocked.IsBan = true;
    var deleted = CreateUser(deletedPhone, legacyPassword, confirmed: true);
    deleted.IsDelete = true;
    fixture.Users.AddRange(admin, blocked, deleted, CreateUser(unverifiedPhone, legacyPassword, confirmed: false));
    fixture.Users.AddRange(CreateUser(disputedPhone, legacyPassword, true), CreateUser("09100000018", legacyPassword, true));
    await fixture.SaveChangesAsync();

    await fixture.Database.ExecuteSqlRawAsync("""
        DROP TABLE dbo.SmsLoginChallenges;
        DROP INDEX IX_Users_MobileLoginKey ON dbo.Users;
        DROP INDEX IX_Users_MobileLoginKey_SmsLoginExcluded ON dbo.Users;
        ALTER TABLE dbo.Users DROP COLUMN MobileLoginKey;
        DECLARE @constraint sysname = (
            SELECT dc.name FROM sys.default_constraints AS dc
            JOIN sys.columns AS c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
            WHERE dc.parent_object_id = OBJECT_ID(N'dbo.Users') AND c.name = N'SmsLoginExcluded'
        );
        EXEC(N'ALTER TABLE dbo.Users DROP CONSTRAINT ' + @constraint);
        ALTER TABLE dbo.Users DROP COLUMN SmsLoginExcluded;

        UPDATE dbo.Users SET Mobile = N'+۹۸ ۹۱۰-۰۰۰-۰۰۱۵' WHERE Mobile = N'09100000018';
        """);
    await fixture.Database.ExecuteSqlRawAsync("""
        CREATE TABLE dbo.__EFMigrationsHistory (
            MigrationId nvarchar(150) NOT NULL PRIMARY KEY,
            ProductVersion nvarchar(32) NOT NULL
        );
        EXEC(N'CREATE SCHEMA [fixture_deploy] AUTHORIZATION [dbo];');
        CREATE USER [fixture_deploy] WITHOUT LOGIN WITH DEFAULT_SCHEMA = [fixture_deploy];
        GRANT CREATE TABLE TO [fixture_deploy];
        GRANT ALTER ON SCHEMA::dbo TO [fixture_deploy];
        GRANT SELECT, INSERT, UPDATE ON SCHEMA::dbo TO [fixture_deploy];
        """);
    var script = fixture.GetService<IMigrator>().GenerateScript(
        "20240719102109_Add_ProductInventory_field",
        "20261107120000_AddSmsLogin",
        MigrationsSqlGenerationOptions.Idempotent);
    var batches = System.Text.RegularExpressions.Regex.Split(script, @"^GO\s*$",
        System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    await fixture.Database.OpenConnectionAsync();
    try
    {
        await fixture.Database.ExecuteSqlRawAsync("""
            EXECUTE AS USER = N'fixture_deploy';
            IF SCHEMA_NAME() <> N'fixture_deploy' THROW 51000, 'Expected a non-dbo default schema.', 1;
            """);
        for (var run = 0; run < 2; run++)
        {
            foreach (var batch in batches.Where(b => !string.IsNullOrWhiteSpace(b)))
                await fixture.Database.ExecuteSqlRawAsync(batch);
        }
        await fixture.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'dbo.SmsLoginChallenges',N'U') IS NULL
                THROW 51001, 'SMS challenge table must be created in dbo.', 1;
            REVERT;
            """);
    }
    finally
    {
        await fixture.Database.CloseConnectionAsync();
    }

    Check(MobileNumber.Normalize("+۹۸ (۹۱۰) ۰۰۰-۰۰۱۱") == adminPhone, "Normalize Persian international phone");
    Check(MobileNumber.Normalize("٠٩١٠٠٠٠٠٠١١") == adminPhone, "Normalize Arabic phone digits");
    Check(MobileNumber.Normalize("0910a000011") is null, "Reject invalid characters");

    await using (var duplicate = new WindowDbContext(options))
    {
        duplicate.Users.Add(CreateUser(adminPhone, legacyPassword, true));
        try
        {
            await duplicate.SaveChangesAsync();
            throw new InvalidOperationException("Expected unique normalized phone constraint.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
        }
    }

    Check(await fixture.Users.AsNoTracking().CountAsync(u =>
        EF.Property<string>(u, MobileLoginKey.Name) == disputedPhone &&
        EF.Property<bool>(u, MobileLoginKey.Excluded) && u.Password == legacyPassword) == 2,
        "Migration preserves and excludes both historical accounts sharing a mobile");
    Check(!((await Send(disputedPhone)).Sent) && sender.Count(disputedPhone) == 0,
        "Do not send a code or provision an account for a disputed mobile");
    await fixture.Users.Where(u => u.Mobile == disputedAlias)
        .ExecuteUpdateAsync(s => s.SetProperty(u => u.Mobile, "09100000017"));
    clock.Advance(TimeSpan.FromMinutes(2));
    Check(!((await Send(disputedPhone)).Sent) && sender.Count(disputedPhone) == 0,
        "A lone account flagged for resolution remains excluded");

    var oldChallenge = await Send(newlyDisputedPhone);
    Check(oldChallenge.Sent, "Start a challenge before a dispute");
    var incoming = CreateUser(newlyDisputedPhone, legacyPassword, true);
    fixture.Users.Add(incoming);
    fixture.Entry(incoming).Property(MobileLoginKey.Excluded).CurrentValue = true;
    await fixture.SaveChangesAsync();
    Check(await Verify(newlyDisputedPhone, oldChallenge, sender.Code(newlyDisputedPhone)) is null,
        "Recheck account eligibility when consuming a code");

    var adminChallenge = await Send(adminPhone);
    Check(adminChallenge.Sent && sender.Code(adminPhone).Length == 6, "Send six digits to normalized legacy account");
    var signedInAdmin = await Verify(adminPhone, adminChallenge, sender.Code(adminPhone));
    Check(signedInAdmin is { IsAdmin: true } && signedInAdmin.Id == admin.Id &&
        signedInAdmin.Password == legacyPassword, "Administrator can sign in and retains the existing password");
    Check(await Verify(adminPhone, adminChallenge, sender.Code(adminPhone)) is null, "Reject OTP replay");

    foreach (var ineligible in new[] { blockedPhone, deletedPhone })
    {
        var attempt = await Send(ineligible);
        Check(!attempt.Sent && sender.Count(ineligible) == 0, "Ineligible accounts must not receive a code");
    }

    var unverified = await Send(unverifiedPhone);
    var claimed = await Verify(unverifiedPhone, unverified, sender.Code(unverifiedPhone));
    Check(claimed is { IsMobileConfirm: true, Password: "!sms-only", ExpireMobileSMSDateTime: null } &&
        claimed.MobileActivationCode != "123456", "An unverified account loses its unproven credential");

    const string freshPhone = "09100000021";
    var sends = await Task.WhenAll(Send(freshPhone), Send(freshPhone));
    Check(sends.Count(s => s.Sent) == 1 && sender.Count(freshPhone) == 1, "Parallel requests send at most one code");
    var fresh = sends.Single(s => s.Sent);
    Check(!await fixture.Users.AsNoTracking().AnyAsync(u => u.Mobile == freshPhone), "Do not create account before proof");
    var freshCode = sender.Code(freshPhone);
    Check(await InContext(s => s.VerifyAsync(freshPhone, Guid.NewGuid(), freshCode, default)) is null, "Reject another browser challenge");
    var verifications = await Task.WhenAll(Verify(freshPhone, fresh, freshCode), Verify(freshPhone, fresh, freshCode));
    Check(verifications.Count(u => u is not null) == 1, "Consume a code once under parallel verification");
    Check(verifications.Single(u => u is not null) is { IsMobileConfirm: true, IsAdmin: false, Password: "!sms-only" } &&
        await fixture.Users.AsNoTracking().CountAsync(u => u.Mobile == freshPhone) == 1, "Create exactly one verified SMS-only account");

    const string retryPhone = "09100000022";
    var first = await Send(retryPhone);
    var originalCode = sender.Code(retryPhone);
    var wrong = DifferentFrom(originalCode);
    await Task.WhenAll(Enumerable.Range(0, 4).Select(_ => Verify(retryPhone, first, wrong)));
    Check(!((await Send(retryPhone, first.ChallengeId)).Sent), "Block early resend at the server");
    clock.Advance(TimeSpan.FromMinutes(2));
    Check(await Verify(retryPhone, first, sender.Code(retryPhone)) is null, "Reject expired codes");
    var second = await Send(retryPhone, first.ChallengeId);
    Check(second.Sent && second.ChallengeId != first.ChallengeId && sender.Count(retryPhone) == 2 &&
        sender.Code(retryPhone) != originalCode, "Resend creates a fresh challenge and code");
    await Verify(retryPhone, second, DifferentFrom(sender.Code(retryPhone)));
    Check(await Verify(retryPhone, second, sender.Code(retryPhone)) is null, "Resend preserves the five-guess limit");
    Check((await fixture.SmsLoginChallenges.AsNoTracking().SingleAsync(c => c.Mobile == retryPhone)).FailedAttempts == 5,
        "Parallel guesses are counted atomically");

    const string failedPhone = "09100000023";
    sender.RejectedMobile = failedPhone;
    var failure = await Send(failedPhone);
    Check(!failure.Sent && await Verify(failedPhone, failure, sender.Code(failedPhone)) is null &&
        !await fixture.Users.AsNoTracking().AnyAsync(u => u.Mobile == failedPhone), "Provider rejection must not authorize the code");
    sender.RejectedMobile = null;

    const string capPhone = "09100000024";
    var cap = await Send(capPhone);
    for (var i = 0; i < 2; i++)
    {
        clock.Advance(TimeSpan.FromMinutes(2));
        cap = await Send(capPhone, cap.ChallengeId);
        Check(cap.Sent, "Allow sends within the per-number budget");
    }
    clock.Advance(TimeSpan.FromMinutes(10));
    cap = await Send(capPhone, cap.ChallengeId);
    Check(cap.Sent, "Allow the fourth send in the rate window");
    clock.Advance(TimeSpan.FromMinutes(1));
    Check(!(await Send(capPhone, cap.ChallengeId)).Sent, "A window reset cannot shorten the current cooldown");
    clock.Advance(TimeSpan.FromMinutes(1));
    Check((await Send(capPhone, cap.ChallengeId)).Sent, "Allow resend once both limits expire");

    await using (var recoveryDb = new WindowDbContext(options))
    {
        var account = await recoveryDb.Users.SingleAsync(u => u.Mobile == freshPhone);
        account.MobileActivationCode = "543210";
        account.ExpireMobileSMSDateTime = DateTime.Now.AddMinutes(-3);
        await recoveryDb.SaveChangesAsync();

        var service = new UserService(new ConfigurationBuilder().Build(), new UserRepository(recoveryDb),
            null!, recoveryDb, null!, null!, sender);
        var reset = new ResetPasswordViewModel
        {
            ActiveCode = "۵۴۳۲۱۰", Password = "new-fixture-password", ConfirmPassword = "new-fixture-password"
        };
        Check(await service.ResetUserPassword(reset, freshPhone) == ResetPasswordResult.WrongActiveCode, "Reject an expired recovery code");
        account.ExpireMobileSMSDateTime = DateTime.Now;
        await recoveryDb.SaveChangesAsync();
        Check(await service.ResetUserPassword(reset, freshPhone) == ResetPasswordResult.Success, "An SMS-only account can set a password");
        Check(await service.ResetUserPassword(reset, freshPhone) == ResetPasswordResult.WrongActiveCode, "Consume the recovery code once");
        Check(await recoveryDb.Users.AsNoTracking().AnyAsync(u => u.Mobile == freshPhone &&
            u.Password == PasswordHelper.EncodePasswordMd5(reset.Password)), "Store the existing password hash format");
    }

    await HttpChecks.RunAsync(options, sender, clock);
    Console.WriteLine("PASS: SQL migration, legacy normalization, disputed mobiles, admin and ordinary SMS sign-in, replay, concurrency, cooldown, failures and HTTP flow.");
}
finally
{
    await fixture.Database.EnsureDeletedAsync();
}

Task<SmsLoginSendResult> Send(string mobile, Guid? previous = null) =>
    InContext(s => s.SendAsync(mobile, previous, default));

Task<User?> Verify(string mobile, SmsLoginSendResult challenge, string code) =>
    InContext(s => s.VerifyAsync(mobile, challenge.ChallengeId!.Value, code, default));

async Task<T> InContext<T>(Func<SmsLoginService, Task<T>> action)
{
    await using var db = new WindowDbContext(options);
    return await action(new SmsLoginService(db, sender, protector, clock));
}

static User CreateUser(string mobile, string password, bool confirmed) => new()
{
    Mobile = mobile,
    Username = mobile,
    Password = password,
    IsMobileConfirm = confirmed,
    MobileActivationCode = "123456",
    EmailActivationCode = Guid.NewGuid().ToString("N"),
    ExpireMobileSMSDateTime = DateTime.Now
};

static string DifferentFrom(string code) => code == "000000" ? "111111" : "000000";

static void Check(bool result, string description)
{
    if (!result)
        throw new InvalidOperationException(description);
}

sealed class TestClock : TimeProvider
{
    private DateTimeOffset _now = DateTimeOffset.UtcNow;
    public override DateTimeOffset GetUtcNow() => _now;
    public void Advance(TimeSpan duration) => _now += duration;
}

sealed class OfflineSms : ISMSService
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _messages = new();
    public string? RejectedMobile { get; set; }

    public string Code(string mobile) => _messages[mobile].Last();
    public int Count(string mobile) => _messages.TryGetValue(mobile, out var codes) ? codes.Count : 0;

    public Task<SendResult?> SendLookupSMS(string receptor, string token, string? token2, string? token3, string template)
    {
        if (template != "Register" || token.Length != 6 || !token.All(char.IsAsciiDigit))
            throw new InvalidOperationException("Unexpected provider payload.");

        _messages.GetOrAdd(receptor, _ => new()).Enqueue(token);
        return Task.FromResult<SendResult?>(receptor == RejectedMobile ? null : new SendResult { Messageid = 1, Status = 1 });
    }

    public Task<SendResult?> SendSimpleSMS(string receptor, string message) => throw new NotSupportedException();
}
