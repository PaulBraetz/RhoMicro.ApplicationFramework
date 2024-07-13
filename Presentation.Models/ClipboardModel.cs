namespace RhoMicro.ApplicationFramework.Presentation.Models;
using System;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IClipboardModel"/>.
/// </summary>
public sealed class ClipboardModel : IClipboardModel
{
    /// <inheritdoc/>
    public async ValueTask<String> ReadTextAsync(CancellationToken cancellationToken)
    {
        var result = await TextCopy.ClipboardService.GetTextAsync(cancellationToken).ConfigureAwait(true) ?? String.Empty;

        return result;
    }

    /// <inheritdoc/>
    public ValueTask WriteTextAsync(String text, CancellationToken cancellationToken) => new(TextCopy.ClipboardService.SetTextAsync(text, cancellationToken));
}
