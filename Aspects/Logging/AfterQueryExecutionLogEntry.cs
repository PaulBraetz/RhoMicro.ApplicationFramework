namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Log entry for logging results of query executions.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="scope">The result of the query execution.</param>
/// <param name="formatter">The formatter used to format the result.</param>
/// <param name="level">The level at which the result is to be logged.</param>
public sealed class AfterQueryExecutionLogEntry<TRequest, TResult>(TResult scope,
                         IStaticFormatter<TResult> formatter,
                         LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Executed {typeof(TRequest).Name}: {formatter.Format(scope)}";
}
