namespace RhoMicro.ApplicationFramework.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
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
    ///// Adds appsettings to the app builder.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddAppSettings(this WebClientGuiAppBuilder appBuilder) =>
    //    appBuilder.AddAppSettings<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds configuration based file logging to the builders capabilities.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddFileLogging(this WebClientGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddFileLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureOptions);
    ///// <summary>
    ///// Adds configuration based console logging to the builders capabilities.
    ///// </summary>
    //private static WebClientGuiAppBuilder AddConsoleLogging(this WebClientGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddConsoleLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureOptions);
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
