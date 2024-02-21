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
public readonly struct ThreadIdLogEntry(
    Int32 threadId,
    LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<ThreadIdLogEntry>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <summary>
    /// Gets a log entry that has been initialized with the current thread id.
    /// </summary>
    public static ThreadIdLogEntry Current => new(Environment.CurrentManagedThreadId);
    /// <inheritdoc/>
    public String Evaluate() => $"ThreadId: {threadId}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(ThreadIdLogEntry left, ThreadIdLogEntry right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(ThreadIdLogEntry left, ThreadIdLogEntry right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(ThreadIdLogEntry other) => throw new NotImplementedException();
}
