namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents a generic success result.
/// </summary>
public readonly struct Success;
/// <summary>
/// Represents a generic failure result.
/// </summary>
public readonly record struct Failure(Optional<String> Reason)
{
    /// <summary>
    /// Initializes a new instance that does not present a reason for the failure.
    /// </summary>
    public Failure() : this(Optional.None<String>()) { }
    /// <summary>
    /// Initializes a new instance that presents a reason for the failure.
    /// </summary>
    /// <param name="reason">The reason the failure occurred.</param>
    public Failure(String reason) : this(Optional.Some(reason)) { }
}
/// <summary>
/// Represents a generic result capable of representing either a success or a failure.
/// </summary>
/// <typeparam name="TSuccess">The type of success represented.</typeparam>
/// <typeparam name="TFailure">The type of failure represented.</typeparam>
public readonly partial struct Result<[UnionType(Alias = "Success")] TSuccess, [UnionType(Alias = "Failure")] TFailure>;
/// <summary>
/// Provides static factory methods for the <see cref="Result{TSuccess, TFailure}"/> type.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a new generic success result.
    /// </summary>
    /// <returns>A new generic success result.</returns>
    public static Result<Success, Failure> CreateSuccess() => new Success();
    /// <summary>
    /// Creates a new generic success result.
    /// </summary>
    /// <returns>A new generic success result.</returns>
    public static Result<Success, TFailure> CreateSuccess<TFailure>() => new Success();
    /// <summary>
    /// Creates a new generic failure result.
    /// </summary>
    /// <returns>A new generic failure result.</returns>
    public static Result<Success, Failure> CreateFailure() => new Failure();
    /// <summary>
    /// Creates a new generic failure result.
    /// </summary>
    /// <returns>A new generic failure result.</returns>
    public static Result<TSuccess, Failure> CreateFailure<TSuccess>() => new Failure();
}
