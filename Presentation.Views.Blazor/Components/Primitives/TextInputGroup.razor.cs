namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="TextInputGroup{TError}"/> component.
/// </summary>
public interface ITextInputGroupCssStyle : ICssStyle
{
    /// <summary>
    /// The style to apply to the nested text input component.
    /// </summary>
    ICssStyle TextInputStyle { get; }
}

/// <inheritdoc cref="ITextInputGroupCssStyle"/>
public sealed class TextInputGroupCssStyleOptions : CssStyleOptions, ITextInputGroupCssStyle
{
    /// <inheritdoc cref="ITextInputGroupCssStyle.TextInputStyle"/>
    public CssStyleOptions TextInputStyle { get; set; } = new();
    ICssStyle ITextInputGroupCssStyle.TextInputStyle => TextInputStyle;
}
