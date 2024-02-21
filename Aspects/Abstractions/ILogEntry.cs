namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a log entry.
/// </summary>
public interface ILogEntry
{
    /// <summary>
    /// Gets the log leval at which the log is to be logged.
    /// </summary>
    LogLevel Level { get; }
    /// <summary>
    /// Evaluates the log entry, yielding its actual message for logging.
    /// </summary>
    /// <returns></returns>
    String Evaluate();
}
