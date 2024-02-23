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
public sealed class HttpContextConnectionIdLogEntry(String connectionId, LogLevel level = LogLevel.Information) : ILogEntry
{
    /// <inheritdoc/>
    public LogLevel Level { get; } = level;
    /// <inheritdoc/>
    public String Evaluate() => $"Connection id: {connectionId}";
}
