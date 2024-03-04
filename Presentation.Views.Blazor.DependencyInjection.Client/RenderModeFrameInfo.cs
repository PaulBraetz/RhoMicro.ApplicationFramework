namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Contains information on how to create render mode frames around components.
/// </summary>
public sealed class RenderModeFrameInfo
{
    /// <summary>
    /// Gets the type of component wrapping the framed component (to prevent SOE when resolving from DI).
    /// </summary>
    public required Type WrapperType { get; init; }
}