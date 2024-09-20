namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Text.Json;

using RhoMicro.ApplicationFramework.Common.Abstractions;

sealed class ApiServiceClientSettings<TRequest, TResult>(Uri requestUri, JsonSerializerOptions serializerOptions)
    where TRequest : IRequest<TResult>
{
    public JsonSerializerOptions SerializerOptions { get; } = serializerOptions;
    public Uri RequestUri { get; } = requestUri;
}