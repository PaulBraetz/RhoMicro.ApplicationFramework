namespace RhoMicro.ApplicationFramework.Aspects.Decorators;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Unsets the <see cref="SynchronizationContext.Current"/>, so that decorated service continuations will be scheduled on the thread pool.
/// </summary>
/// <typeparam name="TRequest">The type of request the decorated service can execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by the service.</typeparam>
/// <remarks>
/// Initializes a new instance. 
/// </remarks>
/// <param name="decorated">The decorated service.</param>
public sealed class SynchronizationContextDiscardingDecorator<TRequest, TResult>(
    IService<TRequest, TResult> decorated)
    : IService<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken)
    {
        var previousContext = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(null);
        try
        {
            var resultTask = decorated.Execute(request, cancellationToken);
            var result = resultTask.IsCompleted ?
                resultTask.Result :
                await resultTask;

            return result;
        } finally
        {
            SynchronizationContext.SetSynchronizationContext(previousContext);
        }
    }
}
