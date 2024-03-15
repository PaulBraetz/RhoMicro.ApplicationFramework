namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Marker attribute for generated <c>RenderModeProxy</c> components.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class RenderModeProxyAttribute : Attribute
{
    /// <summary>
    /// Gets the proxied component.
    /// </summary>
    public abstract Type GetConstructedComponentType(Type proxyType);
}
/// <summary>
/// Marker attribute for generated <c>RenderModeWrapper</c> components.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class RenderModeWrapperAttribute : Attribute
{
    /// <summary>
    /// Gets the wrapped component.
    /// </summary>
    public abstract Type GetConstructedComponentType(Type wrapperType);
}

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
    public abstract Type OpenWrapperType { get; }
    /// <summary>
    /// Gets the type of render mode wrapper component to use when not omitting render modes.
    /// </summary>
    public abstract Type OpenProxyType { get; }
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
public sealed class OptionalNullRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode? Mode => null;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveAuto"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveAutoRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveAuto;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveServer"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveServerRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveServer;
}

/// <summary>
/// Applies the <see cref="RenderMode.InteractiveWebAssembly"/> render mode to the target component conditionally.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionalInteractiveWebAssemblyRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => RenderMode.InteractiveWebAssembly;
}

/// <summary>
/// Applies the <see cref="NoOpRenderMode"/> render mode to the target component unconditionally.
/// This means that no optional render mode will be applied.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NoOpRenderModeAttribute : OptionalRenderModeAttribute
{
    /// <inheritdoc/>
    public override IComponentRenderMode Mode => NoOpRenderMode.Instance;
}