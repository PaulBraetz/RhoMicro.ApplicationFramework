namespace RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// AN entry for short circuit caches.
/// </summary>
/// <typeparam name="TException">The type of exception cached.</typeparam>
public sealed class ShortCircuitCacheEntry<TException>
{
    private readonly Object _syncLock = new();
    /// <summary>
    /// Gets the exception set.
    /// </summary>
    public TException? Exception { get; private set; }
    /// <summary>
    /// Gets a value indicating whether <see cref="Exception"/> has been set.
    /// </summary>
    public Boolean IsSet { get; private set; }
    /// <summary>
    /// Sets <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The exception to assign to <see cref="Exception"/>.</param>
    public void Set(TException exception)
    {
        if(Exception != null)
            return;

        lock(_syncLock)
        {
            if(Exception != null)
                return;

            Exception = exception;
            IsSet = true;
        }
    }
}
