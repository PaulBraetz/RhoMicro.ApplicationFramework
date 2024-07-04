namespace RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Aspects.Abstractions;

/// <summary>
/// Default implementation of <see cref="ITimeoutProviderSettings"/>.
/// </summary>
public sealed class TimeoutProviderSettings : ITimeoutProviderSettings
{
    /// <inheritdoc cref="ITimeoutProviderSettings.TimeoutSettings"/>
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
    public required List<TimeoutSettings> TimeoutSettings { get; set; } = [];
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
    IEnumerable<ITimeoutSettings> ITimeoutProviderSettings.TimeoutSettings => TimeoutSettings.Cast<ITimeoutSettings>();
}
