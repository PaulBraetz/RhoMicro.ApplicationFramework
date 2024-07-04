namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using System.Threading;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Default implementation of <see cref="ITimeoutProvider"/>.
/// If no timeout can be located for a given category, <see cref="Timeout.InfiniteTimeSpan"/> is used instead.
/// </summary>
public sealed class TimeoutProvider : ITimeoutProvider
{
    TimeoutProvider(Dictionary<String, TimeSpan> timeoutMap) => _timeoutMap = timeoutMap;

    private readonly Dictionary<String, TimeSpan> _timeoutMap;

    /// <summary>
    /// Creates a new instance based on configuration-sourced settings.
    /// </summary>
    /// <param name="providerSettings">
    /// The configuration-sourced settings to use.
    /// </param>
    /// <returns>
    /// A new <see cref="TimeoutProvider"/> instance.
    /// </returns>
    public static TimeoutProvider Create(ITimeoutProviderSettings providerSettings)
    {
        ArgumentNullException.ThrowIfNull(providerSettings);

        var timeoutMap = new Dictionary<String, TimeSpan>();

        foreach(var s in providerSettings.TimeoutSettings)
        {
            timeoutMap[s.Category] = s.Timeout;
        }

        var result = new TimeoutProvider(timeoutMap);

        return result;
    }
    /// <inheritdoc/>
    public TimeSpan GetTimeoutFor(String category) =>
        _timeoutMap.TryGetValue(category, out var timeout)
        ? timeout
        : Timeout.InfiniteTimeSpan;
}