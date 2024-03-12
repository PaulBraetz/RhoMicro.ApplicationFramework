namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Photino.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

/// <summary>
/// Adapts the <see cref="PhotinoBlazorAppBuilder"/> onto the <see cref="IApplicationBuilder{TApplication}"/> integration interface.
/// </summary>
public sealed class PhotinoBlazorAppBuilderAdapter : IApplicationBuilder<PhotinoBlazorAppAdapter>
{
    private PhotinoBlazorAppBuilderAdapter(
        IConfigurationBuilder configuration,
        LoggingBuilder logging,
        PhotinoBlazorAppBuilder builder)
    {
        Configuration = configuration;
        Logging = logging;
        _builder = builder;
    }

    private readonly PhotinoBlazorAppBuilder _builder;
    /// <summary>
    /// Gets the root components registered to the underlying <see cref="PhotinoBlazorAppBuilder"/>.
    /// </summary>
    public RootComponentList RootComponents => _builder.RootComponents;
    /// <summary>
    /// Gets the registered configuration.
    /// </summary>
    public IConfigurationBuilder Configuration { get; }
    /// <summary>
    /// Gets the registered logging builder.
    /// </summary>
    public ILoggingBuilder Logging { get; }
    /// <summary>
    /// Gets the services registered to the underlying <see cref="PhotinoBlazorAppBuilder"/>.
    /// </summary>
    public IServiceCollection Services => _builder.Services;

    internal static PhotinoBlazorAppBuilderAdapter Create(PhotinoBlazorAppBuilder builder)
    {
        var environment = CreateEnvironment();
        var configBuilder = CreateConfiguration(environment);
        var services = InitializeServices(builder.Services, environment, configBuilder);
        var logging = CreateLogging(services);

        var result = new PhotinoBlazorAppBuilderAdapter(configBuilder, logging, builder);

        return result;
    }

    private static IServiceCollection InitializeServices(IServiceCollection services, HostingEnvironment environment, IConfigurationBuilder configBuilder)
    {
        var result = services
            .AddSingleton(environment)
            .AddSingleton((p) => configBuilder.Build())
            .AddSingleton<IConfiguration>(s => s.GetRequiredService<IConfigurationRoot>());

        return result;
    }
    private static LoggingBuilder CreateLogging(IServiceCollection services) => new(services);
    private static IConfigurationBuilder CreateConfiguration(HostingEnvironment environment) =>
        new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true);
    private static HostingEnvironment CreateEnvironment() => new();

    /// <inheritdoc/>
    public PhotinoBlazorAppAdapter Build()
    {
        var app = _builder.Build();

        var env = app.Services.GetRequiredService<HostingEnvironment>();
        app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger<HostingEnvironment>()
            .LogInformation("Environment: {EnvironmentName}", env.EnvironmentName);

        return new(app);
    }
}
