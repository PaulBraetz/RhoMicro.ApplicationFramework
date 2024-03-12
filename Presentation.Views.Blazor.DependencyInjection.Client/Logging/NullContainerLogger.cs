namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;

/// <summary>
/// Implements the null logging pattern for <see cref="IContainerLogger"/>.
/// </summary>
public sealed class NullContainerLogger : IContainerLogger
{
    private NullContainerLogger() { }
    /// <summary>
    /// Gets the singleton instance of <see cref="NullContainerLogger"/>.
    /// </summary>
    public static NullContainerLogger Instance { get; } = new();
    /// <inheritdoc/>
    public void Log(Container container, ILogger logger) { }
}
