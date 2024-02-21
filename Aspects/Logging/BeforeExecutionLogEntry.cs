namespace RhoMicro.ApplicationFramework.Aspects.Logging;
using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Log entry for logging executions (command or query) that are about to commence.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="scope">The scope executed.</param>
/// <param name="formatter">The formatter used to format the scope.</param>
/// <param name="level">The level at which the execution is to be logged.</param>
public readonly struct BeforeExecutionLogEntry<TScope>(
    TScope scope,
    IStaticFormatter<TScope> formatter,
    LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<BeforeExecutionLogEntry<TScope>>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <inheritdoc/>
    public String Evaluate() => $"Execute {typeof(TScope).Name}: {formatter.Format(scope)}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(BeforeExecutionLogEntry<TScope> left, BeforeExecutionLogEntry<TScope> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(BeforeExecutionLogEntry<TScope> left, BeforeExecutionLogEntry<TScope> right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(BeforeExecutionLogEntry<TScope> other) => throw new NotImplementedException();
}
