#pragma warning disable CA1848 // Use the LoggerMessage delegates
namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;

/// <summary>
/// Logs the root object graphs constructed by a container.
/// </summary>
public sealed class ObjectGraphLogger : IContainerLogger
{
    /// <inheritdoc/>
    public void Log(Container container, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(logger);

        var roots = container.GetRootRegistrations();
        foreach(var root in roots)
        {
            var graph = root.VisualizeObjectGraph();
            logger.LogInformation(
                "Injection info: Graph for root {ServiceType}: {Graph}",
                root.ServiceType.FullName,
                graph);
        }
    }
}
