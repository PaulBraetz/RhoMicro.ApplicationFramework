namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Composition.Presentation.Models.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using SimpleInjector;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components;
using Microsoft.JSInterop;

/// <summary>
/// Extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// Registers a platform-specific clipboard implementation to the builder services.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static TSelf AddClipboard<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : BlazorAppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Options.Composer += Composer.Create(static c => c.Register<IClipboardModel>(() =>
        {
            //if(c.GetInstance<IDeploymentPlatformProvider>().DeploymentPlatform is DeploymentPlatform.Desktop)
            //{
            //    return new ClipboardModel();
            //} else
            //{
            //    var jsRuntime = c.GetInstance<IJSRuntime>();
            //    return new JsClipboardModel(jsRuntime);
            //}
            return new JsClipboardModel(c.GetInstance<IJSRuntime>());
        }));

        return appBuilder;
    }
    /// <summary>
    /// Adds blazor capabilities and options to the builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
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

    sealed class ApiServiceClientsOptions : IApiServiceClientsOptions
    {
        public JsonSerializerOptions SerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }
    /// <summary>
    /// Adds api services to the app builder.
    /// </summary>
    /// <param name="appBuilder">The builder to add api services to.</param>
    /// <param name="configureClients">Callback for configuring client settings.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static TSelf AddApiServiceClients<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder, Action<IApiServiceClientsOptions>? configureClients = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : BlazorAppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder.ConfigureOptions(o =>
        {
            o.OnContainerAdd += (o) =>
            {
                var clientsOptions = new ApiServiceClientsOptions();
                configureClients?.Invoke(clientsOptions);

                _ = o.Services
                    .AddHttpClient()
                    .AddTransient(sp => sp.GetRequiredService<IOptions<ApiServicesSettings>>().Value)
                    .AddOptions<ApiServicesSettings>()
                    .BindConfiguration("ApiServicesSettings")
                    .Validate(
                        s => s.GetIsValid(ignoreBaseUri: false),
                        "Endpoints with invalid request type or request uri detected. Service endpoint uris must be well-formed relative uris.  The base uri must be a well-formed absolute uri.")
                    .ValidateOnStart();

                o.Container.Register(() => new ApiServiceSettingsFactory(o.Container.GetInstance<ApiServicesSettings>(), clientsOptions.SerializerOptions));

                var settings = new ApiServicesSettings() { BaseUri = "" };
                var config = appBuilder.Capabilities.Configuration.Build();
                config.Bind("ApiServicesSettings", settings);

                foreach(var (settingsType, serviceType, implementationType) in
                        settings.Services.Select(s => (s.SettingsType, s.ServiceType, s.ImplementationType)))
                {
                    o.Container.Register(settingsType, () => o.Container.GetInstance<ApiServiceSettingsFactory>().Create(settingsType));
                    o.Container.RegisterConditional(serviceType, implementationType, ctx => !ctx.Handled);
                    _ = o.Services.AddHttpClient(implementationType.FullName!);
                }
            };
        });

        return appBuilder;
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
