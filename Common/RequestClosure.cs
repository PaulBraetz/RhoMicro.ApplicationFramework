#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Exceptions;

/// <summary>
/// A closure around a request and the service using which it should be executed.
/// </summary>
/// <typeparam name="TResult">The type of result produced by the request.</typeparam>
/// <typeparam name="TRequest">The type of request captured.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="request">The request captured.</param>
/// <param name="service">The service captured.</param>
public sealed class RequestClosure<TRequest, TResult>(
    TRequest request,
    IService<TRequest, TResult> service)
    : IRequestClosure<TResult>
    where TRequest : IServiceRequest<TResult>
{
    /// <summary>
    /// Executes the captured request using the captured service.
    /// </summary>
    public ValueTask<TResult> Execute() => service.Execute(request);
}
