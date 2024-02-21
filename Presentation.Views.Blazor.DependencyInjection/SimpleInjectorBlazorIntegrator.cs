namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System.Reflection;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Logging;

using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Helper class used to integrate SimpleInjector into the build process of a web application. The instance used for integration must be kept alive for the duration of the application.
/// </summary>
public sealed class SimpleInjectorBlazorIntegrator : IDisposable
{
    private readonly Container _container = new();
    private readonly IContainerLogger _logger;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public SimpleInjectorBlazorIntegrator(IContainerLogger? logger = null)
    {
        _container.Options.PropertySelectionBehavior = new DependencyAttributePropertySelectionBehavior();

        _logger = logger ?? new ContainerDiagnosticsLogger();
    }

    /// <summary>
    /// Creates a new integrator
    /// </summary>
    /// <returns></returns>
    public static SimpleInjectorBlazorIntegrator CreateDefault()
    {
        var result = new SimpleInjectorBlazorIntegrator(
            new CompositeContainerLogger(
                new ContainerDiagnosticsLogger(),
                new FakeWarningsLogger()
#if !DEBUG
                ,new RootGraphLogger()
#endif
                ));

        return result;
    }

    /// <summary>
    /// Integrates SimpleInjector into the application and builds it.
    /// </summary>
    /// <param name="builder">The builder used for building the application.</param>
    /// <param name="compositionRoot">The object graph root to configure the underlying service collection with.</param>
    /// <param name="componentAssemblies">The assemblies to search for implementations of <see cref="IComponent"/> (Blazor components). Types located will be registered to the container.</param>
    /// <returns>An instance of <typeparamref name="TApplication"/>, as built by <paramref name="builder"/>.</returns>
    public TApplication Integrate<TApplication>(IApplicationBuilder<TApplication> builder, IRoot compositionRoot, params Assembly[] componentAssemblies)
        where TApplication : IApplication
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(compositionRoot);

        Initialize(builder.Services, compositionRoot, componentAssemblies);
        var result = builder.Build();
        Finalize(result.Services);
        return result;
    }

    /// <summary>
    /// Configures an instance of <see cref="IServiceCollection"/> before the application is built.
    /// </summary>
    /// <param name="services">The service collection to which to add SimpleInjector.</param>
    /// <param name="compositionRoot">The object graph root to configure the underlying service collection with.</param>
    /// <param name="componentAssemblies">The assemblies to search for implementations of <see cref="IComponent"/>. Types found will be registered in the container.</param>
    private void Initialize(IServiceCollection services, IRoot compositionRoot, params Assembly[] componentAssemblies)
    {
#pragma warning disable IDE0053 // Use expression body for lambda expression
        _ = services.AddSimpleInjector(_container, options =>
        {
            // If you plan on adding AspNetCore as well, change the
            // ServiceScopeReuseBehavior to OnePerNestedScope as follows:
            // options.AddAspNetCore(ServiceScopeReuseBehavior.OnePerNestedScope);

            AddServerSideBlazor(options, componentAssemblies);
        });
#pragma warning restore IDE0053 // Use expression body for lambda expression

        compositionRoot.Compose(_container);
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
        _logger.Log(_container, containerLogger);

        //verify to fail fast after diagnostics logs have been written
        _container.Verify();
    }

    private static void AddServerSideBlazor(
        SimpleInjectorAddOptions options, params Assembly[] assemblies)
    {
        var services = options.Services;

        // Unfortunate nasty hack. We reported this with Microsoft.
        _ = services
            .AddTransient(
                typeof(Microsoft.AspNetCore.Components.Server.CircuitOptions)
                .Assembly
                .GetTypes()
                .First(t => t.FullName == "Microsoft.AspNetCore.Components.Server.ComponentHub"))
            .AddScoped(typeof(IHubActivator<>), typeof(SimpleInjectorBlazorHubActivator<>))
            .AddScoped<IComponentActivator, SimpleInjectorComponentActivator>()
            .AddScoped<ScopeAccessor>()
            .AddTransient<ServiceScopeApplier>();

        RegisterBlazorComponents(options, assemblies);
    }

    private static void RegisterBlazorComponents(
        SimpleInjectorAddOptions options, Assembly[] assemblies)
    {
        var container = options.Container;
        var types = container.GetTypesToRegister<IComponent>(
            assemblies,
            new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true });

        foreach(var type in types)
        {
            if(type.IsGenericTypeDefinition)
            {
                container.Register(type, type, Lifestyle.Transient);
                continue;
            }

            var registration =
                Lifestyle.Transient.CreateRegistration(type, container);

            registration.SuppressDiagnosticWarning(
                DiagnosticType.DisposableTransientComponent,
                "Blazor will dispose components.");

            container.AddRegistration(type, registration);
        }
    }

    /// <inheritdoc/>
    public void Dispose() => _container.Dispose();
}
