namespace RhoMicro.ApplicationFramework.Aspects;
using System.Collections.Concurrent;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

using Timer = System.Timers.Timer;

/// <summary>
/// Default implementation of <see cref="ICache{TKey, TValue}"/>. Values may be cached for a maximum of <see cref="Int32.MaxValue"/>.
/// </summary>
/// <typeparam name="TKey">The type of key used to retrieve values.</typeparam>
/// <typeparam name="TValue">The type of value retrieved.</typeparam>
public sealed partial class AspectCache<TKey, TValue> : ICache<TKey, TValue>, IDisposable
    where TKey : notnull
{
    private readonly ICacheOptions<TKey> _options;
    private readonly ConcurrentDictionary<TKey, CacheEntry> _cache;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="options">The options defining caching behavior.</param>
    public AspectCache(ICacheOptions<TKey> options)
    {
        _options = options;
        _cache = new(_options.KeyComparer);
    }
    private CacheEntry CreateEntry(TKey key, Func<TKey, TValue> valueFactory)
    {
        var lifespanMillis = Math.Clamp(_options.Lifespan.TotalMilliseconds, 0, Int32.MaxValue);
        var timer = new Timer(lifespanMillis);
        timer.Elapsed += (s, e) =>
        {
            if(_cache.Remove(key, out var removed))
            {
                removed.Dispose();
            }
        };
        var value = valueFactory.Invoke(key);
        var entry = new CacheEntry(value, timer);

        timer.Start();

        return entry;
    }
    /// <inheritdoc/>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        if(_options.Lifespan == TimeSpan.Zero)
        {
            return valueFactory.Invoke(key);
        }

        var entry = _cache.GetOrAdd(key, k => CreateEntry(k, valueFactory));
        var result = entry.Value;

        return result;
    }
    /// <inheritdoc/>
    public void Dispose()
    {
        while(!_cache.IsEmpty)
        {
            var keyToRemove = _cache.Keys.First();
            if(_cache.TryRemove(keyToRemove, out var removed))
            {
                removed.Dispose();
            }
        }
    }
}
