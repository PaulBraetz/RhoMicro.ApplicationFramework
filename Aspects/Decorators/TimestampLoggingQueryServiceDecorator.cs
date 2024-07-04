namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorator for services that logs a timestamp before executing the decorated service.
/// </summary>
/// <typeparam name="TRequest">The type of request executed.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a request.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The service used for logging.</param>
/// <param name="timeoutSettings">The timeout settings used for the decorated service.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class TimestampLoggingServiceDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    ILoggingService logger,
    ITimeoutSettings<TRequest> timeoutSettings) :
    IService<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken)
    {
        using(_ = Logs.Log(TimeStampLogEntry.Now(timeoutSettings.Timeout), logger))
        {
            return decorated.Execute(request, cancellationToken);
        }
    }
}
