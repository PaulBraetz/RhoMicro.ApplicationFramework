namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds appsettings to the app builder.
    /// </summary>
    public static WebClientGuiAppBuilder AddAppSettings(this WebClientGuiAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    public static WebClientGuiAppBuilder AddFileLogging(this WebClientGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureOptions);
    /// <summary>
    /// Adds configuration based console logging to the builders capabilities.
    /// </summary>
    public static WebClientGuiAppBuilder AddConsoleLogging(this WebClientGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
        appBuilder.AddConsoleLogging<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>(configureOptions);
    /// <summary>
    /// Adds appsettings and configuration to the app builder.
    /// </summary>
    public static WebClientGuiAppBuilder AddConfiguration(this WebClientGuiAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var configBuilder = new ConfigurationBuilder();

        var result = appBuilder
            .ConfigureBuilder(b => b.Services
                .AddSingleton<IConfigurationBuilder>(configBuilder)
                .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
                .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>())
            ).AddAppSettings<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost, BlazorAppBuilderCapabilities>();

        return result;
    }
}
