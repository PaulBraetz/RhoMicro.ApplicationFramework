namespace RhoMicro.ApplicationFramework.Aspects.Services;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Adapter for logging messages, execution times and exceptions to an instance of <see cref="ILogger"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The logger to use.</param>
public sealed class LoggingService(ILogger logger) : ILoggingService
{
    /// <inheritdoc/>
    public void Log(IEnumerable<ILogEntry> logEntries)
    {
        var logGroups = logEntries
            .Select<ILogEntry, (LogLevel level, Func<String> eval)>(e => (e.Level, e.Evaluate))
            .GroupBy(e => e.level)
            .Select(g => (level: g.Key, evals: g.Select(t => t.eval)))
            .Where(t => logger.IsEnabled(t.level));

        foreach(var (level, evals) in logGroups)
        {
            logger.Log(level, default, evals, null, Format);
        }
    }

    private static String Format(IEnumerable<Func<String>> evals, Exception? exception)
    {
        var evaluated = evals.Select(e => e.Invoke());
        var result = String.Join('\n', evaluated);

        return result;
    }

    /// <inheritdoc/>
    public void Log(ILogEntry logEntry)
    {
        ArgumentNullException.ThrowIfNull(logEntry);

        logger.Log(logEntry.Level, default, logEntry, null, (s, e) => s.Evaluate());
    }
}
