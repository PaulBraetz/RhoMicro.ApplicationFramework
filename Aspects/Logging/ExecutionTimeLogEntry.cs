namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Command for logging execution times.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="executionTime">The execution time measured.</param>
/// <param name="level">The level at which the execution time is to be logged.</param>
public sealed class ExecutionTimeLogEntry<TScope>(
    TimeSpan executionTime,
    LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <inheritdoc/>
    public String Evaluate() => $"Done with {typeof(TScope).Name}: {executionTime}";
}
