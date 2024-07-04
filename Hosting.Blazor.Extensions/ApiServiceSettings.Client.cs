namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Text.Json;

using RhoMicro.ApplicationFramework.Common.Abstractions;

partial class ApiServiceSettings
{
    private Type _settingsType = null!;
    private Type _serviceType = null!;
    private Type _implementationType = null!;

    public Type SettingsType
    {
        get
        {
            ThrowIfUninitialized();
            return _settingsType;
        }
    }
    public Type ServiceType
    {
        get
        {
            ThrowIfUninitialized();
            return _serviceType;
        }
    }
    public Type ImplementationType
    {
        get
        {
            ThrowIfUninitialized();
            return _implementationType;
        }
    }

    partial void OnRequestTypeNameSet(Type requestType, Type resultType, Type requestDtoType, Type resultDtoType)
    {
        var settingsType = typeof(ApiServiceClientSettings<,>).MakeGenericType(requestType, resultType);
        var serviceType = typeof(IService<,>).MakeGenericType(
            requestType,
            resultType);
        var implementationType = typeof(ApiServiceClient<,,,>).MakeGenericType(
            requestType,
            resultType,
            requestDtoType,
            resultDtoType);

        _settingsType = settingsType;
        _serviceType = serviceType;
        _implementationType = implementationType;
    }

    private Uri BuildRequestUri(String baseUri)
    {
        Uri requestUri;
        try
        {
            var uriString = baseUri + Endpoint;
            requestUri = new Uri(uriString, UriKind.Absolute);
        } catch(Exception ex)
        {
            throw new InvalidOperationException($"Unable to construct request uri from base uri '{baseUri}' and endpoint '{Endpoint}'.", ex);
        }

        return requestUri;
    }

    public Object BuildGenericSettings(String baseUri, JsonSerializerOptions serializerOptions)
    {
        ThrowIfUninitialized();

        ArgumentNullException.ThrowIfNull(baseUri);
        ArgumentNullException.ThrowIfNull(serializerOptions);

        var requestUri = BuildRequestUri(baseUri);

        Object? instance;
        try
        {
            instance = Activator.CreateInstance(_settingsType, requestUri, serializerOptions);
        } catch(Exception ex)
        {
            throw new InvalidOperationException($"Unable to construct instance of request uri settings with uri '{requestUri}'.", ex);
        }

        if(instance is null)
            throw new InvalidOperationException($"Unable to construct request uri from base uri '{baseUri}' and endpoint '{Endpoint}'.");

        return instance;
    }
}

sealed class ApiServiceClientSettings<TRequest, TResult>(Uri requestUri, JsonSerializerOptions serializerOptions)
    where TRequest : IRequest<TResult>
{
    public JsonSerializerOptions SerializerOptions { get; } = serializerOptions;
    public Uri RequestUri { get; } = requestUri;
}