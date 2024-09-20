namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    public static WebServerGuiAppBuilder CreateBuilder(Action<WebServerGuiAppBuilderCreationOptions>? configure = null)
    {
        var options = CreateBuilderCreationOptions(configure);
        var builder = WebApplication.CreateBuilder(options.AppOptions);
        var capabilities = CreateCapabilities(options, builder);

        return new WebServerGuiAppBuilder(builder, capabilities).LogFeature("WebServerGuiAppBuilder", "built");
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebServerGuiAppBuilder CreateBuilder(out WebServerGuiAppBuilder appBuilder, Action<WebServerGuiAppBuilderCreationOptions>? configure = null) =>
        appBuilder = CreateBuilder(configure);

    private static BlazorAppBuilderCapabilities CreateCapabilities(WebServerGuiAppBuilderCreationOptions options, WebApplicationBuilder builder)
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
            EnvironmentConfiguration = EnvironmentConfiguration.Create(options.AppOptions.EnvironmentName),
            SetupLoggingCallback = options.SetupLoggingCallback
        };
        return capabilities;
    }
    private static WebServerGuiAppBuilderCreationOptions CreateBuilderCreationOptions(Action<WebServerGuiAppBuilderCreationOptions>? configure)
    {
        var options = new WebServerGuiAppBuilderCreationOptions();
        configure?.Invoke(options);
        return options;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider() => UnderlyingApp.Services;
    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(CancellationToken cancellationToken)
    {
        var result = UnderlyingApp.RunAsync(cancellationToken);

        return result;
    }
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(CancellationToken cancellationToken) => UnderlyingApp.Run();
}
