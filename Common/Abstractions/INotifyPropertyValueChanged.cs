namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Declares an event that notifies observers about property values having changed.
/// </summary>
public interface INotifyPropertyValueChanged
{
    /// <summary>
    /// Invoked after a property has changed.
    /// </summary>
    event EventHandler<PropertyValueChangeBaseEventArgs> PropertyValueChanged;
}
