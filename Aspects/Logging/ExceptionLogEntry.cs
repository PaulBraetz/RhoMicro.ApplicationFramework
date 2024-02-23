namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Wrapper for logging exceptions.
/// </summary>
/// <remarks>
/// Initializes a new instance with the <see cref="LogLevel.Error"/> level.
/// </remarks>
/// <param name="exception">The exception to be logged.</param>
/// <param name="level">The level at which the entry is to be logged.</param>
public sealed class ExceptionLogEntry<TScope>(
    Exception exception,
    LogLevel level = LogLevel.Error) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Error in {typeof(TScope).Name}: {exception}";
}