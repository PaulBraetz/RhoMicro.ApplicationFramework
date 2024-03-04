namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;

using Photino.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Adapts the <see cref="PhotinoBlazorApp"/> onto the integration interface <see cref="IApplication"/>.
/// </summary>
/// <param name="app">The <see cref="PhotinoBlazorApp"/> to adapt.</param>
public sealed class PhotinoBlazorAppAdapter(PhotinoBlazorApp app) : IApplication
{
    /// <summary>
    /// Gets the adapted app.
    /// </summary>
    public PhotinoBlazorApp App => app;
    /// <inheritdoc/>
    public IServiceProvider Services => app.Services;
}
