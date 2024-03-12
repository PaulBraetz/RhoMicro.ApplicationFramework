namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Exceptions;

public static partial class AmbientCallbackStateQueue<T>
{
    private sealed partial class Context
    {
        public struct LateScope : IDisposable
        {
            private Int32 _disposedValue = BooleanState.FalseState;

            private readonly Func<T> _itemFactory;

            private LateScope(Func<T> itemFactory) => _itemFactory = itemFactory;

            public static LateScope Create(Func<T> itemFactory, Action<IEnumerable<T>> callback)
            {
                var result = new LateScope(itemFactory);

                Instance.OnScopeCreated();

                Instance.SetCallback(callback);

                return result;
            }

            public void Dispose()
            {
                if(Interlocked.CompareExchange(
                    ref _disposedValue,
                    BooleanState.TrueState,
                    BooleanState.FalseState) == BooleanState.FalseState)
                {
                    var item = _itemFactory.Invoke();
                    Instance.Enqueue(item);
                    Instance.OnScopeDisposed();
                } else
                {
                    throw Throw.ObjectDisposedException.LateScope;
                }
            }
        }
    }
}
