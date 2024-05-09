namespace RhoMicro.ApplicationFramework.Hosting;
using SimpleInjector;
using RhoMicro.ApplicationFramework.Composition;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector.Integration.ServiceCollection;
using System.Reflection;

/// <summary>
/// Represents options to be applied to a <see cref="AppBuilder{TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp}"/>.
/// </summary>
public sealed class AppBuilderOptions
{
    internal void InvokeOnBeforeContainerComposed(ContainerOptions options) => OnBeforeContainerComposed?.Invoke(options);
    internal void InvokeOnContainerAdd(SimpleInjectorAddOptions options) => OnContainerAdd?.Invoke(options);

    /// <summary>
    /// Gets or sets the configuration to be applied to the <see cref="Container"/> before <see cref="Composer"/> is applied.
    /// </summary>
    public event Action<ContainerOptions>? OnBeforeContainerComposed;
    /// <summary>
    /// Gets or sets the configuration to be applied to the <see cref="Container"/> upon having been added to the underlying <see cref="IServiceCollection"/>.
    /// </summary>
    public event Action<SimpleInjectorAddOptions>? OnContainerAdd = options =>
    {
        options.Container.ResolveUnregisteredType += (s, e) =>
        {
            if(e.Handled || e.UnregisteredServiceType.GetCustomAttribute<ResolveUnregisteredTypeAttribute>() is null)
                return;

            e.Register(() => ActivatorUtilities.CreateInstance(options.Container, e.UnregisteredServiceType));
        };
    };
    /// <summary>
    /// Gets or sets the composer to compose object graphs in the <see cref="Container"/> used for creating apps.
    /// </summary>
    public IComposer Composer { get; set; } = Composition.Composer.Empty;
    /// <summary>
    /// Gets or sets the options to use when running apps.
    /// </summary>
    public AppRunOptions AppRunOptions { get; set; } = new();
}
