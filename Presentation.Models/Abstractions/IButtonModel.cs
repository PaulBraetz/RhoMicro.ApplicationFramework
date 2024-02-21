namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Represents a model with a single, (possibly) asynchronous handler.
/// </summary>
public interface IButtonModel
{
    /// <summary>
    /// Gets or sets the label to be displayed on the button.
    /// </summary>
    String? Label { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the button should be disabled.
    /// </summary>
    Boolean Disabled { get; set; }
    /// <summary>
    /// Gets a value indicating whether the buttons underlying action is currently being invoked.
    /// </summary>
    Boolean Loading { get; }
    /// <summary>
    /// Gets or sets a value that indicates whether multiple parallel invocations of <see cref="Click"/> should be allowed.
    /// If the value returned is <see langword="true"/>, <see cref="Click"/> should not <see langword="await"/> a previous, still in flight call.
    /// Otherwise; the call should <see langword="await"/> the previous call, set <see cref="Loading"/> to <see langword="false"/> and then ignore the current call.
    /// </summary>
    Boolean AllowParallelClicks { get; set; }
    /// <summary>
    /// Represents the click event. Depending on the parallel click behaviour, multiple invocations of <see cref="Click"/> may trigger parallel invocations of handlers to this event.
    /// </summary>
    event AsyncEventHandler? Clicked;
    /// <summary>
    /// Clicks the button.
    /// </summary>
    Task Click(CancellationToken cancellationToken);
}
