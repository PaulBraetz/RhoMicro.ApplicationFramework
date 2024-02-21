#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Exceptions;

/// <summary>
/// <para>
/// A closure around a request and the service using which it should be executed.
/// </para>
/// <para>
/// Attention: Make sure that code utilizing this interface is not violating the CQRS pattern.
/// </para>
/// </summary>
/// <typeparam name="TResult">The type of result produced by the request.</typeparam>
/// <typeparam name="TRequest">The type of request captured.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="request">The request captured.</param>
/// <param name="service">The service captured.</param>
public readonly struct RequestClosure<TRequest, TResult>(
    TRequest request,
    IService<TRequest, TResult> service)
    : IRequestClosure<TResult>, IEquatable<RequestClosure<TRequest, TResult>>
    where TRequest : IServiceRequest<TResult>
{
    /// <summary>
    /// Executes the captured request using the captured service.
    /// </summary>
    public ValueTask<TResult> Execute() => service.Execute(request);
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw Throw.NotSupportedException.ClosureEquals;
    /// <inheritdoc/>
    public Boolean Equals(RequestClosure<TRequest, TResult> other) => throw Throw.NotSupportedException.ClosureEquals;
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw Throw.NotSupportedException.ClosureGetHashCode;
    /// 
    public static Boolean operator ==(RequestClosure<TRequest, TResult> left, RequestClosure<TRequest, TResult> right) =>
        left.Equals(right);
    /// 
    public static Boolean operator !=(RequestClosure<TRequest, TResult> left, RequestClosure<TRequest, TResult> right) =>
        !( left == right );
}
