namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// Default implementation of <see cref="ILoggingBuilder"/>.
/// </summary>
/// <param name="services">The service collection underlying the logging builder.</param>
public sealed class LoggingBuilder(IServiceCollection services) : ILoggingBuilder
{
    /// <inheritdoc/>
    public IServiceCollection Services { get; } = services;
}
