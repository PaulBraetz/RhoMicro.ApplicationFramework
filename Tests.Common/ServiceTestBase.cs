namespace RhoMicro.ApplicationFramework.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleInjector;

/// <summary>
/// Base class for testing services.
/// </summary>
/// <typeparam name="TService"></typeparam>
public abstract class ServiceTestBase<TService>
    where TService : class
{
    /// <summary>
    /// Gets the service under test.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected TService Service { get; private set; }
    private Container Container { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Sets up the instance for a test run.
    /// </summary>
    [TestInitialize]
    public virtual async Task Setup()
    {
        if(Container != null)
            await Cleanup().ConfigureAwait(continueOnCapturedContext: false);

        var container = new Container();
        container.Options.AllowOverridingRegistrations = true;
        Configure(container);
        container.Verify();

        Service = container.GetInstance<TService>();
        OnAfterConfigure(container);

        Container = container;
    }
    /// <summary>
    /// Invoked after the service has been initialized and the container is ready for resolving dependencies.
    /// </summary>
    /// <param name="container">The container used to resolve dependencies.</param>
    protected virtual void OnAfterConfigure(Container container) { }
    /// <summary>
    /// Cleans up the instance after a test run.
    /// </summary>
    [TestCleanup]
    public virtual async Task Cleanup()
    {
        if(Service is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        } else if(Service is IDisposable disposable)
        {
            disposable.Dispose();
        }

        await Container!.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
    }
    /// <summary>
    /// Configures the container used for instantiating the service under test.
    /// </summary>
    /// <param name="container">The container to configure.</param>
    protected virtual void Configure(Container container)
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register<TService>(Lifestyle.Transient);
    }
}