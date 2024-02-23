namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="DynamicMultiControlItem{TSubControlModel}"/> component.
/// </summary>
public interface IDynamicMultiControlItemCssStyle : ICssStyle
{
    /// <summary>
    /// The style to apply to the nested remove button component.
    /// </summary>
    ICssStyle RemoveButtonStyle { get; }
}

/// <inheritdoc cref="IDynamicMultiControlItemCssStyle"/>
public sealed class DynamicMultiControlItemCssStyleOptions : CssStyleOptions, IDynamicMultiControlItemCssStyle
{
    /// <inheritdoc cref="IDynamicMultiControlItemCssStyle.RemoveButtonStyle"/>
    public CssStyleOptions RemoveButtonStyle { get; set; } = new();
    ICssStyle IDynamicMultiControlItemCssStyle.RemoveButtonStyle => RemoveButtonStyle;
}

/// <summary>
/// </summary>
/// <typeparam name="TSubControlModel"></typeparam>
public partial class DynamicMultiControlItem<TSubControlModel> : ModelComponentBase<IDynamicMultiControlItemModel<TSubControlModel>, IDynamicMultiControlItemCssStyle>
{
    private RenderFragment DefaultRemoveButton(IButtonModel button)
    {
        return result;
        void result(RenderTreeBuilder b)
        {
            b.OpenElement(0, "div");
            b.AddAttribute(1, "class", Style.RemoveButtonStyle.GetCssClass());
            b.OpenComponent<Button>(2);
            b.AddAttribute(3, "Value", button);
            b.CloseComponent();
            b.CloseElement();
        }
    }
}
