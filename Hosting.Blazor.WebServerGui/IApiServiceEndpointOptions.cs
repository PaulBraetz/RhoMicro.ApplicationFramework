namespace RhoMicro.ApplicationFramework.Hosting;
using System.Text.Json;

/// <summary>
/// Used to configure api service endpoints.
/// </summary>
public interface IApiServiceEndpointsOptions
{
    /// <summary>
    /// Gets or sets the options used for serializing/deserializing request/result dtos.
    /// </summary>
    JsonSerializerOptions SerializerOptions { get; set; }
}