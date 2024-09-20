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
using System.Text.Json;
using System.Runtime.CompilerServices;

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

        const String feature = "FileLogging";

        var config = appBuilder.Capabilities.Configuration.Build();
        _ = appBuilder.Capabilities.Logging.AddFile(config, configureOptions ?? ( static o => { } ));
        appBuilder.Options.OnContainerAdd += o =>
        {
            _ = o.AddLogging();
            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
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

        const String feature = "FileLogging";

        appBuilder.Options.OnContainerAdd += o =>
        {
            _ = o.AddLogging();
            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
        };

        return appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added");
    }

    /// <summary>
    /// Adds api services to the app builder.
    /// </summary>
    /// <param name="appBuilder">The builder to add api services to.</param>
    /// <param name="configureClients">Callback for configuring client settings.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static TSelf AddApiServiceClients<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder, Action<ApiServiceOptions>? configureClients = null)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : AppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        const String feature = "ApiServiceClients";

        appBuilder.Options.OnContainerAdd += (o) =>
        {
            var clientsOptions = new ApiServiceOptions();
            configureClients?.Invoke(clientsOptions);

            _ = o.Services
                .AddHttpClient()
                .AddTransient(sp => sp.GetRequiredService<IOptions<ApiServicesSettings>>().Value)
                .AddOptions<ApiServicesSettings>()
                .BindConfiguration("ApiServicesSettings")
                .Validate(
                    s => s.GetIsValid(ignoreBaseUri: false),
                    "Endpoints with invalid request type or request uri detected. Service endpoint uris must be well-formed relative uris. The base uri must be a well-formed absolute uri.")
                .ValidateOnStart();

            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added options");

            o.Container.Register(() => new ApiServiceSettingsFactory(o.Container.GetInstance<ApiServicesSettings>(), clientsOptions.SerializerOptions));

            var servicesSettings = new ApiServicesSettings() { BaseUri = "" };
            var config = appBuilder.Capabilities.Configuration.Build();
            config.Bind("ApiServicesSettings", servicesSettings);

            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, $"using base uri {servicesSettings.BaseUri}");

            foreach(var (settingsType, serviceType, implementationType, serviceSettings) in
                    servicesSettings.Services.Select(s => (s.SettingsType, s.ServiceType, s.ImplementationType, s)))
            {
                o.Container.Register(settingsType, () => o.Container.GetInstance<ApiServiceSettingsFactory>().Create(settingsType));
                o.Container.RegisterConditional(serviceType, implementationType, ctx => !ctx.Handled);
                _ = o.Services.AddHttpClient(implementationType.FullName!);

                _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, $"added client {serviceSettings.Request} at {serviceSettings.Endpoint}");
            }

            _ = appBuilder.LogFeature<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(feature, "added services");
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
