namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="NumberInputGroup{TNumber, TError}"/> component.
/// </summary>
public interface INumberInputGroupCssStyle : ICssStyle
{
    /// <summary>
    /// The style to apply to the nested number input component.
    /// </summary>
    ICssStyle NumberInputStyle { get; }
}

/// <inheritdoc cref="INumberInputGroupCssStyle"/>
public sealed class NumberInputGroupCssStyleSettings : CssStyleSettings, INumberInputGroupCssStyle
{
    /// <inheritdoc cref="INumberInputGroupCssStyle.NumberInputStyle"/>
    public CssStyleSettings NumberInputStyle { get; set; } = new();
    ICssStyle INumberInputGroupCssStyle.NumberInputStyle => NumberInputStyle;
}