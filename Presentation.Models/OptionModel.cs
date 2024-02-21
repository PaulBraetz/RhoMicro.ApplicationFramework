namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IOptionModel{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value represented by the option.</typeparam>
public sealed class OptionModel<TValue> : HasObservableProperties, IOptionModel<TValue>
{
    private Boolean _disabled;
    private Boolean _isSelected;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="disabled">Indicates whether the option is disabled.</param>
    /// <param name="isSelected">Indicates whether the option is currently selected.</param>
    /// <param name="value">The options value.</param>
    /// <param name="id">The options id. This id is required to be unique in respect to other options provided the parent control.</param>
    /// <param name="name">The name of this option.</param>
    public OptionModel(TValue value, Boolean disabled, Boolean isSelected, String id, String name)
    {
        _disabled = disabled;
        _isSelected = isSelected;
        Value = value;
        Id = id;
        Name = name;
    }
    /// <inheritdoc/>
    public String Name { get; }
    /// <inheritdoc/>
    public TValue Value { get; }
    /// <inheritdoc/>
    public Boolean Disabled
    {
        get => _disabled;
        set => ExchangeBackingField(ref _disabled, value);
    }
    /// <inheritdoc/>
    public Boolean IsSelected
    {
        get => _isSelected;
        set
        {
            _ = ExchangeBackingField(ref _isSelected, value);
            if(value)
            {
                Selected?.Invoke(this);
            }
        }
    }
    /// <inheritdoc/>
    public String Id { get; }
    /// <inheritdoc/>
    public event Action<IOptionModel<TValue>>? Selected;
}
