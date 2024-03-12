namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;

/// <summary>
/// Common interface for logging composition information.
/// </summary>
public interface IContainerLogger
{
    /// <summary>
    /// Logs information about the composition.
    /// </summary>
    /// <param name="container">The container to log information about.</param>
    /// <param name="logger">The logger which to log messages about the container to.</param>
    void Log(Container container, ILogger logger);
}
