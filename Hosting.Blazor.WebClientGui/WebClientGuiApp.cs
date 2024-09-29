namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Common.Environment;

using RhoMicro.ApplicationFramework.Composition;

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
    public static WebClientGuiAppBuilder CreateBuilder(Action<WebClientGuiAppBuilderCreationOptions>? configure = null)
    {
        //we create/clone options because the environment can only be queried 
        //after creating an instance of the builder, which requires args in the 
        //first place
        var options = CreateBuilderCreationOptions(configure);
        var builder = WebAssemblyHostBuilder.CreateDefault(options.Args);
        var configuredOptions = ConfigureBuilderCreationOptions(options, configure, builder);
        var capabilities = CreateCapabilities(configuredOptions, builder);

        return new WebClientGuiAppBuilder(builder, capabilities).LogFeature("WebClientGuiAppBuilder", "built");
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static WebClientGuiAppBuilder CreateBuilder(out WebClientGuiAppBuilder builder, Action<WebClientGuiAppBuilderCreationOptions>? configure = null)
    {
        builder = CreateBuilder(configure);

        return builder;
    }

    private static BlazorAppBuilderCapabilities CreateCapabilities(WebClientGuiAppBuilderCreationOptions options, WebAssemblyHostBuilder builder)
    {
        var environment = options.EnvironmentConfiguration;

        var capabilities = new BlazorAppBuilderCapabilities()
        {
            Components = [],
            Services = builder.Services.AddConfiguration(builder.Configuration),
            Configuration = builder.Configuration,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = environment,
            SetupLoggingCallback = options.SetupLoggingCallback
        };
        return capabilities;
    }

    private static WebClientGuiAppBuilderCreationOptions ConfigureBuilderCreationOptions(
        WebClientGuiAppBuilderCreationOptions options,
        Action<WebClientGuiAppBuilderCreationOptions>? configure,
        WebAssemblyHostBuilder underlyingBuilder)
    {
        var clone = new WebClientGuiAppBuilderCreationOptions(options)
        {
            EnvironmentConfiguration = EnvironmentConfiguration.Create(underlyingBuilder.HostEnvironment.Environment),
        };
        configure?.Invoke(clone);
        return clone;
    }
    private static WebClientGuiAppBuilderCreationOptions CreateBuilderCreationOptions(Action<WebClientGuiAppBuilderCreationOptions>? configure)
    {
        var options = new WebClientGuiAppBuilderCreationOptions();
        configure?.Invoke(options);
        return options;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider() => UnderlyingApp.Services;
    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return UnderlyingApp.RunAsync();
    }
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(CancellationToken cancellationToken) =>
        throw new NotSupportedException($"{nameof(WebAssemblyHost)} does not support synchronous execution.");
}
