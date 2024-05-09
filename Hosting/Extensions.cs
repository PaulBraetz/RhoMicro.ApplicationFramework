namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds appsettings to the app builders capabilities.
    /// </summary>
    public static TSelf AddAppSettings<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var config = appBuilder.Capabilities.Configuration
            .AddJsonFile("appsettings.json");

        var environmentConfig = appBuilder.Capabilities.EnvironmentConfiguration;
        if(!EnvironmentConfiguration.Unknown.Equals(environmentConfig))
        {
            _ = config.AddJsonFile($"appsettings.{environmentConfig.Name}.json", optional: true);
        }

        return appBuilder;
    }
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static TSelf AddFileLogging<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        Action<FileLoggerOptions>? configureOptions = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var config = appBuilder.Capabilities.Configuration.Build();
        _ = appBuilder.Capabilities.Logging.AddFile(config, configureOptions ?? ( static o => { } ));

        return appBuilder;
    }
    /// <summary>
    /// Adds configuration based console logging to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static TSelf AddConsoleLogging<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        Action<ConsoleLoggerOptions>? configureOptions = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder.Capabilities.Logging.AddConsole(configureOptions ?? ( static o => { } ));

        return appBuilder;
    }
}
