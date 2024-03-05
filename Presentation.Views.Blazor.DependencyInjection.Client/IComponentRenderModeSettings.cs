namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

/// <summary>
/// Defines settings related to component render modes.
/// </summary>
public interface IComponentRenderModeSettings
{
    /// <summary>
    /// Gets a value indicating whether render modes should be applied.
    /// </summary>
    Boolean ApplyRenderMode { get; }
}
