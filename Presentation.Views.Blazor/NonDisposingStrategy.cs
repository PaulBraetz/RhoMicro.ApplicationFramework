namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Dispose strategy that never disposes instances any time <see cref="Dispose(T)"/> is called.
/// </summary>
/// <typeparam name="T">The type of objects never to dispose of.</typeparam>
public sealed class NonDisposingStrategy<T> : IDisposeStrategy<T>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types <- use is not common due to singleton di container
    public static NonDisposingStrategy<T> Instance { get; } = new();
#pragma warning restore CA1000 // Do not declare static members on generic types
    /// <inheritdoc/>
    public void Dispose(T disposable) { }
}
