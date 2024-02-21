#pragma warning disable CA2000 // Dispose objects before losing scope
namespace RhoMicro.ApplicationFramework.Aspects.Tests;

using RhoMicro.ApplicationFramework.Aspects;
using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Contains tests for <see cref="AspectCache{TKey, TValue}"/>.
/// </summary>

[TestClass]
public class CacheTests
{
    private static Object[][] DataShortLifespan
    {
        get
        {
            return
            [
                [
                    new CacheOptions<String>(
                        TimeSpan.FromMilliseconds(432),
                        EqualityComparer<String>.Default),
                    "Key",
                    new ValueFactory<String, String>()
                    {
                        Value="MyValue"
                    }
                ],
                [
                    new CacheOptions<String>(
                        TimeSpan.FromMilliseconds(25),
                        EqualityComparer<String>.Default),
                    "MyKey",
                    new ValueFactory<String, String>()
                    {
                        Value="SomeMoreValues"
                    }
                ],
                [
                    new CacheOptions<String>(
                        TimeSpan.FromMilliseconds(123),
                        EqualityComparer<String>.Default),
                    "12121212",
                    new ValueFactory<String, String>(){
                        Value="ValueGoesHere"
                    }
                ]
            ];
        }
    }
    private static Object[][] DataInfiniteLifespan
    {
        get
        {
            return DataShortLifespan.Select(ps =>
                new Object[]
                {
                    new CacheOptions<String>(
                        TimeSpan.MaxValue,
                        ((CacheOptions<String>)ps[0]).KeyComparer),
                    ps[1],
                    ps[2]
                }).ToArray();
        }
    }

    /// <summary>
    /// Asserts that the value factory will be called when getting a value for the first time.
    /// </summary>
    /// <param name="options">The options passed to the cache.</param>
    /// <param name="key">The key to request a value for.</param>
    /// <param name="valueFactory">The factory mock to be called by the cache.</param>
    [TestMethod]
    [DynamicData(nameof(DataShortLifespan))]
    public void InvokesFactoryOnFirstCall(ICacheOptions<String> options, String key, ValueFactory<String, String> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        var cache = new AspectCache<String, String>(options);

        _ = cache.GetOrAdd(key, valueFactory.CreateValue);

        Assert.IsTrue(valueFactory.FactoryWasCalled);
    }

    /// <summary>
    /// Asserts that the value factory will not be called when getting a value for the second time (using the same key).
    /// </summary>
    /// <param name="options">The options passed to the cache.</param>
    /// <param name="key">The key to request a value for.</param>
    /// <param name="valueFactory">The factory mock to be called by the cache.</param>
    [TestMethod]
    [DynamicData(nameof(DataInfiniteLifespan))]
    public void DoesNotCallFactoryOnSecondCall(ICacheOptions<String> options, String key, ValueFactory<String, String> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        var cache = new AspectCache<String, String>(options);
        _ = cache.GetOrAdd(key, valueFactory.CreateValue);
        valueFactory.Reset();
        _ = cache.GetOrAdd(key, valueFactory.CreateValue);

        Assert.IsFalse(valueFactory.FactoryWasCalled);
    }

    /// <summary>
    /// Asserts that the value factory will be called when getting a value for the second time (using the same key) after the lifespan has elapsed.
    /// A tolerance of +10ms is afforded to the cache.
    /// </summary>
    /// <param name="options">The options passed to the cache.</param>
    /// <param name="key">The key to request a value for.</param>
    /// <param name="valueFactory">The factory mock to be called by the cache.</param>
    [TestMethod]
    [DynamicData(nameof(DataShortLifespan))]
    public async Task CallsFactoryOnSecondCallAfterLifespan(ICacheOptions<String> options, String key, ValueFactory<String, String> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);
        ArgumentNullException.ThrowIfNull(options);

        var cache = new AspectCache<String, String>(options);
        _ = cache.GetOrAdd(key, valueFactory.CreateValue);

        await Task.Delay(options.Lifespan).ConfigureAwait(continueOnCapturedContext: false);
        await Task.Delay(10).ConfigureAwait(continueOnCapturedContext: false); //grace delay

        valueFactory.Reset();
        _ = cache.GetOrAdd(key, valueFactory.CreateValue);

        Assert.IsTrue(valueFactory.FactoryWasCalled);
    }
}