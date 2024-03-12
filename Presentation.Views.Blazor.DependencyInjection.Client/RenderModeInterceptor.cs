namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

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
    public Boolean ApplyRenderMode(IComponentRenderMode? renderMode, IOptionalRenderModeComponent component)
    {
        var result = !(
            //desktop does not support render modes at all
            executionEnvironment.DeploymentPlatform == DeploymentPlatform.Desktop
            //wasm runtime does not support ssr
            || executionEnvironment.OperatingSystem.IsBrowser() && renderMode is InteractiveServerRenderMode
            //noOp signals inheritance from parent
            || renderMode is NoOpRenderMode );

        return result;
    }
}