namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Logging;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Provides a default integration strategy for server applications.
/// </summary>
/// <param name="composer"></param>
/// <param name="componentAssemblies"></param>
/// <param name="containerLogger"></param>
/// <param name="renderModeOptions"></param>
public sealed class DefaultServerStrategy(
    IComposer composer,
    IEnumerable<Assembly> componentAssemblies,
    IContainerLogger containerLogger,
    RenderModeOptions renderModeOptions)
    : DefaultClientStrategy(composer, componentAssemblies, containerLogger, renderModeOptions)
{
    /// <summary>
    /// Creates an instance of the <see cref="DefaultClientStrategy"/> for web servers.
    /// </summary>
    /// <param name="composer"></param>
    /// <param name="componentAssemblies"></param>
    /// <returns></returns>
    public static DefaultServerStrategy CreateWeb(IComposer composer, IEnumerable<Assembly> componentAssemblies) =>
        Create(composer, componentAssemblies, new() { OmitRenderModes = false });
    /// <summary>
    /// Creates an instance of the <see cref="DefaultClientStrategy"/> for local applications.
    /// </summary>
    /// <param name="composer"></param>
    /// <param name="componentAssemblies"></param>
    /// <returns></returns>
    public static DefaultServerStrategy CreateLocal(IComposer composer, IEnumerable<Assembly> componentAssemblies) =>
        Create(composer, componentAssemblies, new() { OmitRenderModes = true });

    private static DefaultServerStrategy Create(IComposer composer, IEnumerable<Assembly> componentAssemblies, RenderModeOptions activatorOptions) =>
    new(composer, componentAssemblies, CompositeContainerLogger.Default, activatorOptions)
    {
        IsDefault = true
    };

    /// <inheritdoc/>
    public override void NotifyVerificationError(DiagnosticVerificationException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        //await integrator logs roughly
        Thread.Sleep(2500);
        Console.WriteLine(String.Concat(Enumerable.Repeat('─', 100)));
        Console.WriteLine(exception.Message);
        Environment.Exit(1);
    }
    /// <inheritdoc/>
    public override void SimpleInjectorSetup(SimpleInjectorAddOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        base.SimpleInjectorSetup(options);

        var services = options.Services;

        // Unfortunate nasty hack. We reported this with Microsoft.
        _ = services
            .AddTransient(
                typeof(Microsoft.AspNetCore.Components.Server.CircuitOptions)
                .Assembly
                .GetTypes()
                .First(t => t.FullName == "Microsoft.AspNetCore.Components.Server.ComponentHub"))
            .AddScoped(typeof(IHubActivator<>), typeof(SimpleInjectorBlazorHubActivator<>));
    }
}
