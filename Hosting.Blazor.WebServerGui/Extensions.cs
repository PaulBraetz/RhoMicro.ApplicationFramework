namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds blazor to the web server app builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A new combined composer.</returns>
    public static WebServerGuiAppBuilder AddBlazor(this WebServerGuiAppBuilder appBuilder) =>
        appBuilder.AddBlazor<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();

    ///// <summary>
    ///// Adds default component models to the app builders capabilities.
    ///// </summary>
    //private static WebServerGuiAppBuilder AddDefaultModels(this WebServerGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultModels<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds default component views to the app builders capabilities.
    ///// </summary>
    //private static WebServerGuiAppBuilder AddDefaultViews(this WebServerGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultViews<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds appsettings to the app builder.
    ///// </summary>
    //private static WebServerGuiAppBuilder AddAppSettings(this WebServerGuiAppBuilder appBuilder) =>
    //    appBuilder.AddAppSettings<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds configuration based file logging to the builders capabilities.
    ///// </summary>
    ///// <param name="appBuilder"></param>
    ///// <param name="configureOptions"></param>
    ///// <returns></returns>
    //private static WebServerGuiAppBuilder AddFileLogging(this WebServerGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddFileLogging<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configureOptions);
    ///// <summary>
    ///// Adds configuration based console logging to the builders capabilities.
    ///// </summary>
    //private static WebServerGuiAppBuilder AddConsoleLogging(this WebServerGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddConsoleLogging<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configureOptions);
}
