namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Wrapper for logging exceptions.
/// </summary>
/// <remarks>
/// Initializes a new instance with the <see cref="LogLevel.Error"/> level.
/// </remarks>
/// <param name="exception">The exception to be logged.</param>
/// <param name="level">The level at which the entry is to be logged.</param>
public readonly struct ExceptionLogEntry<TScope>(
    Exception exception,
    LogLevel level = LogLevel.Error) : ILogEntry, IEquatable<ExceptionLogEntry<TScope>>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <inheritdoc/>
    public String Evaluate() => $"Error in {typeof(TScope).Name}: {exception}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(ExceptionLogEntry<TScope> left, ExceptionLogEntry<TScope> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(ExceptionLogEntry<TScope> left, ExceptionLogEntry<TScope> right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(ExceptionLogEntry<TScope> other) => throw new NotImplementedException();
}