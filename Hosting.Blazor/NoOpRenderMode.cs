namespace RhoMicro.ApplicationFramework.Hosting;

using System;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a render mode that signals proxy components not to emit a render mode for the wrapped component.
/// </summary>
public sealed class NoOpRenderMode : IComponentRenderMode, IEquatable<NoOpRenderMode>
{
    private NoOpRenderMode() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static NoOpRenderMode Instance { get; } = new NoOpRenderMode();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override Boolean Equals(Object? obj) => obj is NoOpRenderMode;
    public Boolean Equals(NoOpRenderMode? other) => true;
    public override Int32 GetHashCode() => 0;
    public static Boolean operator ==(NoOpRenderMode left, NoOpRenderMode right) => true;
    public static Boolean operator !=(NoOpRenderMode left, NoOpRenderMode right) => false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
