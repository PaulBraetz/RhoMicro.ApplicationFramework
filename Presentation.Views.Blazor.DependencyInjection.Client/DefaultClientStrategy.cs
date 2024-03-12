namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;
using System.Reflection;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Logging;
using RhoMicro.CodeAnalysis;

using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Provides a default integration strategy for client applications.
/// </summary>
/// <param name="composer"></param>
/// <param name="componentAssemblies"></param>
/// <param name="containerLogger"></param>
public partial class DefaultClientStrategy(
    IComposer composer,
    IEnumerable<Assembly> componentAssemblies,
    IContainerLogger containerLogger) : IIntegrationStrategy
{
    /// <summary>
    /// Creates an instance of the <see cref="DefaultClientStrategy"/>.
    /// </summary>
    /// <param name="composer"></param>
    /// <param name="componentAssemblies"></param>
    /// <returns></returns>
    public static DefaultClientStrategy CreateClient(IComposer composer, IEnumerable<Assembly> componentAssemblies) =>
        new(Composition.Composer.Create(c =>
        {
            c.RegisterSingleton<IRenderModeInterceptor, RenderModeInterceptor>();
            composer.Compose(c);
        }), componentAssemblies, CompositeContainerLogger.Default)
        {
            IsDefault = true
        };

    /// <summary>
    /// Gets a value indicating whether this instance was created using a default factory.
    /// </summary>
    protected Boolean IsDefault { get; init; }

    /// <inheritdoc/>
    public IComposer Composer { get; } = composer;
    /// <inheritdoc/>
    public IEnumerable<Assembly> ComponentAssemblies { get; } = componentAssemblies;
    /// <inheritdoc/>
    public IContainerLogger ContainerLogger => containerLogger;
    /// <inheritdoc/>
    public virtual void NotifyVerificationError(DiagnosticVerificationException exception) { }
    /// <inheritdoc/>
    public virtual void SimpleInjectorSetup(SimpleInjectorAddOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        AddBlazor(options);
        RegisterBlazorComponents(options);
    }

    private static void AddBlazor(SimpleInjectorAddOptions options)
    {
        var services = options.Services;

        // Unfortunate nasty hack. We reported this with Microsoft.
        _ = services
            .AddScoped<IComponentActivator, SimpleInjectorComponentActivator>()
            .AddScoped<ScopeAccessor>()
            .AddTransient<ServiceScopeApplier>();
    }
    private void RegisterBlazorComponents(SimpleInjectorAddOptions options)
    {
        var container = options.Container;
        var registrations = container.GetTypesToRegister<IComponent>(
                ComponentAssemblies,
                new TypesToRegisterOptions { IncludeGenericTypeDefinitions = true })
            .Where(componentType => componentType.GetCustomAttribute<ExcludeComponentFromContainerAttribute>() == null)
            .Select(GetComponentRegistration);

        foreach(var (componentType, implementationInfo) in registrations)
        {
            var componentImplementation = componentType;
            if(implementationInfo.TryAsHelperComponents(out var helperComponents))
            {
                //intercept component type registration if helper attribute is detected (custom render mode was used) and we do not omit render modes
                RegisterBlazorComponent(container, helperComponents.ProxyType, helperComponents.ProxyType);
                componentImplementation = helperComponents.WrapperType;
            }

            RegisterBlazorComponent(container, componentType, componentImplementation);
        }
    }

    private static void RegisterBlazorComponent(Container container, Type componentType, Type componentImplementation)
    {
        if(componentType.IsGenericTypeDefinition)
        {
            container.Register(componentType, componentImplementation, Lifestyle.Transient);
            return;
        }

        var registration = Lifestyle.Transient.CreateRegistration(componentImplementation, container);

        registration.SuppressDiagnosticWarning(
            DiagnosticType.DisposableTransientComponent,
            "Blazor will dispose components.");

        container.AddRegistration(componentType, registration);
    }

    private KeyValuePair<Type, ImplementationInfo> GetComponentRegistration(Type componentType)
    {
        var helperAttribute = componentType.GetCustomAttribute<RenderModeHelperComponentsAttribute>();

        ImplementationInfo result = helperAttribute != null ?
            helperAttribute :
            componentType;

        return KeyValuePair.Create(componentType, result);
    }

    [UnionType<Type>(Alias = "DeclaredComponent")]
    [UnionType<RenderModeHelperComponentsAttribute>(Alias = "HelperComponents")]
    private readonly partial struct ImplementationInfo;
}
