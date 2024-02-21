namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Adds result caching functionality to a service.
/// </summary>
/// <typeparam name="TRequest">The type of query to execute.</typeparam>
/// <typeparam name="TResult">The type of result yielded by executing a query.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="decorated">The decorated service.</param>
/// <param name="cache">The cache used to store results.</param>
/// <param name="logger">The logger toi use when logging cache hits.</param>
public sealed partial class ResultCacheServiceProxy<TRequest, TResult>(
    IService<TRequest, TResult> decorated,
    ICache<TRequest, Task<TResult>> cache,
    ILoggingService logger)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var factory = new ValueFactory(CreateResult);

        using(_ = Logs.LogLate(createLog, logger))
        {
            return await cache.GetOrAdd(request, factory.CreateValue).ConfigureAwait(continueOnCapturedContext: false);
        }

        ILogEntry createLog() =>
            factory!.FactoryCalled ?
            new ResultCacheMissLogEntry<TRequest>() :
            new ResultCacheHitLogEntry<TRequest>();
    }
    private async Task<TResult> CreateResult(TRequest query)
    {
        var result = await decorated.Execute(query).ConfigureAwait(continueOnCapturedContext: false);
        return result;
    }
}
