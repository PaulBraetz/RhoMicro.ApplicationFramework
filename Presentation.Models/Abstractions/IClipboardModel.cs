namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides access to the clipboard.
/// </summary>
public interface IClipboardModel
{
    /// <summary>
    /// Reads the last text from the clipboard.
    /// </summary>
    /// <param name="cancellationToken">
    /// The cancellation token used to signal the reading task to be cancelled.
    /// </param>
    /// <returns>
    /// A task that, upon completion contains the text read from the clipboard.
    /// </returns>
    ValueTask<String> ReadTextAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Writes a text to the clipboard.
    /// </summary>
    /// <param name="text">
    /// The text to write to the clipboard.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token used to signal the writing task to be cancelled.
    /// </param>
    /// <returns>
    /// A task that will complete upon the text having been written to the clipboard.
    /// </returns>
    ValueTask WriteTextAsync(String text, CancellationToken cancellationToken = default);
}