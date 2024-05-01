namespace RhoMicro.ApplicationFramework.Hosting; using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides the capabilities provided by and required by app builders.
/// </summary>
public class AppBuilderCapabilities
{
    /// <summary>
    /// Gets the service collection used for configuring dependency injection.
    /// </summary>
    public required IServiceCollection Services { get; init; }
    /// <summary>
    /// Gets the builder used for configuring logging.
    /// </summary>
    public required ILoggingBuilder Logging { get; init; }
    /// <summary>
    /// Gets the builder used for configuration.
    /// </summary>
    public required IConfigurationBuilder Configuration { get; init; }
    /// <summary>
    /// Gets the execution environment configuration.
    /// </summary>
    public required IEnvironmentConfiguration EnvironmentConfiguration { get; init; }
}
