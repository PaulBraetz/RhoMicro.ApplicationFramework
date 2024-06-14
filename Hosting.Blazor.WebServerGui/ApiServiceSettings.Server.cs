namespace RhoMicro.ApplicationFramework.Hosting;
using System;

partial class ApiServiceSettings
{
    private Type _endpointHandlerType = null!;
    partial void OnRequestTypeNameSet(Type requestType, Type resultType, Type requestDtoType, Type resultDtoType)
    {
        var endpointHandlerType = typeof(ApiServiceEndpointHandler<,,,>).MakeGenericType(requestType, resultType, requestDtoType, resultDtoType);
        _endpointHandlerType = endpointHandlerType;
    }

    public ApiServiceEndpointHandlerMetadata BuildHandlerMetadata()
    {
        ThrowIfUninitialized();

        var result = new ApiServiceEndpointHandlerMetadata(
            EndpointHandlerType: _endpointHandlerType,
            Endpoint: Endpoint);

        return result;
    }
}
