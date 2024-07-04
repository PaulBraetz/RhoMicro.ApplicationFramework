namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for a specific time.
/// </summary>
/// <param name="TimeStamp">The timestamp to log.</param>
/// <param name="Timeout">The timeout after which execution will be requested to be cancelled.</param>
/// <param name="Level">The log level at which the log is to be logged.</param>
public sealed record TimeStampLogEntry(
    DateTimeOffset TimeStamp,
    TimeSpan Timeout,
    LogLevel Level = LogLevel.Information) : ILogEntry
{
    /// <summary>
    /// Creates a new instance with a <see cref="TimeStamp"/> of <see cref="DateTimeOffset.UtcNow"/>.
    /// </summary>
    /// <returns>A new instance with the current time.</returns>
    public static TimeStampLogEntry Now(TimeSpan timeout) => new(DateTimeOffset.UtcNow, timeout);
    /// <inheritdoc/>
    public String Evaluate() => $"Timestamp: {TimeStamp}; Timeout: {Timeout}";
}
