namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System.Reflection;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Logging;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Provides behavior details for <see cref="SimpleInjectorBlazorIntegrator"/>.
/// </summary>
public interface IIntegrationStrategy
{
    /// <summary>
    /// Gets the composer composing the object graph registered to the container used in the integrator.
    /// </summary>
    IComposer Composer { get; }
    /// <summary>
    /// Gets the assemblies whose components to register to the container.
    /// </summary>
    IEnumerable<Assembly> ComponentAssemblies { get; }
    /// <summary>
    /// Gets the logger to log container details after initialization.
    /// </summary>
    IContainerLogger ContainerLogger { get; }
    /// <summary>
    /// Notifies the strategy about any diagnostics exceptions encountered while executing <see cref="Container.Verify()"/>.
    /// </summary>
    /// <param name="exception">
    /// The exception encountered.
    /// </param>
    void NotifyVerificationError(DiagnosticVerificationException exception);
    /// <summary>
    /// Passed to the container upon initialization.
    /// </summary>
    /// <param name="options"></param>
    void SimpleInjectorSetup(SimpleInjectorAddOptions options);
}
