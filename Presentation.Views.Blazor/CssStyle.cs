namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents a style not providing any styling information.
/// </summary>
public class CssStyle : ICssStyle
{
    /// <inheritdoc/>
    public CssClassNames ClassNames { get; } = CssClassNames.Empty;
    /// <summary>
    /// Gets an instance of <see cref="CssStyle"/>.
    /// </summary>
    public static CssStyle Instance { get; } = new();
}
