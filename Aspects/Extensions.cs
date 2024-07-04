namespace RhoMicro.ApplicationFramework.Aspects;

using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Aspects</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Gets the timeout settings for the request type provided.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to get a timeout settings instance for.</typeparam>
    /// <param name="provider">The provider to query for timeout settings.</param>
    /// <returns>
    /// The timeout settings located.
    /// </returns>
    public static ITimeoutSettings<TRequest> GetTimeoutSettingsFor<TRequest>(this ITimeoutProvider provider) =>
        new TimeoutSettings<TRequest>(provider);
}
