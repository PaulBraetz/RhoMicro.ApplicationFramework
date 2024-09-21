namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides an empty implementation of <see cref="IInterceptor{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of object to intercept.
/// </typeparam>
#if !GENERATOR
public
#endif
sealed class NullInterceptor<T> : IInterceptor<T>
{
    /// <summary>
    /// Initializes a new instance. This constructor is provided for DI purposes only.
    /// Prefer <see cref="Instance"/> instead.
    /// </summary>
    public NullInterceptor() { }
    /// <summary>
    /// Gets the singleton instance of <see cref="NullInterceptor{T}"/>.
    /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static NullInterceptor<T> Instance { get; } = new();
#pragma warning restore CA1000 // Do not declare static members on generic types
    /// <inheritdoc/>
    public ValueTask<T> Intercept(T obj, CancellationToken cancellationToken) =>
#if !GENERATOR
        ValueTask.FromResult(obj);
#else
        throw new NotImplementedException();
#endif
}