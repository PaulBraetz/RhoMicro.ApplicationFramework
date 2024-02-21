namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using System.Runtime.CompilerServices;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Base for types exposing property changes.
/// </summary>
public abstract class HasObservableProperties : INotifyPropertyValueChanged, INotifyPropertyValueChanging
{
    /// <summary>
    /// <para>
    /// Assigns <paramref name="newValue"/> to <paramref name="location"/> if
    /// the value previously stored at <paramref name="location"/> was not equal
    /// to <paramref name="newValue"/>. Equality is established using the
    /// comparer supplied.
    /// </para>
    /// <para>
    /// Before assigning <paramref name="newValue"/> to <paramref name="location"/>,
    /// <see cref="InvokePropertyValueChanging{TProperty}(String, TProperty, TProperty)"/>
    /// will be invoked.
    /// </para>
    /// <para>
    /// After <paramref name="location"/> has been assigned <paramref name="newValue"/>, 
    /// <see cref="InvokePropertyValueChanged{TProperty}(String, TProperty, TProperty)"/>
    /// will be invoked.
    /// </para>
    /// <para>
    /// This method is not implemented to be safely used in a concurrent access scenario.
    /// </para>
    /// </summary>
    /// <param name="location">The location which to conditionally assign <paramref name="newValue"/>.</param>
    /// <param name="newValue">The new value to assign to <paramref name="location"/>.</param>
    /// <param name="comparer">The comparer to use when evaluating equality between the old and new value.</param>
    /// <param name="propertyName">The name of the property whose value might change.</param>
    /// <returns>The value previously stored at <paramref name="location"/>.</returns>
    protected TProperty ExchangeBackingField<TProperty>(
        ref TProperty location,
        TProperty newValue,
        IEqualityComparer<TProperty> comparer,
        [CallerMemberName] String propertyName = null!)
    {
        ArgumentNullException.ThrowIfNull(comparer);

        var oldValue = location;
        if(!comparer.Equals(oldValue, newValue))
        {
            location = newValue;
            InvokePropertyValueChanged(
                propertyName,
                oldValue: oldValue,
                newValue: newValue);
        }

        return oldValue;
    }
    /// <summary>
    /// <para>
    /// Assigns <paramref name="newValue"/> to <paramref name="location"/> if
    /// the value previously stored at <paramref name="location"/> was not equal
    /// to <paramref name="newValue"/>. Equality is established using the
    /// default equality comparer.
    /// </para>
    /// <para>
    /// Before assigning <paramref name="newValue"/> to <paramref name="location"/>,
    /// <see cref="InvokePropertyValueChanging{TProperty}(String, TProperty, TProperty)"/>
    /// will be invoked.
    /// </para>
    /// <para>
    /// After <paramref name="location"/> has been assigned <paramref name="newValue"/>, 
    /// <see cref="InvokePropertyValueChanged{TProperty}(String, TProperty, TProperty)"/>
    /// will be invoked.
    /// </para>
    /// <para>
    /// This method is not implemented to be safely used in a concurrent access scenario.
    /// </para>
    /// </summary>
    /// <param name="location">The location which to conditionally assign <paramref name="newValue"/>.</param>
    /// <param name="newValue">The new value to assign to <paramref name="location"/>.</param>
    /// <param name="propertyName">The name of the property whose value might change.</param>
    /// <returns>The value previously stored at <paramref name="location"/>.</returns>
    protected T ExchangeBackingField<T>(ref T location, T newValue, [CallerMemberName] String propertyName = null!) =>
        ExchangeBackingField(ref location, newValue, EqualityComparer<T>.Default, propertyName);

    /// <summary>
    /// Invokes the <see cref="PropertyValueChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property whose value changed.</param>
    /// <param name="oldValue">The properties value before changing.</param>
    /// <param name="newValue">The properties value after changing.</param>
    protected void InvokePropertyValueChanged<TProperty>(
        String propertyName,
        TProperty oldValue,
        TProperty newValue)
    {
        var eventArgs = new PropertyValueChangeArgs<TProperty>(
            oldValue: oldValue,
            newValue: newValue,
            propertyName);
        PropertyValueChanged?.Invoke(this, eventArgs);

        OnPropertyValueChanged(eventArgs);
    }
    /// <summary>
    /// Invokes the <see cref="PropertyValueChanging"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property whose value will changing.</param>
    /// <param name="oldValue">The properties value before changing.</param>
    /// <param name="newValue">The properties value after changing.</param>
    protected void InvokePropertyValueChanging<TProperty>(
        String propertyName,
        TProperty oldValue,
        TProperty newValue)
    {
        var eventArgs = new PropertyValueChangeArgs<TProperty>(
            oldValue: oldValue,
            newValue: newValue,
            propertyName);
        PropertyValueChanging?.Invoke(this, eventArgs);

        OnPropertyValueChanging(eventArgs);
    }
    /// <summary>
    /// Invoked after the <see cref="PropertyValueChanged"/> event has been invoked by 
    /// <see cref="InvokePropertyValueChanged{TProperty}(String, TProperty, TProperty)"/>;
    /// </summary>
    /// <param name="eventArgs">The arguments passed to the event handler.</param>
    protected virtual void OnPropertyValueChanged(PropertyValueChangeBaseEventArgs eventArgs) { }
    /// <summary>
    /// Invoked after the <see cref="PropertyValueChanging"/> event has been invoked by 
    /// <see cref="InvokePropertyValueChanging{TProperty}(String, TProperty, TProperty)"/>;
    /// </summary>
    /// <param name="eventArgs">The arguments passed to the event handler.</param>
    protected virtual void OnPropertyValueChanging(PropertyValueChangeBaseEventArgs eventArgs) { }

    /// <inheritdoc/>
    public event EventHandler<PropertyValueChangeBaseEventArgs>? PropertyValueChanged;
    /// <inheritdoc/>
    public event EventHandler<PropertyValueChangeBaseEventArgs>? PropertyValueChanging;
}
