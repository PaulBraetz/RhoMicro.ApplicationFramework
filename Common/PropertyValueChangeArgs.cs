namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Concrete args for the <see cref="INotifyPropertyValueChanging.PropertyValueChanging"/>
/// or <see cref="INotifyPropertyValueChanged.PropertyValueChanged"/> events.
/// </summary>
/// <typeparam name="TProperty">The type of property that has changed or is changing.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="oldValue">The properties value before the change.</param>
/// <param name="newValue">The properties value after the change.</param>
/// <param name="propertyName">The name of the property.</param>
public sealed class PropertyValueChangeArgs<TProperty>(
    TProperty oldValue,
    TProperty newValue,
    String propertyName) : PropertyValueChangeBaseEventArgs(propertyName)
{
    /// <summary>
    /// Gets the properties old value.
    /// </summary>
    public TProperty OldValue { get; } = oldValue;
    /// <summary>
    /// Gets the properties new value.
    /// </summary>
    public TProperty NewValue { get; } = newValue;
}
