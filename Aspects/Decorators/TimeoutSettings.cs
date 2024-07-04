namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
/// <summary>
/// Default implementation of <see cref="ITimeoutSettings"/>.
/// </summary>
/// <param name="category">The request category to provide timeout settings for.</param>
/// <param name="timeout">The time to wait before requesting execution of a request to be cancelled.</param>
public class TimeoutSettings(String category, TimeSpan timeout) : ITimeoutSettings
{
    /// <inheritdoc/>
    public String Category { get; set; } = category;
    /// <inheritdoc/>
    public TimeSpan Timeout { get; set; } = timeout;
    /// <summary>
    /// Initializes a new instance with the timeout provided.
    /// </summary>
    /// <param name="timeout">The time to wait before requesting execution of a request to be cancelled.</param>
    public static TimeoutSettings<T> Create<T>(TimeSpan timeout)
    {
        var result = new TimeoutSettings<T>(timeout);

        return result;
    }
}

/// <summary>
/// Default implementation of <see cref="ITimeoutSettings{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of 
/// </typeparam>
public sealed class TimeoutSettings<T> : TimeoutSettings, ITimeoutSettings<T>
{
    /// <summary>
    /// Initializes a new instance based on the timeout provided by <paramref name="provider"/> 
    /// for the fully qualified name of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="provider">The provider to query for a timeout.</param>
    public TimeoutSettings(ITimeoutProvider provider)
        : base(GetCategory(), ( provider ?? throw new ArgumentNullException(nameof(provider)) ).GetTimeoutFor(GetCategory()))
    { }
    internal TimeoutSettings(TimeSpan timeout)
        : base(GetCategory(), timeout) { }
    private static String GetCategory() => typeof(T).FullName ?? String.Empty;
}