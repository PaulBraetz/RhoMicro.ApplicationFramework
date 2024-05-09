namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using Microsoft.AspNetCore.Components;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
public interface IModelComponent<TModel> : IModelComponent<TModel, ICssStyle>;

/// <summary>
/// Base component view for a model of <typeparamref name="TModel"/>.
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
public interface IModelComponent<TModel, TStyle> : IComponent<TStyle>
    where TStyle : ICssStyle
{
    /// <summary>
    /// Gets or sets the component model.
    /// </summary>
    TModel Value { get; set; }
    /// <summary>
    /// Gets or sets the callback invoked on <see cref="Value"/> changing.
    /// </summary>
    EventCallback<TModel> ValueChanged { get; set; }
}