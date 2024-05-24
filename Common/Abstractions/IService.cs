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

/// <summary>
/// Service implementing the request object pattern.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TSuccess">The type of success to produce.</typeparam>
/// <typeparam name="TFailure">The type of failure to produce.</typeparam>
public interface IService<TRequest, TSuccess, TFailure>
    where TRequest : IServiceRequest<TSuccess, TFailure>
{
    /// <summary>
    /// Executes a request.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <returns>The result of the execution.</returns>
    ValueTask<Result<TSuccess, TFailure>> Execute(TRequest request);
}
