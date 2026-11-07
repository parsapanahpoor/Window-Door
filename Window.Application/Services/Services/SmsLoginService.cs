using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;

namespace Window.Application.Services;

public sealed class SmsLoginService(
    WindowDbContext db,
    ISMSService sms,
    IDataProtectionProvider dataProtection,
    TimeProvider timeProvider)
{
    public const string AuthenticationMethod = "sms";
    private const string NoPassword = "!sms-only";
    private const int MaxSends = 4;
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan RateWindow = TimeSpan.FromMinutes(15);
    private readonly IDataProtector _protector = dataProtection.CreateProtector("Window.SmsLogin.Code.v1");

    public async Task<SmsLoginSendResult> SendAsync(string mobile, Guid? previous, CancellationToken cancellationToken)
    {
        if (MobileNumber.Normalize(mobile) != mobile)
            return new(null, false);

        var (account, excluded) = await FindAccountAsync(mobile, cancellationToken);
        if (excluded || account is { IsDelete: true } or { IsBan: true })
            return new(null, false, RequiresPassword: true);

        var now = timeProvider.GetUtcNow();
        var challenge = await db.SmsLoginChallenges.FindAsync([mobile], cancellationToken);
        if (previous.HasValue && challenge?.ChallengeId != previous.Value)
            return new(null, false);

        if (challenge is not null && RetryAt(challenge) > now)
            return new(challenge.ChallengeId == previous ? previous : null, false);

        if (challenge is null)
        {
            challenge = new SmsLoginChallenge { Mobile = mobile };
            db.SmsLoginChallenges.Add(challenge);
        }

        if (challenge.WindowStartedAt + RateWindow <= now)
        {
            challenge.WindowStartedAt = now;
            challenge.SendCount = 0;
            challenge.FailedAttempts = 0;
        }

        var previousCode = challenge.ProtectedCode;
        challenge.ChallengeId = Guid.NewGuid();
        challenge.ProtectedCode = null;
        challenge.ExpiresAt = now;
        challenge.NextSendAt = now + CodeLifetime;
        challenge.SendCount++;

        if (!await TrySaveAsync(cancellationToken))
            return new(null, false);


        string code;
        do
        {
            code = RandomNumberGenerator.GetInt32(1_000_000).ToString("D6", CultureInfo.InvariantCulture);
        } while (previousCode is not null && CodeMatches(previousCode, code));
        if (await sms.SendLookupSMS(mobile, code, null, null, "Register") is null)
            return new(challenge.ChallengeId, false);

        // Preserve a code already accepted by the provider if the browser disconnects.
        now = timeProvider.GetUtcNow();
        challenge.ExpiresAt = now + CodeLifetime;
        challenge.NextSendAt = challenge.ExpiresAt;
        challenge.ProtectedCode = _protector.Protect(code);

        return new(challenge.ChallengeId, await TrySaveAsync(CancellationToken.None));
    }

    public async Task<int> SecondsUntilResendAsync(string mobile, Guid id, CancellationToken cancellationToken)
    {
        var challenge = await db.SmsLoginChallenges.AsNoTracking()
            .SingleOrDefaultAsync(c => c.Mobile == mobile && c.ChallengeId == id, cancellationToken);
        return challenge is null ? 0 : Math.Max(0, (int)Math.Ceiling((RetryAt(challenge) - timeProvider.GetUtcNow()).TotalSeconds));
    }

    public async Task<SmsLoginVerificationResult> VerifyAsync(string mobile, Guid id, string code, CancellationToken cancellationToken)
    {
        if (code.Length != 6 || !code.All(char.IsAsciiDigit))
            return default;

        await using var transaction = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, cancellationToken);
        var now = timeProvider.GetUtcNow();
        var pending = db.SmsLoginChallenges.Where(c => c.Mobile == mobile && c.ChallengeId == id
            && c.ProtectedCode != null && c.ExpiresAt > now && c.FailedAttempts < MaxFailedAttempts);

        // Charge a guess and lock the challenge before comparing the secret.
        if (await pending.ExecuteUpdateAsync(s => s.SetProperty(c => c.FailedAttempts, c => c.FailedAttempts + 1), cancellationToken) == 0)
            return default;

        var challenge = await db.SmsLoginChallenges.SingleAsync(c => c.Mobile == mobile, cancellationToken);
        if (challenge.ExpiresAt <= timeProvider.GetUtcNow() || !CodeMatches(challenge.ProtectedCode!, code))
        {
            await transaction.CommitAsync(CancellationToken.None);
            return default;
        }

        var (user, excluded) = await FindAccountAsync(mobile, cancellationToken);
        if (excluded || user is { IsDelete: true } or { IsBan: true })
            return new(null, RequiresPassword: true);
        if (user is null)
        {
            user = new User
            {
                Mobile = mobile,
                Username = mobile,
                EmailActivationCode = Guid.NewGuid().ToString("N")
            };
            db.Users.Add(user);

            var campaign = await db.AllBulkSMSs.FirstOrDefaultAsync(p => !p.IsDelete && p.Mobile == mobile, cancellationToken);
            if (campaign is not null)
                campaign.IsUserRegistered = true;
        }

        if (!user.IsMobileConfirm)
        {
            // A password picked before phone ownership was proven cannot become a valid login.
            user.Password = NoPassword;
            user.IsMobileConfirm = true;
            user.MobileActivationCode = Guid.NewGuid().ToString("N");
            user.ExpireMobileSMSDateTime = null;
        }

        challenge.ProtectedCode = null;
        challenge.FailedAttempts = 0;

        if (!await TrySaveAsync(cancellationToken))
            return default;

        await transaction.CommitAsync(cancellationToken);
        return new(user);
    }

    public Task<bool> CanUseSessionAsync(ulong userId, CancellationToken cancellationToken) =>
        db.Users.AnyAsync(u => u.Id == userId && !u.IsDelete && !u.IsBan, cancellationToken);

    private async Task<(User? Account, bool Excluded)> FindAccountAsync(string mobile, CancellationToken cancellationToken)
    {
        var matches = await db.Users
            .Where(u => EF.Property<string>(u, MobileLoginKey.Name) == mobile)
            .Select(u => new { Account = u, Excluded = EF.Property<bool>(u, MobileLoginKey.Excluded) })
            .Take(2)
            .ToListAsync(cancellationToken);

        if (matches.Count > 1 || matches.Count == 1 && matches[0].Excluded)
            return (null, true);

        return (matches.SingleOrDefault()?.Account, false);
    }

    private static DateTimeOffset RetryAt(SmsLoginChallenge challenge)
    {
        var windowEnd = challenge.WindowStartedAt + RateWindow;
        var limitReached = challenge.SendCount >= MaxSends || challenge.FailedAttempts >= MaxFailedAttempts;
        return limitReached && windowEnd > challenge.NextSendAt ? windowEnd : challenge.NextSendAt;
    }

    private bool CodeMatches(string protectedCode, string code)
    {
        try
        {
            return CryptographicOperations.FixedTimeEquals(
                Encoding.ASCII.GetBytes(_protector.Unprotect(protectedCode)), Encoding.ASCII.GetBytes(code));
        }
        catch (CryptographicException)
        {
            return false;
        }
    }

    private async Task<bool> TrySaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return false;
        }
    }


}

public sealed record SmsLoginSendResult(Guid? ChallengeId, bool Sent, bool RequiresPassword = false);

public readonly record struct SmsLoginVerificationResult(User? Account, bool RequiresPassword = false);
