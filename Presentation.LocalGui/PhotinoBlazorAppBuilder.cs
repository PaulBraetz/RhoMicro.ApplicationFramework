namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.IO;
using System.Net.Http;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

using PhotinoNET;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Builder for photino applications.
/// </summary>
public sealed class PhotinoBlazorAppBuilder : IApplicationBuilder<PhotinoBlazorApp>
{
    private PhotinoBlazorAppBuilder(
        IServiceCollection services,
        IHostingEnvironment environment,
        IConfigurationBuilder configuration,
        LoggingBuilder logging)
    {
        RootComponents = new();
        Services = services;
        Environment = environment;
        Configuration = configuration;
        Logging = logging;
    }

    /// <summary>
    /// Creates a new <see cref="PhotinoBlazorAppBuilder"/> using the program arguments provided.
    /// </summary>
    /// <returns></returns>
    public static PhotinoBlazorAppBuilder CreateDefault(params String[] args)
    {
        var environment = CreateEnvironment();
        var configBuilder = CreateConfiguration(environment);
        var services = CreateServices(environment, configBuilder);
        var logging = CreateLogging(services);
        var builder = new PhotinoBlazorAppBuilder(services, environment, configBuilder, logging);

        return builder;
    }

    private static ServiceCollection CreateServices(HostingEnvironment environment, IConfigurationBuilder configBuilder)
    {
        var result = new ServiceCollection();

        _ = result
            .AddSingleton<IHostingEnvironment>(environment)
            .AddSingleton((p) => configBuilder.Build())
            .AddSingleton<IConfiguration>(s => s.GetRequiredService<IConfigurationRoot>())
            .AddOptions<PhotinoBlazorAppConfiguration>()
            .Configure(opts =>
            {
                opts.AppBaseUri = new Uri(PhotinoWebViewManager.AppBaseUri);
                opts.HostPage = "index.html";
            });

        _ = result
            .AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<PhotinoHttpHandler>();
                return new HttpClient(handler) { BaseAddress = new Uri(PhotinoWebViewManager.AppBaseUri) };
            })
            .AddSingleton(sp =>
            {
                var manager = sp.GetRequiredService<PhotinoWebViewManager>();
                var store = sp.GetRequiredService<JSComponentConfigurationStore>();

                return new BlazorWindowRootComponents(manager, store);
            })
            .AddSingleton<Dispatcher, PhotinoDispatcher>()
            .AddSingleton<IFileProvider>(_ =>
            {
                var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
                return new PhysicalFileProvider(root);
            })
            .AddSingleton<JSComponentConfigurationStore>()
            .AddSingleton<PhotinoBlazorAppFactory>()
            .AddSingleton<PhotinoHttpHandler>()
            .AddSingleton<PhotinoSynchronizationContext>()
            .AddSingleton<PhotinoWebViewManager>()
            .AddSingleton(new PhotinoWindow())
            .AddBlazorWebView();

        return result;
    }
    private static LoggingBuilder CreateLogging(ServiceCollection services) =>
        new(services);
    private static IConfigurationBuilder CreateConfiguration(HostingEnvironment environment) =>
        new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true);
    private static HostingEnvironment CreateEnvironment() => new();

    /// <summary>
    /// Gets the root component collection.
    /// </summary>
    public RootComponentList RootComponents { get; }
    /// <summary>
    /// Gets the builder used for building up application configuration.
    /// </summary>
    public IConfigurationBuilder Configuration { get; private set; }
    /// <summary>
    /// Gets the builder used for building up application logging.
    /// </summary>
    public ILoggingBuilder Logging { get; private set; }
    /// <summary>
    /// Gets the service collection composing the applications object graph.
    /// </summary>
    public IServiceCollection Services { get; }
    /// <summary>
    /// Gets the applications hosting environment.
    /// </summary>
    public IHostingEnvironment Environment { get; }

    /// <summary>
    /// Builds a new <see cref="PhotinoBlazorApp"/>.
    /// </summary>
    public PhotinoBlazorApp Build()
    {
        // register root components with DI container
        // Services.AddSingleton(RootComponents);

        var sp = Services.BuildServiceProvider();
        var app = sp.GetRequiredService<PhotinoBlazorAppFactory>().Create(sp, RootComponents);

        var env = app.Services.GetRequiredService<IHostingEnvironment>();

#pragma warning disable CA1848 // Use the LoggerMessage delegates
        app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger<IHostingEnvironment>()
            .LogInformation("Environment: {Environment}", env.EnvironmentName);
#pragma warning restore CA1848 // Use the LoggerMessage delegates

        return app;
    }
}
