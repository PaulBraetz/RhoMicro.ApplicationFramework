namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Default implementation for <see cref="IRenderModeInterceptor"/>.
/// </summary>
/// <param name="executionEnvironment">
/// The environment in which components whose render modes to intercept are running.
/// </param>
public sealed class RenderModeInterceptor(IExecutionEnvironment executionEnvironment) : IRenderModeInterceptor
{
    /// <inheritdoc/>
    public IComponentRenderMode? GetRenderMode(IOptionalRenderModeComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        var result = executionEnvironment.IsDesktop()
            ? NoOpRenderMode.Instance
            : component.OptionalRenderMode;

        return result;
    }
}