namespace RhoMicro.ApplicationFramework.Aspects.Decorators;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Logging;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Logging decorator for services.
/// </summary>
/// <typeparam name="TRequest">The type of request to execute and log.</typeparam>
/// <typeparam name="TResult">The type of result to yield and log.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="logger">The logger to use for logging execution status.</param>
/// <param name="decorated">The decorated service.</param>
/// <param name="formatter">The formatter to use when determining the logged request value.</param>
/// <param name="resultFormatter">The formatter to use when determining the logged result value.</param>
public sealed class ExecutionLoggingServiceDecorator<TRequest, TResult>(
    ILoggingService logger,
    IService<TRequest, TResult> decorated,
    IStaticFormatter<TRequest> formatter,
    IStaticFormatter<TResult> resultFormatter)
    : IService<TRequest, TResult>
    where TRequest : IServiceRequest<TResult>
{
    private static readonly AsyncLocal<(Boolean set, TResult? result)> _localResult = new();

    /// <inheritdoc/>
    public async ValueTask<TResult> Execute(TRequest request)
    {
        using(_ = Logs.Log(new BeforeExecutionLogEntry<TRequest>(request, formatter), logger))
        {
            using(_ = Logs.LogLateConditional(confirmSecondLog, createSecondLog, logger))
            {
                var result = await decorated.Execute(request).ConfigureAwait(continueOnCapturedContext: false);

                _localResult.Value = (true, result);

                return result;
            }
        }

        Boolean confirmSecondLog() => _localResult.Value.set;
        ILogEntry createSecondLog()
        {
            var queryResult = _localResult.Value.result;
            _localResult.Value = (false, default);
            var result = new AfterQueryExecutionLogEntry<TRequest, TResult>(queryResult, resultFormatter);

            return result;
        }
    }
}
