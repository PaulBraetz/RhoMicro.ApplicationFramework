namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Hosting;

using SimpleInjector;

/// <summary>
/// Represents a console line app adapter builder.
/// </summary>
/// <param name="underlyingBuilder">The builder being adapted.</param>
/// <param name="capabilities">The capabilities attached to this builder.</param>
public sealed class CliAppBuilder
    (HostApplicationBuilder underlyingBuilder, AppBuilderCapabilities capabilities)
    : AppBuilder<CliAppBuilder, CliApp, HostApplicationBuilder, IHost>
{
    /// <inheritdoc/>
    public override AppBuilderCapabilities Capabilities { get; } = capabilities;
    /// <inheritdoc/>
    public override HostApplicationBuilder UnderlyingBuilder { get; } = underlyingBuilder;
    /// <inheritdoc/>
    protected override CliAppBuilder Self => this;

    /// <inheritdoc/>
    protected override IHost BuildUnderlyingApp() => UnderlyingBuilder.Build();
    /// <inheritdoc/>
    protected override CliApp CreateApp(IHost underlyingApp, Container container, AppRunOptions appRunOptions)
        => new(underlyingApp, container, appRunOptions);
}
