namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using RhoMicro.ApplicationFramework.Hosting;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Dynamic component able to render a component based on the type of model it should receive.
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
/// <param name="settings">The settings informing rendering of the underlying component.</param>
public sealed class DynamicModelComponent<TModel>(DynamicModelComponentSettings<TModel> settings) : DynamicStyledModelComponent<TModel, ICssStyle>(settings);

/// <summary>
/// Dynamic component able to render a component based on the type of model and style it should receive.
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
/// <param name="settings">The settings informing rendering of the underlying component.</param>
public partial class DynamicStyledModelComponent<TModel, TStyle>(DynamicStyledModelComponentSettings<TModel, TStyle> settings) : ModelComponentBase<TModel, TStyle>
    where TStyle : ICssStyle
{
    private readonly Dictionary<String, Object?> _parameters = [];

    /// <summary>
    /// Gets the component being rendered in place of this dynamic component; or <see langword="null"/> if none has been rendered yet.
    /// </summary>
    public IModelComponent<TModel, TStyle>? RenderedComponent { get; private set; }

    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _parameters.Clear();

        foreach(var parameterValue in parameters)
        {
            _parameters[parameterValue.Name] = parameterValue.Value;
        }

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent(0, settings.ComponentType.AsType);

        var i = 1;
        foreach(var (name, value) in _parameters)
        {
            builder.AddComponentParameter(i, name, value);
            i++;
        }

        builder.AddComponentReferenceCapture(i, c => RenderedComponent = c as IModelComponent<TModel, TStyle>);

        builder.CloseComponent();
    }
}
