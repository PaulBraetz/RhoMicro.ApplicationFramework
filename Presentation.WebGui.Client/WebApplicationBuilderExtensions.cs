namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using System.Reflection;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Provides extensions for integrating <see cref="WebAssemblyHost"/>, with SimpleInjector.
/// </summary>
public static class WebAssemblyHostBuilderExtensions
{
    /// <summary>
    /// Integrates SimpleInjector into a <see cref="WebAssemblyHostBuilder"/> and builds the <see cref="WebAssemblyHost"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="integrationStrategy">The strategy to integrate with.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="WebAssemblyHost"/>.</returns>
    public static WebAssemblyHost IntegrateSimpleInjector(
        this WebAssemblyHostBuilder builder,
        IIntegrationStrategy integrationStrategy,
        out IDisposable containerLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var integrator = new SimpleInjectorBlazorIntegrator(integrationStrategy);
        var result = integrator
            .Integrate(new WebAssemblyHostBuilderAdapter(builder))
            .App;

        containerLifetime = integrator;

        return result;
    }
    /// <summary>
    /// Integrates SimpleInjector into a <see cref="WebAssemblyHostBuilder"/> and builds the <see cref="WebAssemblyHost"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="composer">The composer composing the object graph registered to the container used in the integrator.</param>
    /// <param name="componentAssemblies">The assemblies whose components to register to the container.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="WebAssemblyHost"/>.</returns>
    public static WebAssemblyHost IntegrateSimpleInjectorWeb(
        this WebAssemblyHostBuilder builder,
        IComposer composer,
        IEnumerable<Assembly> componentAssemblies,
        out IDisposable containerLifetime)
        => builder.IntegrateSimpleInjector(
            DefaultClientStrategy.CreateClient(composer, componentAssemblies),
            out containerLifetime);
}
