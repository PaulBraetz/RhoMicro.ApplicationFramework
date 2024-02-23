namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Command for logging execution times.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="executionTimeTicks">The execution time measured.</param>
/// <param name="level">The level at which the execution time is to be logged.</param>
public sealed class ExecutionTimeLogEntry<TScope>(
    Int64 executionTimeTicks,
    LogLevel level = LogLevel.Information) : ILogEntry
{
    private readonly Lazy<String> _evaluation = new(() => Format(executionTimeTicks));

    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    private static readonly Int64 _microSecondTicks = TimeSpan.FromMilliseconds(0.001).Ticks;
    private static readonly Int64 _milliSecondTicks = TimeSpan.FromMilliseconds(1).Ticks;
    private static readonly Int64 _secondTicks = TimeSpan.FromSeconds(1).Ticks;
    private static readonly Int64 _minuteTicks = TimeSpan.FromMinutes(1).Ticks;

    /// <inheritdoc/>
    public String Evaluate() => _evaluation.Value;
    private static String Format(Int64 executionTimeTicks)
    {
        var executionTimeMicros = executionTimeTicks / _microSecondTicks;
        var executionTimeFormatted =
            executionTimeTicks >= _minuteTicks ?
            $"{executionTimeMicros / 60e6:F1}m" :
            executionTimeTicks >= _secondTicks ?
            $"{executionTimeMicros / 1e6:F1}s" :
            executionTimeTicks >= _milliSecondTicks ?
            $"{executionTimeMicros / 1e3:F1}ms" :
            $"{executionTimeMicros}μs";

        return $"Done with {typeof(TScope).Name}: {executionTimeFormatted}";
    }
}
