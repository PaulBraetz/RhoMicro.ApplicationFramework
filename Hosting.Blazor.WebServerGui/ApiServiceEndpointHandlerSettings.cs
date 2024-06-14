namespace RhoMicro.ApplicationFramework.Hosting;
using System.Text.Json;

sealed class ApiServiceEndpointHandlerSettings(JsonSerializerOptions serializerOptions)
{
    public JsonSerializerOptions SerializerOptions { get; } = serializerOptions;
}
