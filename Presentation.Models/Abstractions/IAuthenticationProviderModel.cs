namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a model used for providing component access authentication.
/// </summary>
public interface IAuthenticationProviderModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the current user is authenticated.
    /// </summary>
    public Boolean IsAuthenticated { get; }
}
