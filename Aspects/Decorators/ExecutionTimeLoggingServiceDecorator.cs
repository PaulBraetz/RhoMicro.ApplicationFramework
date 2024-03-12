namespace RhoMicro.ApplicationFramework.Aspects.Decorators;
using System.Diagnostics;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Execution time logging decorator for services.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute and log.</typeparam>
/// <typeparam name="TResult">The type of result to yield and log.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The logger to use for logging execution time.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class ExecutionTimeLoggingServiceDecorator<TRequest, TResult>(
    ILoggingService logger,
    IService<TRequest, TResult> decorated)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        using(_ = Logs.LogLate(createLog, logger))
        {
            var result = await decorated.Execute(request).ConfigureAwait(continueOnCapturedContext: false);

            return result;
        }

        ILogEntry createLog()
        {
            stopWatch!.Stop();
            var result = new ExecutionTimeLogEntry<TRequest>(stopWatch!.ElapsedTicks);

            return result;
        }
    }
}
