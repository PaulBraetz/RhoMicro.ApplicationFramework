namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using SimpleInjector;

/// <summary>
/// Helper class used to integrate SimpleInjector into the build process of a web application. The instance used for integration must be kept alive for the duration of the application.
/// </summary>
public class SimpleInjectorBlazorIntegrator : IDisposable
{
    private readonly Container _container = new();
    /// <summary>
    /// Gets the integration strategy for this integrator.
    /// </summary>
    protected IIntegrationStrategy Strategy { get; }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public SimpleInjectorBlazorIntegrator(IIntegrationStrategy strategy)
    {
        _container.Options.PropertySelectionBehavior = new DependencyAttributePropertySelectionBehavior();
        Strategy = strategy;
    }

    /// <summary>
    /// Integrates SimpleInjector into the application and builds it.
    /// </summary>
    /// <param name="builder">The builder used for building the application.</param>
    /// <returns>An instance of <typeparamref name="TApplication"/>, as built by <paramref name="builder"/>.</returns>
    public TApplication Integrate<TApplication>(IApplicationBuilder<TApplication> builder)
        where TApplication : IApplication
    {
        ArgumentNullException.ThrowIfNull(builder);

        Initialize(builder.Services);
        var result = builder.Build();
        FinalizeSafe(result.Services);
        return result;
    }

    /// <summary>
    /// Configures an instance of <see cref="IServiceCollection"/> before the application is built.
    /// </summary>
    /// <param name="services">The service collection to which to add SimpleInjector.</param>
    private void Initialize(IServiceCollection services)
    {
        _ = services.AddSimpleInjector(_container, Strategy.SimpleInjectorSetup)
            .AddScoped(provider => provider.GetRequiredService<IServiceScopeFactory>().CreateScope());

        Strategy.Composer.Compose(_container);
    }

    /// <summary>
    /// Configures an instance of <see cref="IServiceProvider"/> after the application has been built.
    /// Notifies the strategy about <see cref="DiagnosticVerificationException"/>.
    /// </summary>
    /// <param name="services">The service provider to configure.</param>
    private void FinalizeSafe(IServiceProvider services)
    {
        try
        {
            Finalize(services);
        } catch(DiagnosticVerificationException ex)
        {
            Strategy.NotifyVerificationError(ex);
            throw;
        }
    }

    /// <summary>
    /// Configures an instance of <see cref="IServiceProvider"/> after the application has been built.
    /// </summary>
    /// <param name="services">The service provider to configure.</param>
    private void Finalize(IServiceProvider services)
    {
        _ = services.UseSimpleInjector(_container);

        //verify only to enable analysis
        _container.Verify(VerificationOption.VerifyOnly);

        var loggerFactory = services.GetService<ILoggerFactory>();
        ILogger containerLogger = loggerFactory != null ?
            loggerFactory.CreateLogger<Container>() :
            NullLogger.Instance;
        Strategy.ContainerLogger.Log(_container, containerLogger);

        //verify to fail fast after diagnostics logs have been written
        _container.Verify();
    }

    /// <inheritdoc/>
    public void Dispose() => _container.Dispose();
}
