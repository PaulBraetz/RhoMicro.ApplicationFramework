namespace RhoMicro.ApplicationFramework.Hosting;

using System;

sealed record ApiServiceEndpointHandlerMetadata(Type EndpointHandlerType, String Endpoint);
