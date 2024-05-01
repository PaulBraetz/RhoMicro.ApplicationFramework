namespace RhoMicro.ApplicationFramework.Aspects;

using System.Collections.Concurrent;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Helper class for ambient progress reporting.
/// </summary>
public static partial class Progress
{
    sealed class SynchronousProgress<T>(Action<T> handler) : IProgress<T>
    {
        public void Report(T value) => handler.Invoke(value);
    }

    private static readonly SemaphoreSlim _contextGate = new(1);
    private static AsyncLocal<ConcurrentDictionary<Type, Object>> Context { get; set; } = new();

    /// <summary>
    /// Reports a value to the ambient <see cref="IProgress{T}"/>.
    /// </summary>
    public static void Report<T>(T value)
    {
        if(Context.Value?.TryGetValue(typeof(T), out var weakProgress) ?? false)
        {
            var strongProgress = (AggregateProgress<T>)weakProgress;
            strongProgress.Report(value);
        }
    }
    /// <summary>
    /// Registers a progress handler to the ambient context.
    /// </summary>
    /// <typeparam name="T">The type of progress update value to report to <paramref name="handler"/>.</typeparam>
    /// <param name="handler">The progress handler to register to the context.</param>
    /// <returns>The subscription whose disposal unregisters <paramref name="handler"/> from the local context.</returns>
    public static ProgressSubscription<T> Register<T>(Action<T> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var result = Register(new SynchronousProgress<T>(handler));

        return result;
    }

    /// <summary>
    /// Registers a <see cref="IProgress{T}"/> to the ambient context.
    /// </summary>
    /// <typeparam name="T">The type of progress update value to report to <paramref name="progress"/>.</typeparam>
    /// <param name="progress">The progress to register to the context.</param>
    /// <returns>The subscription whose disposal unregisters <paramref name="progress"/> from the local context.</returns>
    public static ProgressSubscription<T> Register<T>(IProgress<T> progress)
    {
        if(Context.Value == null)
        {
            using var _ = _contextGate.WaitDisposable();
            Context.Value ??= new();
        }

        var weakAggregate = Context.Value.GetOrAdd(typeof(T), static t => new AggregateProgress<T>());
        var strongAggregate = (AggregateProgress<T>)weakAggregate;
        var id = strongAggregate.Add(progress);
        var result = new ProgressSubscription<T>(id, strongAggregate, progress);

        return result;
    }
}
