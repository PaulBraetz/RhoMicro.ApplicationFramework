namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a model used for authenticating component access.
/// </summary>
public interface IAuthenticationModel
{
    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    public Boolean IsAuthenticated { get; }
}
