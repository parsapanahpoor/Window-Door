using Window.Domain.Entities.Notification;

namespace Window.Application.Services.Interfaces;

public enum SmsNotificationEnqueueResult
{
    Queued,
    Duplicate,
    InvalidRecipient,
    Disabled
}

public interface ISmsNotificationQueue
{
    /// <summary>
    /// Adds an outbox row to the caller's unit of work. It never saves, sends or commits.
    /// The caller must call SaveChanges/Commit for the row to persist or roll back with it.
    /// </summary>
    Task<SmsNotificationEnqueueResult> EnqueueAsync(
        SmsNotificationKind kind,
        string eventKey,
        string recipientMobile,
        string? token1 = null,
        string? token2 = null,
        string? token3 = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Claims and processes a bounded batch. Each row is saved independently. Safe for
    /// multiple concurrent workers; a row is only delivered once per successful send.
    /// </summary>
    Task<int> DispatchAsync(string workerId, int batchSize = 20, CancellationToken cancellationToken = default);
}
