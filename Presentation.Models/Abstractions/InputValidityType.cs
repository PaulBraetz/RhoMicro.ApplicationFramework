namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Contains types of value validity.
/// </summary>
public enum InputValidityType
{
    /// <summary>
    /// Denotes that no validity information should be displayed.
    /// </summary>
    None,
    /// <summary>
    /// Denotes that the value is valid.
    /// </summary>
    Valid,
    /// <summary>
    /// Denotes that the value is invalid.
    /// </summary>
    Invalid
}
