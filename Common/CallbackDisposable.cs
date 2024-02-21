#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Exceptions;

/// <summary>
/// Callback-based implementation of <see cref="IDisposable"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="callback">The callback to invoke upon <see cref="Dispose"/> being called.</param>
public readonly struct CallbackDisposable(Action callback) : IDisposable, IEquatable<CallbackDisposable>
{
    /// <inheritdoc/>
    public void Dispose() => callback.Invoke();
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw Throw.NotSupportedException.CallbackDisposableEquals;
    /// <inheritdoc/>
    public Boolean Equals(CallbackDisposable other) => throw Throw.NotSupportedException.CallbackDisposableEquals;
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw Throw.NotSupportedException.CallbackDisposableGetHashCode;
    /// <inheritdoc/>
    public static Boolean operator ==(CallbackDisposable left, CallbackDisposable right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(CallbackDisposable left, CallbackDisposable right) => !( left == right );
}
