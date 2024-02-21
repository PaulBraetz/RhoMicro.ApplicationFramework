namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Common.Abstractions;

public sealed partial class ResultCacheServiceProxy<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    private sealed class ValueFactory(Func<TRequest, Task<TResult>> factory)
    {
        public Boolean FactoryCalled { get; private set; }

        public Task<TResult> CreateValue(TRequest key)
        {
            FactoryCalled = true;
            return factory.Invoke(key);
        }
    }
}
