namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a dto used to trasfer a service result.
/// </summary>
/// <typeparam name="TResult">The type of result represented.</typeparam>
public interface IApiResultDto<TResult>
{
    /// <summary>
    /// Gets the result represented by this dto.
    /// </summary>
    /// <returns>The result represented by this dto.</returns>
    TResult ToResult();
}