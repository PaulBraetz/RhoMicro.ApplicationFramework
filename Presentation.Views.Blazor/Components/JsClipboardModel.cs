namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components;

using Microsoft.JSInterop;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Clipboard that accesses the underlying clipboard through a JavaScript runtime.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="jsRuntime">The runtime used for invoking the javascript clipboard functionality.</param>
public sealed class JsClipboardModel(IJSRuntime jsRuntime) : IClipboardModel
{
    /// <inheritdoc/>
    public ValueTask<String> ReadTextAsync(CancellationToken cancellationToken) =>
        jsRuntime.InvokeAsync<String>("navigator.clipboard.readText", cancellationToken);

    /// <inheritdoc/>
    public ValueTask WriteTextAsync(String text, CancellationToken cancellationToken) =>
        jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", cancellationToken, text);
}
