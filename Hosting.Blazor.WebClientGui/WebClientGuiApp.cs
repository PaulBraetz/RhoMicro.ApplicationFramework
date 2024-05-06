namespace RhoMicro.ApplicationFramework.Hosting;

using System.Security;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;

/// <summary>
/// Represents a blazor wasm app adapter,
/// </summary>
/// <param name="underlyingApp">The underlying app being adapted.</param>
/// <param name="container">The container used.</param>
/// <param name="options">The options to use when running.</param>
public sealed class WebClientGuiApp(
    WebAssemblyHost underlyingApp,
    Container container,
    AppRunOptions options)
    : App<WebClientGuiApp, WebAssemblyHost>(underlyingApp, container, options)
{
    /// <inheritdoc/>
    protected override WebClientGuiApp Self => this;

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebClientGuiAppBuilder CreateBuilder(Action<WebClientGuiAppBuilderCreationSettings>? configure = null)
    {
        var builderSettings = CreateBuilderCreationSettings(configure);
        var builder = WebAssemblyHostBuilder.CreateDefault(builderSettings.Args);
        var configuredBuilderSettings = ConfigureBuilderCreationSettings(builderSettings, configure, builder);
        var capabilities = CreateCapabilities(configuredBuilderSettings, builder);

        return new WebClientGuiAppBuilder(builder, capabilities);
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebClientGuiAppBuilder CreateBuilder(out WebClientGuiAppBuilder builder, Action<WebClientGuiAppBuilderCreationSettings>? configure = null)
    {
        builder = CreateBuilder(configure);

        return builder;
    }

    private static BlazorAppBuilderCapabilities CreateCapabilities(WebClientGuiAppBuilderCreationSettings builderSettings, WebAssemblyHostBuilder builder)
    {
        var environment = builderSettings.EnvironmentConfiguration;

        var capabilities = new BlazorAppBuilderCapabilities()
        {
            Components = [],
            Services = builder.Services.AddConfiguration(builder.Configuration),
            Configuration = builder.Configuration,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = environment
        };
        return capabilities;
    }

    private static WebClientGuiAppBuilderCreationSettings ConfigureBuilderCreationSettings(
        WebClientGuiAppBuilderCreationSettings builderSettings,
        Action<WebClientGuiAppBuilderCreationSettings>? configure,
        WebAssemblyHostBuilder underlyingBuilder)
    {
        var clone = new WebClientGuiAppBuilderCreationSettings(builderSettings)
        {
            EnvironmentConfiguration = EnvironmentConfiguration.Create(underlyingBuilder.HostEnvironment.Environment)
        };
        configure?.Invoke(clone);
        return clone;
    }
    private static WebClientGuiAppBuilderCreationSettings CreateBuilderCreationSettings(Action<WebClientGuiAppBuilderCreationSettings>? configure)
    {
        var builderSettings = new WebClientGuiAppBuilderCreationSettings();
        configure?.Invoke(builderSettings);
        return builderSettings;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider(WebAssemblyHost underlyingApp)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        return underlyingApp.Services;
    }

    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(WebAssemblyHost underlyingApp, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        cancellationToken.ThrowIfCancellationRequested();

        return underlyingApp.RunAsync();
    }
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(WebAssemblyHost underlyingApp, CancellationToken cancellationToken) =>
        throw new NotSupportedException($"{nameof(WebAssemblyHost)} does not support synchronous execution.");
}
