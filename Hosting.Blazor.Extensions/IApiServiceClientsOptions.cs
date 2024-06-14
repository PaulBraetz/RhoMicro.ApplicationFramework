namespace RhoMicro.ApplicationFramework.Hosting;

using System.Text.Json;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// used to configure api client services.
/// </summary>
public interface IApiServiceClientsOptions
{
    /// <summary>
    /// Gets or sets the serializer options used by api service clients.
    /// </summary>
    JsonSerializerOptions SerializerOptions { get; set; }
}
