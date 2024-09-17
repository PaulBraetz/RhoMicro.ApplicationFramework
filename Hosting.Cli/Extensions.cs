namespace RhoMicro.ApplicationFramework.Hosting;

using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NReco.Logging.File;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

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
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static CliAppBuilder AddHostedServices(this CliAppBuilder appBuilder, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        var paramExpr = Expression.Parameter(typeof(SimpleInjectorAddOptions));
        var addExprs = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IHostedService)))
            .Select(t =>
            {
                var method = ( typeof(SimpleInjectorGenericHostExtensions)
                    .GetMethod(nameof(SimpleInjectorGenericHostExtensions.AddHostedService))
                    ?? throw new InvalidOperationException($"Unable to locate method '{nameof(SimpleInjectorGenericHostExtensions.AddHostedService)}' in type  '{typeof(SimpleInjectorGenericHostExtensions).FullName}'.") )
                    .MakeGenericMethod(t);
                var callExpr = Expression.Call(method, paramExpr);

                return callExpr;
            });
        var body = Expression.Block(addExprs);
        var lambdaExpr = Expression.Lambda<Action<SimpleInjectorAddOptions>>(body, paramExpr);
        var handler = lambdaExpr.Compile();
        appBuilder.Options.OnContainerAdd += handler;

        return appBuilder;
    }
}
