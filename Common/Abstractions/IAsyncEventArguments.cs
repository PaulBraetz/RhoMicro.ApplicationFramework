namespace RhoMicro.ApplicationFramework.Common.Abstractions;
/// <summary>
/// Represents event arguments for asynchronous events.
/// </summary>
public interface IAsyncEventArguments
{
    /// <summary>
    /// Gets the cancellation token used to signal event handlers to cancel their operation.
    /// </summary>
    CancellationToken CancellationToken { get; }
}

/// <summary>
/// Represents event arguments for asynchronous events that communicate some additional data.
/// </summary>
/// <typeparam name="T">The type fo additional data communicated to event handlers.</typeparam>
public interface IAsyncEventArguments<T> : IAsyncEventArguments
{
    /// <summary>
    /// Gets the additional data communicated by the event.
    /// </summary>
    public T Value { get; }
}
