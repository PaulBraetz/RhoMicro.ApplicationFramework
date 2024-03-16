namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="DateInputGroup{TError}"/> component.
/// </summary>
public interface IDateInputGroupCssStyle : ICssStyle
{
    /// <summary>
    /// Gets the style to apply to the nested date input component.
    /// </summary>
    public ICssStyle DateInputStyle { get; }
}

/// <inheritdoc cref="IDateInputGroupCssStyle"/>
public sealed class DateInputGroupCssStyleSettings : CssStyleSettings, IDateInputGroupCssStyle
{
    /// <inheritdoc cref="IDateInputGroupCssStyle.DateInputStyle"/>
    public CssStyleSettings DateInputStyle { get; set; } = new();
    ICssStyle IDateInputGroupCssStyle.DateInputStyle => DateInputStyle;
}
