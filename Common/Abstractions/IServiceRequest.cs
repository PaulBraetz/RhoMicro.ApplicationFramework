namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a request parameter object whose execution must result in a specific result type.
/// </summary>
/// <typeparam name="TResult">The type of result to be produced by a service.</typeparam>
public interface IServiceRequest<TResult>
{
    /// <summary>
    /// The token used to signal the service execution to be cancelled.
    /// </summary>
    CancellationToken CancellationToken { get; }
}

/// <summary>
/// Represents a request parameter object whose execution must result in a <see cref="Result{TSuccess, TFailure}"/>.
/// </summary>
/// <typeparam name="TSuccess">The type of success to produce.</typeparam>
/// <typeparam name="TFailure">The type of failure to produce.</typeparam>
public interface IServiceRequest<TSuccess, TFailure> : IServiceRequest<Result<TSuccess, TFailure>>;