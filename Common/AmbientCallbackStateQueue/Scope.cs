namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Exceptions;

public static partial class AmbientCallbackStateQueue<T>
{
    private sealed partial class Context
    {
        public struct Scope : IDisposable
        {
            private Int32 _disposedValue = BooleanState.FalseState;

            public Scope() { }

            public static Scope Create(T item, Action<IEnumerable<T>> callback)
            {
                var result = new Scope();

                Instance.OnScopeCreated();

                Instance.SetCallback(callback);
                Instance.Enqueue(item);

                return result;
            }
            public void Dispose()
            {
                if(Interlocked.CompareExchange(
                    ref _disposedValue,
                    BooleanState.TrueState,
                    BooleanState.FalseState) == BooleanState.FalseState)
                {
                    Instance.OnScopeDisposed();
                } else
                {
                    throw Throw.ObjectDisposedException.Scope;
                }
            }
        }
    }
}
