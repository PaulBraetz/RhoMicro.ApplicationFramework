namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Represents configuration-based settings used by the default <see cref="ITimeoutProvider"/>.
/// </summary>
public interface ITimeoutProviderSettings
{
    /// <summary>
    /// Gets the configured timeout settings.
    /// </summary>
    IEnumerable<ITimeoutSettings> TimeoutSettings { get; }
}
