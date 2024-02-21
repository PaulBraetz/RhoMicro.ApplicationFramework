#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Exceptions;

/// <summary>
/// Callback-based implementation of <see cref="IAsyncDisposable"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="callback">The callback to invoke upon <see cref="DisposeAsync"/> being called.</param>
public readonly struct CallbackAsyncDisposable(Func<ValueTask> callback) : IAsyncDisposable, IEquatable<CallbackAsyncDisposable>
{
    /// <inheritdoc/>
    public ValueTask DisposeAsync() => callback.Invoke();
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw Throw.NotSupportedException.CallbackDisposableEquals;
    /// <inheritdoc/>
    public Boolean Equals(CallbackAsyncDisposable other) => throw Throw.NotSupportedException.CallbackDisposableEquals;
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw Throw.NotSupportedException.CallbackDisposableGetHashCode;
    /// <inheritdoc/>
    public static Boolean operator ==(CallbackAsyncDisposable left, CallbackAsyncDisposable right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(CallbackAsyncDisposable left, CallbackAsyncDisposable right) => !( left == right );
}
