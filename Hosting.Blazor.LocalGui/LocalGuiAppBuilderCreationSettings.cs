namespace RhoMicro.ApplicationFramework.Hosting;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides settings for creating local gui app builders.
/// </summary>
public sealed class LocalGuiAppBuilderCreationSettings
{
    /// <summary>
    /// Gets the arguments passed to the program entry point.
    /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
    public String[] Args { get; set; } = [];
#pragma warning restore CA1819 // Properties should not return arrays
    /// <summary>
    /// Gets the environment configuration using which to create an app builder.
    /// </summary>
    public IEnvironmentConfiguration EnvironmentConfiguration { get; set; }
        = Common.Environment.EnvironmentConfiguration.CreateFromEnvironmentVariable();
}
