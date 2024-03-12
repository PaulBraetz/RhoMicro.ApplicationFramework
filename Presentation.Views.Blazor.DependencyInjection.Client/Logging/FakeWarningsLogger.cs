#pragma warning disable CA1848 // Use the LoggerMessage delegates
namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;

/// <summary>
/// Logs detected fakes in the service registered to the container.
/// </summary>
public sealed class FakeWarningsLogger : IContainerLogger
{
    /// <inheritdoc/>
    public void Log(Container container, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(logger);

        var fakes = container.GetCurrentRegistrations()
            .Where(r =>
                r.ImplementationType.Name.Contains("fake", StringComparison.InvariantCultureIgnoreCase) ||
                r.ImplementationType.Namespace != null &&
                r.ImplementationType.Namespace.Contains("fake", StringComparison.InvariantCultureIgnoreCase));
        foreach(var fake in fakes)
        {
            logger.LogWarning(
                "Injection warning: Fake detected: {ServiceType}->{ImplementationType}",
                fake.ServiceType.FullName,
                fake.ImplementationType.FullName);
        }
    }
}
