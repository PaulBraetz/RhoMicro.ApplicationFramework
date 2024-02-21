namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;
/// <summary>
/// Service for logging multiple log entries.
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// Logs multiple log entries.
    /// </summary>
    /// <param name="logEntries">The log entries to log.</param>
    void Log(IEnumerable<ILogEntry> logEntries);
    /// <summary>
    /// Logs a single log entry.
    /// </summary>
    /// <param name="logEntry">The log entry to log.</param>
    void Log(ILogEntry logEntry);
}
