namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.Extensions.Configuration;
using NReco.Logging.File;
using RhoMicro.ApplicationFramework.Common.Environment;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.RequiredPropertyValidation.RhoMicro.RequiredPropertyValidation;
using SimpleInjector;
using Microsoft.Extensions.Hosting;
using SimpleInjector.Integration.ServiceCollection;
using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
#pragma warning disable CA1724
public static class Extensions
{
    /// <summary>
    /// Logs to the app builders setup logging callback a message about a feature.
    /// </summary>
    public static TSelf LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        String feature,
        String message)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Capabilities.SetupLoggingCallback.Invoke($"[{feature}] {message}");

        return appBuilder;
    }
    /// <summary>
    /// Adds validation that assures all required non-null properties on resolved instances are not null.
    /// </summary>
    public static TSelf AddRequiredPropertyValidation<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        Action<RequiredPropertyValidationConfiguration>? configure = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Options.OnContainerAdd += o => _ = o.Services.AddRequiredPropertyValidation(configure);

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>("RequiredPropertyValidation", "added");
    }
    /// <summary>
    /// Adds default aspects to the application being built.
    /// </summary>
    public static TSelf AddAspects<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        Lifestyle lifestyle,
        CommonAspects aspects = CommonAspects.All,
        Action<InterceptorAppendContext>? appendInterceptors = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        appBuilder.Options.Composer += AspectComposers.CreateDefault(lifestyle, aspects, appendInterceptors);

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>("Aspects", "added");
    }
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the lifestyle provided.
    /// </summary>
    public static TSelf AddTimeout<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder, Lifestyle lifestyle)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        const String feature = "Timeout";
        _ = appBuilder
            .ConfigureOptions(o => o.OnContainerAdd += o =>
            {
                _ = o.Services
                    .AddTransient((Func<IServiceProvider, ITimeoutProviderSettings>)( sp => sp.GetRequiredService<IOptions<TimeoutProviderSettings>>().Value ))
                    .AddOptions<TimeoutProviderSettings>()
                    .BindConfiguration(nameof(Aspects.Decorators.TimeoutProviderSettings))
                    .ValidateOnStart();
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added options");
            });

        appBuilder.Options.Composer += Composer.Create(c =>
            {
                c.RegisterSingleton<ITimeoutProvider>(() =>
                {
                    var settings = c.GetInstance<ITimeoutProviderSettings>();
                    var result = TimeoutProvider.Create(settings);

                    return result;
                });
                c.RegisterConditional(typeof(ITimeoutSettings<>), typeof(TimeoutSettings<>), lifestyle, ctx => !ctx.Handled);
                c.RegisterDecorator(typeof(IService<,>), typeof(TimeoutDecorator<,>), lifestyle);
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
            });

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }
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

        const String feature = "Appsettings";

        var config = appBuilder.Capabilities.Configuration.AddJsonFile("appsettings.json");
        _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added appsettings.json");

        var environmentConfig = appBuilder.Capabilities.EnvironmentConfiguration;
        if(!EnvironmentConfiguration.Unknown.Equals(environmentConfig))
        {
            var envAppsettings = $"appsettings.{environmentConfig.Name}.json";
            _ = config.AddJsonFile(envAppsettings, optional: true);
            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, $"added {envAppsettings}");
        }

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }
    /// <summary>
    /// Adds configuration based file logging to the builders capabilities.
    /// </summary>
    public static TSelf AddFileLogging<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder,
        String configSection = "Logging",
        Action<FileLoggerOptions>? configureOptions = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        const String feature = "FileLogging";

        var config = appBuilder.Capabilities.Configuration.Build().GetSection(configSection);
        _ = appBuilder.Capabilities.Logging.AddFile(config, configureOptions ?? ( static o => { } ));
        appBuilder.Options.OnContainerAdd += o =>
        {
            try
            {
                _ = o.AddLogging();
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
            } catch(InvalidOperationException ex)
            when(ex.Message == "The AddLogging extension method can only be called once on a Container instance.")
            {
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "services already added");
            }
        };

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }
    /// <summary>
    /// Adds logging support to the builders capabilities.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns></returns>
    public static TSelf AddLogging<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        const String feature = "Logging";

        appBuilder.Options.OnContainerAdd += o =>
        {
            try
            {
                _ = o.AddLogging();
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
            } catch(InvalidOperationException ex)
            when(ex.Message == "The AddLogging extension method can only be called once on a Container instance.")
            {
                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "services already added");
            }
        };

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }
    /// <summary>
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static TSelf AddHostedServices<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(this TSelf appBuilder, params Assembly[] assemblies)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        const String feature = "HostedServices";

        var paramExpr = Expression.Parameter(typeof(SimpleInjectorAddOptions));
        var addExprs = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IHostedService)))
            .SelectMany<Type, Expression>(t =>
            {
                var method = ( typeof(SimpleInjectorGenericHostExtensions)
                    .GetMethod(nameof(SimpleInjectorGenericHostExtensions.AddHostedService))
                    ?? throw new InvalidOperationException($"Unable to locate method '{nameof(SimpleInjectorGenericHostExtensions.AddHostedService)}' in type  '{typeof(SimpleInjectorGenericHostExtensions).FullName}'.") )
                    .MakeGenericMethod(t);
                var callExpr = Expression.Call(method, paramExpr);

                Expression<Action> logExpr = () => appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, $"added {t.Name}");

                return [callExpr, logExpr];
            });
        var body = Expression.Block(addExprs);
        var lambdaExpr = Expression.Lambda<Action<SimpleInjectorAddOptions>>(body, paramExpr);
        var handler = lambdaExpr.Compile();
        appBuilder.Options.OnContainerAdd += handler;

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }
}
