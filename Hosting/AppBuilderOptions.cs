namespace RhoMicro.ApplicationFramework.Hosting;
using SimpleInjector;
using RhoMicro.ApplicationFramework.Composition;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Represents options to be applied to a <see cref="AppBuilder{TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp}"/>.
/// </summary>
public sealed class AppBuilderOptions
{
    /// <summary>
    /// Gets or sets the configuration to be applied to the <see cref="Container"/> before <see cref="Composer"/> is applied.
    /// </summary>
    public Action<ContainerOptions>? OnBeforeContainerComposed { get; set; }
    /// <summary>
    /// Gets or sets the configuration to be applied to the <see cref="Container"/> upon having been added to the underlying <see cref="IServiceCollection"/>.
    /// </summary>
    public Action<SimpleInjectorAddOptions>? OnContainerAdd { get; set; } =
        options => options.AddLogging();
    /// <summary>
    /// Gets or sets the composer to compose object graphs in the <see cref="Container"/> used for creating apps.
    /// </summary>
    public IComposer Composer { get; set; } = Composition.Composer.Empty;
    /// <summary>
    /// Gets or sets the options to use when running apps.
    /// </summary>
    public AppRunOptions AppRunOptions { get; set; } = new();
}
