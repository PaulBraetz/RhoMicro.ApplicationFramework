namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.AspNetCore.Components;

using SimpleInjector;

/// <summary>
/// Blazor component activator that integrates with SimpleInjector.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="applier">The ambient scope applier to use.</param>
/// <param name="container">The container using which to resolve components.</param>
public sealed class SimpleInjectorComponentActivator(ServiceScopeApplier applier, Container container) : IComponentActivator
{
    /// <inheritdoc/>
    public IComponent CreateInstance(Type componentType)
    {
        applier.ApplyServiceScope();

        IServiceProvider provider = container;
        var component = provider.GetService(componentType)
            ?? Activator.CreateInstance(componentType)
            ?? throw new ArgumentException($"Unable to instantiate instance of type {componentType}.", nameof(componentType));

        return (IComponent)component;
    }
}
