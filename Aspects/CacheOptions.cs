namespace RhoMicro.ApplicationFramework.Aspects;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Options for configuring the caching decorator.
/// </summary>
/// <param name="Lifespan">The lifespan of cached results.</param>
/// <param name="KeyComparer">The comparer used to compare keys.</param>
public sealed record CacheOptions<TKey>(
    TimeSpan Lifespan,
    IEqualityComparer<TKey> KeyComparer)
    : ICacheOptions<TKey>;
