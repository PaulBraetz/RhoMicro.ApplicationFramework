namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    public static CliAppBuilder AddAppSettings(this CliAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>();

    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static CliAppBuilder AddFileLogging(this CliAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(configureOptions);

    /// <summary>
    /// Adds configuration based console logging to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static CliAppBuilder AddConsoleLogging(this CliAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder.AddLogging<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>();
        _ = appBuilder.Capabilities.Logging.AddConsole(configureOptions ?? ( static o => { } ));

        return appBuilder;
    }
}
