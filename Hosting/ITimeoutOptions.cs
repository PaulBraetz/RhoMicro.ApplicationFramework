namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Used to configure service timeouts.
/// </summary>
public interface ITimeoutOptions
{
    /// <summary>
    /// Sets the provider used to obtain a timeout for the type of request provided.
    /// </summary>
    /// <param name="requestType">The type of request to set a timeout for.</param>
    /// <param name="timeout">The timeout to set for requests of the type provided.</param>
    void SetTimeout(Type requestType, TimeSpan timeout);
}
