namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for the current http requests connection id.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="connectionId">The current requests connection id.</param>
/// <param name="level">The level at which the connection id is to be logged.</param>
public readonly struct HttpContextConnectionIdLogEntry(String connectionId, LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<HttpContextConnectionIdLogEntry>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Connection id: {connectionId}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(HttpContextConnectionIdLogEntry left, HttpContextConnectionIdLogEntry right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(HttpContextConnectionIdLogEntry left, HttpContextConnectionIdLogEntry right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(HttpContextConnectionIdLogEntry other) => throw new NotImplementedException();
}
