namespace RhoMicro.ApplicationFramework.Common;
/// <summary>
/// Empty implementation of <see cref="IDisposable"/>.
/// </summary>
public sealed class NullDisposable : IDisposable
{
    /// <summary>
    /// Initializes a new instance. If possible, use <see cref="Instance"/> instead.
    /// </summary>
    public NullDisposable() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    /// <summary>
    /// Gets the 
    /// </summary>
    public static NullDisposable Instance { get; } = new();
    /// <inheritdoc/>
    public void Dispose() { }
}
