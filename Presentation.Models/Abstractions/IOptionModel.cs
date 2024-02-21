namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a single alternative to select.
/// </summary>
/// <typeparam name="TValue">The type of value represented by the option.</typeparam>
public interface IOptionModel<TValue>
{
    /// <summary>
    /// Gets the name of this option. 
    /// The name should not be assumed to be unique in respect to other options provided the parent control.
    /// </summary>
    String Name { get; }
    /// <summary>
    /// Gets the options value.
    /// </summary>
    TValue Value { get; }
    /// <summary>
    /// Gets or sets a value indicating whether the option is disabled.
    /// </summary>
    Boolean Disabled { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the option is currently selected.
    /// </summary>
    Boolean IsSelected { get; set; }
    /// <summary>
    /// Gets the options id. 
    /// This id may be assumed to be unique in respect to other options provided the parent control.
    /// </summary>
    String Id { get; }
    /// <summary>
    /// Invoked after the option has been selected.
    /// The event argument passed will be the invoking instance.
    /// </summary>
    event Action<IOptionModel<TValue>>? Selected;
}
