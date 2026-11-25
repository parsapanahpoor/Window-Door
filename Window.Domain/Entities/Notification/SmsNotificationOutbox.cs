using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Notification;

public enum SmsNotificationKind
{
    SellerRegisteredAdmin = 1,
    SellerReviewRequestedAdmin = 2,
    SubscriptionExpiringSeller = 3,
    SubscriptionExpiredSeller = 4
}

public enum SmsNotificationStatus
{
    Pending = 0,
    Sending = 1,
    Sent = 2,
    Failed = 3,
    Disabled = 4
}

/// <summary>
/// Transactional outbox row for a template SMS. Rows are written in the caller's unit of
/// work and sent later by <see cref="Window.Application.Services.Interfaces.ISmsNotificationQueue"/>.
/// </summary>
public class SmsNotificationOutbox : BaseEntity
{
    public SmsNotificationKind Kind { get; set; }

    /// <summary>Unique business key, e.g. SellerRegistered:{marketId}. Combined with the normalized mobile.</summary>
    public string EventKey { get; set; } = string.Empty;

    /// <summary>Normalized Iranian mobile used for deduplication and delivery.</summary>
    public string RecipientMobile { get; set; } = string.Empty;

    public string? Token1 { get; set; }

    public string? Token2 { get; set; }

    public string? Token3 { get; set; }

    public SmsNotificationStatus Status { get; set; } = SmsNotificationStatus.Pending;

    public int Attempts { get; set; }

    public DateTimeOffset NextAttemptAtUtc { get; set; }

    public DateTimeOffset? LeaseUntilUtc { get; set; }

    public Guid? ClaimToken { get; set; }

    public string? ProviderMessageId { get; set; }

    public string? ProviderStatus { get; set; }

    /// <summary>Sanitized error text; never contains the phone, API key or template content.</summary>
    public string? LastError { get; set; }

    public DateTimeOffset? SentAtUtc { get; set; }
}
