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
public sealed class ServiceTypeLogEntry(
    Type serviceType,
    LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Service Type: {serviceType.Name}";
}