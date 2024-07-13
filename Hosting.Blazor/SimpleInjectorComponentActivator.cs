namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;

/// <summary>
/// Blazor component activator that integrates with SimpleInjector.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="applier">The ambient scope applier to use.</param>
/// <param name="container">The container using which to resolve components.</param>
/// <param name="options">The options for this component activator.</param>
public sealed class SimpleInjectorComponentActivator(
    ServiceScopeApplier applier,
    Container container,
    ComponentActivatorOptions options) : IComponentActivator
{
    /// <inheritdoc/>
    public IComponent CreateInstance(Type componentType)
    {
        ArgumentNullException.ThrowIfNull(componentType);

        applier.ApplyServiceScope();

        IServiceProvider provider = container;
        var component = provider.GetService(componentType);

        var activateUnregistered = false;
        if(component is null)
        {
            activateUnregistered = options.UnregisteredComponentActivationBehavior.Activate(componentType);
            if(activateUnregistered)
                component = ActivatorUtilities.CreateInstance(provider, componentType);
        }

        if(component is null)
        {
            var reason = activateUnregistered
                ? $"but even though the {nameof(options.UnregisteredComponentActivationBehavior)} of type {options.UnregisteredComponentActivationBehavior.GetType()} defined in the injected {nameof(ComponentActivatorOptions)} support activation of the component type, an instance could not be created."
                : $"and the {nameof(ComponentActivatorOptions.UnregisteredComponentActivationBehavior)} of type {options.UnregisteredComponentActivationBehavior.GetType()} defined in the injected {nameof(ComponentActivatorOptions)} do not support activation of the component type.";
            throw new ArgumentException($"Unable to instantiate component of type {componentType}. The component type was not registered to the di container, {reason}", nameof(componentType));
        }

        return (IComponent)component;
    }
}
