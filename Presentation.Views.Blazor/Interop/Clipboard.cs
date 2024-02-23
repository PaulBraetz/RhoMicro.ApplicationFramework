namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Interop;

using Microsoft.JSInterop;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions.Interop;

/// <summary>
/// Clipboard that accesses the underlying clipboard through a JavaScript runtime.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="jsRuntime">The runtime used for invoking the javascript clipboard functionality.</param>
public sealed class Clipboard(IJSRuntime jsRuntime) : IClipboardModel
{
    /// <summary>
    /// Reads text stored in the clipboard.
    /// </summary>
    /// <returns>A task that, upon completion, will contain the text read from the clipboard.</returns>
    public ValueTask<String> ReadAsync() =>
        jsRuntime.InvokeAsync<String>("navigator.clipboard.readText");

    /// <summary>
    /// Writes text to the clipboard.
    /// </summary>
    /// <param name="text">The text to write to the clipboard.</param>
    /// <returns>A task that will complete upon the text having been written to the clipboard.</returns>
    public ValueTask WriteTextAsync(String text) =>
        jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}
