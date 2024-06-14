namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

/// <summary>
/// Represents a blazor web app adapter builder.
/// </summary>
/// <param name="underlyingBuilder">The builder being adapted.</param>
/// <param name="capabilities">The capabilities attached to the builder.</param>
public sealed class WebServerGuiAppBuilder
    (WebApplicationBuilder underlyingBuilder, BlazorAppBuilderCapabilities capabilities)
    : BlazorAppBuilder<WebServerGuiAppBuilder, WebServerGuiApp, WebApplicationBuilder, WebApplication>
{
    /// <inheritdoc/>
    public override BlazorAppBuilderCapabilities Capabilities { get; } = capabilities;
    /// <inheritdoc/>
    protected override WebServerGuiAppBuilder Self => this;
    /// <inheritdoc/>
    public override WebApplicationBuilder UnderlyingBuilder { get; } = underlyingBuilder;
    /// <inheritdoc/>
    protected override WebApplication BuildUnderlyingApp() => UnderlyingBuilder.Build();
    /// <inheritdoc/>
    protected override WebServerGuiApp CreateApp(WebApplication underlyingApp, Container container, AppRunOptions appRunOptions)
        => new(underlyingApp, container, appRunOptions);
    /// <inheritdoc/>
    protected override IDeploymentPlatformProvider GetDeploymentPlatformProvider(IServiceProvider serviceProvider) =>
        new DeploymentPlatformProvider(DeploymentPlatform.Server);
    /// <inheritdoc/>
    protected override void OnSimpleInjectorAdd(SimpleInjectorAddOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        base.OnSimpleInjectorAdd(options);

        _ = options.AddAspNetCore();

        _ = options.Services
            .AddTransient(
                typeof(Microsoft.AspNetCore.Components.Server.CircuitOptions)
                .Assembly
                .GetTypes()
                .First(t => t.FullName == "Microsoft.AspNetCore.Components.Server.ComponentHub"))
            .AddScoped(typeof(IHubActivator<>), typeof(SimpleInjectorBlazorHubActivator<>));
    }
}
