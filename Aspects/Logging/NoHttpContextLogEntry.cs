namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for not executing in the context of a http request.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="level">The level at which the connection id is to be logged.</param>
public readonly struct NoHttpContextLogEntry(LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<NoHttpContextLogEntry>
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;
    /// <inheritdoc/>
    public String Evaluate() => "No http context";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(NoHttpContextLogEntry left, NoHttpContextLogEntry right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(NoHttpContextLogEntry left, NoHttpContextLogEntry right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(NoHttpContextLogEntry other) => throw new NotImplementedException();
}
