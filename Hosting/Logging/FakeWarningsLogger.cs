namespace RhoMicro.ApplicationFramework.Hosting;

using System.Reflection;

using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Composition;

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
                r.ImplementationType.GetCustomAttribute<FakeServiceAttribute>() != null ||
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
