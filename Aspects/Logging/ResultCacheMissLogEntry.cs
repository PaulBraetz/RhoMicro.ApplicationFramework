namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Log entry for cache misses.
/// </summary>
/// <typeparam name="TRequest">The type of query whose cache was missed.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="level">The level at which the cache miss is to be logged.</param>
public readonly struct ResultCacheMissLogEntry<TRequest>(LogLevel level = LogLevel.Information) : ILogEntry, IEquatable<ResultCacheMissLogEntry<TRequest>>
{
    private readonly LogLevel? _level = level;
    /// <inheritdoc/>
    public LogLevel Level => _level ?? LogLevel.Information;
    /// <inheritdoc/>
    public String Evaluate() => $"Cache miss for {typeof(TRequest).Name}";
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw new NotImplementedException();
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw new NotImplementedException();
    /// <inheritdoc/>
    public static Boolean operator ==(ResultCacheMissLogEntry<TRequest> left, ResultCacheMissLogEntry<TRequest> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(ResultCacheMissLogEntry<TRequest> left, ResultCacheMissLogEntry<TRequest> right) => !( left == right );
    /// <inheritdoc/>
    public Boolean Equals(ResultCacheMissLogEntry<TRequest> other) => throw new NotImplementedException();
}
