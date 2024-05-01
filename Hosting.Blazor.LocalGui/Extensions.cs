namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

using Photino.Blazor;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds appsettings to the app builder.
    /// </summary>
    public static LocalGuiAppBuilder AddAppSettings(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    public static LocalGuiAppBuilder AddFileLogging(this LocalGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configureOptions);
    /// <summary>
    /// Adds configuration based console logging to the builders capabilities.
    /// </summary>
    public static LocalGuiAppBuilder AddConsoleLogging(this LocalGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
        appBuilder.AddConsoleLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configureOptions);
    /// <summary>
    /// Adds appsettings and configuration to the app builder.
    /// </summary>
    public static LocalGuiAppBuilder AddConfiguration(this LocalGuiAppBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var configBuilder = new ConfigurationBuilder();

        var result = appBuilder
            .ConfigureBuilder(b => b.Services
                .AddSingleton<IConfigurationBuilder>(configBuilder)
                .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
                .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>())
            ).AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();

        return result;
    }
}
