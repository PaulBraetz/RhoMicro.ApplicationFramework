namespace RhoMicro.ApplicationFramework.Common.Abstractions;

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