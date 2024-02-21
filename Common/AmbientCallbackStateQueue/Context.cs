namespace RhoMicro.ApplicationFramework.Common;

using System.Collections.Concurrent;

using RhoMicro.ApplicationFramework.Common.Exceptions;

public static partial class AmbientCallbackStateQueue<T>
{
    private sealed partial class Context
    {
        private Context() { }
        private static readonly AsyncLocal<Context> _local = new()
        {
            Value = new Context()
        };
        private static Context Instance
        {
            get
            {
                _local.Value ??= new Context();

                return _local.Value;
            }
        }

        private Int32 _scopeCount;
        private ConcurrentQueue<T> Queue { get; } = new();
        private Action<IEnumerable<T>>? Callback { get; set; }
        private void SetCallback(Action<IEnumerable<T>> callback) => Callback ??= callback;
        private void Enqueue(T item) => Queue.Enqueue(item);
        private void OnScopeCreated() => Interlocked.Increment(ref _scopeCount);
        private void OnScopeDisposed()
        {
            var scopeCount = Interlocked.Decrement(ref _scopeCount);
            if(scopeCount == 0)
            {
                Callback?.Invoke(Queue);
                Queue.Clear();
                Callback = null;
            } else if(scopeCount == -1)
            {
                throw Throw.InvalidOperationException.ScopeCount;
            }
        }
    }
}
