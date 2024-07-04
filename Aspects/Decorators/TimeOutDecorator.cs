namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Adds a timeout aspect to decorated services.
/// </summary>
/// <typeparam name="TRequest">The type of request whose execution to add a timeout to.</typeparam>
/// <typeparam name="TResult">The result produced by executing a request.</typeparam>
/// <param name="decorated">The decorated service.</param>
/// <param name="settings">The settings determining the time to wait before requesting execution to be cancelled.</param>
public sealed class TimeoutDecorator<TRequest, TResult>
    (IService<TRequest, TResult> decorated, ITimeoutSettings<TRequest> settings)
    : IService<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    /// <inheritdoc/>
    public ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
#pragma warning restore CA2000 // Dispose objects before losing scope
        cts.CancelAfter(settings.Timeout);

        return decorated.Execute(request, cts.Token);
    }
}
