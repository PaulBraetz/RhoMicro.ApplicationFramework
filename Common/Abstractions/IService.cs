namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Service implementing the request object pattern.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a request.</typeparam>
#if !GENERATOR
public
#endif
interface IService<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <summary>
    /// Executes a request.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <param name="cancellationToken">The token used to signal execution to be cancelled.</param>
    /// <returns>The result of the execution.</returns>
    ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken);
}
