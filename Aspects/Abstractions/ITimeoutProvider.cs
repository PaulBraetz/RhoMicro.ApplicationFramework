namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Provides the timeout to use when executing a request of a specific category.
/// </summary>
public interface ITimeoutProvider
{
    /// <summary>
    /// Gets the timeout configured for the category specified; or a default timeout 
    /// (usually <see cref="Timeout.InfiniteTimeSpan"/>, but not necessarily) if none 
    /// could be located.
    /// </summary>
    /// <param name="category">
    /// The category for which to retrieve the timeout.
    /// </param>
    /// <returns>
    /// The timeout for the category provided.
    /// </returns>
    TimeSpan GetTimeoutFor(String category);
}
