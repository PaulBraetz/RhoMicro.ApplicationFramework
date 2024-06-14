namespace RhoMicro.ApplicationFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using RhoMicro.ApplicationFramework.Common.Abstractions;

using SimpleInjector;

/// <summary>
/// Represents a generic app adapter.
/// </summary>
/// <typeparam name="TSelf">The type of adapter.</typeparam>
/// <typeparam name="TUnderlyingApp">The type of app being adapted.</typeparam>
/// <param name="underlyingApp">The app being adapted.</param>
/// <param name="container">The dependency injection container used.</param>
/// <param name="options">The options used for running the app.</param>
public abstract class App<TSelf, TUnderlyingApp>(
    TUnderlyingApp underlyingApp,
    Container container,
    AppRunOptions options)
    : DisposableBase
    where TSelf : App<TSelf, TUnderlyingApp>
{
    /// <summary>
    /// Gets the app underlying this wrapper.
    /// </summary>
    public TUnderlyingApp UnderlyingApp => underlyingApp;
    /// <summary>
    /// Gets a reference to this adapter instance.
    /// </summary>
    protected abstract TSelf Self { get; }
    private readonly List<Action<TUnderlyingApp, Container>> _configures = [];
    /// <summary>
    /// Applies a configuration to the underlying app. Configurations applied by repeated calls are cumulative.
    /// </summary>
    /// <param name="configure">The configuration to apply.</param>
    /// <returns>A reference to this instance; for chaining of further methods.</returns>
    public TSelf ConfigureUnderlyingApp(Action<TUnderlyingApp, Container> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        _configures.Add(configure);

        return Self;
    }
    /// <summary>
    /// Gets the service provider attached to the underlying app.
    /// </summary>
    /// <returns>The service provider attached to the underlying app.</returns>
    protected abstract IServiceProvider GetServiceProvider();
    /// <summary>
    /// Runs the underlying app asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token used to signal app execution to be cancelled (cooperatively).</param>
    /// <returns>A task representing the app execution.</returns>
    protected abstract Task RunUnderlyingApplicationAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Runs the underlying app.
    /// </summary>
    /// <param name="underlyingApp">The app to run.</param>
    /// <param name="cancellationToken">The token used to signal app execution to be cancelled (cooperatively).</param>
    protected abstract void RunUnderlyingApplication(CancellationToken cancellationToken);
    /// <summary>
    /// Runs the app adapter asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token used to signal app execution to be cancelled (cooperatively).</param>
    /// <returns>A task representing the app execution.</returns>
    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        VerifyAndConfigure(cancellationToken);

        var result = RunUnderlyingApplicationAsync(cancellationToken);

        return result;
    }
    /// <summary>
    /// Runs the app adapter.
    /// </summary>
    /// <param name="cancellationToken">The token used to signal app execution to be cancelled (cooperatively).</param>
    public void Run(CancellationToken cancellationToken = default)
    {
        VerifyAndConfigure(cancellationToken);

        RunUnderlyingApplication(cancellationToken);
    }

    private void VerifyAndConfigure(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        VerifySafe();

        _configures.ForEach(c => c.Invoke(underlyingApp, container));
    }

    private void VerifySafe()
    {
        try
        {
            Verify();
        } catch(DiagnosticVerificationException ex)
        {
            options.VerificationExceptionHandler.Invoke(ex);
        }
    }
    private void Verify()
    {
        var services = GetServiceProvider();
        _ = services.UseSimpleInjector(container);

        var service = services.GetService<ILoggerFactory>();
        ILogger logger;
        if(service == null)
        {
            logger = NullLogger.Instance;
        } else
        {
            ILogger instance = service.CreateLogger<Container>();
            logger = instance;
        }

        container.Verify(VerificationOption.VerifyOnly);
        options.Logger.Log(container, logger);

        container.Verify(VerificationOption.VerifyAndDiagnose);
    }
    /// <inheritdoc/>
    protected override void DisposeManaged()
    {
        if(underlyingApp is IDisposable disposableApp)
            disposableApp.Dispose();

        container.Dispose();

        base.DisposeManaged();
    }
}
