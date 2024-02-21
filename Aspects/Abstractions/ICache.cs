namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Represents a cache cache of values, to be retrieved using a key.
/// </summary>
/// <typeparam name="TKey">The type of key used to retrieve values.</typeparam>
/// <typeparam name="TValue">The type of value retrieved.</typeparam>
public interface ICache<TKey, TValue>
{
    /// <summary>
    /// Retrieves a value, or creates a ne wone if it does not exist.
    /// </summary>
    /// <param name="key">The key identifying the value.</param>
    /// <param name="valueFactory">The factory used to create and add a value if it is missing.</param>
    /// <returns>
    /// The value associated with the key.
    /// </returns>
    TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
}
