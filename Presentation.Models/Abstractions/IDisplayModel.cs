namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a simple display field for rendering a String representation of a value.
/// </summary>
/// <typeparam name="TValue">The type of value to display.</typeparam>
public interface IDisplayModel<TValue>
{
    /// <summary>
    /// Gets or sets the value to diplay.
    /// </summary>
    TValue Value { get; set; }
    /// <summary>
    /// Gets a String representation of <see cref="Value"/> or <see cref="Placeholder"/> if <see cref="Value"/> is <see langword="null"/>.
    /// </summary>
    String DisplayText { get; }
    /// <summary>
    /// Gets or sets the placeholder to display if <see cref="Value"/> is <see langword="null"/>.
    /// </summary>
    String Placeholder { get; set; }
}
