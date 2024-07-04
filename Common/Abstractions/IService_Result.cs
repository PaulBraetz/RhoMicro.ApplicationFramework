namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using System.Threading.Tasks;

/// <summary>
/// Service implementing the request object pattern.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TSuccess">The type of success to produce.</typeparam>
/// <typeparam name="TFailure">The type of failure to produce.</typeparam>
public interface IService<TRequest, TSuccess, TFailure>
    where TRequest : IRequest<TSuccess, TFailure>
{
    /// <summary>
    /// Executes a request.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <returns>The result of the execution.</returns>
    ValueTask<Result<TSuccess, TFailure>> Execute(TRequest request);
}