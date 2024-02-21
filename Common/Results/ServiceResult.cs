namespace RhoMicro.ApplicationFramework.Common.Results;

using RhoMicro.ApplicationFramework.Common.Results;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents a general service result.
/// </summary>
[UnionType<CompletedSuccessResult, CompliantlyCancelledResult>(Options = UnionTypeOptions.None)]
public sealed partial class ServiceResult
{
    private Task<ServiceResult>? _task;

    /// <summary>
    /// Gets this instance wrapped in a <see cref="System.Threading.Tasks.Task"/>.
    /// </summary>
    public Task<ServiceResult> Task => _task ??= System.Threading.Tasks.Task.FromResult(this);
    /// <summary>
    /// Gets the singleton instance for a fully completed operations result.
    /// </summary>
    public static ServiceResult Completed { get; } = new ServiceResult(CompletedSuccessResult.Instance);
    /// <summary>
    /// Gets the singleton instance for a compliantly cancelled operations result.
    /// </summary>
    public static ServiceResult CompliantlyCancelled { get; } = new(CompliantlyCancelledResult.Instance);
}
