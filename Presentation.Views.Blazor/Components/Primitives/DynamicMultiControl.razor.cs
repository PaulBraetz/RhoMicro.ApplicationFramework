namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using System.ComponentModel.DataAnnotations;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// 
/// </summary>
public partial class DynamicMultiControl<TSubControlModel> : ModelComponentBase<IDynamicMultiControlModel<TSubControlModel>>
{
    /// <summary>
    /// Gets or sets the template used to display the label.
    /// </summary>
    [Parameter]
    public required RenderFragment<String> Label { get; set; } = DefaultLabel;
    /// <summary>
    /// Gets or sets the template used to display the description.
    /// </summary>
    [Parameter]
    public required RenderFragment<String> Description { get; set; } = DefaultDescription;
    /// <summary>
    /// Gets or sets the template to display an item.
    /// </summary>
    [Parameter]
    public required RenderFragment<IDynamicMultiControlItemModel<TSubControlModel>> Item { get; set; }
    /// <summary>
    /// Gets or sets the template to display the add button.
    /// </summary>
    [Parameter]
    public required RenderFragment<IButtonModel> AddButton { get; set; } = DefaultAddButton;

    private static RenderFragment DefaultLabel(String value)
    {
        return result;
        void result(RenderTreeBuilder builder)
        {
            if(String.IsNullOrEmpty(value))
                return;
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "text-krtWhite text-2xl mb-2 text-center font-light");
            builder.AddContent(2, value);
            builder.CloseComponent();
        }
    }
    private static RenderFragment DefaultDescription(String value)
    {
        return result;
        void result(RenderTreeBuilder builder)
        {
            if(String.IsNullOrEmpty(value))
                return;
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "input-group-description");
            builder.AddContent(2, value);
            builder.CloseComponent();
        }
    }
    private static RenderFragment DefaultAddButton(IButtonModel value)
    {
        return result;
        void result(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, typeof(Button));
            builder.AddAttribute(1, "Value", value);
            builder.CloseComponent();
        }
    }
}
