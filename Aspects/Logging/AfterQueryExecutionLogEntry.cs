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
public readonly struct AfterQueryExecutionLogEntry<TRequest, TResult>(TResult scope,
                         IStaticFormatter<TResult> formatter,
                         LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<AfterQueryExecutionLogEntry<TRequest, TResult>>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <inheritdoc/>
    public String Evaluate() => $"Executed {typeof(TRequest).Name}: {formatter.Format(scope)}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(AfterQueryExecutionLogEntry<TRequest, TResult> left, AfterQueryExecutionLogEntry<TRequest, TResult> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(AfterQueryExecutionLogEntry<TRequest, TResult> left, AfterQueryExecutionLogEntry<TRequest, TResult> right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(AfterQueryExecutionLogEntry<TRequest, TResult> other) => throw new NotImplementedException();
}
