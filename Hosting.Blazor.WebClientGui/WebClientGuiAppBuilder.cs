namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;

/// <summary>
/// Represents a blazor wasm app adapter builder.
/// </summary>
/// <param name="underlyingBuilder">The builder being adapted.</param>
/// <param name="capabilities">The capabilities attached to this app builder.</param>
public sealed class WebClientGuiAppBuilder
    (WebAssemblyHostBuilder underlyingBuilder, BlazorAppBuilderCapabilities capabilities)
    : BlazorAppBuilder<WebClientGuiAppBuilder, WebClientGuiApp, WebAssemblyHostBuilder, WebAssemblyHost>
{
    /// <inheritdoc/>
    public override BlazorAppBuilderCapabilities Capabilities { get; } = capabilities;
    /// <inheritdoc/>
    public override WebAssemblyHostBuilder UnderlyingBuilder { get; } = underlyingBuilder;
    /// <inheritdoc/>
    protected override WebClientGuiAppBuilder Self => this;
    /// <inheritdoc/>
    protected override WebAssemblyHost BuildUnderlyingApp() => UnderlyingBuilder.Build();
    /// <inheritdoc/>
    protected override IDeploymentPlatformProvider GetDeploymentPlatformProvider(IServiceProvider serviceProvider) =>
        new DeploymentPlatformProvider(DeploymentPlatform.Client);
    /// <inheritdoc/>
    protected override WebClientGuiApp CreateApp(WebAssemblyHost underlyingApp, Container container, AppRunOptions appRunOptions)
        => new(underlyingApp, container, appRunOptions);
}
