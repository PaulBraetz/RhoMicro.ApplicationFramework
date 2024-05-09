namespace RhoMicro.ApplicationFramework.Composition;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IConfiguration"/> infrastructure to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configBuilder"></param>
    /// <returns></returns>
    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfigurationBuilder configBuilder) => services
        .AddSingleton(configBuilder)
        .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
        .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>());
}
