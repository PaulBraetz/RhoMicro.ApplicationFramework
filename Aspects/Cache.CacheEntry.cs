namespace RhoMicro.ApplicationFramework.Aspects;

using Timer = System.Timers.Timer;

public sealed partial class AspectCache<TKey, TValue> where TKey : notnull
{
    private readonly struct CacheEntry(TValue value, Timer expiryTimer) : IDisposable
    {
        public readonly TValue Value = value;

        public void Dispose() => expiryTimer.Dispose();
    }
}
