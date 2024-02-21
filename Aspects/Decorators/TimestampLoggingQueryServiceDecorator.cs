namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorator for query services that logs a timestamp before executing the decorated service.
/// </summary>
/// <typeparam name="TRequest">The type of query executed.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a query.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The service used for logging.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class TimestampLoggingServiceDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    ILoggingService logger) :
    IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        using(_ = Logs.Log(TimeStampLogEntry.Now, logger))
        {
            return decorated.Execute(request);
        }
    }
}
