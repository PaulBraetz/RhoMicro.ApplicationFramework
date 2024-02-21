namespace RhoMicro.ApplicationFramework.Common;

using System.Diagnostics.CodeAnalysis;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Abstract event args for the <see cref="INotifyPropertyValueChanging.PropertyValueChanging"/>
/// or <see cref="INotifyPropertyValueChanged.PropertyValueChanged"/> events.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="propertyName">The name of the property.</param>
public abstract class PropertyValueChangeBaseEventArgs(String propertyName) : EventArgs
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public String PropertyName { get; } = propertyName;
    /// <summary>
    /// Attempts to convert the current instance to an instance of <see cref="PropertyValueChangeArgs{TProperty}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of property.</typeparam>
    /// <returns>
    /// This instance converted to <see cref="PropertyValueChangeArgs{TProperty}"/> if it 
    /// is of type <see cref="PropertyValueChangeArgs{TProperty}"/>; otherwise,
    /// <see langword="null"/>.
    /// </returns>
    public PropertyValueChangeArgs<TProperty>? As<TProperty>() =>
        this as PropertyValueChangeArgs<TProperty>;
    /// <summary>
    /// Gets a value indicating if the current instance can be converted to an 
    /// instance of <see cref="PropertyValueChangeArgs{TProperty}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of property.</typeparam>
    /// <param name="converted">
    /// The converted instance, if it could be created; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if this instance can be converted to
    /// <see cref="PropertyValueChangeArgs{TProperty}"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public Boolean Is<TProperty>([NotNullWhen(true)] out PropertyValueChangeArgs<TProperty>? converted)
    {
        if(this is PropertyValueChangeArgs<TProperty> c)
        {
            converted = c;
            return true;
        }

        converted = null;
        return false;
    }
    /// <summary>
    /// Gets a value indicating if the current instance can be converted to an 
    /// instance of <see cref="PropertyValueChangeArgs{TProperty}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of property.</typeparam>
    /// <returns>
    /// <see langword="true"/> if this instance can be converted to
    /// <see cref="PropertyValueChangeArgs{TProperty}"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public Boolean Is<TProperty>() => this is PropertyValueChangeArgs<TProperty>;
}
