namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System.Collections.Generic;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Implements the <see cref="ICssStyle"/> interface for sourcing 
/// styles from the application configuration using the options pattern.
/// </summary>
public class CssStyleSettings : ICssStyle
{
    private CssClassNames _classNamesSet = CssClassNames.Empty;
    private List<String> _classNames = [];

    /// <inheritdoc/>
    CssClassNames ICssStyle.ClassNames => _classNamesSet;
    /// <inheritdoc cref="ICssStyle.ClassNames"/>
#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA2227 // Collection properties should be read only
    public List<String> ClassNames
    {
        get => _classNames;
        set
        {
            _classNames = value;
            _classNamesSet = CssClassNames.Create(value);
        }
    }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning restore CA1002 // Do not expose generic lists
}
