namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Builder;

using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Options for creating web server gui app builders.
/// </summary>
public sealed class WebServerGuiAppBuilderCreationSettings
{
    /// <summary>
    /// Gets or sets the settings to supply to the underlying app builder.
    /// </summary>
    public WebApplicationOptions BuilderSettings { get; set; } = new()
    {
        EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    };
}
