namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common.Results;

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
/// Represents a request parameter object whose execution must result in a result of type <see cref="ServiceResult"/>.
/// </summary>
public interface IServiceRequest : IServiceRequest<ServiceResult>;
