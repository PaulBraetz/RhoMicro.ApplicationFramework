namespace RhoMicro.ApplicationFramework.Aspects.Logging;
using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Log entry for logging executions (command or request) that are about to commence.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="scope">The scope executed.</param>
/// <param name="formatter">The formatter used to format the scope.</param>
/// <param name="level">The level at which the execution is to be logged.</param>
public sealed class BeforeExecutionLogEntry<TScope>(
    TScope scope,
    IStaticFormatter<TScope> formatter,
    LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Execute {typeof(TScope).Name}: {formatter.Format(scope)}";
}
