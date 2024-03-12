namespace RhoMicro.ApplicationFramework.Aspects.Proxies;

using RhoMicro.ApplicationFramework.Aspects;
using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Adds exception caching functionality to a service.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a request.</typeparam>
/// <typeparam name="TException">The type of exceptions cached.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="decorated">The decorated service.</param>
/// <param name="cache">The cache used to store results.</param>
public sealed class ShortCircuitServiceProxy<TRequest, TResult, TException>(
    IService<TRequest, TResult> decorated,
    ICache<TRequest, ShortCircuitCacheEntry<TException>> cache)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
    where TException : Exception
{

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var entry = cache.GetOrAdd(request, k => new ShortCircuitCacheEntry<TException>());
        if(entry.IsSet)
            throw entry.Exception!;

        try
        {
            //await to force exception
            var result = await decorated.Execute(request).ConfigureAwait(continueOnCapturedContext: false);

            return result;
        } catch(TException ex)
        {
            entry.Set(ex);
            throw;
        }
    }
}
