namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;
/// <summary>
/// Provides timeout settings for timeout aspects.
/// </summary>
public interface ITimeoutSettings
{
    /// <summary>
    /// Gets the request category to provide timeout settings for.
    /// </summary>
    String Category { get; }
    /// <summary>
    /// Gets the time to wait before requesting execution of a request to be cancelled.
    /// </summary>
    TimeSpan Timeout { get; }
}
/// <summary>
/// Provides timeout settings for timeout aspects.
/// </summary>
/// <typeparam name="T">The type of request to set cancellation timeouts for.</typeparam>
public interface ITimeoutSettings<T> : ITimeoutSettings;
