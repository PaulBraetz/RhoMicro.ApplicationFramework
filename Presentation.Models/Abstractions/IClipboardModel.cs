namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides access to the clipboard.
/// </summary>
public interface IClipboardModel
{
    /// <summary>
    /// Reads the last text from the clipboard.
    /// </summary>
    /// <returns>
    /// A task that, upon completion contains the text read from the clipboard.
    /// </returns>
    ValueTask<String> ReadAsync();
    /// <summary>
    /// Writes a text to the clipboard.
    /// </summary>
    /// <param name="text">
    /// The text to write to the clipboard.
    /// </param>
    /// <returns>
    /// A task that will complete upon the text having been written to the clipboard.
    /// </returns>
    ValueTask WriteTextAsync(String text);
}