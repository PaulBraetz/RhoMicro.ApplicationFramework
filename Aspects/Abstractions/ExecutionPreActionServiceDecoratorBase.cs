namespace RhoMicro.ApplicationFramework.Aspects.Abstractions;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Base class for decorators defining actions to be executed before the decorated services execution.
/// </summary>
/// <typeparam name="TRequest">The type of request the decorated service can execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by the service.</typeparam>
/// <remarks>
/// Initializes a new instance. 
/// </remarks>
/// <param name="decorated">The decorated service.</param>
public abstract class ExecutionPreActionServiceDecoratorBase<TRequest, TResult>(IService<TRequest, TResult> decorated)
    : IService<TRequest, TResult>
    where TRequest : IRequest<TResult>
{

    /// <summary>
    /// Executes the pre-action.
    /// </summary>
    protected virtual ValueTask ExecutePreAction(TRequest request) => ValueTask.CompletedTask;

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken)
    {
        await ExecutePreAction(request);
        var result = await decorated.Execute(request, cancellationToken);

        return result;
    }
}