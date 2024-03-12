namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a render mode that signals proxy components not to emit a render mode for the wrapped component.
/// </summary>
public sealed class NoOpRenderMode : IComponentRenderMode
{
    private NoOpRenderMode() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static NoOpRenderMode Instance { get; } = new NoOpRenderMode();
}