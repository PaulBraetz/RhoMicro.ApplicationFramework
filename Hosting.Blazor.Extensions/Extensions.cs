namespace RhoMicro.ApplicationFramework.Hosting;

using System.Runtime.CompilerServices;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Composition.Presentation.Models.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using SimpleInjector;
using SimpleInjector.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using RhoMicro.ApplicationFramework.Common.Environment;
using System.Reflection;

/// <summary>
/// Extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// Creates a composer combining the composer provided with default composers for blazor applications
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A new combined composer.</returns>
    public static TSelf AddBlazor<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : BlazorAppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder
            .ConfigureCapabilities(c =>
            {
                _ = c.Components.Add(typeof(DynamicModelComponent<>).Assembly);
            })
            .ConfigureOptions(o =>
            {
                o.OnContainerAdd += _ => AddDynamicComponentSettings<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(appBuilder);
                o.Composer = Composer.Create(
                    appBuilder.Options.Composer,
                    BlazorModelsComposer,
                    CreateStylesComposer(appBuilder.Capabilities.Configuration.Build()),
                    AspectComposers.Default,
                    PresentationComposers.Models);
            });

        return appBuilder;
    }

    private static void AddDynamicComponentSettings<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : BlazorAppBuilderCapabilities
    {
        var handledComponentInfos = new HashSet<ModelComponentInfo>();
        var ambiguousComponentInfos = new HashSet<ModelComponentInfo>();

        var dynamicComponentInfos = new List<(Type settingsType, Object settingsInstance)>();

        appBuilder.Capabilities.Components
            .Where(ModelComponentType.IsModelComponentType)
            .Select(t => (ModelComponentType)t)
            .SelectMany(t => t.Infos.Select(i => (componentType: t, info: i)))
            .ForEach((t) =>
            {
                var (componentType, info) = t;

                if(ambiguousComponentInfos.Contains(info) || !handledComponentInfos.Add(info))
                {
                    _ = handledComponentInfos.Remove(info);
                    _ = ambiguousComponentInfos.Add(info);

                    if(appBuilder.Capabilities.IgnoreAmbiguousDynamicComponentRegistrations)
                        return;

                    throw new InvalidOperationException(
                        $"A model component type with the same model and style type arguments ({info.ModelType}, {info.StyleType}) as {componentType.AsType} has already been registered.");
                }

                var settingsType = info.GetDynamicComponentSettingsType();
                var settingsInstance = Activator.CreateInstance(settingsType, componentType)
                    ?? throw new InvalidOperationException(
                        $"Unable to construct instance of component type with the model and style type arguments ({info.ModelType}, {info.StyleType}): {componentType.AsType}.");

                dynamicComponentInfos.Add((settingsType, settingsInstance));
            });

        appBuilder.Options.Composer += Composer.Create(c =>
        {
            foreach(var (settingsType, settingsInstance) in dynamicComponentInfos)
            {
                c.RegisterInstance(settingsType, settingsInstance);
            }
        });
    }

    /// <summary>
    /// Creates a composer combining the composer provided with a blazor specific model composer instance.
    /// </summary>
    /// <param name="composer">The composer to combine.</param>
    /// <returns>A new combined composer.</returns>
    internal static IComposer WithBlazorModels(this IComposer composer) => composer + BlazorModelsComposer;
    /// <summary>
    /// Creates a composer for blazor specific model implementations.
    /// </summary>
    /// <returns>The model composer instance.</returns>
    private static IComposer BlazorModelsComposer { get; } =
        Composer.Create(c =>
        {
            c.Register<INavigationManager, NavigationManager>(Lifestyle.Scoped);
        });
    /// <summary>
    /// Creates a composer combining the composer provided with a styles composer instance.
    /// </summary>
    /// <param name="composer">The composer to combine.</param>
    /// <param name="configuration">The configuration providing styling information.</param>
    /// <returns>A new combined composer.</returns>
    internal static IComposer WithStyles(this IComposer composer, IConfiguration configuration) => Composer.Create(composer, CreateStylesComposer(configuration));

    /// <summary>
    /// Creates a styles composer instance.
    /// </summary>
    /// <param name="configuration">The configuration providing styling information.</param>
    /// <returns>The styles composer instance.</returns>
    private static IComposer CreateStylesComposer(IConfiguration configuration) =>
        Composer.Create(c =>
        {
            c.RegisterInstance<ICssStyle>(new CssStyle());

            var cssSettingsSection = configuration.GetSection("CssStyleSettings");

            var settingsTypes = typeof(CssStyleSettings).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Settings", StringComparison.Ordinal) && t.IsAssignableTo(typeof(ICssStyle)));

            foreach(var settingsType in settingsTypes)
            {
                var optionsImplType = typeof(ConfigureNamedOptions<>).MakeGenericType(settingsType);
                var optionsServiceType = typeof(IOptions<>).MakeGenericType(settingsType);

                var cssStyleName = settingsType.Name[..^"Settings".Length];

                var instance = Activator.CreateInstance(optionsImplType, null, (Object settings) => cssSettingsSection.GetSection(cssStyleName).Bind(settings))
                    ?? throw new InvalidOperationException($"Unable to construct options: {settingsType}");
            }
        });

    /// <summary>
    /// Adds default component models to the app builders capabilities.
    /// </summary>
    private static TSelf AddDefaultModels<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Options.Composer += PresentationComposers.Models;

        return appBuilder;
    }
}

file static class ModelComponentTypeExtensions
{
    public static Type GetDynamicComponentSettingsType(this ModelComponentInfo info) =>
        info.HasStyle
        ? typeof(DynamicStyledModelComponentSettings<,>).MakeGenericType(info.ModelType, info.StyleType)
        : typeof(DynamicModelComponentSettings<>).MakeGenericType(info.ModelType);
}
