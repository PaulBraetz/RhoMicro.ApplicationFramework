namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Strategy for handling exceptions thrown.
/// </summary>
public interface IExceptionHandler
{
    /// <summary>
    /// Gets a value indicating whether the handler can handle the exception at hand.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns><see langword="true"/> if the handler can handle the exception; otherwise, <see langword="false"/>.</returns>
    Boolean CanHandle(Exception ex);
    /// <summary>
    /// Handles an exception.
    /// </summary>
    /// <param name="ex">The exception to handle.</param>
    /// <returns>A task representing the handling of the exception.</returns>
    ValueTask Handle(Exception ex);
}
