namespace RhoMicro.ApplicationFramework.Hosting;

using System.Text.Json;

/// <summary>
/// Used to configure api client and endpoint services.
/// </summary>
public sealed class ApiServiceOptions
{
    /// <summary>
    /// Gets or sets the serializer options used by api services.
    /// </summary>
    public JsonSerializerOptions SerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web);
}
