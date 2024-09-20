namespace RhoMicro.ApplicationFramework.Hosting;

using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    public static CliAppBuilder CreateBuilder(Action<CliAppBuilderCreationOptions>? configure = null)
    {
        var options = GetBuilderCreationOptions(configure);
        var builder = Host.CreateApplicationBuilder(options.BuilderSettings);
        var capabilities = CreateCapabilities(options, builder);

        return new CliAppBuilder(builder, capabilities).LogFeature("CliAppBuilder", "built");
    }

    private static AppBuilderCapabilities CreateCapabilities(CliAppBuilderCreationOptions settings, HostApplicationBuilder builder)
    {
        var configBuilder = CreateConfigBuilder(settings.BuilderSettings);
        var result = new AppBuilderCapabilities()
        {
            Services = builder.Services
                        .AddSingleton<IConfigurationBuilder>(configBuilder)
                        .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
                        .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>()),
            Configuration = configBuilder,
            Logging = new LoggingBuilder(builder.Services),
            EnvironmentConfiguration = EnvironmentConfiguration.Create(settings.BuilderSettings.EnvironmentName),
            SetupLoggingCallback = settings.SetupLoggingCallback
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

    private static CliAppBuilderCreationOptions GetBuilderCreationOptions(Action<CliAppBuilderCreationOptions>? configure)
    {
        var options = new CliAppBuilderCreationOptions()
        {
            BuilderSettings = new HostApplicationBuilderSettings()
            {
                Configuration = new()
            }
        };

        configure?.Invoke(options);

        return options;
    }

    /// <inheritdoc/>
    protected override IServiceProvider GetServiceProvider() => UnderlyingApp.Services;
    /// <inheritdoc/>
    protected override Task RunUnderlyingApplicationAsync(CancellationToken cancellationToken) => UnderlyingApp.RunAsync(cancellationToken);
    /// <inheritdoc/>
    protected override void RunUnderlyingApplication(CancellationToken cancellationToken) => UnderlyingApp.Run();
}
