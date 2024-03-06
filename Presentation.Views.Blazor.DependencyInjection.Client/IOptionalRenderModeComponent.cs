namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a component with optional render modes.
/// </summary>
public interface IOptionalRenderModeComponent : IComponent
{
    /// <summary>
    /// Gets the <see cref="IComponentRenderMode"/> to optionally apply to the component.
    /// </summary>
    IComponentRenderMode? OptionalRenderMode { get; }
    /// <summary>
    /// Gets the <see cref="IComponentRenderMode"/> to optionally applied to the components parent.
    /// </summary>
    IComponentRenderMode? ParentOptionalRenderMode { get; }
}