namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;

/// <summary>
/// Represents a console line app adapter.
/// </summary>
/// <param name="underlyingApp">The generic host being adapted.</param>
/// <param name="container">The container used.</param>
/// <param name="options">The options to apply when running.</param>
public sealed class CliApp(
    IHost underlyingApp,
    Container container,
    AppRunOptions options)
    : App<CliApp, IHost>(underlyingApp, container, options)
{
    /// <inheritdoc/>
    protected override CliApp Self => this;
    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    /// <returns>A new builder instance.</returns>
    public static CliAppBuilder CreateBuilder(Action<CliAppBuilderCreationSettings>? configure = null)
    {
        var builderSettings = GetBuilderSettings(configure);
        var builder = Host.CreateApplicationBuilder(builderSettings);
        var capabilities = CreateCapabilities(builderSettings, builder);

        return new CliAppBuilder(builder, capabilities);
    }

    private static AppBuilderCapabilities CreateCapabilities(HostApplicationBuilderSettings builderSettings, HostApplicationBuilder builder)
    {
        var configBuilder = CreateConfigBuilder(builderSettings);
        var result = new AppBuilderCapabilities()
        {
            Services = builder.Services
                        .AddSingleton<IConfigurationBuilder>(configBuilder)
                        .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
                        .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>()),
            Configuration = configBuilder,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = new EnvironmentConfiguration(builderSettings.EnvironmentName ?? "")
        };

        return result;
    }

    private static ConfigurationBuilder CreateConfigBuilder(HostApplicationBuilderSettings builderSettings)
    {
        var configBuilder = new ConfigurationBuilder();
        foreach(var source in builderSettings.Configuration?.Sources ?? [])
        {
            configBuilder.Sources.Add(source);
        }

        return configBuilder;
    }

    private static HostApplicationBuilderSettings GetBuilderSettings(Action<CliAppBuilderCreationSettings>? configure)
    {
        var options = new CliAppBuilderCreationSettings()
        {
            BuilderSettings = new HostApplicationBuilderSettings()
            {
                Configuration = new()
            }
        };

        configure?.Invoke(options);

        var builderSettings = options.BuilderSettings;
        return builderSettings;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider(IHost underlyingApp)
    {
        ArgumentNullException.ThrowIfNull(underlyingApp);

        return underlyingApp.Services;
    }

    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(IHost underlyingApp, CancellationToken cancellationToken)
        => underlyingApp.RunAsync(cancellationToken);
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(IHost underlyingApp, CancellationToken cancellationToken) => underlyingApp.Run();
}
