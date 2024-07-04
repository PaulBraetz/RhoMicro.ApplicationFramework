namespace RhoMicro.ApplicationFramework.Common.Abstractions;
/// <summary>
/// Represents the result of a request to an api.
/// </summary>
/// <typeparam name="TResult">The type of result (CRTP).</typeparam>
/// <typeparam name="TDto">The type of dto used to transfer the result from the api.</typeparam>
public interface IApiResult<TResult, TDto>
    where TDto : IApiResultDto<TResult>
{
    /// <summary>
    /// Gets the dto required to transfer this result from the api.
    /// </summary>
    /// <returns>A dto representing this result.</returns>
    TDto ToDto();
}
