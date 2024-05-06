namespace RhoMicro.ApplicationFramework.Hosting;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides settings for creating web client gui app builders.
/// </summary>
public sealed class WebClientGuiAppBuilderCreationSettings
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public WebClientGuiAppBuilderCreationSettings() { }
    /// <summary>
    /// Initializes a new instance based on the properties provided by another.
    /// </summary>
    /// <param name="settings">
    /// The instance whose properties to copy.
    /// </param>
    public WebClientGuiAppBuilderCreationSettings(WebClientGuiAppBuilderCreationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        (Args, EnvironmentConfiguration) = (settings.Args, settings.EnvironmentConfiguration);
    }

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
        = Common.Environment.EnvironmentConfiguration.CreateFromEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
}