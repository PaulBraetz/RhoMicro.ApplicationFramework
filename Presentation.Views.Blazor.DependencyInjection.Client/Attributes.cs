namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Marks the target component to be excluded from conventional DI registrations via the containing assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ExcludeComponentFromContainerAttribute : Attribute;

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

/// <summary>
/// Applies render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class OptionalRenderModeAttribute : Attribute
{
    /// <summary>
    /// Gets the render mode to apply to the component.
    /// </summary>
    public abstract IComponentRenderMode? Mode { get; }
}

/// <summary>
/// Applies the <see langword="null"/> render mode to the target component.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NoRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode? Mode => null;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveAuto"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveAutoAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveAuto;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveServer"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveServerAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveServer;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveWebAssembly"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveWebAssemblyAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveWebAssembly;
}