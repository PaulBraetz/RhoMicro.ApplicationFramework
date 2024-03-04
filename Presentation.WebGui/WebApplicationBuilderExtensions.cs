namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using System.Reflection;

using Microsoft.AspNetCore.Builder;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Provides extensions for integrating <see cref="WebApplication"/>, with SimpleInjector.
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Integrates SimpleInjector into a <see cref="WebApplicationBuilder"/> and builds the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="integrationStrategy">The strategy to integrate with.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="WebApplication"/>.</returns>
    public static WebApplication IntegrateSimpleInjector(
        this WebApplicationBuilder builder,
        IIntegrationStrategy integrationStrategy,
        out IDisposable containerLifetime)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var integrator = new SimpleInjectorBlazorIntegrator(integrationStrategy);
        var result = integrator
            .Integrate(new WebApplicationBuilderAdapter(builder))
            .App;

        containerLifetime = integrator;

        return result;
    }
    /// <summary>
    /// Integrates SimpleInjector into a <see cref="WebApplicationBuilder"/> and builds the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="builder">The builder to use for integration.</param>
    /// <param name="composer">The composer composing the object graph registered to the container used in the integrator.</param>
    /// <param name="componentAssemblies">The assemblies whose components to register to the container.</param>
    /// <param name="containerLifetime">The resulting <see cref="SimpleInjector.Container"/> lifetime.</param>
    /// <returns>A new, SimpleInjector-integrated <see cref="WebApplication"/>.</returns>
    public static WebApplication IntegrateSimpleInjectorWeb(
        this WebApplicationBuilder builder,
        IComposer composer,
        IEnumerable<Assembly> componentAssemblies,
        out IDisposable containerLifetime)
        => builder.IntegrateSimpleInjector(
            DefaultServerStrategy.CreateWeb(composer, componentAssemblies),
            out containerLifetime);
}
