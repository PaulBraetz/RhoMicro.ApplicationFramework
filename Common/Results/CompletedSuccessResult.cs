namespace RhoMicro.ApplicationFramework.Common.Results;
/// <summary>
/// Represents a fully completed operations result.
/// </summary>
public sealed class CompletedSuccessResult : AwaitableResultBase<CompletedSuccessResult>
{
    private CompletedSuccessResult() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CompletedSuccessResult Instance { get; } = new();
    /// <inheritdoc/>
    public override String ToString() => nameof(CompletedSuccessResult);
}
