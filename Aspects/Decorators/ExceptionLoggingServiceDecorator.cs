namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Exception logging decorator for query services.
/// </summary>
/// <typeparam name="TRequest">The type of query to execute and log.</typeparam>
/// <typeparam name="TResult">The type of result to yield and log.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The logger to use for logging execution status.</param>
/// <param name="decorated">The decorated service.</param>
public sealed class ExceptionLoggingServiceDecorator<TRequest, TResult>(
    ILoggingService logger,
    IService<TRequest, TResult> decorated)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    private static readonly AsyncLocal<Exception?> _localException = new();

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        using(_ = Logs.LogLateConditional(confirmPush, createLog, logger))
        {
            try
            {
                //await in order to force exception
                var result = await decorated.Execute(request).ConfigureAwait(continueOnCapturedContext: false);

                return result;
            } catch(Exception ex)
            {
                _localException.Value = ex;
                throw;
            }
        }

        Boolean confirmPush() => _localException.Value != null;

        ILogEntry createLog()
        {
            var exception = _localException.Value!;
            _localException.Value = null;
            var logEntry = new ExceptionLogEntry<TRequest>(exception);

            return logEntry;
        }
    }
}
