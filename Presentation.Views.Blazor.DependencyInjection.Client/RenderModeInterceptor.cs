namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using RhoMicro.ApplicationFramework.Common.Abstractions;

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

        var renderMode = component.ParentOptionalRenderMode is not null and not NoOpRenderMode
        ? component.ParentOptionalRenderMode
        : component.OptionalRenderMode;

        var forceNoOp =
            //desktop does not support render modes at all
            executionEnvironment.DeploymentPlatform == DeploymentPlatform.Desktop
            //wasm runtime does not support ssr
            || executionEnvironment.OperatingSystem.IsBrowser() && renderMode is InteractiveServerRenderMode;

        var result = forceNoOp ? NoOpRenderMode.Instance : renderMode;

        return result;
    }
}