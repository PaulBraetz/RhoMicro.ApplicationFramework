namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common.Results;

/// <summary>
/// Represents a command whose dependencies are fully captured.
/// </summary>
public interface IRequestClosure : IRequestClosure<ServiceResult>;
/// <summary>
/// <para>
/// Represents a command whose dependencies are fully captured.
/// </para>
/// <para>
/// Attention: Make sure that code utilizing this interface is not violating the CQRS pattern.
/// </para>
/// </summary>
/// <typeparam name="TResult">The type of result produced by the command.</typeparam>
public interface IRequestClosure<TResult>
{
    /// <summary>
    /// Executes a command.
    /// </summary>
    /// <returns>The result of executing the captured command.</returns>
    ValueTask<TResult> Execute();
}