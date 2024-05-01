namespace RhoMicro.ApplicationFramework.Hosting;

using System;

using Photino.Blazor;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;

/// <summary>
/// Represents a local photino app adapter builder.
/// </summary>
/// <param name="underlyingBuilder">The builder being adapted.</param>
/// <param name="capabilities">The capabilities attached to this app builder.</param>
public sealed class LocalGuiAppBuilder
    (PhotinoBlazorAppBuilder underlyingBuilder, BlazorAppBuilderCapabilities capabilities)
    : BlazorAppBuilder<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp>
{
    /// <inheritdoc/>
    public override BlazorAppBuilderCapabilities Capabilities { get; } = capabilities;
    /// <inheritdoc/>
    public override PhotinoBlazorAppBuilder UnderlyingBuilder { get; } = underlyingBuilder;
    /// <inheritdoc/>
    protected override LocalGuiAppBuilder Self => this;
    /// <inheritdoc/>
    protected override PhotinoBlazorApp BuildUnderlyingApp() => UnderlyingBuilder.Build();
    /// <inheritdoc/>
    protected override LocalGuiApp CreateApp(PhotinoBlazorApp underlyingApp, Container container, AppRunOptions appRunOptions)
        => new(underlyingApp, container, appRunOptions);
    /// <inheritdoc/>
    protected override IDeploymentPlatformProvider GetDeploymentPlatformProvider(IServiceProvider serviceProvider) =>
        new DeploymentPlatformProvider(DeploymentPlatform.Desktop);
    /// <inheritdoc/>
    protected override void OnAfterConfigureBuilder()
    {
        base.OnAfterConfigureBuilder();
        foreach(var (rootComponent, _) in UnderlyingBuilder.RootComponents)
        {
            _ = Capabilities.Components.Add(rootComponent);
        }
    }
}
