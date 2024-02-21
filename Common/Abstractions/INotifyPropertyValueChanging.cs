namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using System;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Declares an event that notifies observers about property values changing.
/// </summary>
public interface INotifyPropertyValueChanging
{
    /// <summary>
    /// Invoked before a property is changed.
    /// </summary>
    event EventHandler<PropertyValueChangeBaseEventArgs>? PropertyValueChanging;
}
