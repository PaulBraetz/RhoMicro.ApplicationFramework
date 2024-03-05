namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System.Reflection;
using System.Runtime.CompilerServices;

using Photino.Blazor;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Provides extensions for integrating <see cref="PhotinoBlazorApp"/>, with SimpleInjector.
/// </summary>
public static class PhotinoBlazorAppBuilderExtensions
{
    /// <summary>
    /// Wraps the <see cref="PhotinoBlazorAppBuilder"/> in an adapter onto the <see cref="IApplicationBuilder{TApplication}"/>
    /// integration interface.
    /// </summary>
    /// <param name="builder">The builder to adapt.</param>
    /// <returns>The new adapter.</returns>
    public static PhotinoBlazorAppBuilderAdapter WithAdapter(this PhotinoBlazorAppBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var result = PhotinoBlazorAppBuilderAdapter.Create(builder);

        return result;
    }

    /// <summary>
    /// Integrates SimpleInjector into a <see cref="PhotinoBlazorAppBuilder"/> and builds the <see cref="PhotinoBlazorApp"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="integrationStrategy">The strategy to integrate with.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="PhotinoBlazorApp"/>.</returns>
    public static PhotinoBlazorApp IntegrateSimpleInjector(
        this IApplicationBuilder<PhotinoBlazorAppAdapter> builder,
        IIntegrationStrategy integrationStrategy,
        out IDisposable containerLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var integrator = new SimpleInjectorBlazorIntegrator(integrationStrategy);
        var result = integrator
            .Integrate(builder)
            .App;

        containerLifetime = integrator;

        return result;
    }
    /// <summary>
    /// Integrates SimpleInjector into a <see cref="PhotinoBlazorAppBuilder"/> and builds the <see cref="PhotinoBlazorApp"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="composer">The composer composing the object graph registered to the container used in the integrator.</param>
    /// <param name="componentAssemblies">The assemblies whose components to register to the container.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="PhotinoBlazorApp"/>.</returns>
    public static PhotinoBlazorApp IntegrateSimpleInjectorLocal(
        this IApplicationBuilder<PhotinoBlazorAppAdapter> builder,
        IComposer composer,
        IEnumerable<Assembly> componentAssemblies,
        out IDisposable containerLifetime)
        => builder.IntegrateSimpleInjector(
            DefaultServerStrategy.CreateLocal(composer, componentAssemblies),
            out containerLifetime);
}
