namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common.Results;

/// <summary>
/// Decorates simple services with the cancellation short-circuit aspect.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="decorated">The decorated service.</param>
public sealed class ServiceCancellationDecorator<TRequest>(IService<TRequest> decorated)
    : IService<TRequest>
    where TRequest : IServiceRequest
{

    /// <inheritdoc/>
    public ValueTask<ServiceResult> Execute(TRequest request)
    {
        var result = request.CancellationToken.IsCancellationRequested ?
            ServiceResult.CompliantlyCancelled.Task.AsValueTask() :
            decorated.Execute(request);

        return result;
    }
}
