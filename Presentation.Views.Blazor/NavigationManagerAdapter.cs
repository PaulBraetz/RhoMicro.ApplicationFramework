namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Adapts <see cref="NavigationManager"/> onto <see cref="INavigationManager"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="navigationManager">The navigation manager to adapt to <see cref="INavigationManager"/>.</param>
public sealed class NavigationManagerAdapter(NavigationManager navigationManager) : INavigationManager
{
    /// <inheritdoc/>
    public void NavigateTo(String route) =>
        navigationManager.NavigateTo(route);
}
