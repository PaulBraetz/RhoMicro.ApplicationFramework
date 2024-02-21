namespace RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Extensions for the <c>RhoMicro.ApplicationFramework.Common</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Awaits a semaphore once and returns a disposable that, upon disposing, releases the semaphore once.
    /// </summary>
    /// <param name="gate">The semaphore to enter and release.</param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> token to observe.
    /// </param>
    /// <returns></returns>
    public static async Task<IDisposable> WaitDisposableAsync(this SemaphoreSlim gate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(gate);

        await gate.WaitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        var result = new CallbackDisposable(() => gate.Release());

        return result;
    }
    /// <summary>
    /// Awaits a semaphore once and returns a disposable that, upon disposing, releases the semaphore once.
    /// </summary>
    /// <param name="gate">The semaphore to enter and release.</param>
    /// <param name="cancellationToken">
    /// The <see cref="CancellationToken"/> token to observe.
    /// </param>
    /// <returns></returns>
    public static IDisposable WaitDisposable(this SemaphoreSlim gate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(gate);

        gate.Wait(cancellationToken);

        var result = new CallbackDisposable(() => gate.Release());

        return result;
    }
    /// <summary>
    /// Evaluates an enumeration by executing an action over all items contained.
    /// </summary>
    /// <typeparam name="T">The type of item enumerated.</typeparam>
    /// <param name="items">The enumeration to evaluate.</param>
    /// <param name="action">The action to invoke on every item enumerated.</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(action);

        foreach(var item in items)
            action(item);
    }
    /// <summary>
    /// <para>
    /// Executes a command on the service provided.
    /// </para>
    /// <para>
    /// Attention: Make sure that code utilizing this interface is not violating the CQRS pattern.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the command.</typeparam>
    /// <typeparam name="TRequest">The type of command to execute.</typeparam>
    /// <param name="request">The command to execute.</param>
    /// <param name="service">The service to execute the command on.</param>
    /// <returns>A task representing the commands execution.</returns>
    public static ValueTask<TResult> Using<TRequest, TResult>(this TRequest request, IService<TRequest, TResult> service)
        where TRequest : IServiceRequest<TResult>
    {
        ArgumentNullException.ThrowIfNull(service);

        var result = service.Execute(request);

        return result;
    }
    /// <summary>
    /// <para>
    /// Captures a cqrs command and the service to execute it into a closure command.
    /// </para>
    /// <para>
    /// Attention: Make sure that code utilizing this interface is not violating the CQRS pattern.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the command.</typeparam>
    /// <typeparam name="TRequest">The type of command to capture.</typeparam>
    /// <param name="command">The command to capture.</param>
    /// <param name="service">The service to capture.</param>
    /// <returns>A closure command capturing the command and the service using which to execute it.</returns>
    public static IRequestClosure<TResult> Capture<TRequest, TResult>(this TRequest command, IService<TRequest, TResult> service)
        where TRequest : IServiceRequest<TResult>
    {
        var result = new RequestClosure<TRequest, TResult>(command, service);

        return result;
    }
    /// <summary>
    /// Wraps an instance of <see cref="Task{TResult}"/> in an instance of <see cref="ValueTask{TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The type of result produced by the task.</typeparam>
    /// <param name="task">The task to wrap.</param>
    /// <returns>A new instance of <see cref="ValueTask{TResult}"/>, wrapping <paramref name="task"/>.</returns>
    public static ValueTask<T> AsValueTask<T>(this Task<T> task)
    {
        //TODO: find and replace instances of ValueTask.FromResult(ServiceResult)
        var result = new ValueTask<T>(task);

        return result;
    }
}
