using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Entities.Account;

public sealed class SmsLoginChallenge
{
    [Key, MaxLength(11)]
    public string Mobile { get; set; } = null!;

    public Guid ChallengeId { get; set; }

    [MaxLength(512)]
    public string? ProtectedCode { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset NextSendAt { get; set; }
    public DateTimeOffset WindowStartedAt { get; set; }
    public int SendCount { get; set; }
    public int FailedAttempts { get; set; }

    [Timestamp]
    public byte[] Version { get; set; } = null!;
}
