namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for cache misses.
/// </summary>
/// <typeparam name="TRequest">The type of request whose cache was missed.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="level">The level at which the cache miss is to be logged.</param>
public sealed class ResultCacheMissLogEntry<TRequest>(LogLevel level = LogLevel.Information) : ILogEntry
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;
    /// <inheritdoc/>
    public String Evaluate() => $"Cache miss for {typeof(TRequest).Name}";
}
