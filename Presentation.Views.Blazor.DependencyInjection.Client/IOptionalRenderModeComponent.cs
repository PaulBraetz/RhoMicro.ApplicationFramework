namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a component with optional render modes.
/// </summary>
public interface IOptionalRenderModeComponent : IComponent
{
    /// <summary>
    /// Gets or sets the <see cref="IComponentRenderMode"/> to optionally apply to the component.
    /// </summary>
    [Parameter]
    IComponentRenderMode? OptionalRenderMode { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="IComponentRenderMode"/> to optionally applied to the components parent.
    /// </summary>
    [CascadingParameter]
    IComponentRenderMode? ParentOptionalRenderMode { get; set; }
}