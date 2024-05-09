namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

using RhoMicro.ApplicationFramework.Hosting;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class HostingExtensions
{
    /// <summary>
    /// Adds default component views to the app builders capabilities.
    /// </summary>
    public static TSelf AddDefaultViews<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>(
        this TSelf appBuilder)
        where TSelf : AppBuilder<TSelf, TApp, TUnderlyingBuilder, TUnderlyingApp, TCapabilities>
        where TApp : App<TApp, TUnderlyingApp>
        where TCapabilities : BlazorAppBuilderCapabilities
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _ = appBuilder.Capabilities.Components.Add(typeof(Age).Assembly);

        return appBuilder;
    }
}
