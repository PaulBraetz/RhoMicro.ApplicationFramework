namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="SelectInputGroup{TValue, TError}"/> component.
/// </summary>
public interface ISelectInputGroupCssStyle : ICssStyle
{
    /// <summary>
    /// The style to apply to the nested select input component.
    /// </summary>
    ICssStyle SelectInputStyle { get; }
}

/// <inheritdoc cref="ISelectInputGroupCssStyle"/>
public sealed class SelectInputGroupCssStyleOptions : CssStyleOptions, ISelectInputGroupCssStyle
{
    /// <inheritdoc cref="ISelectInputGroupCssStyle.SelectInputStyle"/>
    public CssStyleOptions SelectInputStyle { get; set; } = new();
    ICssStyle ISelectInputGroupCssStyle.SelectInputStyle => SelectInputStyle;
}
