namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Event arguments for asynchronous events.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="cancellationToken">The cancellation token used to signal event handlers to cancel their operation.</param>
public class AsyncEventArgs(CancellationToken cancellationToken) : IAsyncEventArguments
{
    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; } = cancellationToken;
    /// <summary>
    /// Gets an instance with its <see cref="CancellationToken"/> set to <see cref="CancellationToken.None"/>.
    /// </summary>
    public static AsyncEventArgs Default { get; } = new(CancellationToken.None);
}

/// <summary>
/// Event arguments for asynchronous events that communicate some additional data.
/// </summary>
/// <typeparam name="T">The type fo additional data communicated to event handlers.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="value">The additional data to communicate to handlers.</param>
/// <param name="cancellationToken">The cancellation token used to signal event handlers to cancel their operation.</param>
public class AsyncEventArgs<T>(T value, CancellationToken cancellationToken) : AsyncEventArgs(cancellationToken), IAsyncEventArguments<T>
{
    /// <inheritdoc/>
    public T Value { get; } = value;
}