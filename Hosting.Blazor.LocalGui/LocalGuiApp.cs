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
    public static LocalGuiAppBuilder CreateBuilder(Action<LocalGuiAppBuilderCreationSettings>? configure = null)
    {
        var builderSettings = CreateBuilderSettings(configure);
        var builder = PhotinoBlazorAppBuilder.CreateDefault(builderSettings.Args);
        var capabilities = CreateCapabilities(builderSettings, builder);

        return new LocalGuiAppBuilder(builder, capabilities);
    }
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static LocalGuiAppBuilder CreateBuilder(out LocalGuiAppBuilder appBuilder, Action<LocalGuiAppBuilderCreationSettings>? configure = null) =>
        appBuilder = CreateBuilder(configure);

    private static BlazorAppBuilderCapabilities CreateCapabilities(LocalGuiAppBuilderCreationSettings builderSettings, PhotinoBlazorAppBuilder builder)
    {
        var environment = builderSettings.EnvironmentConfiguration;

        var configBuilder = new ConfigurationBuilder();

        var capabilities = new BlazorAppBuilderCapabilities()
        {
            Components = [],
            Services = builder.Services.AddConfiguration(configBuilder),
            Configuration = configBuilder,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = environment
        };
        return capabilities;
    }
    private static LocalGuiAppBuilderCreationSettings CreateBuilderSettings(Action<LocalGuiAppBuilderCreationSettings>? configure)
    {
        var builderSettings = new LocalGuiAppBuilderCreationSettings();
        configure?.Invoke(builderSettings);
        return builderSettings;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider(PhotinoBlazorApp underlyingApp)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        return underlyingApp.Services;
    }

    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(PhotinoBlazorApp underlyingApp, CancellationToken cancellationToken)
    {
        RunUnderlyingApplication(underlyingApp, cancellationToken);

        return Task.CompletedTask;
    }
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(PhotinoBlazorApp underlyingApp, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        cancellationToken.ThrowIfCancellationRequested();

        underlyingApp.Run();
    }
}
