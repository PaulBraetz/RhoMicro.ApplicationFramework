namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using SimpleInjector;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector.Integration.ServiceCollection;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Represents a generic app adapter builder.
/// </summary>
/// <typeparam name="TSelf">The type of app adapter builder.</typeparam>
/// <typeparam name="TApp">The type of app adapter produced.</typeparam>
/// <typeparam name="TUnderlyingBuilder">The type of builder being adapted.</typeparam>
/// <typeparam name="TUnderlyingApp">The type of app being adapted.</typeparam>
public abstract class AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp>
    : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, AppBuilderCapabilities>
    where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp>
    where TApp : App<TApp, TUnderlyingApp>;

/// <summary>
/// Represents a generic app adapter builder.
/// </summary>
/// <typeparam name="TSelf">The type of app adapter builder.</typeparam>
/// <typeparam name="TApp">The type of app adapter produced.</typeparam>
/// <typeparam name="TUnderlyingBuilder">The type of builder being adapted.</typeparam>
/// <typeparam name="TUnderlyingApp">The type of app being adapted.</typeparam>
/// <typeparam name="TCapabilities">The type of capabilities this builder has.</typeparam>
public abstract class AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
    where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
    where TApp : App<TApp, TUnderlyingApp>
    where TCapabilities : AppBuilderCapabilities
{
    /// <summary>
    /// Gets the options to use when building.
    /// </summary>
    public AppBuilderOptions Options { get; } = new();
    /// <summary>
    /// Gets this builders capabilities.
    /// </summary>
    public abstract TCapabilities Capabilities { get; }
    /// <summary>
    /// Gets the builder being adapted.
    /// </summary>
    public abstract TUnderlyingBuilder UnderlyingBuilder { get; }

    /// <summary>
    /// Gets a strongly typed reference to this instance.
    /// </summary>
    protected abstract TSelf Self { get; }

    /// <summary>
    /// Configures the builder underlying an app builder. Repeated calls to this method are cumulative.
    /// </summary>
    /// <param name="configure">The configuration to apply.</param>
    /// <returns>A reference to this instance; for chaining of further methods.</returns>
    public TSelf ConfigureBuilder(Action<TUnderlyingBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        configure.Invoke(UnderlyingBuilder);

        OnAfterConfigureBuilder();

        return Self;
    }
    /// <summary>
    /// Invoked after each call to <see cref="ConfigureBuilder(Action{TUnderlyingBuilder})"/>.
    /// </summary>
    protected virtual void OnAfterConfigureBuilder() { }
    /// <summary>
    /// Configures the capabilities attached to an app builder. Repeated calls to this method are cumulative.
    /// </summary>
    /// <param name="configure">The configuration to apply.</param>
    /// <returns>A reference to this instance; for chaining of further methods.</returns>
    public TSelf ConfigureCapabilities(Action<TCapabilities> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        configure.Invoke(Capabilities);

        OnAfterConfigureCapabilities();

        return Self;
    }
    /// <summary>
    /// Invoked after each call to <see cref="ConfigureCapabilities(Action{TCapabilities})"/>.
    /// </summary>
    protected virtual void OnAfterConfigureCapabilities() { }
    /// <summary>
    /// Configures the options attached to an app builder. Repeated calls to this method are cumulative.
    /// </summary>
    /// <param name="configure">The configuration to apply.</param>
    /// <returns>A reference to this instance; for chaining of further methods.</returns>
    public TSelf ConfigureOptions(Action<AppBuilderOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        configure.Invoke(Options);

        OnAfterConfigureOptions();

        return Self;
    }
    /// <summary>
    /// Invoked after each call to <see cref="ConfigureOptions(Action{AppBuilderOptions})"/>.
    /// </summary>
    protected virtual void OnAfterConfigureOptions() { }
    /// <summary>
    /// Builds an underlying app from an underlying app builder.
    /// </summary>
    /// <returns>The built app.</returns>
    protected abstract TUnderlyingApp BuildUnderlyingApp();
    /// <summary>
    /// Adapts a built underlying app.
    /// </summary>
    /// <param name="underlyingApp">The underlying app to adapt.</param>
    /// <param name="container">The container used to create the app.</param>
    /// <param name="appRunOptions">The options to apply when running the adapted app.</param>
    /// <returns>The adapted app.</returns>
    protected abstract TApp CreateApp(TUnderlyingApp underlyingApp, Container container, AppRunOptions appRunOptions);

    /// <summary>
    /// Invoked after the container has been created but before it is configured.
    /// </summary>
    /// <param name="container">The container used.</param>
    protected virtual void OnBeforeContainerComposed(Container container) { }

    /// <summary>
    /// Invoked after the container has been configured.
    /// </summary>
    /// <param name="container">The container used.</param>
    protected virtual void OnContainerComposed(Container container) { }

    /// <summary>
    /// Builds an app adapter instance.
    /// </summary>
    /// <returns>A new app adapter instance.</returns>
    public TApp Build()
    {
        //result app should dispose container
#pragma warning disable CA2000 // Dispose objects before losing scope
        var container = CreateContainer();
#pragma warning restore CA2000 // Dispose objects before losing scope
        var underlyingApp = BuildUnderlyingApp();
        var result = CreateApp(underlyingApp, container, Options.AppRunOptions);

        return result;
    }

    /// <summary>
    /// Invoked upon the simple injector <see cref="Container"/> being added to the builders service collection in <see cref="Capabilities"/>.
    /// </summary>
    /// <param name="options">The option used for integrating the simple injector <see cref="Container"/>.</param>
    protected virtual void OnSimpleInjectorAdd(SimpleInjectorAddOptions options) { }

    private void OnSimpleInjectorAddCore(SimpleInjectorAddOptions options)
    {
        Options.InvokeOnContainerAdd(options);
        OnSimpleInjectorAdd(options);
    }

    /// <summary>
    /// Invoked after the container has been created and before any services have been registered.
    /// </summary>
    protected virtual void OnAfterContainerCreated(Container container) { }

    private Container CreateContainer()
    {
        var container = new Container();

        OnAfterContainerCreated(container);

        var services = Capabilities.Services
            .AddSimpleInjector(container, OnSimpleInjectorAddCore)
            .AddScoped(provider => provider.GetRequiredService<IServiceScopeFactory>().CreateScope());

        AddEnvironmentServices(services);

        OnBeforeContainerComposed(container);

        Options.InvokeOnBeforeContainerComposed(container.Options);
        Options.Composer.Compose(container);

        OnContainerComposed(container);

        return container;
    }

    /// <summary>
    /// Gets the deployment platform provider to register to the di container.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to resolve services from the di container.
    /// </param>
    /// <returns>
    /// The <see cref="IDeploymentPlatformProvider"/> to instance inject.
    /// </returns>
    protected virtual IDeploymentPlatformProvider GetDeploymentPlatformProvider(IServiceProvider serviceProvider) =>
        new DeploymentPlatformProvider(DeploymentPlatform.Unknown);

    private void AddEnvironmentServices(IServiceCollection services)
    {
        _ = services
            .AddSingleton(Capabilities.EnvironmentConfiguration)
            .AddSingleton<IOperatingSystemInfo>(new OperatingSystemInfo())
            .AddSingleton(GetDeploymentPlatformProvider)
            .AddSingleton<IExecutionEnvironment>(static sp =>
            {
                var environmentConfig = sp.GetRequiredService<IEnvironmentConfiguration>();
                var osInfo = sp.GetRequiredService<IOperatingSystemInfo>();
                var deploymentPlatform = sp.GetRequiredService<IDeploymentPlatformProvider>().DeploymentPlatform;

                var result = new ExecutionEnvironment(environmentConfig, osInfo, deploymentPlatform);

                return result;
            });
    }
}
