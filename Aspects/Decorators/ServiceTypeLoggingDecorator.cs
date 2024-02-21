namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorates a service with the service type logging aspect.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a request.</typeparam>
/// <typeparam name="TService">The type of service decorated.</typeparam>
/// <remarks>
/// <typeparamref name="TService"/> must not be constrained so as to allow correct composition of decorators.
/// It is only used to provide the service type underlying the decorator hierarchy.
/// </remarks>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The logger to use for logging execution status.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class ServiceTypeLoggingDecorator<TRequest, TResult, TService>(
    ILoggingService logger,
    IService<TRequest, TResult> decorated) : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        var entry = new ServiceTypeLogEntry(typeof(TService));
        using var _ = Logs.Log(entry, logger);
        return decorated.Execute(request);
    }
}
