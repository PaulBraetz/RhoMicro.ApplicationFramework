namespace RhoMicro.ApplicationFramework.Hosting;

using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

using RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
#pragma warning disable CA1724
public static class Extensions
{
    /// <summary>
    /// Logs to the app builders setup logging callback a message about a feature.
    /// </summary>
    public static CliAppBuilder LogFeature(this CliAppBuilder appBuilder, String feature, String message) =>
        appBuilder.LogFeature<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(feature, message);
    /// <summary>
    /// Adds appsettings to the app builder.
    /// </summary>
    public static CliAppBuilder AddAppSettings(this CliAppBuilder appBuilder) =>
        appBuilder.AddAppSettings<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>();

    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    public static CliAppBuilder AddFileLogging(this CliAppBuilder appBuilder, String configSection = "Logging", Action<FileLoggerOptions>? configureOptions = null) =>
        appBuilder.AddFileLogging<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(configSection, configureOptions);

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

        return appBuilder.LogFeature("ConsoleLogging", "added");
    }
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the lifestyle provided.
    /// </summary>
    public static CliAppBuilder AddTimeout(this CliAppBuilder appBuilder, Lifestyle lifestyle) =>
        appBuilder.AddTimeout<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(lifestyle);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static CliAppBuilder AddTimeout(this CliAppBuilder appBuilder) =>
        appBuilder.AddTimeout(Lifestyle.Scoped);
    /// <summary>
    /// Adds default aspects to the application being built.
    /// </summary>
    public static CliAppBuilder AddAspects(this CliAppBuilder appBuilder, Lifestyle lifestyle, CommonAspects aspects = CommonAspects.All, Action<InterceptorAppendContext>? appendInterceptors = null) =>
        appBuilder.AddAspects<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(lifestyle, aspects, appendInterceptors);
    /// <summary>
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static CliAppBuilder AddHostedServices(this CliAppBuilder appBuilder, params Assembly[] assemblies) =>
        appBuilder.AddHostedServices<CliAppBuilder, CliApp, HostApplicationBuilder, IHost, AppBuilderCapabilities>(assemblies);
}
