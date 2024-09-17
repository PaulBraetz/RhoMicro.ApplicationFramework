namespace RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Aspects.Decorators;

/// <summary>
/// Defines common aspects to register.
/// </summary>
[Flags]
public enum CommonAspects
{
    /// <summary>
    /// Register no aspects.
    /// </summary>
    None,
    /// <summary>
    /// Register all common aspects.
    /// </summary>
    All = Timestamp | ExecutionTime | Exception | ThreadId | Execution | ServiceType,
    /// <summary>
    /// Register the <see cref="TimestampLoggingServiceDecorator{TRequest, TResult}"/> aspect.
    /// </summary>
    Timestamp = 1,
    /// <summary>
    /// Register the <see cref="ExecutionTimeLoggingServiceDecorator{TRequest, TResult}"/> aspect.
    /// </summary>
    ExecutionTime = 2,
    /// <summary>
    /// Register the <see cref="ExceptionLoggingServiceDecorator{TRequest, TResult}"/> aspect.
    /// </summary>
    Exception = 4,
    /// <summary>
    /// Register the <see cref="ThreadIdLoggingServiceDecorator{TRequest, TResult}"/> aspect.
    /// </summary>
    ThreadId = 8,
    /// <summary>
    /// Register the <see cref="ExecutionLoggingServiceDecorator{TRequest, TResult}"/> aspect.
    /// </summary>
    Execution = 16,
    /// <summary>
    /// Register the <see cref="ServiceTypeLoggingDecorator{TRequest, TResult, TService}"/> aspect.
    /// </summary>
    ServiceType = 32
}
