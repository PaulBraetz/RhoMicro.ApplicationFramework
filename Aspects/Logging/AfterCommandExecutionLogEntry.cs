namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for logging command executions that have just concluded.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="level">The level at which the execution is to be logged.</param>
public sealed class AfterCommandExecutionLogEntry<TCommand>(LogLevel level = LogLevel.Information) : ILogEntry
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;
    /// <inheritdoc/>
    public String Evaluate() => $"Executed {typeof(TCommand).Name}";
}
