namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IOptionModel{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value represented by the option.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="disabled">Indicates whether the option is disabled.</param>
/// <param name="isSelected">Indicates whether the option is currently selected.</param>
/// <param name="value">The options value.</param>
/// <param name="id">The options id. This id is required to be unique in respect to other options provided the parent control.</param>
/// <param name="name">The name of this option.</param>
public sealed class OptionModel<TValue>(TValue value, Boolean disabled, Boolean isSelected, String id, String name)
    : HasObservableProperties, IOptionModel<TValue>
{
    /// <inheritdoc/>
    public String Name { get; } = name;
    /// <inheritdoc/>
    public TValue Value { get; } = value;
    /// <inheritdoc/>
    public Boolean Disabled
    {
        get => disabled;
        set => ExchangeBackingField(ref disabled, value);
    }
    /// <inheritdoc/>
    public Boolean IsSelected
    {
        get => isSelected;
        set
        {
            _ = ExchangeBackingField(ref isSelected, value);
            if(value)
            {
                Selected?.Invoke(this);
            }
        }
    }
    /// <inheritdoc/>
    public String Id { get; } = id;
    /// <inheritdoc/>
    public event Action<IOptionModel<TValue>>? Selected;
}
