namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IInputModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="valueDefaultProvider">The default value provider to use for initializing <see cref="Value"/>.</param>
/// <param name="errorDefaultProvider">The default value provider to use for initializing <see cref="ErrorValue"/>.</param>
public class InputModel<TValue, TError>(
    IDefaultValueProvider<TValue> valueDefaultProvider,
    IDefaultValueProvider<TError> errorDefaultProvider) : HasObservableProperties, IInputModel<TValue, TError>
{
    private TValue _value = valueDefaultProvider.GetDefault();
    private TValue _autoCompleteValue = valueDefaultProvider.GetDefault();
    private Boolean _autoCompleteEnabled;
    private TError _error = errorDefaultProvider.GetDefault();
    private InputValidityType _validity;
    private String _placeholder = String.Empty;

    /// <inheritdoc/>
    public virtual TValue Value
    {
        get => _value;
        set => ExchangeBackingField(ref _value, value);
    }
    /// <inheritdoc/>
    public virtual TValue AutoCompleteValue
    {
        get => _autoCompleteValue;
        protected set => ExchangeBackingField(ref _autoCompleteValue, value);
    }
    /// <inheritdoc/>
    public Boolean AutoCompleteEnabled
    {
        get => _autoCompleteEnabled;
        set => ExchangeBackingField(ref _autoCompleteEnabled, value);
    }
    /// <inheritdoc/>
    public String Placeholder
    {
        get => _placeholder;
        set => ExchangeBackingField(ref _placeholder, value);
    }
    /// <inheritdoc/>
    public TError ErrorValue
    {
        get => _error;
        set => ExchangeBackingField(ref _error, value);
    }

    /// <inheritdoc/>
    public event AsyncEventHandler<IAsyncEventArguments<TValue>>? Entered;

    /// <inheritdoc/>
    public Task Enter() => Entered.InvokeAsync(this, Value);
    /// <inheritdoc/>
    public InputValidityType Validity
    {
        get => _validity;
        private set => ExchangeBackingField(ref _validity, value);
    }
    /// <inheritdoc/>
    public void UnsetValidity()
    {
        Validity = InputValidityType.None;
        ErrorValue = errorDefaultProvider.GetDefault();
    }
    /// <inheritdoc/>
    public void SetInvalid(TError errorValue)
    {
        Validity = InputValidityType.Invalid;
        ErrorValue = errorValue;
    }
    /// <inheritdoc/>
    public void SetValid()
    {
        Validity = InputValidityType.Valid;
        ErrorValue = errorDefaultProvider.GetDefault();
    }
}
