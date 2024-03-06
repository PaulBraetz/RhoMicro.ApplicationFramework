﻿namespace RhoMicro.ApplicationFramework.Template_Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Contains template composers.
/// </summary>
public static class TemplateComposers
{
    private static IComposer CreateDefault(DeploymentPlatform deploymentPlatform) => Composer.Create(
        Presentation.Models,
        BlazorViewComposers.Default,
#if DEBUG
        Common.CreateDebug(deploymentPlatform)
#else
        Common.CreateRelease(deploymentPlatform)
#endif
        );
    /// <summary>
    /// Gets the default composition root for web application servers.
    /// </summary>
    public static IComposer WebGui { get; } = CreateDefault(DeploymentPlatform.Server);
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer WebGuiClient { get; } = CreateDefault(DeploymentPlatform.Server);
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer LocalGui { get; } = CreateDefault(DeploymentPlatform.Desktop);
}
