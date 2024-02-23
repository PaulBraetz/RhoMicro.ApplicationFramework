namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Observer that provides no reaction to values published.
/// </summary>
public sealed class NullObserver<T> : IObserver<T>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types <- use is not common due to singleton di container
    public static NullObserver<T> Instance { get; } = new();
#pragma warning restore CA1000 // Do not declare static members on generic types
    /// <inheritdoc/>
    public void Notify(T value) { }
}
