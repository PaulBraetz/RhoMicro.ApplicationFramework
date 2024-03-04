namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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