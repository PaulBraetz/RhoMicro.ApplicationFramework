namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IDisplayModel{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to display.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="valueDefaultProvider">The default value provider to use for initializing <see cref="Value"/>.</param>
/// <param name="formatter">The formatter to format a String representation of the value.</param>
public class DisplayModel<TValue>(IDefaultValueProvider<TValue> valueDefaultProvider, IStaticFormatter<TValue> formatter) : HasObservableProperties, IDisplayModel<TValue>
{
    private TValue _value = valueDefaultProvider.GetDefault();
    private String _placeholder = String.Empty;
    private String? _displayText;

    /// <inheritdoc/>
    public TValue Value
    {
        get => _value;
        set
        {
            _ = ExchangeBackingField(ref _value, value);
            DisplayText = formatter.Format(value);
        }
    }
    /// <inheritdoc/>
    public String DisplayText
    {
        get => _displayText ?? Placeholder;
        private set => ExchangeBackingField(ref _displayText, value);
    }
    /// <inheritdoc/>
    public String Placeholder
    {
        get => _placeholder;
        set => ExchangeBackingField(ref _placeholder, value);
    }
    /// <inheritdoc/>
    protected override void OnPropertyValueChanged(PropertyValueChangeBaseEventArgs eventArgs)
    {
        ArgumentNullException.ThrowIfNull(eventArgs);

        if(eventArgs.PropertyName is nameof(Value) or nameof(Placeholder))
        {
            InvokePropertyValueChanged(nameof(DisplayText), DisplayText, DisplayText);
        }
    }
}
