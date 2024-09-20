namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Linq.Expressions;
using System.Reflection;

using RhoMicro.ApplicationFramework.Common;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleInjector.Lifestyles;
using System.Text.Json;
using RhoMicro.RequiredPropertyValidation.RhoMicro.RequiredPropertyValidation;
using NReco.Logging.File;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
#pragma warning disable CA1724
public static class Extensions
{
    /// <summary>
    /// Logs to the app builders setup logging callback a message about a feature.
    /// </summary>
    public static WebServerGuiAppBuilder LogFeature(this WebServerGuiAppBuilder appBuilder, String feature, String message) =>
        appBuilder.LogFeature<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(feature, message);
    /// <summary>
    /// Registers a platform-specific clipboard implementation to the builder services.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebServerGuiAppBuilder AddClipboard(this WebServerGuiAppBuilder appBuilder) =>
        appBuilder.AddClipboard<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds validation that assures all required non-null properties on resolved instances are not null.
    /// </summary>
    public static WebServerGuiAppBuilder AddRequiredPropertyValidation(this WebServerGuiAppBuilder appBuilder, Action<RequiredPropertyValidationConfiguration>? configure = null) =>
        appBuilder.AddRequiredPropertyValidation<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configure);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the lifestyle provided.
    /// </summary>
    public static WebServerGuiAppBuilder AddTimeout(this WebServerGuiAppBuilder appBuilder, Lifestyle lifestyle) =>
        appBuilder.AddTimeout<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(lifestyle);
    /// <summary>
    /// Adds timeout aspects and related configuration to the application using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static WebServerGuiAppBuilder AddTimeout(this WebServerGuiAppBuilder appBuilder) =>
        appBuilder.AddTimeout(Lifestyle.Scoped);
    /// <summary>
    /// Adds blazor to the web server app builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebServerGuiAppBuilder AddBlazor(this WebServerGuiAppBuilder appBuilder) =>
        appBuilder.AddBlazor<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
    /// <summary>
    /// Adds api services to the app builder.
    /// </summary>
    /// <param name="appBuilder">The builder to add api services to.</param>
    /// <param name="configureClients">Callback for configuring which kinds of api clients to register.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebServerGuiAppBuilder AddApiServiceClients(this WebServerGuiAppBuilder appBuilder, Action<ApiServiceOptions> configureClients) =>
        appBuilder.AddApiServiceClients<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(configureClients);
    /// <summary>
    /// Adds and configures conventional api service endpoints.
    /// </summary>
    /// <param name="builder">The builder to add endpoint handlers to.</param>
    /// <param name="configureEndpoints">Callback for configuring endpoint settings.</param>
    /// <returns>A reference to the builder, for chaining of further method calls.</returns>
    public static WebServerGuiAppBuilder AddApiServiceEndpoints(this WebServerGuiAppBuilder builder, Action<ApiServiceOptions>? configureEndpoints = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        const String feature = "ApiServiceEndpoints";

        builder.Options.OnContainerAdd += (o) =>
        {
            var options = new ApiServiceOptions();
            configureEndpoints?.Invoke(options);

            _ = o.Services
                .AddTransient(sp =>
                    sp.GetRequiredService<IOptions<ApiServicesSettings>>().Value)
                .AddOptions<ApiServicesSettings>()
                .BindConfiguration("ApiServicesSettings")
                .Validate(
                    s => s.GetIsValid(ignoreBaseUri: true),
                    "Endpoints with invalid request type or request uri detected. Service endpoint uris must be well-formed relative uris.")
                .ValidateOnStart();

            _ = builder.LogFeature(feature, "added options");

            o.Container.Register<ApiServiceEndpointHandlerMetadataProvider>();
            o.Container.RegisterInstance(new ApiServiceEndpointHandlerSettings(options.SerializerOptions));

            _ = builder.LogFeature(feature, "added services");
        };

        return builder.LogFeature(feature, "added");
    }
    /// <summary>
    /// Maps configured api service endpoints.
    /// </summary>
    /// <param name="app">The app to map endpoints on.</param>
    /// <returns>A reference to the app, for chaining of further method calls.</returns>
    public static WebServerGuiApp MapApiServiceEndpoints(this WebServerGuiApp app)
    {
        ArgumentNullException.ThrowIfNull(app);

        _ = app.ConfigureUnderlyingApp((app, container) =>
        {
            using var scope = AsyncScopedLifestyle.BeginScope(container);

            var logger = scope.GetInstance<ILogger>();

            container.GetInstance<ApiServiceEndpointHandlerMetadataProvider>()
                .GetMetadata()
                .ForEach(d =>
                {
                    var (handlerType, route) = d;
                    var contextExpr = Expression.Parameter(typeof(HttpContext), "context");
                    var servicesExpr = Expression.Constant(container);
                    var createInstanceMethod = typeof(ActivatorUtilities)
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name is nameof(ActivatorUtilities.CreateInstance) && m.IsGenericMethodDefinition)
                        .Single()
                        .MakeGenericMethod(handlerType);
                    var parametersExpr = Expression.Constant(Array.Empty<Object>());
                    var createInstanceExpr = Expression.Call(createInstanceMethod, servicesExpr, parametersExpr);
                    var handleExpr = Expression.Call(createInstanceExpr, "Handle", null, contextExpr);
                    var lambda = Expression.Lambda<RequestDelegate>(handleExpr, contextExpr);
                    var handler = lambda.Compile();

                    _ = app.MapPost(route, handler);
                    
                    logger.LogInformation("Mapping {Handler} to {Route}", ( handlerType.GenericTypeArguments.FirstOrDefault() ?? handlerType ).Name, route);
                });
        });

        return app;
    }
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
    public static WebServerGuiAppBuilder AddConsoleLogging(this WebServerGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder.AddLogging<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>();
        _ = appBuilder.Capabilities.Logging.AddConsole(configureOptions ?? ( static o => { } ));

        return appBuilder.LogFeature("ConsoleLogging", "added");
    }
    /// <summary>
    /// Adds all services implementing <see cref="IHostedService"/> from the assemblies provided as hosted services to the app being built.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static WebServerGuiAppBuilder AddHostedServices(this WebServerGuiAppBuilder appBuilder, params Assembly[] assemblies) =>
        appBuilder.AddHostedServices<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication, BlazorAppBuilderCapabilities>(assemblies);
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
}
