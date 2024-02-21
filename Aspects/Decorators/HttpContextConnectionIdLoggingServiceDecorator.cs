namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using Microsoft.AspNetCore.Http;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorator for query services that logs the http context connection id before executing the decorated service.
/// </summary>
/// <typeparam name="TRequest">The type of query to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a query.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The service used for logging.</param>
/// <param name="decorated">The decorated service.</param>
/// <param name="contextAccessor">The accessor used to obtain the <see cref="HttpContext.Connection"/> and its id.</param>
public sealed class HttpContextConnectionIdLoggingServiceDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    IHttpContextAccessor contextAccessor,
    ILoggingService logger) :
    IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        ILogEntry log = contextAccessor.HttpContext != null ?
            new HttpContextConnectionIdLogEntry(contextAccessor.HttpContext.Connection.Id) :
            new NoHttpContextLogEntry();

        using(_ = Logs.Log(log, logger))
        {
            return decorated.Execute(request);
        }
    }
}
