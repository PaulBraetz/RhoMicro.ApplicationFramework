namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a display component for self-updating age displays.
/// </summary>
public interface IAgeModel
{
    /// <summary>
    /// Gets the formatted (current) age.
    /// </summary>
    String Value { get; }
    /// <summary>
    /// Gets or sets the strategy used to format an age upon an update.
    /// </summary>
    Func<TimeSpan, String> FormatAge { get; set; }
    /// <summary>
    /// Gets or sets the origin of the age displayed.
    /// </summary>
    DateTimeOffset Origin { get; set; }
}
