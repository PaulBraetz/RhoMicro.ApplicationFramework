namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Exceptions;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
public abstract class ModelComponentBase<TModel> : ModelComponentBase<TModel, ICssStyle>;

/// <summary>
/// Base component view for a model of <typeparamref name="TModel"/>.
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
public abstract class ModelComponentBase<TModel, TStyle> : ComponentBase<TStyle>
    where TStyle : ICssStyle
{
    /// <summary>
    /// Gets the component model.
    /// </summary>
    protected TModel Model => Value;
#pragma warning disable BL0007 // Component parameters should be auto properties
    /// <summary>
    /// Gets or sets the component model.
    /// </summary>
    [Parameter]
    public virtual TModel Value
    {
        get => _value;
        set
        {
            var oldValue = _value;
            _value = value;
            if(!ValuesAreEqual(oldValue, value))
                InvokeValueChanged(value).GetAwaiter().GetResult();
        }
    }
#pragma warning restore BL0007 // Component parameters should be auto properties
    private async Task InvokeValueChanged(TModel newValue)
    {
        await ValueChanged.InvokeAsync(newValue).ConfigureAwait(false);
        OnValueChanged();
    }
    /// <summary>
    /// Invoked after the <see cref="ValueChanged"/> event having been invoked.
    /// </summary>
    protected virtual void OnValueChanged() { }
    /// <summary>
    /// Gets a value indicating whether <see cref="Value"/> (a.k.a. <see cref="Model"/>) is 
    /// required to not be <see langword="null"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <see cref="Value"/> is required to not 
    /// be <see langword="null"/>; otherwise, <see langword="false"/>.
    /// </returns>
    protected virtual Boolean ModelIsRequired() => true;
    /// <summary>
    /// Used to determine whether an assignment to <see cref="Value"/> or <see cref="Model"/>
    /// will cause the <see cref="ValueChanged"/> event to be invoked.
    /// </summary>
    /// <param name="oldValue">The old value to replace.</param>
    /// <param name="newValue">The new value to replace the old one with.</param>
    /// <returns><see langword="true"/> if the old value is equal to the new one; otherwise, <see langword="false"/>.</returns>
    protected virtual Boolean ValuesAreEqual(TModel oldValue, TModel newValue) =>
        EqualityComparer<TModel>.Default.Equals(oldValue, newValue);

    /// <summary>
    /// Gets or sets the callback invoked on <see cref="Value"/> changing.
    /// </summary>
    [Parameter]
    public EventCallback<TModel> ValueChanged { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private TModel _value;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        RegisterModelPropertyChangedHandler();
        CheckNullModel();
        base.OnParametersSet();
    }
    private void CheckNullModel()
    {
        if(ModelIsRequired() && Value == null)
        {
            throw new ParameterNullException(nameof(Value), typeof(TModel), GetType());
        }
    }
    private void RegisterModelPropertyChangedHandler()
    {
        if(Model is INotifyPropertyChanged observable)
        {
            observable.PropertyChanged += (s, e) => InvokeAsync(StateHasChanged);
        }
    }
}
