namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;

/// <summary>
/// Composite logger for logging different types of information about a container.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="children">
/// The loggers used when calling <see cref="Log(Container, ILogger)"/>.
/// </param>
public sealed class CompositeContainerLogger(IEnumerable<IContainerLogger> children) : IContainerLogger
{
    /// <summary>
    /// Gets the default composite logger, including:
    /// <list type="bullet">
    /// <item>
    /// <see cref="ContainerDiagnosticsLogger"/>
    /// </item>
    /// <item>
    /// <see cref="FakeWarningsLogger"/>
    /// </item>
    /// <item>
    /// <see cref="ObjectGraphLogger"/>
    /// </item>
    /// </list>
    /// </summary>
    public static IContainerLogger Default { get; } = new CompositeContainerLogger(new ContainerDiagnosticsLogger(), new FakeWarningsLogger(), new ObjectGraphLogger());

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="children">
    /// The loggers used when calling <see cref="Log(Container, ILogger)"/>.
    /// </param>
    public CompositeContainerLogger(params IContainerLogger[] children)
        : this((IEnumerable<IContainerLogger>)children) { }

    /// <inheritdoc/>
    public void Log(Container container, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(logger);

        logger.Log(LogLevel.Information,
                   default,
                   children,
                   null,
                   (c, e) => $"Container loggers used: {String.Join(", ", c.Select(c => c.GetType().Name))}");
        foreach(var child in children)
        {
            child.Log(container, logger);
        }
    }
}
