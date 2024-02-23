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
public sealed class NoHttpContextLogEntry(LogLevel level = LogLevel.Information) : ILogEntry
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;
    /// <inheritdoc/>
    public String Evaluate() => "No http context";
}
