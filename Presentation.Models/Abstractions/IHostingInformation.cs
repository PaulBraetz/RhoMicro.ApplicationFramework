namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Models hosting information and provides methods for creating URIs based on that information.
/// </summary>
public interface IHostingInformation
{
    /// <summary>
    /// Gets the name of the current host.
    /// </summary>
    String Host { get; } //localhost //xy.abc.com
    /// <summary>
    /// Gets the name of the scheme of the current host.
    /// </summary>
    String Scheme { get; } //http/https
    /// <summary>
    /// Gets the port of the current host.
    /// </summary>
    Int32 Port { get; } //5001 //80
    /// <summary>
    /// Build a URI using the relative path, query and fragment provided,
    /// as well as the host, scheme and port defined.
    /// </summary>
    /// <param name="path">The path of the URI to create.</param>
    /// <param name="query">The query of the URI to create.</param>
    /// <returns>A new URI.</returns>
    Uri CreateUri(PathString path, QueryString query);
}