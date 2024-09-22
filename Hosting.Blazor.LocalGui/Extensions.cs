namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Hosting;
using System.Reflection;

using NReco.Logging.File;

using Photino.Blazor;

using RhoMicro.RequiredPropertyValidation.RhoMicro.RequiredPropertyValidation;

using SimpleInjector;
using RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
#pragma warning disable CA1724
public static class Extensions
{
    /// <summary>
    /// Logs to the app builders setup logging callback a message about a feature.
    /// </summary>
    public static LocalGuiAppBuilder LogFeature(this LocalGuiAppBuilder appBuilder, String feature, String message) =>
        appBuilder.LogFeature<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(feature, message);
    /// <summary>
    /// Registers a platform-specific clipboard implementation to the builder services.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static LocalGuiAppBuilder AddClipboard(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddClipboard<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds validation that assures all required non-null properties on resolved instances are not null.
    /// </summary>
    public static LocalGuiAppBuilder AddRequiredPropertyValidation(this LocalGuiAppBuilder appBuilder, Action<RequiredPropertyValidationConfiguration>? configure = null) =>
        appBuilder.AddRequiredPropertyValidation<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configure);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the lifestyle provided.
    /// </summary>
    public static LocalGuiAppBuilder AddTimeout(this LocalGuiAppBuilder appBuilder, Lifestyle lifestyle) =>
        appBuilder.AddTimeout<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(lifestyle);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static LocalGuiAppBuilder AddTimeout(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddTimeout(Lifestyle.Scoped);
    /// <summary>
    /// Adds default aspects to the application being built.
    /// </summary>
    public static LocalGuiAppBuilder AddAspects(this LocalGuiAppBuilder appBuilder, Lifestyle lifestyle, CommonAspects aspects = CommonAspects.All, Action<InterceptorAppendContext>? appendInterceptors = null) =>
        appBuilder.AddAspects<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(lifestyle, aspects, appendInterceptors);
    /// <summary>
    /// Adds appsettings to the app builder.
    /// </summary>
    public static LocalGuiAppBuilder AddAppSettings(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds blazor to the local app builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A new combined composer.</returns>
    public static LocalGuiAppBuilder AddBlazor(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddBlazor<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();

    /// <summary>
    /// Adds api services to the app builder.
    /// </summary>
    /// <param name="appBuilder">The builder to add api services to.</param>
    /// <param name="configureClients">Callback for configuring which kinds of api clients to register.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static LocalGuiAppBuilder AddApiServiceClients(this LocalGuiAppBuilder appBuilder, Action<IApiServiceClientsOptions>? configureClients = null) =>
        appBuilder.AddApiServiceClients<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configureClients);
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    public static LocalGuiAppBuilder AddFileLogging(this LocalGuiAppBuilder appBuilder, String configSection = "Logging", Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configSection, configureOptions);
    /// <summary>
    /// Adds logging support to the builders capabilities.
    /// </summary>
    public static LocalGuiAppBuilder AddConsoleLogging(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static LocalGuiAppBuilder AddHostedServices(this LocalGuiAppBuilder appBuilder, params Assembly[] assemblies) =>
        appBuilder.AddHostedServices<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(assemblies);
    ///// <summary>
    ///// Adds default component models to the app builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddDefaultModels(this LocalGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultModels<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds default component views to the app builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddDefaultViews(this LocalGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultViews<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds appsettings and configuration to the app builder.
    ///// </summary>
    //private static LocalGuiAppBuilder AddConfiguration(this LocalGuiAppBuilder appBuilder)
    //{
    //    ArgumentNullException.ThrowIfNull(appBuilder);

    //    var configBuilder = new ConfigurationBuilder();

    //    var result = appBuilder
    //        .ConfigureBuilder(b => b.Services
    //            .AddSingleton<IConfigurationBuilder>(configBuilder)
    //            .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
    //            .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>())
    //        ).AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();

    //    return result;
    //}
}
