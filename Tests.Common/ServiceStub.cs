namespace RhoMicro.ApplicationFramework.Tests.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Stub for command services.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResult"></typeparam>
public sealed class ServiceStub<TRequest, TResult> :
    IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    /// <summary>
    /// Gets a value indicating whether <see cref="Execute(TRequest)"/> has been called.
    /// </summary>
    public Boolean ExecuteCalled { get; private set; }
    /// <summary>
    /// Gets or sets the strategy to use when invoking <see cref="Execute(TRequest)"/>.
    /// </summary>
    public Func<TRequest, ValueTask<TResult>>? ExecuteStrategy { get; set; }

    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request)
    {
        ExecuteCalled = true;

        return ExecuteStrategy == null ?
            throw new InvalidOperationException($"{nameof(ExecuteStrategy)} was not set.") :
            ExecuteStrategy(request);
    }
}
