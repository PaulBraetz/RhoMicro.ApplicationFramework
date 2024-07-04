namespace RhoMicro.ApplicationFramework.Hosting;

using System.Text.Json;

/// <summary>
/// Used to configure api client services.
/// </summary>
public interface IApiServiceClientsOptions
{
    /// <summary>
    /// Gets or sets the serializer options used by api service clients.
    /// </summary>
    JsonSerializerOptions SerializerOptions { get; set; }
}
