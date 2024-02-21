namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing value options.
/// </summary>
/// <typeparam name="T">The type of value to be represented by options.</typeparam>
public interface IOptionModelFactory<T>
{
    /// <summary>
    /// Creates a new instance using the value provided.
    /// </summary>
    /// <param name="disabled">Indicates whether the option is disabled.</param>
    /// <param name="isSelected">Indicates whether the option is currently selected.</param>
    /// <param name="value">The options value.</param>
    /// <returns>A new option representing the value provided.</returns>
    IOptionModel<T> Create(T value, Boolean disabled = false, Boolean isSelected = false);
}
