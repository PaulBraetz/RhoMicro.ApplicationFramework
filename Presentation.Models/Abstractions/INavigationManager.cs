namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Manages site navigation.
/// </summary>
public interface INavigationManager
{
    /// <summary>
    /// Navigates to another route.
    /// </summary>
    /// <param name="route">The route to navigate to.</param>
    void NavigateTo(String route);
}
