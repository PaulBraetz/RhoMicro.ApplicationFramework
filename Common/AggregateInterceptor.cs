namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Aggregates interceptors for collection injection.
/// </summary>
/// <typeparam name="T">
/// The type of object to intercept.
/// </typeparam>
/// <param name="interceptors">
/// The interceptor collection to wrap.
/// </param>
public sealed class AggregateInterceptor<T>(
    IEnumerable<IInterceptor<T>> interceptors) :
    IInterceptor<T>
{
    //eagerly enumerate as simpleinjector injects an enumerable that always 
    //yields from the container resolve
    private readonly IEnumerable<IInterceptor<T>> _interceptors = interceptors.ToList();
    /// <inheritdoc/>
    public async ValueTask<T> Intercept(T obj, CancellationToken cancellationToken)
    {
        foreach(var interceptor in _interceptors)
            obj = await interceptor.Intercept(obj, cancellationToken).ConfigureAwait(false);

        return obj;
    }
}
