namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a model used for retrieving component access authentication.
/// </summary>
public interface IAuthenticationConsumerModel
{
    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    public Boolean IsAuthenticated { get; }
}
