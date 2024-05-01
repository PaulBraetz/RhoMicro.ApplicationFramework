namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common.Results;

/// <summary>
/// Represents a request execution whose request and service is captured.
/// </summary>
public interface IRequestClosure : IRequestClosure<ServiceResult>;

/// <summary>
/// Represents a request execution whose request and service is captured.
/// </summary>
/// <typeparam name="TResult">The type of result produced by the request execution.</typeparam>
public interface IRequestClosure<TResult>
{
    /// <summary>
    /// Executes a command.
    /// </summary>
    /// <returns>The result of executing the captured command.</returns>
    ValueTask<TResult> Execute();
}