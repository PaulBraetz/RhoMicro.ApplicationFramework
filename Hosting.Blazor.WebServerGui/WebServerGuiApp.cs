namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;

/// <summary>
/// Represents a blazor web app adapter,
/// </summary>
/// <param name="underlyingApp">The underlying app being adapted.</param>
/// <param name="container">The container used.</param>
/// <param name="options">The options to use when running.</param>
public sealed class WebServerGuiApp(
    WebApplication underlyingApp,
    Container container,
    AppRunOptions options)
    : App<WebServerGuiApp, WebApplication>(underlyingApp, container, options)
{
    /// <inheritdoc/>
    protected override WebServerGuiApp Self => this;

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebServerGuiAppBuilder CreateBuilder(Action<WebServerGuiAppBuilderCreationSettings>? configure = null)
    {
        var builderCreationSettings = CreateBuilderCreationSettings(configure);
        var builder = WebApplication.CreateBuilder(builderCreationSettings.BuilderSettings);
        var capabilities = CreateCapabilities(builderCreationSettings.BuilderSettings, builder);

        return new WebServerGuiAppBuilder(builder, capabilities);
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebServerGuiAppBuilder CreateBuilder(out WebServerGuiAppBuilder appBuilder, Action<WebServerGuiAppBuilderCreationSettings>? configure = null) =>
        appBuilder = CreateBuilder(configure);

    private static BlazorAppBuilderCapabilities CreateCapabilities(WebApplicationOptions builderSettings, WebApplicationBuilder builder)
    {
        var capabilities = new BlazorAppBuilderCapabilities()
        {
            Components = [],
            Services = builder.Services
                .AddSingleton<IConfigurationBuilder>(builder.Configuration)
                .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
                .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>()),
            Configuration = builder.Configuration,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = EnvironmentConfiguration.Create(builderSettings.EnvironmentName)
        };
        return capabilities;
    }
    private static WebServerGuiAppBuilderCreationSettings CreateBuilderCreationSettings(Action<WebServerGuiAppBuilderCreationSettings>? configure)
    {
        var builderSettings = new WebServerGuiAppBuilderCreationSettings();
        configure?.Invoke(builderSettings);
        return builderSettings;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider(WebApplication underlyingApp)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        return underlyingApp.Services;
    }

    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(WebApplication underlyingApp, CancellationToken cancellationToken) => underlyingApp.RunAsync(cancellationToken);
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(WebApplication underlyingApp, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        underlyingApp.Run();
    }
}
