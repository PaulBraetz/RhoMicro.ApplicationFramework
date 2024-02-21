namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Exceptions;

public static partial class AmbientCallbackStateQueue<T>
{
    private sealed partial class Context
    {
        public struct LateConditionalScope : IDisposable
        {
            private Int32 _disposedValue = BooleanState.FalseState;

            private readonly Func<T> _itemFactory;
            private readonly Func<Boolean> _confirmPush;

            private LateConditionalScope(Func<T> itemFactory, Func<Boolean> confirmPush)
            {
                _itemFactory = itemFactory;
                _confirmPush = confirmPush;
            }

            public static LateConditionalScope Create(Func<Boolean> confirmPush, Func<T> itemFactory, Action<IEnumerable<T>> callback)
            {
                var result = new LateConditionalScope(itemFactory, confirmPush);
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
                    if(_confirmPush.Invoke())
                    {
                        var item = _itemFactory.Invoke();
                        Instance.Enqueue(item);
                    }

                    Instance.OnScopeDisposed();
                } else
                {
                    throw Throw.ObjectDisposedException.LateConditionalScope;
                }
            }
        }
    }
}
