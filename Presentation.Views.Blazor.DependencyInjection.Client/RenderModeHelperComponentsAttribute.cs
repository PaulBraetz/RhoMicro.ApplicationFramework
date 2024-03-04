namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System;

/// <summary>
/// Helper attribute used to register the required component frame and wrapper types.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class RenderModeHelperComponentsAttribute : Attribute
{
    /// <summary>
    /// Gets the type of render mode frame component to use when not omitting render modes.
    /// </summary>
    public abstract Type FrameType { get; }
    /// <summary>
    /// Gets the type of render mode wrapper component to use when not omitting render modes.
    /// </summary>
    public abstract Type WrapperType { get; }
}
