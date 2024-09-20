namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Options for creating cli app builders.
/// </summary>
public sealed class CliAppBuilderCreationOptions
{
    /// <summary>
    /// Gets the settings to supply to the underlying app builder.
    /// </summary>
    public required HostApplicationBuilderSettings BuilderSettings { get; set; }
    /// <summary>
    /// Gets a callback to be used for logging setup actions before the DI
    /// pipeline is able to resolve loggers.
    /// </summary>
    public Action<String> SetupLoggingCallback { get; set; } = Console.WriteLine;
}
