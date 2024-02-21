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
public readonly struct AfterCommandExecutionLogEntry<TCommand>(LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<AfterCommandExecutionLogEntry<TCommand>>
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;

    /// <inheritdoc/>
    public String Evaluate() => $"Executed {typeof(TCommand).Name}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(AfterCommandExecutionLogEntry<TCommand> left, AfterCommandExecutionLogEntry<TCommand> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(AfterCommandExecutionLogEntry<TCommand> left, AfterCommandExecutionLogEntry<TCommand> right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(AfterCommandExecutionLogEntry<TCommand> other) => throw new NotImplementedException();
}
