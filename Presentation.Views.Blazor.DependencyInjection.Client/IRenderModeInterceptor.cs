namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Intercepts applications of <see cref="IComponentRenderMode"/> to components.
/// </summary>
public interface IRenderModeInterceptor
{
    /// <summary>
    /// Gets the render mode to apply to a component.
    /// </summary>
    /// <param name="component">The component to apply a render mode to.</param>
    /// <returns>
    /// The render mode to apply to <paramref name="component"/>; or <see cref="NoOpRenderMode"/> if none should be applied.
    /// </returns>
    IComponentRenderMode? GetRenderMode(IOptionalRenderModeComponent component);
}
