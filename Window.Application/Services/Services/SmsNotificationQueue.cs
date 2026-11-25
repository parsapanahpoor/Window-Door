using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Notification;

namespace Window.Application.Services.Services;

public sealed class SmsNotificationQueue(
    WindowDbContext context,
    ISMSService sms,
    IConfiguration configuration,
    TimeProvider timeProvider,
    ILogger<SmsNotificationQueue> logger) : ISmsNotificationQueue
{
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LeaseDuration = TimeSpan.FromMinutes(2);

    public async Task<SmsNotificationEnqueueResult> EnqueueAsync(
        SmsNotificationKind kind,
        string eventKey,
        string recipientMobile,
        string? token1 = null,
        string? token2 = null,
        string? token3 = null,
        CancellationToken cancellationToken = default)
    {
        var mobile = MobileNumber.Normalize(recipientMobile);
        if (mobile is null || string.IsNullOrWhiteSpace(eventKey))
            return SmsNotificationEnqueueResult.InvalidRecipient;

        var duplicate = await context.SmsNotificationOutbox
            .AsNoTracking()
            .AnyAsync(row => row.EventKey == eventKey && row.RecipientMobile == mobile && !row.IsDelete, cancellationToken);

        // The caller may enqueue several recipients in one unit of work before saving.
        var pendingDuplicate = context.SmsNotificationOutbox.Local
            .Any(row => !row.IsDelete && row.EventKey == eventKey && row.RecipientMobile == mobile);

        if (duplicate || pendingDuplicate)
            return SmsNotificationEnqueueResult.Duplicate;

        var (enabled, template) = ReadSettings(kind);
        var now = timeProvider.GetUtcNow();

        context.SmsNotificationOutbox.Add(new SmsNotificationOutbox
        {
            Kind = kind,
            EventKey = eventKey,
            RecipientMobile = mobile,
            Token1 = token1,
            Token2 = token2,
            Token3 = token3,
            Status = enabled && template.Length > 0 ? SmsNotificationStatus.Pending : SmsNotificationStatus.Disabled,
            Attempts = 0,
            NextAttemptAtUtc = now,
            CreateDate = DateTime.Now,
            IsDelete = false
        });

        return enabled && template.Length > 0
            ? SmsNotificationEnqueueResult.Queued
            : SmsNotificationEnqueueResult.Disabled;
    }

    public async Task<int> DispatchAsync(string workerId, int batchSize = 20, CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0) return 0;

        var now = timeProvider.GetUtcNow();
        var candidateIds = await context.SmsNotificationOutbox
            .AsNoTracking()
            .Where(row => !row.IsDelete
                && row.NextAttemptAtUtc <= now
                && ((row.Status == SmsNotificationStatus.Pending
                        && (row.LeaseUntilUtc == null || row.LeaseUntilUtc < now))
                    || (row.Status == SmsNotificationStatus.Sending
                        && row.LeaseUntilUtc != null && row.LeaseUntilUtc < now)))
            .OrderBy(row => row.Id)
            .Select(row => row.Id)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        var claimToken = Guid.NewGuid();
        var claimedIds = new List<ulong>();
        foreach (var id in candidateIds)
        {
            var claimed = await context.SmsNotificationOutbox
                .Where(row => row.Id == id
                    && ((row.Status == SmsNotificationStatus.Pending
                            && (row.LeaseUntilUtc == null || row.LeaseUntilUtc < now))
                        || (row.Status == SmsNotificationStatus.Sending
                            && row.LeaseUntilUtc != null && row.LeaseUntilUtc < now)))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(row => row.ClaimToken, claimToken)
                    .SetProperty(row => row.LeaseUntilUtc, now + LeaseDuration)
                    .SetProperty(row => row.Status, SmsNotificationStatus.Sending), cancellationToken);

            if (claimed == 1) claimedIds.Add(id);
        }

        if (claimedIds.Count == 0) return 0;

        // Claims changed rows outside the tracker; drop stale tracked copies before processing.
        context.ChangeTracker.Clear();

        var rows = await context.SmsNotificationOutbox
            .Where(row => row.ClaimToken == claimToken)
            .ToListAsync(cancellationToken);

        var sent = 0;
        foreach (var row in rows)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var (enabled, template) = ReadSettings(row.Kind);
            if (!enabled || template.Length == 0)
            {
                row.Status = SmsNotificationStatus.Disabled;
                ClearClaim(row);
                await context.SaveChangesAsync(cancellationToken);
                continue;
            }

            try
            {
                var result = await sms.SendLookupSMS(row.RecipientMobile, row.Token1 ?? string.Empty, row.Token2, row.Token3, template);
                if (result is null)
                {
                    RecordFailure(row);
                }
                else
                {
                    row.Status = SmsNotificationStatus.Sent;
                    row.ProviderMessageId = result.Messageid.ToString();
                    row.ProviderStatus = result.Status.ToString();
                    row.SentAtUtc = timeProvider.GetUtcNow();
                    row.LastError = null;
                    ClearClaim(row);
                    sent++;
                }
            }
            catch (Exception ex)
            {
                RecordFailure(row, ex.GetType().Name);
            }

            // Persist each recipient independently so one failure cannot hide another success.
            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("SMS outbox {EventId} kind {Kind} finished with {Status} after {Attempts} attempts (worker {Worker})",
                row.Id, row.Kind, row.Status, row.Attempts, workerId);
        }

        return sent;
    }

    private void RecordFailure(SmsNotificationOutbox row, string? reason = null)
    {
        row.Attempts++;
        row.LastError = reason ?? "provider rejected the message";
        ClearClaim(row);

        if (row.Attempts >= MaxAttempts)
        {
            row.Status = SmsNotificationStatus.Failed;
            return;
        }

        row.Status = SmsNotificationStatus.Pending;
        var delayMinutes = Math.Min(60, Math.Pow(2, row.Attempts));
        row.NextAttemptAtUtc = timeProvider.GetUtcNow().AddMinutes(delayMinutes);
    }

    private static void ClearClaim(SmsNotificationOutbox row)
    {
        row.ClaimToken = null;
        row.LeaseUntilUtc = null;
    }

    private (bool Enabled, string Template) ReadSettings(SmsNotificationKind kind)
    {
        var section = configuration.GetSection($"SmsNotifications:{kind}");
        var enabled = bool.TryParse(section["Enabled"], out var value) && value;
        var template = section["Template"]?.Trim() ?? string.Empty;
        return (enabled, template);
    }
}
