namespace RhoMicro.ApplicationFramework.Aspects.Tests;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Factory mock used to assert the functionality of caches.
/// </summary>
/// <typeparam name="TKey">The type of key for which values will be retrieved.</typeparam>
/// <typeparam name="TValue">The type of values to be retrieved.</typeparam>
public sealed class ValueFactory<TKey, TValue>
{
    /// <summary>
    /// Gets or sets the value to return when calling <see cref="CreateValue(TKey)"/>.
    /// </summary>
    public required TValue Value { get; set; }
    /// <summary>
    /// Gets a value indicating whether <see cref="CreateValue(TKey)"/> has been called.
    /// </summary>
    public Boolean FactoryWasCalled { get; private set; }
    /// <summary>
    /// Gets the key last passed to <see cref="CreateValue(TKey)"/>.
    /// </summary>
    public TKey? KeyPassed { get; private set; }
    /// <summary>
    /// Resets <see cref="FactoryWasCalled"/> and <see cref="KeyPassed"/>.
    /// </summary>
    public void Reset()
    {
        FactoryWasCalled = false;
        KeyPassed = default;
    }
    /// <summary>
    /// Factory method stub for <see cref="ICache{TKey, TValue}.GetOrAdd(TKey, Func{TKey, TValue})"/> parameter.
    /// </summary>
    /// <param name="key">The key to be assigned to <see cref="KeyPassed"/>.</param>
    /// <returns>The value returned by <see cref="Value"/>.</returns>
    public TValue CreateValue(TKey key)
    {
        FactoryWasCalled = true;
        KeyPassed = key;
        return Value;
    }
}