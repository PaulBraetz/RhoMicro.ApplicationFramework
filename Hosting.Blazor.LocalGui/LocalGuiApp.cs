namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Configuration;

using Photino.Blazor;

using SimpleInjector;

using RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Represents a local photino blazor app adapter.
/// </summary>
/// <param name="underlyingApp">The underlying app.</param>
/// <param name="container">The container used.</param>
/// <param name="options">The options to use when running.</param>
public sealed class LocalGuiApp(
    PhotinoBlazorApp underlyingApp,
    Container container,
    AppRunOptions options)
    : App<LocalGuiApp, PhotinoBlazorApp>(underlyingApp, container, options)
{
    /// <inheritdoc/>
    protected override LocalGuiApp Self => this;

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static LocalGuiAppBuilder CreateBuilder(Action<LocalGuiAppBuilderCreationOptions>? configure = null)
    {
        var options = CreateBuilderOptions(configure);
        var builder = PhotinoBlazorAppBuilder.CreateDefault(options.Args);
        var capabilities = CreateCapabilities(options, builder);

        return new LocalGuiAppBuilder(builder, capabilities).LogFeature("LocalGuiAppBuilder", "built");
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static LocalGuiAppBuilder CreateBuilder(out LocalGuiAppBuilder appBuilder, Action<LocalGuiAppBuilderCreationOptions>? configure = null) =>
        appBuilder = CreateBuilder(configure);

    private static BlazorAppBuilderCapabilities CreateCapabilities(LocalGuiAppBuilderCreationOptions options, PhotinoBlazorAppBuilder builder)
    {
        var environment = options.EnvironmentConfiguration;

        var configBuilder = new ConfigurationBuilder();

        var capabilities = new BlazorAppBuilderCapabilities()
        {
            Components = [],
            Services = builder.Services.AddConfiguration(configBuilder),
            Configuration = configBuilder,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = environment,
            SetupLoggingCallback = options.SetupLoggingCallback
        };
        return capabilities;
    }
    private static LocalGuiAppBuilderCreationOptions CreateBuilderOptions(Action<LocalGuiAppBuilderCreationOptions>? configure)
    {
        var options = new LocalGuiAppBuilderCreationOptions();
        configure?.Invoke(options);
        return options;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider() => UnderlyingApp.Services;
    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(CancellationToken cancellationToken)
    {
        RunUnderlyingApplication(cancellationToken);

        return Task.CompletedTask;
    }
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        UnderlyingApp.Run();
    }
}
