namespace RhoMicro.ApplicationFramework.Aspects.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Helper class for ambient logging to <see cref="AmbientCallbackStateQueue{T}"/> where <c>T</c> is <see cref="ILogEntry"/>.
/// </summary>
public static class Logs
{
    /// <summary>
    /// Queues a log for logging on the supplied logging service.
    /// </summary>
    /// <param name="log">The log to queue for logging.</param>
    /// <param name="loggingService">The service to use for logging the log.</param>
    /// <returns>A disposable scope, that, upon disposal may trigger a cascading log to the service provided.</returns>
    public static IDisposable Log(ILogEntry log, ILoggingService loggingService)
    {
        ArgumentNullException.ThrowIfNull(loggingService);

        var result = AmbientCallbackStateQueue<ILogEntry>.Enqueue(log, loggingService.Log);

        return result;
    }
    /// <summary>
    /// Upon disposal of the scope returned, conditionally queues a log for logging on the supplied logging service.
    /// </summary>
    /// <param name="confirmLog">The callback invoked to check whether the log should still be enqueued.</param>
    /// <param name="logFactory">The factory used to obtain the log to be logged.</param>
    /// <param name="loggingService">The service to use for logging the log.</param>
    /// <returns>A disposable scope, that, upon disposal will conditionally enqueue the log and may trigger a cascading log to the service provided.</returns>
    public static IDisposable LogLateConditional(Func<Boolean> confirmLog, Func<ILogEntry> logFactory, ILoggingService loggingService)
    {
        ArgumentNullException.ThrowIfNull(loggingService);

        var result = AmbientCallbackStateQueue<ILogEntry>.EnqueueLateConditional(confirmLog, logFactory, loggingService.Log);

        return result;
    }
    /// <summary>
    /// Upon disposal of the scope returned, queues a log for logging on the supplied logging service.
    /// </summary>
    /// <param name="logFactory">The factory used to obtain the log to be logged.</param>
    /// <param name="loggingService">The service to use for logging the log.</param>
    /// <returns>A disposable scope, that, upon disposal will enqueue the log and may trigger a cascading log to the service provided.</returns>
    public static IDisposable LogLate(Func<ILogEntry> logFactory, ILoggingService loggingService)
    {
        ArgumentNullException.ThrowIfNull(loggingService);

        var result = AmbientCallbackStateQueue<ILogEntry>.EnqueueLate(logFactory, loggingService.Log);

        return result;
    }
}
