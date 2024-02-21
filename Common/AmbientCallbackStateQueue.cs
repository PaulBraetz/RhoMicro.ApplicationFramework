#pragma warning disable CA1000 // Do not declare static members on generic types
namespace RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Represents an ambient state queue that persists through asynchronous operations logical contexts. 
/// States will be handled by a single handler upon their outmost enqueue scope being disposed.
/// </summary>
/// <typeparam name="T">The type of item queued.</typeparam>
public static partial class AmbientCallbackStateQueue<T>
{
    /// <summary>
    /// Enqueues an item to the queue and registers a callback for when the queue will be unrolled.
    /// </summary>
    /// <param name="item">The item to enqueue.</param>
    /// <param name="callback">The callback to invoke upon the outmost enqueue scope having been disposed of.</param>
    /// <returns>A disposable scope, that, upon disposing, may cause the callback passed to be invoked.</returns>
    public static IDisposable Enqueue(T item, Action<IEnumerable<T>> callback)
    {
        var result = Context.Scope.Create(item, callback);

        return result;
    }
    /// <summary>
    /// Upon disposing the scope returned, conditionally enqueues an item in the queue and registers a callback for when the queue will be unrolled.
    /// </summary>
    /// <param name="confirmEnqueue">The callback used to confirm the enqueue upon scope disposal.</param>
    /// <param name="itemFactory">The item factory to invoke for obtaining the item to enqueue.</param>
    /// <param name="callback">The callback to invoke upon the outmost enqueue scope having been disposed of.</param>
    /// <returns>A disposable scope, that, upon disposing, enqueues the item in the queue and may cause the callback passed to be invoked.</returns>
    public static IDisposable EnqueueLateConditional(Func<Boolean> confirmEnqueue, Func<T> itemFactory, Action<IEnumerable<T>> callback)
    {
        var result = Context.LateConditionalScope.Create(confirmEnqueue, itemFactory, callback);

        return result;
    }
    /// <summary>
    /// Upon disposing the scope returned, enqueues an item in the queue and registers a callback for when the queue will be unrolled.
    /// </summary>
    /// <param name="itemFactory">The item factory to invoke for obtaining the item to enqueue.</param>
    /// <param name="callback">The callback to invoke upon the outmost enqueue scope having been disposed of.</param>
    /// <returns>A disposable scope, that, upon disposing, enqueues the item in the queue and may cause the callback passed to be invoked.</returns>
    public static IDisposable EnqueueLate(Func<T> itemFactory, Action<IEnumerable<T>> callback)
    {
        var result = Context.LateScope.Create(itemFactory, callback);

        return result;
    }
}
