namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for logging thread ids.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="threadId">The current thread id.</param>
/// <param name="level">The level at which the thread id is to be logged.</param>
public sealed class ThreadIdLogEntry(
    Int32 threadId,
    LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <summary>
    /// Gets a log entry that has been initialized with the current thread id.
    /// </summary>
    public static ThreadIdLogEntry Current => new(Environment.CurrentManagedThreadId);
    /// <inheritdoc/>
    public String Evaluate() => $"ThreadId: {threadId}";
}
