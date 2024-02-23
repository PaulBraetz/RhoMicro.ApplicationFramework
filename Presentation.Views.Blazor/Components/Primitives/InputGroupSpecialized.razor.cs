﻿namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using System.ComponentModel.DataAnnotations;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Base class for input controls that provide a label, description and error.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by the model.</typeparam>
/// <typeparam name="TError">The type of error displayed by the model.</typeparam>
/// <typeparam name="TInput">The type of input model used to obtain input.</typeparam>
public partial class InputGroupSpecialized<TInput, TValue, TError> :
    ModelComponentBase<IInputGroupModel<TInput, TValue, TError>>
    where TInput : IInputModel<TValue, TError>
{
    private RenderFragment? _label;
    private RenderFragment? _description;

    /// <summary>
    /// Gets or sets the input template.
    /// </summary>
    [Parameter]
    public required RenderFragment<TInput> Input { get; set; }
    /// <summary>
    /// Gets or sets the error template.
    /// </summary>
    [Parameter]
    public required RenderFragment<TError> Error { get; set; }
#pragma warning disable BL0007 // Component parameters should be auto properties
    /// <summary>
    /// Gets or sets the label template.
    /// </summary>
    [Parameter]
    public required RenderFragment Label
    {
        get => _label ?? DefaultLabel;
        set => _label = value;
    }
    /// <summary>
    /// Gets or sets the description template.
    /// </summary>
    [Parameter]
    public required RenderFragment Description
    {
        get => _description ?? DefaultDescription;
        set => _description = value;
    }
#pragma warning restore BL0007 // Component parameters should be auto properties
    /// <summary>
    /// Renders the default label component that should be rendered if no value is provided for <see cref="Label"/>.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to which the content should be written.</param>
    protected virtual void DefaultLabel(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if(String.IsNullOrEmpty(Model.Label))
            return;
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "mb-2 text-center font-light text-krtWhite text-2xl");
        builder.AddContent(2, Model.Label);
        builder.CloseComponent();
    }
    /// <summary>
    /// Renders the default description component that should be rendered if no value is provided for <see cref="Description"/>.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> to which the content should be written.</param>
    protected virtual void DefaultDescription(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if(String.IsNullOrEmpty(Model.Description))
            return;
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "input-group-description");
        builder.AddContent(2, Model.Description);
        builder.CloseComponent();
    }
}
