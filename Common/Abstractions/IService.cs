namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Service implementing the request object pattern.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a request.</typeparam>
public interface IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    /// <summary>
    /// Executes a request.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <returns>The result of the execution.</returns>
    ValueTask<TResult> Execute(TRequest request);
}
