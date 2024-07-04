namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a request parameter object whose execution must result in a <see cref="Result{TSuccess, TFailure}"/>.
/// </summary>
/// <typeparam name="TSuccess">The type of success to produce.</typeparam>
/// <typeparam name="TFailure">The type of failure to produce.</typeparam>
public interface IRequest<TSuccess, TFailure> : IRequest<Result<TSuccess, TFailure>>;