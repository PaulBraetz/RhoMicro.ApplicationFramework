namespace RhoMicro.ApplicationFramework.Composition.Presentation.Models.Blazor;

using System;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

internal sealed class NavigationManager(Microsoft.AspNetCore.Components.NavigationManager adapted) : INavigationManager
{
    public void NavigateTo(String route) => adapted.NavigateTo(route);
}
