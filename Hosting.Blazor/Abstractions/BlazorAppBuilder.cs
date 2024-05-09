namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using RhoMicro.CodeAnalysis;

using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Represents an app builder for local or web blazor apps.
/// </summary>
/// <typeparam name="TSelf">The type of app adapter builder.</typeparam>
/// <typeparam name="TApp">The type of app adapter produced.</typeparam>
/// <typeparam name="TUnderlyingBuilder">The type of builder being adapted.</typeparam>
/// <typeparam name="TUnderlyingApp">The type of app being adapted.</typeparam>
public abstract class BlazorAppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp>
    : BlazorAppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, BlazorAppBuilderCapabilities>
    where TSelf : BlazorAppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp>
    where TApp : App<TApp, TUnderlyingApp>;

/// <summary>
/// Represents an app builder for local or web blazor apps.
/// </summary>
/// <typeparam name="TSelf">The type of app adapter builder.</typeparam>
/// <typeparam name="TApp">The type of app adapter produced.</typeparam>
/// <typeparam name="TUnderlyingBuilder">The type of builder being adapted.</typeparam>
/// <typeparam name="TUnderlyingApp">The type of app being adapted.</typeparam>
/// <typeparam name="TCapabilities">The type of capabilities this builder has.</typeparam>
public abstract partial class BlazorAppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
    : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
    where TSelf : BlazorAppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
    where TApp : App<TApp, TUnderlyingApp>
    where TCapabilities : BlazorAppBuilderCapabilities
{
    /// <inheritdoc/>
    protected override void OnAfterContainerCreated(Container container)
    {
        ArgumentNullException.ThrowIfNull(container);

        base.OnAfterContainerCreated(container);
        container.Options.PropertySelectionBehavior = new DependencyAttributePropertySelectionBehavior();
    }

    /// <inheritdoc/>
    protected override void OnBeforeContainerComposed(Container container)
    {
        ArgumentNullException.ThrowIfNull(container);

        base.OnBeforeContainerComposed(container);
        container.RegisterSingleton<IRenderModeInterceptor, RenderModeInterceptor>();
    }

    /// <inheritdoc/>
    protected override void OnSimpleInjectorAdd(SimpleInjectorAddOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        base.OnSimpleInjectorAdd(options);

        AddBlazor(options);
        RegisterBlazorComponents(options);
    }

    private static void AddBlazor(SimpleInjectorAddOptions options)
    {
        var services = options.Services;

        // Unfortunate nasty hack. We reported this with Microsoft.
        _ = services
            .AddSingleton<ComponentActivatorOptions>()
            .AddScoped<IComponentActivator, SimpleInjectorComponentActivator>()
            .AddScoped<ScopeAccessor>()
            .AddTransient<ServiceScopeApplier>();
    }
    private void RegisterBlazorComponents(SimpleInjectorAddOptions options)
    {
        var container = options.Container;
        var registrations = Capabilities.Components.Select(GetComponentRegistration);

        foreach(var (componentType, implementationInfo) in registrations)
        {
            var componentImplementation = componentType;

            if(implementationInfo.TryAsHelperComponents(out var helperComponents))
            {
                //intercept component type registration if helper attribute is detected (custom render mode was used)
                var proxyType = helperComponents.OpenProxyType;
                //register proxy separately for resolution in wrapper
                RegisterBlazorComponent(container, proxyType, proxyType);
                var wrapperType = helperComponents.OpenWrapperType;
                //register wrapper as implementation for component <- interception
                componentImplementation = wrapperType;
            }

            RegisterBlazorComponent(container, componentType, componentImplementation);
        }
    }
    private static void RegisterBlazorComponent(Container container, Type componentType, Type componentImplementation)
    {
        container.Register(componentType, componentImplementation, Lifestyle.Transient);

        if(componentType.GetTypeInfo().ContainsGenericParameters)
            return;

        container.GetRegistration(componentType)?.Registration
            .SuppressDiagnosticWarning(
            DiagnosticType.DisposableTransientComponent,
            "Blazor will dispose components.");
    }
    private KeyValuePair<Type, ImplementationInfo> GetComponentRegistration(Type componentType)
    {
        var helperAttribute = componentType.GetCustomAttribute<RenderModeHelperComponentsAttribute>(inherit: false);

        ImplementationInfo result = helperAttribute != null ?
            helperAttribute :
            componentType;

        return KeyValuePair.Create(componentType, result);
    }

    [UnionType<Type>(Alias = "DeclaredComponent")]
    [UnionType<RenderModeHelperComponentsAttribute>(Alias = "HelperComponents")]
    private readonly partial struct ImplementationInfo;
}
