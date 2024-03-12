#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
namespace RhoMicro.ApplicationFramework.Aspects.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    where TRequest : IServiceRequest<TResult>
{
    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var previousContext = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(null);
        try
        {
            var resultTask = decorated.Execute(request);
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
