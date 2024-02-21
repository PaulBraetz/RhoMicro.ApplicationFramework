namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Generic implementation of input models.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <typeparam name="TInput">The type of input model used to obtain input.</typeparam>
public class InputGroupModel<TInput, TValue, TError> : HasObservableProperties, IInputGroupModel<TInput, TValue, TError>
    where TInput : IInputModel<TValue, TError>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="input">The input control to use.</param>
    public InputGroupModel(TInput input)
    {
        _label = String.Empty;
        _description = String.Empty;

        Input = input;
        Input.PropertyValueChanged += (s, e) => InvokePropertyValueChanged(nameof(Input), Input, Input);
    }

    private String _label = String.Empty;
    private String _description = String.Empty;

    /// <inheritdoc/>
    public String Label
    {
        get => _label;
        set => ExchangeBackingField(ref _label, value);
    }
    /// <inheritdoc/>
    public String Description
    {
        get => _description;
        set => ExchangeBackingField(ref _description, value);
    }
    /// <inheritdoc/>
    public TInput Input { get; }
}
/// <summary>
/// Generic implementation of input models.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public class InputGroupModel<TValue, TError> : InputGroupModel<IInputModel<TValue, TError>, TValue, TError>, IInputGroupModel<TValue, TError>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="input">The input control to use.</param>
    public InputGroupModel(IInputModel<TValue, TError> input) : base(input)
    {
    }
}
