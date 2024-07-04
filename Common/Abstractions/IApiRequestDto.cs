namespace RhoMicro.ApplicationFramework.Common.Abstractions;
/// <summary>
/// Represents a dto representing a request to be transferred to an api.
/// </summary>
/// <typeparam name="TRequest">The type of request represented.</typeparam>
/// <typeparam name="TResult">The type of result produced by the request.</typeparam>
public interface IApiRequestDto<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <summary>
    /// Gets the request represented by this dto.
    /// </summary>
    /// <returns>The request represented by this dto.</returns>
    TRequest ToRequest();
}
