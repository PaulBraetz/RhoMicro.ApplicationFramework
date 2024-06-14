namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Decorates services with the cancellation short-circuit aspect.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TResult">The type of result to produce.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="decorated">The decorated service.</param>
public sealed class ServiceCancellationDecorator<TRequest, TResult>(IService<TRequest, TResult> decorated)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        request.CancellationToken.ThrowIfCancellationRequested();

        return decorated.Execute(request);
    }
}
