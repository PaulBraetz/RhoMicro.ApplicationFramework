namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Event handler that performs an asynchronous handling operation.
/// </summary>
/// <typeparam name="TEventArgs">The type of event arguments supported by the handler.</typeparam>
/// <param name="sender">The instance raising the event.</param>
/// <param name="args">The event arguments.</param>
/// <returns>A task representing the handlers asynchronous execution.</returns>
public delegate Task AsyncEventHandler<TEventArgs>(Object? sender, TEventArgs args)
    where TEventArgs : IAsyncEventArguments;
/// <summary>
/// Event handler that performs an asynchronous handling operation.
/// </summary>
/// <param name="sender">The instance raising the event.</param>
/// <param name="args">The event arguments.</param>
/// <returns>A task representing the handlers asynchronous execution.</returns>
public delegate Task AsyncEventHandler(Object? sender, IAsyncEventArguments args);
/// <summary>
/// Contains extensions for <see cref="AsyncEventHandler"/>.
/// </summary>
public static class AsyncEventHandlerExtensions
{
    /// <summary>
    /// Asynchronously invokes all delegates registered to the handler.
    /// </summary>
    /// <param name="handler">The event handler whose delegates to invoke.</param>
    /// <param name="sender">The instance raising the event.</param>
    /// <param name="args">The event arguments.</param>
    /// <returns>A task that will complete upon all of the handlers delegates having completed, or a completed task if the handler was <see langword="null"/>.</returns>
    public static Task InvokeAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs>? handler,
        Object? sender,
        TEventArgs args)
        where TEventArgs : IAsyncEventArguments
    {
        var delegates = handler?.GetInvocationList();

        if(delegates == null || delegates.Length == 0)
            return Task.CompletedTask;

        if(delegates.Length == 1)
            return ( (AsyncEventHandler<TEventArgs>)delegates[0] ).Invoke(sender, args);

        var invocations = delegates
            .Cast<AsyncEventHandler<TEventArgs>>()
            .Select(d => d.Invoke(sender, args));

        return Task.WhenAll(invocations);
    }
    /// <summary>
    /// Asynchronously invokes all delegates registered to the handler.
    /// </summary>
    /// <param name="handler">The event handler whose delegates to invoke.</param>
    /// <param name="sender">The instance raising the event.</param>
    /// <param name="args">The event arguments.</param>
    /// <returns>
    /// A task that will complete upon all of the handlers delegates having completed, 
    /// or a completed task if the handler was <see langword="null"/>.
    /// </returns>
    public static Task InvokeAsync(
        this AsyncEventHandler? handler,
        Object? sender,
        IAsyncEventArguments args)
    {
        var delegates = handler?.GetInvocationList();

        if(delegates == null || delegates.Length == 0)
            return Task.CompletedTask;

        if(delegates.Length == 1)
            return ( (AsyncEventHandler)delegates[0] ).Invoke(sender, args);

        var invocations = delegates
            .Cast<AsyncEventHandler>()
            .Select(d => d.Invoke(sender, args));

        return Task.WhenAll(invocations);
    }
    /// <summary>
    /// Asynchronously invokes all delegates registered to the handler.
    /// </summary>
    /// <param name="handler">The event handler whose delegates to invoke.</param>
    /// <param name="sender">The instance raising the event.</param>
    /// <param name="cancellationToken">The cancellation token used to create the event arguments.</param>
    /// <param name="value">The additional data communicated by the event.</param>
    /// <returns>
    /// A task that will complete upon all of the handlers delegates having completed, 
    /// or a completed task if the handler was <see langword="null"/>.
    /// </returns>
    public static Task InvokeAsync<T>(
        this AsyncEventHandler<IAsyncEventArguments<T>>? handler,
        Object? sender,
        T value,
        CancellationToken cancellationToken = default)
    {
        var delegates = handler?.GetInvocationList();

        if(delegates == null || delegates.Length == 0)
            return Task.CompletedTask;

        var args = new AsyncEventArgs<T>(value, cancellationToken);

        if(delegates.Length == 1)
            return ( (AsyncEventHandler<IAsyncEventArguments<T>>)delegates[0] ).Invoke(sender, args);

        var invocations = delegates
            .Cast<AsyncEventHandler>()
            .Select(d => d.Invoke(sender, args));

        return Task.WhenAll(invocations);
    }
    /// <summary>
    /// Asynchronously invokes all delegates registered to the handler.
    /// </summary>
    /// <param name="handler">The event handler whose delegates to invoke.</param>
    /// <param name="sender">The instance raising the event.</param>
    /// <param name="cancellationToken">The cancellation token used to create the event arguments.</param>
    /// <returns>
    /// A task that will complete upon all of the handlers delegates having completed, 
    /// or a completed task if the handler was <see langword="null"/>.
    /// </returns>
    public static Task InvokeAsync(
        this AsyncEventHandler? handler,
        Object? sender,
        CancellationToken cancellationToken = default)
    {
        var delegates = handler?.GetInvocationList();

        if(delegates == null || delegates.Length == 0)
            return Task.CompletedTask;

        var args = new AsyncEventArgs(cancellationToken);

        if(delegates.Length == 1)
            return ( (AsyncEventHandler)delegates[0] ).Invoke(sender, args);

        var invocations = delegates
            .Cast<AsyncEventHandler>()
            .Select(d => d.Invoke(sender, args));

        return Task.WhenAll(invocations);
    }
}
