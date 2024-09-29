namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Builder;

/// <summary>
/// Options for creating web server gui app builders.
/// </summary>
public sealed class WebServerGuiAppBuilderCreationOptions
{
    /// <summary>
    /// Gets or sets the settings to supply to the underlying app builder.
    /// </summary>
    public WebApplicationOptions AppOptions { get; set; } = new()
    {
        EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    };
    /// <summary>
    /// Gets a callback to be used for logging setup actions before the DI
    /// pipeline is able to resolve loggers.
    /// </summary>
    public Action<String> SetupLoggingCallback { get; set; } = Console.WriteLine;
}
