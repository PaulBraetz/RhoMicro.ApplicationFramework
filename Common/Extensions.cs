namespace RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Extensions for the <c>RhoMicro.ApplicationFramework.Common</c> namespace.
/// </summary>
public static class Extensions
{
#pragma warning disable CS1573 // docs are inherited
    /// <inheritdoc cref="IIndexableSet{TSelf, T}.Remove(IEnumerable{T})"/>
    /// <param name="set">The set to remove elements from.</param>
    public static TSelf Remove<TSelf, T>(this IIndexableSet<TSelf, T> set, params T[] elements)
        where T : notnull
        where TSelf : IIndexableSet<TSelf, T>
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.Remove(elements);
    }
    /// <inheritdoc cref="IIndexableSet{TSelf, T}.Add(IEnumerable{T})"/>
    /// <param name="set">The set to add elements to.</param>
    public static TSelf Add<TSelf, T>(this IIndexableSet<TSelf, T> set, params T[] elements)
        where T : notnull
        where TSelf : IIndexableSet<TSelf, T>
    {
        ArgumentNullException.ThrowIfNull(set);

        return set.Add(elements);
    }
#pragma warning restore CS1573 // docs are inherited

    /// <summary>
    /// Gets a value indicating whether the specified runtime configuration is the debug ("DEBUG") configuration.
    /// </summary>
    /// <param name="configuration">The runtime configuration to check.</param>
    /// <returns>
    /// <see langword="true"/> if the configuration is the debug configuration; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsDevelopment(this IEnvironmentConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var result = configuration.Name == EnvironmentConfiguration.Development.Name;

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
    public static async Task<CallbackDisposable> WaitDisposableAsync(this SemaphoreSlim gate, CancellationToken cancellationToken = default)
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
    public static CallbackDisposable WaitDisposable(this SemaphoreSlim gate, CancellationToken cancellationToken = default)
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
    /// Executes a command on the service provided.
    /// </para>
    /// <para>
    /// Attention: Make sure that code utilizing this interface is not violating the CQRS pattern.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">The type of command to execute.</typeparam>
    /// <typeparam name="TSuccess">The type of success produced by the command.</typeparam>
    /// <typeparam name="TFailure">The type of failure produced by the command.</typeparam>
    /// <param name="request">The command to execute.</param>
    /// <param name="service">The service to execute the command on.</param>
    /// <returns>A task representing the commands execution.</returns>
    public static ValueTask<Result<TSuccess, TFailure>> Using<TRequest, TSuccess, TFailure>(this TRequest request, IService<TRequest, TSuccess, TFailure> service)
        where TRequest : IServiceRequest<TSuccess, TFailure>
    {
        ArgumentNullException.ThrowIfNull(service);

        var result = service.Execute(request);

        return result;
    }
    /// <summary>
    /// Captures a request and the service to execute it into a closure.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the request.</typeparam>
    /// <typeparam name="TRequest">The type of request to capture.</typeparam>
    /// <param name="request">The request to capture.</param>
    /// <param name="service">The service to capture.</param>
    /// <returns>A request closure capturing the request and the service using which to execute it.</returns>
    public static IRequestClosure<TResult> Capture<TRequest, TResult>(this TRequest request, IService<TRequest, TResult> service)
        where TRequest : IServiceRequest<TResult>
    {
        var result = new RequestClosure<TRequest, TResult>(request, service);

        return result;
    }
    /// <summary>
    /// Captures a request and the service to execute it into a closure.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the request.</typeparam>
    /// <typeparam name="TRequest">The type of request to capture.</typeparam>
    /// <param name="request">The request to capture.</param>
    /// <param name="service">The service to capture.</param>
    /// <returns>A request closure capturing the request and the service using which to execute it.</returns>
    public static IRequestClosure<TResult> Capture<TRequest, TResult>(this IService<TRequest, TResult> service, TRequest request)
        where TRequest : IServiceRequest<TResult>
    {
        var result = new RequestClosure<TRequest, TResult>(request, service);

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
        var result = new ValueTask<T>(task);

        return result;
    }
}

/// <summary>
/// Represents a delegate conforming to the <c>TryXXX</c>-pattern.
/// </summary>
/// <typeparam name="TParameter">The type of parameter taken.</typeparam>
/// <typeparam name="TResult">The type of result produced.</typeparam>
/// <param name="parameter">The parameter used to produce an instance of <typeparamref name="TResult"/>.</param>
/// <param name="result">The created result, if one could be created; otherwise, <see langword="default"/>(<typeparamref name="TResult"/>).</param>
/// <returns><see langword="true"/> if a result could be produced; otherwise, <see langword="false"/>.</returns>
public delegate Boolean TryFactory<in TParameter, TResult>(TParameter parameter, out TResult result);
