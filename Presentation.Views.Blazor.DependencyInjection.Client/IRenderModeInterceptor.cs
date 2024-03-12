namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

using System;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Intercepts applications of <see cref="IComponentRenderMode"/> to components.
/// </summary>
public interface IRenderModeInterceptor
{
    /// <summary>
    /// Gets a value indicating whether a render mode should be applied to a given component.
    /// </summary>
    /// <param name="renderMode">The render mode to intercept.</param>
    /// <param name="component">The component to apply the rendermode to.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="renderMode"/> should be applied to <paramref name="component"/>; otherwise, <see langword="false"/>.
    /// </returns>
    Boolean ApplyRenderMode(IComponentRenderMode? renderMode, IOptionalRenderModeComponent component);
}
