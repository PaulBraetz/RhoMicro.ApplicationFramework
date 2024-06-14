namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a request that crosses application boundaries as a dto via an api.
/// </summary>
/// <typeparam name="TRequest">The type of request (CRTP).</typeparam>
/// <typeparam name="TResult">The type of result produced by executing this request.</typeparam>
/// <typeparam name="TRequestDto">The type of dto transferred to the api.</typeparam>
/// <typeparam name="TResultDto">The type of dto received from the api.</typeparam>
public interface IApiServiceRequest<TRequest, TResult, TRequestDto, TResultDto> : IServiceRequest<TResult>
    where TRequest : IServiceRequest<TResult>
    where TRequestDto : IApiServiceRequestDto<TRequest, TResult>
    where TResult : IApiServiceResult<TResult, TResultDto>
    where TResultDto : IApiServiceResultDto<TResult>
{
    /// <summary>
    /// Gets the dto required to transfer the request.
    /// </summary>
    /// <returns>The dto representing the request, to be transferred to the api.</returns>
    TRequestDto ToDto();
}
