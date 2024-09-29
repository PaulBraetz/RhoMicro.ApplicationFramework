namespace RhoMicro.ApplicationFramework.Hosting;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides settings for creating web client gui app builders.
/// </summary>
public sealed class WebClientGuiAppBuilderCreationOptions
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public WebClientGuiAppBuilderCreationOptions() { }
    /// <summary>
    /// Initializes a new shallow copy instance based on the properties provided by another.
    /// </summary>
    /// <param name="options">
    /// The instance whose properties to copy.
    /// </param>
    public WebClientGuiAppBuilderCreationOptions(WebClientGuiAppBuilderCreationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        (Args, EnvironmentConfiguration, SetupLoggingCallback) = (options.Args, options.EnvironmentConfiguration, options.SetupLoggingCallback);
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
    /// <summary>
    /// Gets a callback to be used for logging setup actions before the DI
    /// pipeline is able to resolve loggers.
    /// </summary>
    public Action<String> SetupLoggingCallback { get; set; } = Console.WriteLine;
}