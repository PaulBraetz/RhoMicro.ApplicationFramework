namespace RhoMicro.ApplicationFramework.Hosting;

sealed class ApiServiceEndpointHandlerMetadataProvider(ApiServicesSettings settings)
{
    public IEnumerable<ApiServiceEndpointHandlerMetadata> GetMetadata() =>
        settings.Services.Select(s => s.BuildHandlerMetadata());
}