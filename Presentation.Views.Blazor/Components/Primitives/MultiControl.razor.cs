namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// View for the multi control.
/// </summary>
/// <typeparam name="TSubControlModel"></typeparam>
public partial class MultiControl<TSubControlModel> : ModelComponentBase<IMultiControlModel<TSubControlModel>>
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
    public required RenderFragment<TSubControlModel> Item { get; set; }
#nullable restore

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
}
