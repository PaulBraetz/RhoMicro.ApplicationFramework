namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Logger for logging the current thread id at query execution time.
/// </summary>
/// <typeparam name="TRequest">The type of query executed.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a query.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The service used for logging.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class ThreadIdLoggingServiceDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    ILoggingService logger) :
    IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        using(_ = Logs.Log(ThreadIdLogEntry.Current, logger))
        {
            return decorated.Execute(request);
        }
    }
}
