namespace RhoMicro.ApplicationFramework.Common;
/// <summary>
/// Empty implementation of <see cref="IDisposable"/>.
/// </summary>
public sealed class NullDisposable : IDisposable
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
    public static NullDisposable Instance { get; } = new();
    /// <inheritdoc/>
    public void Dispose() { }
}
