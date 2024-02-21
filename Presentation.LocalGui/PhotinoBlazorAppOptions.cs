namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;

/// <summary>
/// Options used to configure a <see cref="PhotinoBlazorApp"/>.
/// </summary>
public sealed class PhotinoBlazorAppOptions
{
    /// <summary>
    /// Gets or sets the path to navigate to upon executing <see cref="PhotinoBlazorApp.Run"/>.
    /// </summary>
    public String IndexPath { get; set; } = "/";
}
