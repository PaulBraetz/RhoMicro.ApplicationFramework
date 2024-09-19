namespace RhoMicro.ApplicationFramework.Hosting;
using System.Reflection;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;

using RhoMicro.RequiredPropertyValidation.RhoMicro.RequiredPropertyValidation;

using SimpleInjector;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Registers a platform-specific clipboard implementation to the builder services.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebClientGuiAppBuilder AddClipboard(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddClipboard<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds validation that assures all required non-null properties on resolved instances are not null.
    /// </summary>
    public static WebClientGuiAppBuilder AddRequiredPropertyValidation(this WebClientGuiAppBuilder appBuilder, Action<RequiredPropertyValidationConfiguration>? configure = null) =>
        appBuilder.AddRequiredPropertyValidation<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configure);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the lifestyle provided.
    /// </summary>
    public static WebClientGuiAppBuilder AddTimeout(this WebClientGuiAppBuilder appBuilder, Lifestyle lifestyle) =>
        appBuilder.AddTimeout<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(lifestyle);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static WebClientGuiAppBuilder AddTimeout(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddTimeout(Lifestyle.Scoped);
    /// <summary>
    /// Adds blazor to the web client app builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A new combined composer.</returns>
    public static WebClientGuiAppBuilder AddBlazor(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddBlazor<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds api services to the app builder.
    /// </summary>
    /// <param name="appBuilder">The builder to add api services to.</param>
    /// <param name="configureClients">Callback for configuring which kinds of api clients to register.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebClientGuiAppBuilder AddApiServiceClients(this WebClientGuiAppBuilder appBuilder, Action<IApiServiceClientsOptions>? configureClients = null) =>
        appBuilder.AddApiServiceClients<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureClients);
    /// <summary>
    /// Adds appsettings to the app builder.
    /// </summary>
    public static WebClientGuiAppBuilder AddAppSettings(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds logging support to the builders capabilities.
    /// </summary>
    public static WebClientGuiAppBuilder AddConsoleLogging(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static WebClientGuiAppBuilder AddHostedServices(this WebClientGuiAppBuilder appBuilder, params Assembly[] assemblies) =>
        appBuilder.AddHostedServices<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(assemblies);
    ///// <summary>
    ///// Adds configuration based file logging to the builders capabilities.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddFileLogging(this WebClientGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
    //  appBuilder.AddFileLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureOptions);
    ///// <summary>
    ///// Adds default component models to the app builders capabilities.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddDefaultModels(this WebClientGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultModels<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds default component views to the app builders capabilities.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddDefaultViews(this WebClientGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultViews<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();

    ///// <summary>
    ///// Adds appsettings and configuration to the app builder.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddConfiguration(this WebClientGuiAppBuilder appBuilder)
    //{
    //    ArgumentNullException.ThrowIfNull(appBuilder);

    //    var configBuilder = new ConfigurationBuilder();

    //    var result = appBuilder
    //        .ConfigureBuilder(b => b.Services
    //            .AddSingleton<IConfigurationBuilder>(configBuilder)
    //            .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
    //            .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>())
    //        ).AddAppSettings<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();

    //    return result;
    //}
}
