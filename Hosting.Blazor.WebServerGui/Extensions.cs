namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.AspNetCore.Builder;
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
    public static WebServerGuiAppBuilder AddAppSettings(this WebServerGuiAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static WebServerGuiAppBuilder AddFileLogging(this WebServerGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configureOptions);
    /// <summary>
    /// Adds configuration based console logging to the builders capabilities.
    /// </summary>
    public static WebServerGuiAppBuilder AddConsoleLogging(this WebServerGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
        appBuilder.AddConsoleLogging<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configureOptions);
}
