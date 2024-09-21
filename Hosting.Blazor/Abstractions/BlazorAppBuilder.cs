namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using RhoMicro.CodeAnalysis;

using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.ServiceCollection;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using RhoMicro.RequiredPropertyValidation;
using RhoMicro.RequiredPropertyValidation.RhoMicro.RequiredPropertyValidation;

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

        AddMinorDependencies(options);
        AddBlazor(options);
        RegisterBlazorComponents(options);
        AddStyles(options);
    }

    private static void AddMinorDependencies(SimpleInjectorAddOptions options) => options.Services.AddRequiredPropertyValidation();

    private void AddStyles(SimpleInjectorAddOptions options)
    {
        var addMethod = typeof(InjectionUtils).GetMethod(nameof(InjectionUtils.AddStyle))!;
        var addValidatableMethod = typeof(InjectionUtils).GetMethod(nameof(InjectionUtils.AddValidatableStyle))!;

        var optionsExpr = Expression.Constant(options);
        var bodyExprs = Capabilities.Components
            .Select(c => (componentType: c, attribute: c.GetCustomAttribute<ConfigurableStyleAttribute>()))
            .Where(t => t.attribute != null)
            .DistinctBy(t => t.componentType)
            .Select(t => Expression.Call(
                ( t.attribute!.StyleSettingsType.IsAssignableTo(typeof(IValidateRequiredProperties<>).MakeGenericType(t.attribute.StyleSettingsType))
                ? addValidatableMethod
                : addMethod )
                .MakeGenericMethod(t.attribute!.StyleType, t.attribute.StyleSettingsType),
                optionsExpr,
                Expression.Constant(t.componentType)));
        var body = Expression.Block(bodyExprs);
        var lambda = Expression.Lambda<Action>(body);

        lambda.Compile().Invoke();
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

            RegisterInterception(container, implementationInfo, ref componentImplementation);
            RegisterBlazorComponent(container, componentType, componentImplementation);
        }
    }
    private static void RegisterInterception(Container container, ImplementationInfo implementationInfo, ref Type componentImplementation)
    {
        if(!implementationInfo.TryAsHelperComponents(out var helperComponents))
            return;

        //intercept component type registration if helper attribute is detected (custom render mode was used)
        var proxyType = helperComponents.OpenProxyType;
        //register proxy separately for resolution in wrapper
        RegisterBlazorComponent(container, proxyType, proxyType);
        var wrapperType = helperComponents.OpenWrapperType;
        //register wrapper as implementation for component <- interception
        componentImplementation = wrapperType;
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

file static class InjectionUtils
{
    //keep component type param around in case we do conditional registration later
#pragma warning disable IDE0060 // Remove unused parameter
    public static void AddStyle<TStyle, TStyleSettings>(SimpleInjectorAddOptions options, Type componentType)
#pragma warning restore IDE0060 // Remove unused parameter
        where TStyleSettings : class, TStyle
        where TStyle : class
    {
        _ = options.Services
            .AddTransient<TStyle>(sp => sp.GetRequiredService<IOptions<TStyleSettings>>().Value)
            .AddOptions<TStyleSettings>()
            .BindConfiguration($"Styles:{typeof(TStyleSettings).FullName}")
            .ValidateOnStart();
    }
    public static void AddValidatableStyle<TStyle, TStyleSettings>(SimpleInjectorAddOptions options, Type componentType)
        where TStyleSettings : class, TStyle, IValidateRequiredProperties<TStyleSettings>
        where TStyle : class
    {
        AddStyle<TStyle, TStyleSettings>(options, componentType);
        _ = options.Services.AddSingleton<IValidateOptions<TStyleSettings>, ValidatableValidation<TStyleSettings>>();
    }
    sealed class ValidatableValidation<TStyleSettings>(RequiredPropertyValidator validator) : IValidateOptions<TStyleSettings>
        where TStyleSettings : class, IValidateRequiredProperties<TStyleSettings>
    {
        public ValidateOptionsResult Validate(String? name, TStyleSettings options)
        {
            if(validator.TryValidate(options, out var nullProperties))
                return ValidateOptionsResult.Success;

            return ValidateOptionsResult.Fail(nullProperties.Select(propName => $"Property '{propName}' on options '{name}' cannot be null."));
        }
    }
}
