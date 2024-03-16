namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System.Collections.Generic;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Implements the <see cref="ICssStyle"/> interface for sourcing 
/// styles from the application configuration using the options pattern.
/// </summary>
public class CssStyleSettings : ICssStyle
{
    /// <inheritdoc/>
    IReadOnlyList<String> ICssStyle.Classes => Classes;
    /// <inheritdoc cref="ICssStyle.Classes"/>
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
    public List<String> Classes { get; set; } = [];
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
}
