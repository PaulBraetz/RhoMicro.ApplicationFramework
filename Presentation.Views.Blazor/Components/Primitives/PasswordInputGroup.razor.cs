namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="PasswordInputGroup{TError}"/> component.
/// </summary>
public interface IPasswordInputGroupCssStyle : ICssStyle
{
    /// <summary>
    /// The style to apply to the nested password input component.
    /// </summary>
    ICssStyle PasswordInputStyle { get; }
}

/// <inheritdoc cref="IPasswordInputGroupCssStyle"/>
public sealed class PasswordInputGroupCssStyleOptions : CssStyleOptions, IPasswordInputGroupCssStyle
{
    /// <inheritdoc cref="IPasswordInputGroupCssStyle.PasswordInputStyle"/>
    private CssStyleOptions PasswordInputStyle { get; set; } = new();
    ICssStyle IPasswordInputGroupCssStyle.PasswordInputStyle => PasswordInputStyle;
}