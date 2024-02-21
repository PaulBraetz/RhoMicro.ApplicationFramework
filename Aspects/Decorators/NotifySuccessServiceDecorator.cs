namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorates a command service with success event publishing functionality.
/// </summary>
/// <typeparam name="TRequest">The type of command to execute.</typeparam>
/// <typeparam name="TResult">The type of result to be produced by a service.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="decorated">The service decorated.</param>
/// <param name="observer">The observer notified about successful executions.</param>
public sealed class NotifySuccessServiceDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    IObserver<TRequest> observer)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var result = await decorated.Execute(request).ConfigureAwait(continueOnCapturedContext: false);
        observer.Notify(request);

        return result;
    }
}
