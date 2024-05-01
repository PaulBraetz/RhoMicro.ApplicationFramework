namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Options for creating cli app builders.
/// </summary>
public sealed class CliAppBuilderCreationSettings
{
    /// <summary>
    /// Gets the settings to supply to the underlying app builder.
    /// </summary>
    public HostApplicationBuilderSettings BuilderSettings { get; set; } = new HostApplicationBuilderSettings();
}
