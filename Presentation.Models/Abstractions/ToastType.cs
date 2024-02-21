namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Contains toast type definitions intended to provide visual clues to the user.
/// </summary>
public enum ToastType
{
    /// <summary>
    /// The toast contains general information.
    /// </summary>
    Info,
    /// <summary>
    /// The toast contains warnings that inform about the state of the application or are intended to shift the users behaviour.
    /// </summary>
    Warning,
    /// <summary>
    /// The toast contains information about an error that occured during the applications execution.
    /// </summary>
    Error
}
