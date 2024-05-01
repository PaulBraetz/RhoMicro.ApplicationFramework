namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a component with optional an render mode.
/// </summary>
public interface IOptionalRenderModeComponent : IComponent
{
    /// <summary>
    /// Gets this components requested render mode.
    /// </summary>
    IComponentRenderMode? OptionalRenderMode { get; }
}
