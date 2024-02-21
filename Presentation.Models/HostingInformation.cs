namespace RhoMicro.ApplicationFramework.Presentation.Models;
using Microsoft.AspNetCore.Http;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IHostingInformation"/>.
/// </summary>
public sealed class HostingInformation : IHostingInformation
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="host">The name of the current host.</param>
    /// <param name="scheme">The name of the scheme of the current host.</param>
    /// <param name="port">The port of the current host.</param>
    public HostingInformation(String host, String scheme, Int32 port)
    {
        if(port is < -1 or > 0xffff)
        {
            throw new ArgumentOutOfRangeException(nameof(port), port, $"{nameof(port)} must be between -1 and 0xffff (inclusive).");
        }

        Host = host;
        Scheme = scheme;
        Port = port;
    }
    /// <inheritdoc/>
    public String Host { get; }
    /// <inheritdoc/>
    public String Scheme { get; }
    /// <inheritdoc/>
    public Int32 Port { get; }
    /// <inheritdoc/>
    public Uri CreateUri(PathString path, QueryString query) =>
        new UriBuilder()
        {
            Scheme = Scheme,
            Host = Host,
            Port = Port,
            Path = path,
            Query = query.ToString()
        }.Uri;
}
