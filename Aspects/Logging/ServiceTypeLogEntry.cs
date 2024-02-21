namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for logging a services type.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="serviceType">The type of service to log.</param>
/// <param name="level">The level at which the execution is to be logged.</param>
public readonly struct ServiceTypeLogEntry(
    Type serviceType,
    LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<ServiceTypeLogEntry>
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;

    /// <inheritdoc/>
    public String Evaluate() => $"Service Type: {serviceType.Name}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(ServiceTypeLogEntry left, ServiceTypeLogEntry right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(ServiceTypeLogEntry left, ServiceTypeLogEntry right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(ServiceTypeLogEntry other) => throw new NotImplementedException();
}