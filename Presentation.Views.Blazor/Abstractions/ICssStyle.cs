namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents a general component style.
/// </summary>
public interface ICssStyle
{
    /// <summary>
    /// Gets the class names to apply to the top level component element.
    /// </summary>
    IReadOnlyList<String> Classes { get; }
}
