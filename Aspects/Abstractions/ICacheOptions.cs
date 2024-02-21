namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Represents options for configuring the caching behaviour.
/// </summary>
public interface ICacheOptions<TKey>
{
    /// <summary>
    /// Gets the lifespan of cached results.
    /// </summary>
    TimeSpan Lifespan { get; }
    /// <summary>
    /// Gets the comparer used to compare keys.
    /// </summary>
    IEqualityComparer<TKey> KeyComparer { get; }
}