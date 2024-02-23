namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System.Collections.Generic;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents a style not providing any styling information.
/// </summary>
public class CssStyle : ICssStyle
{
    /// <inheritdoc/>
    public IReadOnlyList<String> Classes { get; } = [];
}
